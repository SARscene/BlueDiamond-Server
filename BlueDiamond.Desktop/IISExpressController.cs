using BlueDiamond.Utility;
using BlueToque.Utility;
using System;
using System.Diagnostics;
using System.IO;

namespace BlueDiamond.Desktop
{
    /// <summary>
    /// This class encapsulates the configuration of the IISExpress server
    /// </summary>
    class IISExpressController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public IISExpressController()
        {

        }

        #region properties
  
        /// <summary>
        /// the path to the IISExpress executable
        /// depends on whether this is a 64bit execuable or not
        /// </summary>
        public string IISPath
        {
            get
            {
                return OS.Is64Bit ?
                    @"c:\Program Files\IIS Express\iisexpress.exe" :
                    @"c:\Program Files (x86)\IIS Express\iisexpress.exe";
            }
        }

        /// <summary>
        /// the handle to the IISExpress process
        /// </summary>
        private Process IISProcess { get; set; }

        /// <summary>
        /// The IIS Process instance ID
        /// </summary>
        private int InstanceID { get { return IISProcess != null ? IISProcess.Id : -1; } }

        /// <summary>
        /// the Configuration file path
        /// </summary>
        public static string ConfigFilePath
        {
            get { return Path.Combine(Paths.CommonApplicationData, "applicationhost.config"); }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Start the IIS Server with the given port and directory
        /// </summary>
        /// <param name="port">The port to bind to</param>
        /// <param name="virtualDir">The path to the IIS Virtual directory</param>
        public void Start(int port, string virtualDir)
        {
            bool systray = Debugger.IsAttached;
            //string parameters = string.Format(" /path:\"{0}\" /port:{1} /trace:info /systray:{2}", virtualDir, port, systray);
            string parameters = string.Format("/config:\"{0}\" /site:BlueDiamond /trace:info", ConfigFilePath, systray);

            // create the IIS ProcessStartInfo
            // these settings make sure we can capture the trace events from the sub-process
            ProcessStartInfo psi = new ProcessStartInfo(IISPath, parameters)
            {
                UseShellExecute = false,
                RedirectStandardInput = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            if (this.IISProcess != null)
                throw new NotSupportedException("Multiple starts not supported");

            // start the process, hooking the error and output events so trace information can be captured
            this.IISProcess = new Process();
            this.IISProcess.StartInfo = psi;
            this.IISProcess.ErrorDataReceived += iisProcess_ErrorDataReceived;
            this.IISProcess.OutputDataReceived += iisProcess_OutputDataReceived;
            this.IISProcess.Start();
            this.IISProcess.BeginErrorReadLine();
            this.IISProcess.BeginOutputReadLine();
        }

        /// <summary>
        /// Stop the IIE Express process
        /// </summary>
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

        #endregion

        #region private

        void iisProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Trace.TraceInformation(e.Data);
        }

        void iisProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Trace.TraceError(e.Data);
        }

        #endregion
    }
}
