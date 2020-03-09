using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;


//! signal

namespace PUBGBYPASSER
{
    public enum API_METHOD
    {
        POST = 0,
        GET = 1
    };

    public enum API_MODE
    {
        LOGIN = 0,
        LOGOUT = 1,
        PASTEBIN = 2,
        OURCLOUD = 3,
        REGISTER = 4,
        UPDATE
    } ;

    public enum RESPONSE_TYPE
    {
        JSON,
        PASTEBIN,
        GDRIVE,
        MEGADRIVE
    }

    //! support several response type of API
    public interface ITYPE
    {
        RESPONSE_TYPE type { get; }

        RESPONSE_FEEDBACK Response { get; }
    }

    //! variable res API
    public struct ParameterUrl
    {
        public string url;
        public string musename;
        public string password;
        public string museid;
        public API_MODE mode;
        public int timeout;
        public API_METHOD method;
    };

    public struct RESPONSE_FEEDBACK
    {
        public string message;
        public string[] details;
        public bool status;
    };

    //! variabel email
    public struct Parameter_Email
    {
        public string display_name;
        public string msg_body; //! isi body email
        public string msg_subject; //! subject email
        public string receive_address; //! penerima email or alamat email
        public string sender_address; //! pengirim
        //! put here for user name and password
        public string username;
        public string password;
    };

    public delegate void Connect();

    public class NetworkManager
    {

        public NetworkManager()
        {
        }

        ~NetworkManager()
        {
        }

        public RESPONSE_FEEDBACK Connect(ParameterUrl paramurl)
        {
            return new RequestManager(paramurl).Status;
        }
        public void SendEmail(Parameter_Email context_email)
        {
            new SMTPEmail(context_email);
        }
    }

    //! request manager atau mengirim parameter (CRUD) ke server
    //! support method POST
    class RequestManager
    {
        System.Net.HttpWebRequest networkAccesManager;

        System.Net.HttpWebResponse webReply;
        ParameterUrl parameterUrl;

        bool KeepAlive = false;
        bool requestAborted = false;

        event Connect finished;
        event Connect readyRead;

        ITYPE responsemanager;

        public RequestManager(ParameterUrl paramurl)
        {
            parameterUrl = paramurl;
            //! Slot
            this.finished += new Connect(Finished);
            this.readyRead += new Connect(ReadyRead);
            CreateRequest();
        }

        void CreateRequest()
        {
            //System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;

            networkAccesManager = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(new Uri(parameterUrl.url));

            string Query = string.Empty;
            byte[] buffer = new byte[512];

            string method = Enum.GetName(typeof(API_METHOD), (int)parameterUrl.method != -1 ? API_METHOD.POST : API_METHOD.GET);

            networkAccesManager.KeepAlive = this.KeepAlive;
            networkAccesManager.Timeout = parameterUrl.timeout;
            networkAccesManager.Method = method;
            networkAccesManager.ContentType = "application/x-www-form-urlencoded";

            if (method == "POST")
            {

                if (parameterUrl.mode == API_MODE.LOGIN)
                {
                    Query = "user=" + parameterUrl.musename + "&password=" + parameterUrl.password;
                    buffer = Encoding.ASCII.GetBytes(Query);
                }
                if (parameterUrl.mode == API_MODE.LOGOUT)
                {
                    Query = "user=" + parameterUrl.musename;
                    buffer = Encoding.ASCII.GetBytes(Query);
                }
                if (parameterUrl.mode == API_MODE.REGISTER)
                {
                    Query = "user=" + parameterUrl.musename + "&id=" + parameterUrl.museid + "&password=" + parameterUrl.password;
                    buffer = Encoding.ASCII.GetBytes(Query);
                }

                networkAccesManager.ContentLength = buffer.Length; networkAccesManager.GetRequestStream().Write(buffer, 0, buffer.Length);
                networkAccesManager.GetRequestStream().Close();
            }

            webReply = (System.Net.HttpWebResponse)networkAccesManager.GetResponse();

            finished();
            readyRead();
        }

        void Finished()
        {
            if (requestAborted)
            {
                webReply.GetResponseStream().Flush();
                webReply.GetResponseStream().Close();
                webReply.Close();
                throw new Exception(string.Format("NetworkManager slot Finished. {0}", "aborted."));
            }

            if (webReply.StatusCode != System.Net.HttpStatusCode.OK)
            {
                webReply.GetResponseStream().Flush();
                webReply.GetResponseStream().Close();
                webReply.Close();
                throw new Exception(string.Format("NetworkManager slot Finished. code: {0} whats: {1}", webReply.StatusCode, webReply.StatusDescription));
            }
            if (webReply.IsFromCache)
            {
                this.CreateRequest();
                return;
            }
        }

