using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PUBGBYPASSER
{
    //! Class untuk communicate ADB.exe
    //! dengan fungsi umum yang paling banyak di execute
    public class GENERALADBCMD
    {
        string ip = "127.0.0.1"; //! atau localhost
        string port = "5555"; //! default port emulator
        string s = "s";
        string shell = "shell"; //! default shell
        string remount = "mount -o rw,remount"; //! default command mount linux sh

        //! constructor
        //! parameter management form untuk logging 
        //! every process begin with kill due pointing of devices with current 
        public GENERALADBCMD()
        {
            KiLLServer();
        }

        void KiLLServer()
        {
            string comment = "kill-server";
            RESULT_PROCESS PointerProcess = new ADBBridge().exec(comment);
            //! jika error code bukan 0 berarti error dan error nya adaalah 
            if (PointerProcess.code != 0)
                throw new Exception(string.Format("error code:{0} - message {1} ", PointerProcess.code, PointerProcess.error));
        }

        public virtual string InitDevices()
        {
            string comment = "devices";
            RESULT_PROCESS PointerProcess = new ADBBridge().exec(comment);
            //! jika error code bukan 0 berarti error dan error nya adaalah 
            if (PointerProcess.code != 0)
                throw new Exception(string.Format("error code:{0} - message {1} ", PointerProcess.code, PointerProcess.error));
            //! return empty jika tidak ada device yang terdetect
            return PointerProcess.output = string.IsNullOrEmpty(PointerProcess.output) ? PointerProcess.output : string.Empty;
        }

        public virtual string ConnectDevice()
        {
            string commentconnect = "connect";

            string[] comment = { commentconnect, string.Format("{0}:{1}", ip, port) };
            RESULT_PROCESS PointerProcess = new ADBBridge().exec(comment);
            //! jika error code bukan 0 berarti error dan error nya adaalah 
            if (PointerProcess.code != 0)
                throw new Exception(string.Format("error code:{0} - message {1} ", PointerProcess.code, PointerProcess.error));
            //! return connected
            return PointerProcess.output = string.IsNullOrEmpty(PointerProcess.output) ? PointerProcess.output : string.Empty;
        }

        //! path contoh data/data or system or data
        //! no return 
        public virtual void Remount(string path)
        {
            string[] comment = {s
                                   , string.Format("{0}:{1}", ip, port)
                                   , shell
                                   , remount
                                   , string.Format("/{0}", path) };

            RESULT_PROCESS PointerProcess = new ADBBridge().exec(comment);
            //! jika error code bukan 0 berarti error dan error nya adaalah 
            if (PointerProcess.code != 0)
                throw new Exception(string.Format("error code:{0} - message {1} ", PointerProcess.code, PointerProcess.error));
        }
        //! rename file dengan parameter oldfile dan newfile
        //! rename init.vbox86.rc init.vbox86.txt
        public virtual void Rename(string oldfile, string newfile)
        {
            string[] comment = {s
                                   , string.Format("{0}:{1}", ip, port)
                                   , shell
                                   , "mv"
                                   , string.Format("/{0}", oldfile)
                                   , string.Format("/{0}", newfile) };

            RESULT_PROCESS PointerProcess = new ADBBridge().exec(comment);
            //! jika error code bukan 0 berarti error dan error nya adaalah 
            if (PointerProcess.code != 0)
                throw new Exception(string.Format("error code:{0} - message {1} ", PointerProcess.code, PointerProcess.error));
        }
        //! remove file
        //! data/data/simplehat.clicker
        public virtual void Remove(string fileremove, int timeout)
        {
            string[] comment = {s
                                   , string.Format("{0}:{1}", ip, port)
                                   , shell
                                   , "rm"
                                   , "-rf"
                                   , string.Format("/{0}", fileremove)
                                   , string.Format("timeout {0}", timeout) };

            RESULT_PROCESS PointerProcess = new ADBBridge().exec(comment);
            //! jika error code bukan 0 berarti error dan error nya adaalah 
            if (PointerProcess.code != 0)
                throw new Exception(string.Format("error code:{0} - message {1} ", PointerProcess.code, PointerProcess.error));
        }

        public virtual void Lancher(string gamepath)
        {
            string commentlancher = "android.intent.category.LAUNCHER 1";
            string[] comment = {s
                                   , string.Format("{0}:{1}", ip, port)
                                   , shell
                                   , "monkey"
                                   , "-p"
                                   , string.Format("{0}", gamepath)
                                   , "-c"
                                   , commentlancher };

            RESULT_PROCESS PointerProcess = new ADBBridge().exec(comment);
            //! jika error code bukan 0 berarti error dan error nya adaalah 
            if (PointerProcess.code != 0)
                throw new Exception(string.Format("error code:{0} - message {1} ", PointerProcess.code, PointerProcess.error));
        }
    }

    public class BLOCKPortFirewall
    {
        enum ACTION
        {
            BLOCK,
            ALLOW
        }
        enum DIRECTION
        {
            IN,
            OUT
        }
        enum PROTOCOL
        {
            TCP,
            UDP
        }

        string[] applications = { "AndroidEmulator.exe", "aow_exe.exe" };

        string rangeport = "10000-125000";

        string command = "advfirewall firewall add rule name=trojan";

        public void Execute(string drive)
        {
            RESULT_PROCESS process = new NETShell().exec(command);
            if (process.error != null)
                throw new Exception(string.Format("Blocking firewall failed. {0} - code {1}", process.code, process.error));
        }
    }

    //! a class for detect current operating system and get system drives of installation os
    //! a proces must be in thread and support for most windows vista and above
    public static class Util
    {
        //! operating system installaed
        static System.OperatingSystem installedOS = System.Environment.OSVersion;
        //! system directory
        static string osSystemDirectory = System.Environment.SystemDirectory;
        //! active domain name
        static string osDomainName = System.Environment.UserDomainName;
        //! platform operating system
        static bool is64Bit = System.Environment.Is64BitOperatingSystem;
        //! directory Program Files
        static string osProgramFiles = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles, Environment.SpecialFolderOption.None);
        //! directory Program Filesx86
        static string osProgramFilesX86 = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolderOption.None);
        //! list of partition
        static string[] osPartitons = System.Environment.GetLogicalDrives();
        //! build version operating system
        static System.Version osVersion = System.Environment.Version;


        public static string GetSystemDirectory
        {
            get
            {
                if (installedOS.Platform != PlatformID.Win32NT)
                    throw new Exception(string.Format("OS Platform {0}. Not Supported", installedOS.Platform));
                return osSystemDirectory;
            }
        }

        public static string[] Partitions
        {
            get
            {
                return osPartitons;
            }
        }

        public static string GetProgramFilesDirectory
        {
            get
            {
                // if os 64bit and apps is 86bit
                if (is64Bit)
                    return osProgramFilesX86;
                //! default is program files
                return osProgramFiles;
            }
        }

        public static string ExecutableDirectory()
        {
            //! main directory execute
            return System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("...exe", "");
        }

    }

    //! jika ada tambahan fungsi bisa di tambahkan(overide di class berikut)

    public class Common : GENERALADBCMD
    {

        public override string ConnectDevice()
        {
            return base.ConnectDevice();
        }

        public override void Remount(string path)
        {
            base.Remount(path);
        }
        public override void Remove(string fileremove, int timeout)
        {
            //! default time in second 0
            int time = timeout == 0 ? 0 : timeout;
            base.Remove(fileremove, time);
        }

        public override void Rename(string oldfile, string newfile)
        {
            base.Rename(oldfile, newfile);
        }

        public override void Lancher(string path)
        {
            base.Lancher(path);
        }
    }

    //! membuat class baru jika ada comment yang berbeda 
    public class Korea : GENERALADBCMD
    {
        public override string ConnectDevice()
        {
            return base.ConnectDevice();
        }

        public override string InitDevices()
        {
            return base.InitDevices();
        }

        public override void Lancher(string gamepath)
        {
            base.Lancher(gamepath);
        }
        public override void Remount(string path)
        {
            base.Remount(path);
        }
        public override void Remove(string fileremove, int timeout)
        {
            base.Remove(fileremove, timeout);
        }
        public override void Rename(string oldfile, string newfile)
        {
            base.Rename(oldfile, newfile);
        }
    }

    //! class untuk membersihkan applikasi yang telah running
    public class WINDOWProcess
    {
        string[] windowProcess;

        public WINDOWProcess()
        {
            this.GetProcess();
        }
        void GetProcess()
        {
            RESULT_PROCESS Process = new GetHardware().exec("-c");
            if (Process.code != 0)
                throw new Exception(string.Format("error code:{0} - message {1} ", Process.code, Process.error));
            windowProcess = Process.output.Split(' ');
        }

        bool Kill(string processname)
        {
            bool ret = false;
            string[] arg = {"/F"
                               , "/IM"
                               , processname
                               ,  "/T"};
            
            RESULT_PROCESS Process = new KILLProcessWindows().exec(arg);
            if(Process.code != 0)
                throw new Exception(string.Format("error code:{0} - message {1} ", Process.code, Process.error));
            if (Process.output.Contains("SUCCESS"))
                ret = true;
            return ret;
        }

        public void CleanUp()
        {
            bool runkill = true;
            while (runkill)
            {
                foreach (string process in windowProcess)
                {
                    if (process.Contains("AndroidEmulator")) 
                        runkill = Kill(process);
                    if (process.Contains("aow_exe")) 
                        runkill = Kill(process);
                    if (process.Contains("adb")) 
                        runkill = Kill(process);
                    if (process.Contains("bfb"))
                        runkill = Kill(process);
                }
                if (runkill)
                    runkill = false;
            }

        }
    }

    //! class untuk extract zip file
    public static class Uncompress
    {
        // 4K is optimum
        static byte[] buffer = new byte[4096];

        public static void UnCompressZipFile(System.IO.Stream source)
        {
            ICSharpCode.SharpZipLib.Zip.ZipFile zipFile = new ICSharpCode.SharpZipLib.Zip.ZipFile(source);
            try
            {
                foreach (ICSharpCode.SharpZipLib.Zip.ZipEntry entry in zipFile)
                {
                    if (!entry.IsFile) 
                        continue;

                    if (entry.IsCrypted)
                        throw new Exception(string.Format("UnCompressZipFile. {0} ", "Compress file encrypted."));
                    
                    string filetobecreate = System.IO.Path.Combine(Util.ExecutableDirectory(), entry.Name);
                    
                    using (System.IO.Stream data = zipFile.GetInputStream(entry))
                    {
                        using (System.IO.Stream write = System.IO.File.Open(filetobecreate, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                        {
                            try
                            {
                                ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(data, write, buffer);
                            }
                            finally
                            { 
                                write.Close();
                                data.Close();
                            }
                            
                        }
                        
                    }
                }
            }
            finally
            {
                zipFile.IsStreamOwner = true;
                zipFile.Close();
            }
        }
    }

}
