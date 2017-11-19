namespace BlueDiamond.Desktop
{
    partial class BrowserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowserForm));
            this.myExtendedWebBrowser1 = new BlueToque.WebBrowser2.ExtendedWebBrowser();
            this.SuspendLayout();
            // 
            // extendedWebBrowser1
            // 
            this.myExtendedWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myExtendedWebBrowser1.Location = new System.Drawing.Point(0, 0);
            this.myExtendedWebBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.myExtendedWebBrowser1.Name = "extendedWebBrowser1";
            this.myExtendedWebBrowser1.Size = new System.Drawing.Size(534, 431);
            this.myExtendedWebBrowser1.TabIndex = 0;
            // 
            // BrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 431);
            this.Controls.Add(this.myExtendedWebBrowser1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BrowserForm";
            this.Text = "Blue Diamond";
            this.ResumeLayout(false);

        }

        #endregion

        private BlueToque.WebBrowser2.ExtendedWebBrowser myExtendedWebBrowser1;
    }
}