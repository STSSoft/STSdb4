namespace STSdb4.Server
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
            this.MinimizeTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.TabControl = new System.Windows.Forms.TabControl();
            this.Connections = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.usersList = new System.Windows.Forms.ListView();
            this.User = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Disconnect = new System.Windows.Forms.Button();
            this.Exception = new System.Windows.Forms.TabPage();
            this.errorList = new System.Windows.Forms.ListView();
            this.Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Error = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.iconsList = new System.Windows.Forms.ImageList(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TabControl.SuspendLayout();
            this.Connections.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.Exception.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MinimizeTray
            // 
            this.MinimizeTray.BalloonTipText = "STSdb4 Server";
            this.MinimizeTray.Icon = ((System.Drawing.Icon)(resources.GetObject("MinimizeTray.Icon")));
            this.MinimizeTray.Text = "STSdb4 Server";
            this.MinimizeTray.Visible = true;
            this.MinimizeTray.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.Connections);
            this.TabControl.Controls.Add(this.Exception);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.ImageList = this.iconsList;
            this.TabControl.Location = new System.Drawing.Point(0, 24);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(478, 417);
            this.TabControl.TabIndex = 0;
            // 
            // Connections
            // 
            this.Connections.Controls.Add(this.tableLayoutPanel1);
            this.Connections.ImageIndex = 0;
            this.Connections.Location = new System.Drawing.Point(4, 23);
            this.Connections.Name = "Connections";
            this.Connections.Padding = new System.Windows.Forms.Padding(3);
            this.Connections.Size = new System.Drawing.Size(470, 390);
            this.Connections.TabIndex = 1;
            this.Connections.Text = "Connections";
            this.Connections.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.08621F));
            this.tableLayoutPanel1.Controls.Add(this.usersList, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(464, 384);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // usersList
            // 
            this.usersList.AutoArrange = false;
            this.usersList.BackColor = System.Drawing.SystemColors.Window;
            this.usersList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.User});
            this.usersList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usersList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.usersList.Location = new System.Drawing.Point(3, 3);
            this.usersList.Name = "usersList";
            this.usersList.Size = new System.Drawing.Size(458, 347);
            this.usersList.TabIndex = 2;
            this.usersList.UseCompatibleStateImageBehavior = false;
            this.usersList.View = System.Windows.Forms.View.Details;
            this.usersList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.usersList_ItemSelectionChanged);
            // 
            // User
            // 
            this.User.Text = "User";
            this.User.Width = this.Width;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.90393F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 82.09607F));
            this.tableLayoutPanel2.Controls.Add(this.Disconnect, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 356);
            this.tableLayoutPanel2.MaximumSize = new System.Drawing.Size(458, 29);
            this.tableLayoutPanel2.MinimumSize = new System.Drawing.Size(458, 29);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(458, 29);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // Disconnect
            // 
            this.Disconnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Disconnect.Enabled = false;
            this.Disconnect.Location = new System.Drawing.Point(3, 3);
            this.Disconnect.MaximumSize = new System.Drawing.Size(75, 23);
            this.Disconnect.MinimumSize = new System.Drawing.Size(75, 23);
            this.Disconnect.Name = "Disconnect";
            this.Disconnect.Size = new System.Drawing.Size(75, 23);
            this.Disconnect.TabIndex = 0;
            this.Disconnect.Text = "Disconnect";
            this.Disconnect.UseVisualStyleBackColor = true;
            this.Disconnect.Click += new System.EventHandler(this.Disconnect_Click);
            // 
            // Exception
            // 
            this.Exception.Controls.Add(this.errorList);
            this.Exception.ImageIndex = 1;
            this.Exception.Location = new System.Drawing.Point(4, 23);
            this.Exception.Name = "Exception";
            this.Exception.Padding = new System.Windows.Forms.Padding(3);
            this.Exception.Size = new System.Drawing.Size(470, 390);
            this.Exception.TabIndex = 0;
            this.Exception.Text = "Exception";
            this.Exception.UseVisualStyleBackColor = true;
            // 
            // errorList
            // 
            this.errorList.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.errorList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Time,
            this.Error});
            this.errorList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorList.FullRowSelect = true;
            this.errorList.Location = new System.Drawing.Point(3, 3);
            this.errorList.Name = "errorList";
            this.errorList.Size = new System.Drawing.Size(464, 384);
            this.errorList.TabIndex = 0;
            this.errorList.Tag = "";
            this.errorList.UseCompatibleStateImageBehavior = false;
            this.errorList.View = System.Windows.Forms.View.Details;
            // 
            // Time
            // 
            this.Time.Tag = "";
            this.Time.Text = "Time";
            this.Time.Width = 120;
            // 
            // Error
            // 
            this.Error.Tag = "";
            this.Error.Text = "Error";
            this.Error.Width = 340;
            // 
            // iconsList
            // 
            this.iconsList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconsList.ImageStream")));
            this.iconsList.TransparentColor = System.Drawing.Color.Transparent;
            this.iconsList.Images.SetKeyName(0, "MSNLExtRes.dll #186.ico");
            this.iconsList.Images.SetKeyName(1, "msdxm.ocx #5295.ico");
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 3000;
            this.timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.serverToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(478, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // serverToolStripMenuItem
            // 
            this.serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem});
            this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            this.serverToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.serverToolStripMenuItem.Text = "Server";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 441);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(494, 476);
            this.Name = "MainForm";
            this.Text = "STSdb4Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
            this.Resize += new System.EventHandler(this.ServerForm_Resize);
            this.TabControl.ResumeLayout(false);
            this.Connections.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.Exception.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon MinimizeTray;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage Connections;
        private System.Windows.Forms.Button Disconnect;
        private System.Windows.Forms.ListView usersList;
        private System.Windows.Forms.ColumnHeader User;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ImageList iconsList;
        private System.Windows.Forms.TabPage Exception;
        private System.Windows.Forms.ListView errorList;
        internal System.Windows.Forms.ColumnHeader Time;
        internal System.Windows.Forms.ColumnHeader Error;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
    }
}