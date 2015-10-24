using System;
using System.Diagnostics;
using System.IO;
using BlueToque.Utility;

namespace BlueDiamond.Desktop
{
    class IISExpressController
    {
        public IISExpressController()
        {

        }

        private bool is64Bit { get { return IntPtr.Size == 8; } }

        public string IISPath
        {
            get
            {
                return is64Bit ?
                    @"c:\Program Files\IIS Express\iisexpress.exe" :
                    @"c:\Program Files (x86)\IIS Express\iisexpress.exe";
            }
        }

        private Process IISProcess { get; set; }

        private int InstanceID { get { return IISProcess != null ? IISProcess.Id : -1; } }

        public static string ConfigFile
        {
            get { return Path.Combine(Paths.CommonApplicationData, "applicationhost.config"); }
        }

        public void Start(int port, string virtualDir)
        {
            bool systray = Debugger.IsAttached;
            //string parameters = string.Format(" /path:\"{0}\" /port:{1} /trace:info /systray:{2}", virtualDir, port, systray);
            string parameters = string.Format("/config:\"{0}\" /site:BlueDiamond /trace:info", ConfigFile, systray);

            // start IIS
            ProcessStartInfo psi = new ProcessStartInfo(IISPath, parameters);
            psi.UseShellExecute = false;
            psi.RedirectStandardInput = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;

            if (this.IISProcess != null)
                throw new NotSupportedException("Multiple starts not supported");

            this.IISProcess = new Process();
            this.IISProcess.StartInfo = psi;
            this.IISProcess.ErrorDataReceived += iisProcess_ErrorDataReceived;
            this.IISProcess.OutputDataReceived += iisProcess_OutputDataReceived;
            this.IISProcess.Start();
            this.IISProcess.BeginErrorReadLine();
            this.IISProcess.BeginOutputReadLine();
        }

        void iisProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Trace.TraceInformation(e.Data);
        }

        void iisProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Trace.TraceError(e.Data);
        }

        public void Stop()
        {
            if (this.IISProcess == null)
                throw new Exception("Does not look like there was something started yet!");

            if (this.IISProcess.HasExited)
            {
                Trace.TraceWarning("IIS has already exited with code '{0}'", this.IISProcess.ExitCode);
                this.IISProcess.Close();
                return;
            }

            Trace.TraceInformation("Stopping IIS instance #{0}", this.InstanceID);
            IISProcess.CloseMainWindow();
            //ProcessCommunication.SendStopMessageToProcess(this.iisProcess.Id);
            bool exited = this.IISProcess.WaitForExit(5000);
            if (!exited)
            {
                Trace.TraceWarning("Failed to stop IIS instance #{0} (PID {1}), killing it now", this.InstanceID, this.IISProcess.Id);
                IISProcess.Kill();
            }

            this.IISProcess.Close();
        }
    }
}