        void ReadyRead()
        {
            byte[] UTF8 = new byte[0];
            System.IO.Stream response = webReply.GetResponseStream();

            using (System.IO.StreamReader readbuffer = new System.IO.StreamReader(response))
            {
                try
                {
                    string pagecontent = readbuffer.ReadToEnd().Trim();
                    UTF8 = new byte[pagecontent.Length];
                    UTF8 = Encoding.UTF8.GetBytes(pagecontent);
                }
                finally
                {
                    readbuffer.Close();
                }
            }


            if (parameterUrl.mode == API_MODE.LOGIN || parameterUrl.mode == API_MODE.LOGOUT || parameterUrl.mode == API_MODE.REGISTER)
            {
                responsemanager = new JSONResponse(UTF8);
            }
            else if (parameterUrl.mode == API_MODE.OURCLOUD)//! todo write etc host bokep
            {
                
            }
            else if (parameterUrl.mode == API_MODE.UPDATE)
            { 
                responsemanager = new MEGAResponse(UTF8);
            }
            else//! todo response from pastebin
            {
                responsemanager = new STDResponse(UTF8);
            }

        }

        public bool Aborted
        {
            set { requestAborted = true; }
        }

        public RESPONSE_FEEDBACK Status
        {
            get { return responsemanager.Response; }
        }
    }
    //! below class processing response JSON format
    class JSONResponse : ITYPE
    {

        byte[] JSONByte;
        //! response from PHP

        RESPONSE_FEEDBACK response;

        public JSONResponse(byte[] input)
        {
            JSONByte = input;
            response = new RESPONSE_FEEDBACK();
            this.Load();
        }

        ~JSONResponse()
        {
            response.details = new string[] { };
            response.message = string.Empty;
        }

        void Load()
        {
            using (System.Xml.XmlDictionaryReader jsonreader = System.Runtime.Serialization.Json.JsonReaderWriterFactory
                .CreateJsonReader(this.JSONByte, new System.Xml.XmlDictionaryReaderQuotas()))
            {
                try
                {
                    if (jsonreader == null)
                        throw new Exception(string.Format("JsonUtil.Load(). {0}", "failed to load"));
                    ReadyRead(jsonreader);
                }
                finally
                {
                    jsonreader.Close();
                }

            }
        }

        //! noted the response must be jsonformat {"key":"value"}
        void ReadyRead(System.Xml.XmlReader JSONBuffer)
        {
            JSONBuffer.MoveToContent();

            while (JSONBuffer.Read())
            {
                if (JSONBuffer.IsStartElement("details"))
                {
                    JSONBuffer.Read();
                    response.details = JSONBuffer.Value.Length >= 0 ? JSONBuffer.Value.Split(',') : new string[] { };
                }
                if (JSONBuffer.IsStartElement("success"))
                {
                    JSONBuffer.Read();
                    bool.TryParse(JSONBuffer.Value.Trim(), out response.status);
                }
                if (JSONBuffer.IsStartElement("message"))
                {
                    JSONBuffer.Read();
                    response.message = JSONBuffer.Value.Trim();
                }

            }
        }

        public RESPONSE_FEEDBACK Response
        {
            get { return this.response; }
        }

        public RESPONSE_TYPE type
        {
            get { return RESPONSE_TYPE.JSON; }
        }
    }
    //! below class for overwrite host on etc without know the platform, assume all operating system having same directory
    //! not save thread
    class STDResponse : ITYPE
    {
        byte[] buffer;
        RESPONSE_FEEDBACK response;

        public STDResponse(byte[] input)
        {
            buffer = input;
            response = new RESPONSE_FEEDBACK();
            this.load();
            //! emulate response
            response.details = new string[] { };
        }
        ~STDResponse()
        {
            response.details = new string[] { };
            response.message = string.Empty;
        }

        void load()
        {
            string context = string.Empty;
            using (System.IO.Stream stream = new System.IO.MemoryStream(this.buffer))
            {
                try
                {
                    if (stream.Length < 0)
                        throw new Exception(string.Format("STDResponse.Load(). {0}", "failed to load"));
                    using (System.IO.StreamReader TRead = new System.IO.StreamReader(stream))
                    {
                        try
                        {
                            context = TRead.ReadToEnd();
                            if (string.IsNullOrEmpty(context))
                                throw new Exception(string.Format("STDResponse.Load(). {0}", "data pastebin nulled."));
                        }
                        finally
                        {
                            TRead.Close();
                            ReadyWrite(context);
                        }
                    }

                }
                finally
                {
                    stream.Close();
                }
            }
        }
        //! no need catch anything here  
        void ReadyWrite(string data)
        {
            //! assumed an input is text cause from pastebin
            string[] command = { "-n", data };
            RESULT_PROCESS result = new GetHardware().exec(command);
            if (result.code != 0)
                throw new Exception(string.Format("STDResponse.ReadyWrite(). {0}", "write etc host failed"));
            response.message = data;
            response.status = true;
        }
        
