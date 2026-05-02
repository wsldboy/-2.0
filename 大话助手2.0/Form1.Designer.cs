namespace 游戏脚本
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region 窗体设计器生成的代码
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnStart = new System.Windows.Forms.Button();
            this.ck_无限自动 = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除该角色ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ck_宝宝血法 = new System.Windows.Forms.CheckBox();
            this.ck_自动进队 = new System.Windows.Forms.CheckBox();
            this.ck_副本同意 = new System.Windows.Forms.CheckBox();
            this.ck_角色血法 = new System.Windows.Forms.CheckBox();
            this.ck_修装备 = new System.Windows.Forms.CheckBox();
            this.ck_自动归队 = new System.Windows.Forms.CheckBox();
            this.ck_自动领双 = new System.Windows.Forms.CheckBox();
            this.ck_医宝宝 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_刷新角色 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox_petmp = new System.Windows.Forms.ComboBox();
            this.comboBox_pethp = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBox_mp = new System.Windows.Forms.ComboBox();
            this.comboBox_hp = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_注册码 = new System.Windows.Forms.TextBox();
            this.button_登录 = new System.Windows.Forms.Button();
            this.button_解绑 = new System.Windows.Forms.Button();
            this.label_剩余时间 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.LimeGreen;
            this.btnStart.Enabled = false;
            this.btnStart.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(43, 390);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(160, 40);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "▶ 启动脚本/Home";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // ck_无限自动
            // 
            this.ck_无限自动.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.ck_无限自动.Location = new System.Drawing.Point(20, 22);
            this.ck_无限自动.Name = "ck_无限自动";
            this.ck_无限自动.Size = new System.Drawing.Size(85, 35);
            this.ck_无限自动.TabIndex = 0;
            this.ck_无限自动.Text = "无限自动";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 34;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(247, 15);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 28;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(420, 210);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除该角色ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 26);
            // 
            // 删除该角色ToolStripMenuItem
            // 
            this.删除该角色ToolStripMenuItem.Name = "删除该角色ToolStripMenuItem";
            this.删除该角色ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.删除该角色ToolStripMenuItem.Text = "移除该角色";
            this.删除该角色ToolStripMenuItem.Click += new System.EventHandler(this.删除该角色ToolStripMenuItem_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(247, 265);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(420, 165);
            this.txtLog.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ck_宝宝血法);
            this.groupBox1.Controls.Add(this.ck_自动进队);
            this.groupBox1.Controls.Add(this.ck_副本同意);
            this.groupBox1.Controls.Add(this.ck_角色血法);
            this.groupBox1.Controls.Add(this.ck_修装备);
            this.groupBox1.Controls.Add(this.ck_无限自动);
            this.groupBox1.Controls.Add(this.ck_自动归队);
            this.groupBox1.Controls.Add(this.ck_自动领双);
            this.groupBox1.Controls.Add(this.ck_医宝宝);
            this.groupBox1.Location = new System.Drawing.Point(13, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(219, 210);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "功能设置";
            // 
            // ck_宝宝血法
            // 
            this.ck_宝宝血法.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.ck_宝宝血法.Location = new System.Drawing.Point(110, 123);
            this.ck_宝宝血法.Name = "ck_宝宝血法";
            this.ck_宝宝血法.Size = new System.Drawing.Size(102, 42);
            this.ck_宝宝血法.TabIndex = 8;
            this.ck_宝宝血法.Text = "宝宝血法";
            // 
            // ck_自动进队
            // 
            this.ck_自动进队.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.ck_自动进队.Location = new System.Drawing.Point(110, 18);
            this.ck_自动进队.Name = "ck_自动进队";
            this.ck_自动进队.Size = new System.Drawing.Size(90, 42);
            this.ck_自动进队.TabIndex = 5;
            this.ck_自动进队.Text = "自动进队";
            // 
            // ck_副本同意
            // 
            this.ck_副本同意.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.ck_副本同意.Location = new System.Drawing.Point(110, 53);
            this.ck_副本同意.Name = "ck_副本同意";
            this.ck_副本同意.Size = new System.Drawing.Size(90, 42);
            this.ck_副本同意.TabIndex = 6;
            this.ck_副本同意.Text = "副本同意";
            // 
            // ck_角色血法
            // 
            this.ck_角色血法.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.ck_角色血法.Location = new System.Drawing.Point(110, 88);
            this.ck_角色血法.Name = "ck_角色血法";
            this.ck_角色血法.Size = new System.Drawing.Size(90, 42);
            this.ck_角色血法.TabIndex = 7;
            this.ck_角色血法.Text = "角色血法";
            // 
            // ck_修装备
            // 
            this.ck_修装备.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.ck_修装备.Location = new System.Drawing.Point(20, 158);
            this.ck_修装备.Name = "ck_修装备";
            this.ck_修装备.Size = new System.Drawing.Size(85, 42);
            this.ck_修装备.TabIndex = 4;
            this.ck_修装备.Text = "修装备";
            // 
            // ck_自动归队
            // 
            this.ck_自动归队.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.ck_自动归队.Location = new System.Drawing.Point(20, 53);
            this.ck_自动归队.Name = "ck_自动归队";
            this.ck_自动归队.Size = new System.Drawing.Size(85, 42);
            this.ck_自动归队.TabIndex = 1;
            this.ck_自动归队.Text = "自动归队";
            // 
            // ck_自动领双
            // 
            this.ck_自动领双.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.ck_自动领双.Location = new System.Drawing.Point(20, 88);
            this.ck_自动领双.Name = "ck_自动领双";
            this.ck_自动领双.Size = new System.Drawing.Size(85, 42);
            this.ck_自动领双.TabIndex = 2;
            this.ck_自动领双.Text = "自动领双";
            // 
            // ck_医宝宝
            // 
            this.ck_医宝宝.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.ck_医宝宝.Location = new System.Drawing.Point(20, 123);
            this.ck_医宝宝.Name = "ck_医宝宝";
            this.ck_医宝宝.Size = new System.Drawing.Size(85, 42);
            this.ck_医宝宝.TabIndex = 3;
            this.ck_医宝宝.Text = "医宝宝";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // button_刷新角色
            // 
            this.button_刷新角色.Location = new System.Drawing.Point(43, 447);
            this.button_刷新角色.Name = "button_刷新角色";
            this.button_刷新角色.Size = new System.Drawing.Size(160, 37);
            this.button_刷新角色.TabIndex = 5;
            this.button_刷新角色.Text = "刷新角色";
            this.button_刷新角色.UseVisualStyleBackColor = true;
            this.button_刷新角色.Click += new System.EventHandler(this.button_刷新角色_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox_petmp);
            this.groupBox2.Controls.Add(this.comboBox_pethp);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(33, 309);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(183, 54);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "宝宝";
            // 
            // comboBox_petmp
            // 
            this.comboBox_petmp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_petmp.FormattingEnabled = true;
            this.comboBox_petmp.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90"});
            this.comboBox_petmp.Location = new System.Drawing.Point(120, 19);
            this.comboBox_petmp.Name = "comboBox_petmp";
            this.comboBox_petmp.Size = new System.Drawing.Size(43, 20);
            this.comboBox_petmp.TabIndex = 4;
            this.comboBox_petmp.SelectedIndexChanged += new System.EventHandler(this.comboBox_petmp_SelectedIndexChanged);
            // 
            // comboBox_pethp
            // 
            this.comboBox_pethp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_pethp.FormattingEnabled = true;
            this.comboBox_pethp.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90"});
            this.comboBox_pethp.Location = new System.Drawing.Point(39, 20);
            this.comboBox_pethp.Name = "comboBox_pethp";
            this.comboBox_pethp.Size = new System.Drawing.Size(43, 20);
            this.comboBox_pethp.TabIndex = 3;
            this.comboBox_pethp.SelectedIndexChanged += new System.EventHandler(this.comboBox_pethp_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(97, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "法";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "血";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBox_mp);
            this.groupBox3.Controls.Add(this.comboBox_hp);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(33, 240);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(183, 54);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "角色";
            // 
            // comboBox_mp
            // 
            this.comboBox_mp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_mp.FormattingEnabled = true;
            this.comboBox_mp.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90"});
            this.comboBox_mp.Location = new System.Drawing.Point(120, 20);
            this.comboBox_mp.Name = "comboBox_mp";
            this.comboBox_mp.Size = new System.Drawing.Size(43, 20);
            this.comboBox_mp.TabIndex = 3;
            this.comboBox_mp.SelectedIndexChanged += new System.EventHandler(this.comboBox_mp_SelectedIndexChanged);
            // 
            // comboBox_hp
            // 
            this.comboBox_hp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_hp.FormattingEnabled = true;
            this.comboBox_hp.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90"});
            this.comboBox_hp.Location = new System.Drawing.Point(38, 20);
            this.comboBox_hp.Name = "comboBox_hp";
            this.comboBox_hp.Size = new System.Drawing.Size(43, 20);
            this.comboBox_hp.TabIndex = 2;
            this.comboBox_hp.SelectedIndexChanged += new System.EventHandler(this.comboBox_hp_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(97, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "法";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "血";
            // 
            // textBox_注册码
            // 
            this.textBox_注册码.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_注册码.Location = new System.Drawing.Point(247, 455);
            this.textBox_注册码.Name = "textBox_注册码";
            this.textBox_注册码.Size = new System.Drawing.Size(245, 29);
            this.textBox_注册码.TabIndex = 8;
            this.textBox_注册码.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_登录
            // 
            this.button_登录.Location = new System.Drawing.Point(520, 455);
            this.button_登录.Name = "button_登录";
            this.button_登录.Size = new System.Drawing.Size(62, 29);
            this.button_登录.TabIndex = 9;
            this.button_登录.Text = "登录";
            this.button_登录.UseVisualStyleBackColor = true;
            this.button_登录.Click += new System.EventHandler(this.button_登录_Click);
            // 
            // button_解绑
            // 
            this.button_解绑.Location = new System.Drawing.Point(605, 455);
            this.button_解绑.Name = "button_解绑";
            this.button_解绑.Size = new System.Drawing.Size(62, 29);
            this.button_解绑.TabIndex = 10;
            this.button_解绑.Text = "解绑";
            this.button_解绑.UseVisualStyleBackColor = true;
            this.button_解绑.Click += new System.EventHandler(this.button_解绑_Click);
            // 
            // label_剩余时间
            // 
            this.label_剩余时间.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_剩余时间.Location = new System.Drawing.Point(276, 234);
            this.label_剩余时间.Name = "label_剩余时间";
            this.label_剩余时间.Size = new System.Drawing.Size(371, 23);
            this.label_剩余时间.TabIndex = 4;
            this.label_剩余时间.Text = "登录后解锁脚本";
            this.label_剩余时间.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(677, 498);
            this.Controls.Add(this.label_剩余时间);
            this.Controls.Add(this.button_解绑);
            this.Controls.Add(this.button_登录);
            this.Controls.Add(this.textBox_注册码);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button_刷新角色);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "xy2助手2.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.CheckBox ck_无限自动;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ck_自动归队;
        private System.Windows.Forms.CheckBox ck_自动领双;
        private System.Windows.Forms.CheckBox ck_医宝宝;
        private System.Windows.Forms.CheckBox ck_修装备;
        private System.Windows.Forms.CheckBox ck_宝宝血法;
        private System.Windows.Forms.CheckBox ck_自动进队;
        private System.Windows.Forms.CheckBox ck_副本同意;
        private System.Windows.Forms.CheckBox ck_角色血法;
        private System.Windows.Forms.Button button_刷新角色;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 删除该角色ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.ComboBox comboBox_petmp;
        public System.Windows.Forms.ComboBox comboBox_pethp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.ComboBox comboBox_mp;
        public System.Windows.Forms.ComboBox comboBox_hp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox_注册码;
        private System.Windows.Forms.Button button_登录;
        private System.Windows.Forms.Button button_解绑;
        private System.Windows.Forms.Label label_剩余时间;
        private System.Windows.Forms.Timer timer1;
    }
}