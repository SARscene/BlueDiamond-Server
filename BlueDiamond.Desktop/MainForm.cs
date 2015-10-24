using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using BlueToque.Utility;
using BlueDiamond.Desktop.Properties;
using System.Net;
using System.Net.Sockets;
using Zen.Barcode;
using System.Drawing;

namespace BlueDiamond.Desktop
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            BlueToque.Utility.AssemblyInfo info = new BlueToque.Utility.AssemblyInfo();
            Text = string.Format("{0} - {1}", info.Product, info.Version);
            DebugTraceListener.TraceMessage += DebugTraceListener_TraceMessage;
            DebugTraceListener.Start();

            File.WriteAllText(
                IISExpressController.ConfigFile,
                Resources.applicationhost
                    .Replace("{port}", Settings.Default.Port.ToString())
                    .Replace("{path}", Path.GetFullPath(Paths.Expand(Settings.Default.ApplicationDirectory)))
            );

            Url = string.Format("http://{0}:{1}/", Environment.MachineName, Settings.Default.Port);
            myVersionLabel.Text = string.Format("{0} - {1}", info.Product, info.Version);

            string ipAddress = GetIPAddress();
            myIPAddressLabel.Text = string.Format("http://{0}:{1}/", GetIPAddress(), Settings.Default.Port);
            myUrlLabel.Text = Url;

            CodeQrBarcodeDraw bdf = BarcodeDrawFactory.CodeQr;
            myURLPictureBox.Image = bdf.Draw(myIPAddressLabel.Text, 10, 10);

        }

        public string Url { get; set; }

        void DebugTraceListener_TraceMessage(object sender, TraceEventArgs e)
        {
            WriteLine(e.Message);
        }

        string GetIPAddress()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    localIP = ip.ToString();
            }
            return localIP;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Trace.TraceInformation("Loading");
            Start();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Uri uri = new Uri(string.Format("http://localhost:{0}{1}",
                Settings.Default.Port,
                Settings.Default.VirtualDirectory));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }

        IISExpressController m_server;

        void Start()
        {
            try
            {

                string dir = Paths.Expand(Settings.Default.ApplicationDirectory);
                dir = Path.GetFullPath(dir);
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

        private void ShowError(string error)
        {
            
            MessageBox.Show(this, error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void WriteLine(string format, params object[] parameters)
        {
            myRichTextBox.AppendText(string.Format(format, parameters));
            myRichTextBox.ScrollToCaret();
        }

        void Stop()
        {
            try
            {
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

        private void MainForm_Resize(object sender, EventArgs e)
        {
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

        private void myNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
        }

      
    }
}
