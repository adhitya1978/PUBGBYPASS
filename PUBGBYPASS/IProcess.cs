using System;
using System.Collections.Generic;

namespace PUBGBYPASSER
{
    public struct RESULT_PROCESS
    {
        public int code;
        public string error;
        public string output;
    };

    class IProcess
    {
        string _exe;
        RESULT_PROCESS result;

        public IProcess(string exe)
        {
            _exe = exe;
            result = new RESULT_PROCESS();
           
        }

        public RESULT_PROCESS exec(string arg) 
        {
            return exec( new string[] { arg });
        }
        public virtual RESULT_PROCESS exec(string[] args) 
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = string.Join(@"", new string[] { Util.ExecutableDirectory(), _exe });
            startInfo.Arguments = args.ToString().Length != -1 ? args.ToString() : string.Empty;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;

            using (System.Diagnostics.Process execute = System.Diagnostics.Process.Start(startInfo))
            {
                try
                {
                    execute.EnableRaisingEvents = true;
                    execute.OutputDataReceived += execute_OutputDataReceived;
                    execute.ErrorDataReceived += execute_ErrorDataReceived;
                    execute.Exited += execute_Exited; 
                    execute.Start();
                    execute.BeginErrorReadLine();
                    execute.BeginOutputReadLine();
                 
                }
                finally {
                    execute.WaitForExit();
                }
                return result;
            }
        }

        void execute_Exited(object sender, EventArgs e)
        {
            result.code = 0;
        }

        void execute_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if(e.Data != null)
                result.error = e.Data;
        }

        void execute_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if(e.Data != null) result.output =  e.Data;
        }
    }

    public class GetHardware : IProcess
    {
        public GetHardware()
            : base("HardwareId.exe")
        {
        }
        public override RESULT_PROCESS exec(string[] args)
        {
            return base.exec(args);
        }
    }

    public class ADBBridge : IProcess
    {
        public ADBBridge()
            : base("adb.exe")
        {
        }
        public override RESULT_PROCESS exec(string[] args)
        {
            return base.exec(args);
        }
    }

    public class KILLProcessWindows : IProcess
    {
        public KILLProcessWindows()
            : base("taskkill.exe")
        { 
        }
        public override RESULT_PROCESS exec(string[] args)
        {
            return base.exec(args);
        }
    }

    public class NETShell : IProcess
    {
        public NETShell()
            : base("netsh.exe")
        { }
        public override RESULT_PROCESS exec(string[] args)
        {
            return base.exec(args);
        }
    }

    public class AndroidEMU : IProcess
    {
        public AndroidEMU()
            : base("AndroidEmulator.exe")
        { }
        public override RESULT_PROCESS exec(string[] args)
        {
            return base.exec(args);
        }
    }
}