        public RESPONSE_FEEDBACK Response
        {
            get { return this.response; }
        }

        public RESPONSE_TYPE type
        {
            get { return RESPONSE_TYPE.PASTEBIN; }
        }
    }

    class SMTPEmail
    {
        /**
         * After sign into google account, go to:https://www.google.com/settings/security/lesssecureapps
         * 
         * send email with anonymous device
         * 
         * ***/
        int port = 587; //! smtp port
        string host = "smtp.gmail.com"; //! receipent

        Parameter_Email context;

        public SMTPEmail(Parameter_Email context_email)
        {
            context = context_email;
            Sending();
        }
        ~SMTPEmail()
        {
        }

        void Sending()
        {
            //! construct pengirim
            System.Net.Mail.MailAddress pengirim = new MailAddress(context.sender_address, context.display_name);
            //! construct penerima
            System.Net.Mail.MailAddress penerima = new MailAddress(context.receive_address);
            //! construct credential or email username and password
            System.Net.NetworkCredential indetitas = new System.Net.NetworkCredential(context.username, context.password);

            using (System.Net.Mail.SmtpClient smtpClient = new SmtpClient(host, port))
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network; //! always via networks
                smtpClient.EnableSsl = true; //! be aware for port
                smtpClient.Credentials = indetitas;
                smtpClient.SendCompleted += smtpClient_SendCompleted;
                using (System.Net.Mail.MailMessage newmsg = new MailMessage(pengirim, penerima))
                {
                    newmsg.Subject = context.msg_subject;
                    newmsg.Body = context.msg_body;
                    newmsg.Priority = MailPriority.Normal;
                    newmsg.BodyEncoding = UTF8Encoding.UTF8;
                    try
                    {
                        smtpClient.Send(newmsg);
                    }
                    finally
                    {
                        //! dispose or release or buang activity email
                        newmsg.Dispose();
                        smtpClient.Dispose();
                    }
                }
            }

        }
        //! callback or response untuk menangkap error
        void smtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Error.Message))
                throw new Exception(string.Format("Send email error. {0}-{1}", e.Error.StackTrace, e.Error.Message));
        }
    }

    class MEGAResponse : ITYPE
    {
        byte[] buffer;
        
        RESPONSE_FEEDBACK response;

        public MEGAResponse(byte[] input)
        {
            buffer = input;
            response = new RESPONSE_FEEDBACK();
            this.load();
            //! emulate response
            response.details = new string[] {};
        }

        void load()
        {
            byte[] context = new byte[]{};
            using (System.IO.Stream stream = new System.IO.MemoryStream(this.buffer))
            {
                try
                {
                    if (stream.Length < 0)
                        throw new Exception(string.Format("MEGAResponse.Load(). {0}", "failed to load."));
                    ReadyWrite(stream);
                }
                finally
                {
                    stream.Close();
                }
            }
        }
        //! no need catch anything here  
        void ReadyWrite(System.IO.Stream data)
        {
            Uncompress.UnCompressZipFile(data);
            //! clean up
            new WINDOWProcess().CleanUp();
            
            response.message = "none";
            response.status = true;
        }
        

        public RESPONSE_TYPE type
        {
            get { return RESPONSE_TYPE.MEGADRIVE; }
        }

        public RESPONSE_FEEDBACK Response
        {
            get { return this.response; }
        }
    }

    class UpdateRelease
    {
        public enum CompareResult
        {
            UpdateAvailable,
            NoUpdateAvailable,
            ErrorEncountered
        }

        string comparisonPattern = "Major,Minor,Revision,Build";

        AppVersion release;

        string updateUrl;

        delegate void DownloadUpdate();

        public UpdateRelease(AppVersion Release, string UpdateUrl)
        {
            release = Release;
            updateUrl = UpdateUrl;
        }

        void GetUpdate()
        {
            System.Net.WebRequest.DefaultCachePolicy
                = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            try
            {
                System.Xml.XmlDocument Document = new System.Xml.XmlDocument();
                Document.Load(updateUrl);
            }
            finally
            {
            }
        }

        public void AsynvDownloadUpdate(AsyncCallback callback, object o)
        {
            
        }


    }


}
