using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUBGBYPASSER
{
    public partial class RegisterForm : Form, IFORM
    {
        DialogStep actionTaken;
        ManagementForm DialogLogging;
        string password;
        //! main directory API
        string mainUrl = "http://";

        public RegisterForm(System.Windows.Forms.IWin32Window parent, ManagementForm mf)
        {
            InitializeComponent();
            DialogLogging = mf;
            this.MdiParent = parent as DashboardForm;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            actionTaken = DialogStep.LOGIN;
            DialogLogging.RunLoop("RegisterForm");
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxMuseName.Text))
            {
                System.Windows.Forms.MessageBox.Show(this, string.Format("user name smaller than or empty {0}", textBoxMuseName.Text), "User registration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(textBoxMuseId.Text))
            {
                System.Windows.Forms.MessageBox.Show(this, string.Format("user name smaller than or empty {0}", textBoxMuseId.Text), "User registration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ParameterUrl Register = new ParameterUrl();
            Register.url = string.Format("{0}{1}", mainUrl, "register.php");
            Register.musename = textBoxMuseName.Text;
            Register.password = password;
            Register.museid = textBoxMuseId.Text;
            Register.mode = API_MODE.REGISTER;
            Register.timeout = 30000;
            Register.method = API_METHOD.POST;
            try
            {
                RESPONSE_FEEDBACK response = new NetworkManager().Connect(Register);
                if (response.status == false)
                {
                    actionTaken = DialogStep.LOGIN;
                    DialogLogging.RunLoop("RegisterForm");
                    throw new Exception(string.Format("Registration Failed. {0}", response.message));
                }
                //! create email
                Parameter_Email NewEmail = new Parameter_Email();
                NewEmail.display_name = textBoxMuseName.Text;
                NewEmail.msg_subject = string.Format("User name:{0} - id:{1}", textBoxMuseName.Text, textBoxMuseId.Text);
                NewEmail.msg_body = password;
                //! sender email
                NewEmail.sender_address = "xxx@gmail.com";
                //! recepient email
                NewEmail.receive_address = "xxx@yahoo.com";
                //! credential sender email
                NewEmail.username = "";
                NewEmail.password = "";

                //! send email
                new NetworkManager().SendEmail(NewEmail);

                MessageBox.Show(this, string.Format("Registration success {0}. Please be patien an admin will activated.", textBoxMuseName.Text), "User registration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                actionTaken = DialogStep.LOGIN;
                DialogLogging.RunLoop("RegisterForm");
            }
            catch (Exception ex)
            {
                DialogLogging.Append(Level.Error, String.Format("what:{0} {1}", ex.HelpLink, ex.Message));
            }

        }

        public DialogStep StepResult
        {
            get { return this.actionTaken; }
        }

        public string Password
        {
            set
            {
                password = value;
            }
        }

    }
}
