using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PUBGBYPASSER
{
    public enum DialogStep
    {
        LOGIN,
        REGISTER,
        BYPASS
    };

    public class ManagementForm
    {
        readonly System.Collections.Hashtable ScenarioForm;

        DialogStep dialogStep;

        LoggerForm DialogLog;

        //! prepend to mainform
        string museid; //! get from main
        string password;

        private readonly System.Windows.Forms.IWin32Window parent;


        public ManagementForm(System.Windows.Forms.IWin32Window owner)
        {
            parent = owner;
            ScenarioForm = new System.Collections.Hashtable();
            SetupLoggerForm();

        }

        public void SetupLoggerForm()
        {
            DialogLog = new LoggerForm(parent);
            DialogLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            DialogLog.Show();
        }

        public void Append(Level level, string msg)
        {
            DialogLog.SetLogged(level, msg);
        }

        public void init()
        {
            SetupMainForm();
            RunLoop("MainForm");
        }

        public void RunLoop(string nform)
        {
            dialogStep = this.Loopings(nform);
        }
        //! <<-----
        string GetPreviousDialog(Form curForm)
        {
            string currentFormName = curForm.Name;

            if (currentFormName != "MainForm")
                return "MainForm";
            return "MainForm";
        }
        //! ----->>
        string GetNextDialog(Form curForm, DialogStep firstStep)
        {
            string curFormName = curForm.Name;

            if (curFormName == "MainForm" && firstStep == DialogStep.BYPASS)
                return "BypassForm";
            if (curFormName == "MainForm" && firstStep == DialogStep.REGISTER)
                return "RegisterForm";
            return "MainForm";
        }
        //! <>--<>
        DialogStep Loopings(string firstForm)
        {
            DialogStep nextstep = DialogStep.LOGIN;
            string nextForm = firstForm;
            bool StopLoop = true;
            while (StopLoop)
            {
                //! getting step from mainform
                Form currentForm = null;
                currentForm = this.ScenarioForm[firstForm] as Form;
                nextstep = ((IFORM)currentForm).StepResult;

                //! 1. nextstep register
                //! 2. nextstep Bypass

                switch (nextstep)
                {
                    case DialogStep.REGISTER:
                        nextForm = this.GetNextDialog(currentForm, nextstep);
                        NextDialogHandler(nextForm, nextstep);
                        currentForm.Dispose();
                        currentForm = this.ScenarioForm[nextForm] as Form;
                        currentForm.Dock = DockStyle.Top;
                        currentForm.Show();
                        StopLoop = false;
                        break;
                    case DialogStep.BYPASS:
                        nextForm = this.GetNextDialog(currentForm, nextstep);
                        NextDialogHandler(nextForm, nextstep);
                        currentForm.Dispose();
                        currentForm = this.ScenarioForm[nextForm] as Form;
                        currentForm.Dock = DockStyle.Top;
                        currentForm.Show();
                        StopLoop = false;
                        break;
                    case DialogStep.LOGIN:
                        nextForm = this.GetNextDialog(currentForm, nextstep);
                        NextDialogHandler(nextForm, nextstep);
                        currentForm.Dispose();
                        currentForm = this.ScenarioForm[nextForm] as Form;
                        currentForm.Dock = DockStyle.Top;
                        currentForm.Show();
                        StopLoop = false;
                        break;
                }

            }
            return nextstep;
        }

        //! -->>***
        void NextDialogHandler(string curForm, DialogStep step)
        {
            if (curForm.Equals("BypassForm") && step == DialogStep.BYPASS)
            {
                SetupBypassForm();
            }
            if (curForm.Equals("RegisterForm") && step == DialogStep.REGISTER)
            {
                SetupRegisterForm();
            }
            if (curForm.Equals("MainForm") && step == DialogStep.LOGIN)
            {
                SetupMainForm();
            }

        }

        void PreviousDialogHandler(string curForm)
        {
            SetupMainForm();
        }
        //! region for initial parameter form before construct
        #region SetupForm

        void SetupBypassForm()
        {
            this.DisposeBypassForm();
            BypassForm F = new BypassForm(parent, this);
            MainForm mainForm = ScenarioForm["MainForm"] as MainForm;
            this.museid = mainForm.MuseId;
            F.UserID = this.museid;
            ScenarioForm.Remove("BypassForm");
            ScenarioForm.Add("BypassForm", F);
        }

        void SetupRegisterForm()
        {
            this.DisposeRegisterForm();
            DashboardForm passwordFrom = parent as DashboardForm;

            RegisterForm RF = new RegisterForm(parent, this);
            
            RF.Password = this.password;
            
            ScenarioForm.Remove("RegisterForm");
            ScenarioForm.Add("RegisterForm", RF);

        }

        void SetupMainForm()
        {
            this.DisposeMainForm();
            MainForm mainForm = new MainForm(parent, this);
            this.password = ((DashboardForm)parent).GetPassword;
            mainForm.SetPassword = this.password;
            ScenarioForm.Remove("MainForm");
            ScenarioForm.Add("MainForm", mainForm);
        }

        #endregion

        //! region to dispose used form
        #region DisposeForm

        void DisposeRegisterForm()
        {
            RegisterForm f = this.ScenarioForm["RegisterForm"] as RegisterForm;
            if (f != null)
                f.Dispose();
        }

        void DisposeBypassForm()
        {
            BypassForm f = ScenarioForm["BypassForm"] as BypassForm;
            if (f != null)
                f.Dispose();
        }

        void DisposeMainForm()
        {
            MainForm mf = ScenarioForm["MainForm"] as MainForm;
            if (mf != null)
                mf.Dispose();
        }

        void Cleanup()
        {
            BypassForm BF = ScenarioForm["BypassForm"] as BypassForm;
            RegisterForm RF = ScenarioForm["RegisterForm"] as RegisterForm;
            if (BF != null)
                BF.Dispose();
            if (RF != null)
                RF.Dispose();
            ScenarioForm.Clear();
        }
        #endregion

    }
}
