using BlueDiamond.Desktop.Properties;
using BlueToque.Utility;
using BlueDiamond.Utility;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.ComponentModel;

namespace BlueDiamond.Desktop
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            
            // Get the version info & put it into the title bar
            BlueToque.Utility.AssemblyInfo info = new BlueToque.Utility.AssemblyInfo();
            Text = string.Format("{0} - {1}", info.Product, info.Version);

            // The DebugTraceListener hooks into the Trace messages throughout the
            // applicaiton and directs them to the application "console"
            DebugTraceListener.TraceMessage += DebugTraceListener_TraceMessage;
            DebugTraceListener.Start();

            // write out the IIS config file to have the current port and path to the 
            // web server files
            File.WriteAllText(
                IISExpressController.ConfigFilePath,
                Resources.applicationhost
                    .Replace("{port}", Settings.Default.Port.ToString())
                    .Replace("{path}", Path.GetFullPath(Paths.Expand(Settings.Default.ApplicationDirectory)))
            );

            // format the URL to the web server 
            Url = string.Format("http://{0}:{1}/", Environment.MachineName, Settings.Default.Port);

            // show the version number on screen
            myVersionLabel.Text = string.Format("{0} - {1}", info.Product, info.Version);

            // display the web address as an IP address in case name resolution doesn't work on this network
            myIPAddressLabel.Text = string.Format("http://{0}:{1}/", OS.GetIPAddress(), Settings.Default.Port);
            myUrlLabel.Text = Url;

            // Generate the QR code
            myURLPictureBox.Image = GetQRCode(myIPAddressLabel.Text);
        }

        /// <summary>
        /// Url to this web server
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Write all trace messages to the console
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DebugTraceListener_TraceMessage(object sender, TraceEventArgs e)
        {
            WriteLine(e.Message);
        }

        #region overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Trace.TraceInformation("Loading");

            if (DiscoveryServer.FindServer("BlueDiamond", 2000))
            {
                // if the server exists, start a web browser with the URL to the server
            }
            else
            {
                // start the server
                Start();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            
            // this code does nothing
            Uri uri = new Uri(string.Format("http://localhost:{0}{1}",
                Settings.Default.Port,
                Settings.Default.VirtualDirectory));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            // stop the web server
            Stop();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // set the nofication icon when we minimize
            if (FormWindowState.Minimized == this.WindowState)
            {
                myNotifyIcon.Visible = true;
                myNotifyIcon.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                myNotifyIcon.Visible = false;
            }
        }

        #endregion

        IISExpressController m_server;

        #region private methods

        /// <summary>
        /// Start the server
        /// </summary>
        private void Start()
        {
            try
            {
                DiscoveryServer.Start("BlueDiamond");

                // get the full path to "this" directory
                string dir = Paths.Expand(Settings.Default.ApplicationDirectory);
                dir = Path.GetFullPath(dir);

                // create an IIS controller and start it up with the right path
                m_server = new IISExpressController();
                m_server.Start(Settings.Default.Port, dir);
            }
            catch
            {
                ShowError(
                    "Cassini Managed Web Server failed to start listening on port " + Settings.Default.Port + ".\r\n" +
                    "Possible conflict with another Web Server on the same port.");
                return;
            }

        }

        /// <summary>
        /// Stop the server
        /// </summary>
        private void Stop()
        {
            try
            {
                DiscoveryServer.Stop();

                if (m_server != null)
                    m_server.Stop();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error shutting down:\r\n{0}", ex);
            }
            finally
            {
                m_server = null;
            }

            //Close();
        }

        /// <summary>
        /// Show an error dialog
        /// </summary>
        /// <param name="error"></param>
        private void ShowError(string format, params object[] parameters)
        {            
            MessageBox.Show(this, 
                string.Format(format, parameters), 
                "Error", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Error);
        }

        /// <summary>
        /// Write a line to the console in this form
        /// </summary>
        /// <param name="format"></param>
        /// <param name="parameters"></param>
        private void WriteLine(string format, params object[] parameters)
        {
            if (this.IsDisposed)
                return;
            this.InvokeIfRequired(() =>
            {
                myRichTextBox.AppendText(string.Format(format, parameters));
                myRichTextBox.ScrollToCaret();
            });
        }

        /// <summary>
        /// Create QR code from the given string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Image GetQRCode(string value)
        {
            QrEncoder enc = new QrEncoder();
            QrCode code = enc.Encode(value);

            Renderer renderer = new Renderer(5);
            Image image = new Bitmap(256, 256);
            using (Graphics graphics = Graphics.FromImage(image))
                renderer.Draw(graphics, code.Matrix);
            return image;
        }

        #endregion

        #region event handlers

        private void myAuthorLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.BlueToque.ca");   
        }

        private void myUrlLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(myUrlLabel.Text);
        }

        private void myRichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }


        private void myNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
        }
        #endregion


    }
}
