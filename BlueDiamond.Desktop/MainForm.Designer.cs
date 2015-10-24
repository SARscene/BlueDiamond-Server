namespace BlueDiamond.Desktop
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mySplitContainer = new System.Windows.Forms.SplitContainer();
            this.myURLPictureBox = new System.Windows.Forms.PictureBox();
            this.myIPAddressLabel = new System.Windows.Forms.LinkLabel();
            this.myPictureBox = new System.Windows.Forms.PictureBox();
            this.myVersionLabel = new System.Windows.Forms.Label();
            this.myTitleLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.myUrlLabel = new System.Windows.Forms.LinkLabel();
            this.myRichTextBox = new System.Windows.Forms.RichTextBox();
            this.myNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mySplitContainer)).BeginInit();
            this.mySplitContainer.Panel1.SuspendLayout();
            this.mySplitContainer.Panel2.SuspendLayout();
            this.mySplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myURLPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mySplitContainer
            // 
            this.mySplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mySplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mySplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mySplitContainer.Name = "mySplitContainer";
            this.mySplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mySplitContainer.Panel1
            // 
            this.mySplitContainer.Panel1.Controls.Add(this.label2);
            this.mySplitContainer.Panel1.Controls.Add(this.myURLPictureBox);
            this.mySplitContainer.Panel1.Controls.Add(this.myIPAddressLabel);
            this.mySplitContainer.Panel1.Controls.Add(this.myPictureBox);
            this.mySplitContainer.Panel1.Controls.Add(this.myVersionLabel);
            this.mySplitContainer.Panel1.Controls.Add(this.myTitleLabel);
            this.mySplitContainer.Panel1.Controls.Add(this.label1);
            this.mySplitContainer.Panel1.Controls.Add(this.myUrlLabel);
            // 
            // mySplitContainer.Panel2
            // 
            this.mySplitContainer.Panel2.Controls.Add(this.myRichTextBox);
            this.mySplitContainer.Size = new System.Drawing.Size(692, 386);
            this.mySplitContainer.SplitterDistance = 254;
            this.mySplitContainer.TabIndex = 0;
            // 
            // myURLPictureBox
            // 
            this.myURLPictureBox.Location = new System.Drawing.Point(403, 45);
            this.myURLPictureBox.Name = "myURLPictureBox";
            this.myURLPictureBox.Size = new System.Drawing.Size(150, 150);
            this.myURLPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.myURLPictureBox.TabIndex = 7;
            this.myURLPictureBox.TabStop = false;
            // 
            // myIPAddressLabel
            // 
            this.myIPAddressLabel.AutoSize = true;
            this.myIPAddressLabel.Location = new System.Drawing.Point(86, 106);
            this.myIPAddressLabel.Name = "myIPAddressLabel";
            this.myIPAddressLabel.Size = new System.Drawing.Size(110, 16);
            this.myIPAddressLabel.TabIndex = 6;
            this.myIPAddressLabel.TabStop = true;
            this.myIPAddressLabel.Text = "<applicationIP>";
            this.myIPAddressLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.myUrlLabel_LinkClicked);
            // 
            // myPictureBox
            // 
            this.myPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.myPictureBox.Image = global::BlueDiamond.Desktop.Properties.Resources.ICP300;
            this.myPictureBox.Location = new System.Drawing.Point(559, 13);
            this.myPictureBox.Name = "myPictureBox";
            this.myPictureBox.Size = new System.Drawing.Size(121, 115);
            this.myPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.myPictureBox.TabIndex = 5;
            this.myPictureBox.TabStop = false;
            // 
            // myVersionLabel
            // 
            this.myVersionLabel.AutoSize = true;
            this.myVersionLabel.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.myVersionLabel.Location = new System.Drawing.Point(18, 42);
            this.myVersionLabel.Name = "myVersionLabel";
            this.myVersionLabel.Size = new System.Drawing.Size(133, 23);
            this.myVersionLabel.TabIndex = 4;
            this.myVersionLabel.Text = "Version: {0}";
            // 
            // myTitleLabel
            // 
            this.myTitleLabel.AutoSize = true;
            this.myTitleLabel.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.myTitleLabel.Location = new System.Drawing.Point(13, 13);
            this.myTitleLabel.Name = "myTitleLabel";
            this.myTitleLabel.Size = new System.Drawing.Size(544, 29);
            this.myTitleLabel.TabIndex = 2;
            this.myTitleLabel.Text = "Blue Diamond: Simple Incident Management";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Launch:";
            // 
            // myUrlLabel
            // 
            this.myUrlLabel.AutoSize = true;
            this.myUrlLabel.Location = new System.Drawing.Point(86, 81);
            this.myUrlLabel.Name = "myUrlLabel";
            this.myUrlLabel.Size = new System.Drawing.Size(114, 16);
            this.myUrlLabel.TabIndex = 0;
            this.myUrlLabel.TabStop = true;
            this.myUrlLabel.Text = "<applicationUrl>";
            this.myUrlLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.myUrlLabel_LinkClicked);
            // 
            // myRichTextBox
            // 
            this.myRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myRichTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.myRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.myRichTextBox.Name = "myRichTextBox";
            this.myRichTextBox.ReadOnly = true;
            this.myRichTextBox.Size = new System.Drawing.Size(692, 128);
            this.myRichTextBox.TabIndex = 0;
            this.myRichTextBox.Text = "";
            this.myRichTextBox.WordWrap = false;
            this.myRichTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.myRichTextBox_LinkClicked);
            // 
            // myNotifyIcon
            // 
            this.myNotifyIcon.BalloonTipText = "BlueDiamond";
            this.myNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("myNotifyIcon.Icon")));
            this.myNotifyIcon.Text = "BlueDiamond";
            this.myNotifyIcon.Visible = true;
            this.myNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.myNotifyIcon_MouseDoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(403, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Scan with Mobile App";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 386);
            this.Controls.Add(this.mySplitContainer);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "TrueNorth X";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.mySplitContainer.Panel1.ResumeLayout(false);
            this.mySplitContainer.Panel1.PerformLayout();
            this.mySplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mySplitContainer)).EndInit();
            this.mySplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.myURLPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mySplitContainer;
        private System.Windows.Forms.RichTextBox myRichTextBox;
        private System.Windows.Forms.Label myVersionLabel;
        private System.Windows.Forms.Label myTitleLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel myUrlLabel;
        private System.Windows.Forms.PictureBox myPictureBox;
        private System.Windows.Forms.NotifyIcon myNotifyIcon;
        private System.Windows.Forms.LinkLabel myIPAddressLabel;
        private System.Windows.Forms.PictureBox myURLPictureBox;
        private System.Windows.Forms.Label label2;

    }
}

