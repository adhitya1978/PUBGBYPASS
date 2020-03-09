using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PUBGBYPASSER
{
    public partial class MainForm : Form, IFORM
    {
        bool status;
        string password;
        string museid;

        DialogStep actionTaken;

        ManagementForm DialogDashboard;

        public MainForm(System.Windows.Forms.IWin32Window parent, ManagementForm manForm)
        {
            InitializeComponent();
            DialogDashboard = manForm;
            this.MdiParent = parent as DashboardForm;
        }

        //! directory of API
        string mainUrl = "http://";

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            museid = this.textBoxUser.Text;
            ParameterUrl LoginPHP = new ParameterUrl();
            LoginPHP.musename = museid;
            LoginPHP.password = password;
            LoginPHP.mode = API_MODE.LOGIN;
            LoginPHP.timeout = 300000;
            LoginPHP.url = string.Format("{0}{1}", mainUrl, "login.php").Trim();
            LoginPHP.method = API_METHOD.POST;
            try
            {
                RESPONSE_FEEDBACK response = new NetworkManager().Connect(LoginPHP);
                if (response.status == false)
                    throw new Exception(string.Format("Login Failed. {0}", response.message));
                status = response.status;
                actionTaken = DialogStep.BYPASS;
                DialogDashboard.RunLoop("MainForm");
            }
            catch (Exception ex)
            {
                DialogDashboard.Append(Level.Error, String.Format("what:{0} {1}", ex.HelpLink, ex.Message));
            }
        }

        public string SetPassword
        {
            set
            {
                password = value;
            }
        }

        public string MuseId
        {
            get {
                return this.museid;
            }
        }

        public bool STATUS
        {
            get { return this.status; }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            actionTaken = DialogStep.REGISTER;
            DialogDashboard.RunLoop("MainForm");
        }


        public DialogStep StepResult
        {
            get { return this.actionTaken; }
        }

    }
}
