using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUBGBYPASSER
{

    public interface IFORM
    {
        DialogStep StepResult { get; }
    }

    public partial class DashboardForm : Form
    {
        ManagementForm DialogLogging;
        string password;
        public DashboardForm()
        {
            InitializeComponent();
            DialogLogging = new ManagementForm(this);
            GetHardwareId();
            DialogLogging.init();
            /** if u willing using skin form to decorated ui prefered with DMSOFT
            DMSoft.SkinCrafter skin = new DMSoft.SkinCrafter();
            skin.SkinFile = "RedJet";
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Properties.Resources.RedJet))
            {
                skin.LoadSkinFromStream(ms);
                skin.ApplySkin();
                ms.Close();
            }
             * **/
        }

        public string GetPassword
        {
            get { return this.password; }
        }

        void GetHardwareId()
        {
            try
            {
                RESULT_PROCESS Process = new GetHardware().exec("-u");
                if (Process.code != 0)
                    throw new Exception(Process.error);
                password = Process.output;
            }
            catch (Exception ex)
            {
                DialogLogging.Append(Level.Error, string.Format("what:{0}{1}", ex.StackTrace, ex.Message));
            }
        }
    }
}
