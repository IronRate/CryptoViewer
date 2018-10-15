namespace CryptoViewer.Views
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.cspInfoComponent1 = new CryptoViewer.Components.CSPInfoComponent();
            this.cryptoproviderParamsComponent1 = new CryptoViewer.Components.CryptoproviderParamsComponent();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cryptoAlgorithmsComponent1 = new CryptoViewer.Components.CryptoAlgorithmsComponent();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1133, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(0, 25);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(250, 468);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(250, 25);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 468);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // cspInfoComponent1
            // 
            this.cspInfoComponent1.CSPName = "";
            this.cspInfoComponent1.CSPVersion = "";
            this.cspInfoComponent1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cspInfoComponent1.Location = new System.Drawing.Point(253, 77);
            this.cspInfoComponent1.Name = "cspInfoComponent1";
            this.cspInfoComponent1.Size = new System.Drawing.Size(880, 69);
            this.cspInfoComponent1.TabIndex = 4;
            this.cspInfoComponent1.Visible = false;
            // 
            // cryptoproviderParamsComponent1
            // 
            this.cryptoproviderParamsComponent1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cryptoproviderParamsComponent1.Location = new System.Drawing.Point(253, 25);
            this.cryptoproviderParamsComponent1.Name = "cryptoproviderParamsComponent1";
            this.cryptoproviderParamsComponent1.ProviderName = "";
            this.cryptoproviderParamsComponent1.ProviderType = 0;
            this.cryptoproviderParamsComponent1.Size = new System.Drawing.Size(880, 52);
            this.cryptoproviderParamsComponent1.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(313, 213);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(200, 233);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cryptoAlgorithmsComponent1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(192, 207);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Algorithms";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 207);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Containers";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cryptoAlgorithmsComponent1
            // 
            this.cryptoAlgorithmsComponent1.Algorithms = null;
            this.cryptoAlgorithmsComponent1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cryptoAlgorithmsComponent1.Location = new System.Drawing.Point(3, 3);
            this.cryptoAlgorithmsComponent1.Name = "cryptoAlgorithmsComponent1";
            this.cryptoAlgorithmsComponent1.Size = new System.Drawing.Size(186, 201);
            this.cryptoAlgorithmsComponent1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1133, 493);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cspInfoComponent1);
            this.Controls.Add(this.cryptoproviderParamsComponent1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Splitter splitter1;
        private Components.CryptoproviderParamsComponent cryptoproviderParamsComponent1;
        private Components.CSPInfoComponent cspInfoComponent1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Components.CryptoAlgorithmsComponent cryptoAlgorithmsComponent1;
    }
}