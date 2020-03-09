using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

//! below class might be obselete not maintanance due security reason
//! see at native win32 on git

namespace PUBGBYPASSER
{
    public enum SERVERS : int
    {
        GLOBAL = 0,
        VIETNAM = 1,
        KOREA = 2

    };

    public enum SERVER_MODE : int
    {
        MOBILE = 0,
        EMULATOR = 1,
        MTEST
    };

    public enum MODE : int
    {
        OFFLINE = 0,
        ONLINE = 1
    };

    public partial class BypassForm : Form, IFORM
    {
        //! put here API Directory
        string mainUrl = "http://";

        string activeUser;
        bool status;

        DialogStep ActionTaken;

        ManagementForm MainDashboard;

        public BypassForm(System.Windows.Forms.IWin32Window parent, ManagementForm mf)
        {
            InitializeComponent();
            MainDashboard = mf;
            this.MdiParent = parent as DashboardForm;
            string[] servers = Enum.GetNames(typeof(SERVERS));
            string[] servers_mode = Enum.GetNames(typeof(SERVER_MODE));
            comboBoxServer.Items.AddRange(servers);
            comboBoxMode.Items.AddRange(servers_mode);
            comboBoxMode.SelectedIndex = 0;
            comboBoxServer.SelectedIndex = 0;
        }

        public String UserID
        {
            set { 
                activeUser = value;
            }
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            ParameterUrl Logout = new ParameterUrl();
            Logout.musename = activeUser;
            Logout.password = "";
            Logout.mode = API_MODE.LOGOUT;
            Logout.timeout = 300000;
            Logout.url = string.Format("{0}{1}", mainUrl, "logout.php").Trim();
            Logout.method = API_METHOD.POST;
            try
            {
                RESPONSE_FEEDBACK response = new NetworkManager().Connect(Logout);
                if (response.status == false)
                    throw new Exception(string.Format("Logout Failed. {0}", response.message));
                else
                    status = response.status;
                ActionTaken = DialogStep.LOGIN;
                MainDashboard.RunLoop("BypassForm");
            }
            catch (Exception ex)
            {
                MainDashboard.Append(Level.Error, String.Format("what:{0} {1}", ex.HelpLink, ex.Message));
            }

        }

        private void BypassForm_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = activeUser;
        }


        public DialogStep StepResult
        {
            get { return ActionTaken; }
        }


        //! obselete move to native win32 on next release
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            SERVERS server = (SERVERS) Enum.Parse(typeof(SERVERS),comboBoxServer.GetItemText(comboBoxServer.SelectedIndex));
            SERVER_MODE mode = (SERVER_MODE)Enum.Parse(typeof(SERVERS), comboBoxMode.GetItemText(comboBoxMode.SelectedIndex));
            //! GL EMU
            if (mode == SERVER_MODE.EMULATOR && server == SERVERS.GLOBAL)
            {
                Common GServer = new Common();
                //! if no device return 
                if(string.IsNullOrEmpty(GServer.InitDevices())) return;
                GServer.ConnectDevice();
                GServer.Remount("system");
                GServer.Remount("data");
                GServer.Remount("data/data");
                //! launcer game
                GServer.Lancher("com.tencent.ig");
                GServer.Rename("init.vbox86.rc", "initvbox86.txt");
                GServer.Remove("data/data/com.netease.uu", 20);
                GServer.Remove("data/app-lib/com.tencent.ig-1/libcubehawk.so", 0);

            }
            //! KR EMU
            if (mode == SERVER_MODE.EMULATOR && server == SERVERS.KOREA)
            {
            }
            //! VN EMU
            if (mode == SERVER_MODE.EMULATOR && server == SERVERS.VIETNAM)
            {
            }
            //! GL MOBILE
            if (mode == SERVER_MODE.MOBILE && server == SERVERS.GLOBAL)
            {
            }
            //! KR Mobile
            if (mode == SERVER_MODE.MOBILE && server == SERVERS.KOREA)
            {
            }
            //! VN Mobile
            if (mode == SERVER_MODE.MOBILE && server == SERVERS.VIETNAM)
            {
            }
        }
    }
}
