using System;
using System.Windows.Forms;

namespace BlueDiamond.Desktop
{
    public partial class BrowserForm : Form
    {
        public BrowserForm()
        {
            InitializeComponent();
            myExtendedWebBrowser1.ScriptErrorsSuppressed = true;
        }

        public Uri Address
        {
            get { return myExtendedWebBrowser1.Url; }
            set { myExtendedWebBrowser1.Url = value; }
        }
    }
}
