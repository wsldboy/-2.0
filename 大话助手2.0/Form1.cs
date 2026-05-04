using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using 大话助手2._0;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace 游戏脚本
{
    public partial class Form1 : Form
    {
        //声明全局变量
        
        private const int HotKeyID = 9001;
        // 静态全局实例
        public static Form1 Instance { get; private set; }
        // 脚本运行开关
        private bool isRunning = false;
        // 多开大漠对象列表
        private readonly List<大漠中文> dmList = new List<大漠中文>();
        private readonly List<int> hwndlist = new List<int>();
        // 多开脚本列表
        private readonly List<脚本> scriptList = new List<脚本>();
        public int hp阈值;
        public int mp阈值;
        public int pethp阈值;
        public int petmp阈值;
        private static DateTime lastClickTime = DateTime.MinValue;
        private static DateTime lastClickTime_login = DateTime.MinValue;
        public string 用户登录状态;
        public string 注册码;
        public string 到期时间;
        private DateTime endTime;
        public string 文心token;
        private static wxyz 验证 = new wxyz();
        

        public Form1()
        {
            InitializeComponent();
            InitRoleTable();
            Instance = this; 
            解压资源();
            获取角色();
            文心初始化();
            
        }
       
        #region Win32 API 全局热键
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // 组合键修饰符
        public const uint MOD_ALT = 0x0001;
        public const uint MOD_CTRL = 0x0002;
        public const uint MOD_SHIFT = 0x0004;
        public const uint MOD_WIN = 0x0008;
        
        // 窗体加载：注册热键
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 注册 F11 热键  无组合键
            RegisterHotKey(this.Handle, HotKeyID, 0, (uint)Keys.Home);

        }
        // 接收系统热键消息
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // 热键消息值
            const int WM_HOTKEY = 0x0312;
            if (m.Msg == WM_HOTKEY)
            {
                int keyId = m.WParam.ToInt32();
                switch (keyId)
                {
                    case HotKeyID:
                        // 按下 F1 触发按钮1点击
                        btnStart_Click(null,null);
                        break;

                }
            }
        }

        #endregion


        private void InitRoleTable()
        {
            //初始化表头
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("col1", "序号");
            dataGridView1.Columns.Add("col2", "ID");
            dataGridView1.Columns.Add("col3", "角色血");
            dataGridView1.Columns.Add("col4", "角色法");
            dataGridView1.Columns.Add("col5", "宝宝血");
            dataGridView1.Columns.Add("col6", "宝宝法");


            // 设置每一列的宽度
            dataGridView1.Columns[0].Width = 40;   // 序号
            dataGridView1.Columns[1].Width = 180;  // ID
            dataGridView1.Columns[2].Width = 50;   // 角色血
            dataGridView1.Columns[3].Width = 50;  // 角色法
            dataGridView1.Columns[4].Width = 50;  // 宝宝血
            dataGridView1.Columns[5].Width = 50;  // 宝宝法

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }
      
        public void AddLog(string msg)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => AddLog(msg)));
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\r\n");
            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.ScrollToCaret();
        }

        private void 获取角色()
        {
            大漠中文 dmTemp = null;
            try
            {
                dmList.Clear();
                hwndlist.Clear();
                dataGridView1.Rows.Clear();
                scriptList.Clear();

                dmTemp = new 大漠中文();
                // 枚举游戏窗口（建议配置化窗口标题特征，而非硬编码）
                string ret = dmTemp.枚举窗口(0, "$Revision", "Win32Window", 3);
                if (string.IsNullOrWhiteSpace(ret))
                {
                    AddLog("未检测到游戏窗口");
                    return;
                }

                string[] arr = ret.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries); // 移除空元素
                for (int i = 0; i < arr.Length; i++)
                {
                    // 安全转换句柄
                    if (!int.TryParse(arr[i], out int hwndID))
                    {
                        AddLog($"无效窗口句柄：{arr[i]}，跳过");
                        continue;
                    }

                    // 获取窗口标题并安全分割
                    string title = dmTemp.取窗口标题(hwndID);
                    string ID = "未知";
                    if (!string.IsNullOrWhiteSpace(title))
                    {
                        string[] titleParts = title.Split('-');
                        if (titleParts.Length >= 3)
                        {
                            ID = titleParts[2].Trim(); // 去除空格
                        }
                    }

                    // 添加到集合和表格
                    hwndlist.Add(hwndID);
                    dataGridView1.Rows.Add(i + 1, ID, "", "", "", ""); // 初始化血法列为空
                }

                AddLog($"检测到 {hwndlist.Count} 个游戏窗口");
            }
            catch (Exception ex)
            {
                AddLog($"获取角色失败：{ex.Message}");
            }
            finally
            {
                // 确保大漠对象释放
                if (dmTemp != null)
                {
                    dmTemp.释放();
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // 防重复点击（毫秒转秒更易读）
            if ((DateTime.Now - lastClickTime).TotalSeconds < 4)
            {
                AddLog("操作频繁，请4秒后再试");
                return;
            }
            lastClickTime = DateTime.Now;

            if (!isRunning)
            {
                if (hwndlist.Count == 0)
                {
                    AddLog("未检测到有效游戏角色");
                    return;
                }
                // 校验登录状态（提前提示，避免启动后报错）
                if (string.IsNullOrEmpty(用户登录状态) || 用户登录状态 != "1")
                {
                    AddLog("请先完成登录验证");
                    return;
                }

                isRunning = true;
                btnStart.Text = "■ 停止脚本/Home";
                btnStart.BackColor = Color.Red;
                启动脚本线程();
                AddLog("已启动所有角色脚本");
            }
            else
            {
                isRunning = false;
                btnStart.Text = "▶ 启动脚本/Home";
                btnStart.BackColor = Color.LimeGreen;
                停止脚本();
                AddLog("已停止所有角色脚本");
            }
        }
        //停止所有脚本
        private void 停止脚本()
        {
            foreach (var t in scriptList) t.Stop();
            scriptList.Clear();
        }

        private void 启动脚本线程()
        {

            if (用户登录状态 != "1") {
                MessageBox.Show("您还没有登录");
                return; 
            }
                bool a = ck_无限自动.Checked;
                bool b = ck_自动归队.Checked;
                bool c = ck_自动领双.Checked;
                bool d = ck_医宝宝.Checked;
                bool e = ck_修装备.Checked;
                bool f = ck_自动进队.Checked;
                bool g = ck_副本同意.Checked;
                bool h = ck_角色血法.Checked;
                bool i = ck_宝宝血法.Checked;

                
                for (int o = 0; o < hwndlist.Count; o++)
                {
                    大漠中文 dm = new 大漠中文();
                    var task = new 脚本(dm, hwndlist[o] ,o);
                    task.Start(a, b, c, d, e, f, g, h, i);
                    scriptList.Add(task);                
                }
            
        }
      
        
        private void button_刷新角色_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                MessageBox.Show("请先暂停脚本");
                return;
            }
            获取角色();
        }

        private void 删除该角色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 判断有没有选中行
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // 1. 如果没选中行，直接返回
                if (dataGridView1.SelectedRows.Count == 0)
                    return;

                var row = dataGridView1.SelectedRows[0];
                int num = row.Index;
                // 2. 禁止删除系统自动生成的空白新行（关键！！！）
                if (row.IsNewRow)
                {
                    return;
                }

                // 3. 正常删除
                dataGridView1.Rows.Remove(row);
                hwndlist.RemoveAt(num);
                AddLog("已移除该角色");
                


            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 只处理 右键
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                // 选中当前右键点击的行
                dataGridView1.ClearSelection();
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
        }
     
      
        private void 解压资源() 
        {
            try
            {
                // 1. 定义基础路径，使用Path.Combine避免路径拼接错误
                string publicDir = Environment.GetEnvironmentVariable("PUBLIC");
                string baseDir = Path.Combine(publicDir, "dm");
                string imgDir = Path.Combine(baseDir, "img");

                // 2. 创建目录（递归创建，无需判断是否存在）
                Directory.CreateDirectory(imgDir);

                // 3. 定义要释放的资源列表（避免重复）
                var resourceFiles = new Dictionary<string, byte[]>
        {
            { "dm.dll", 大话助手2._0.Properties.Resources.dm },
            { "dmreg.dll", 大话助手2._0.Properties.Resources.dmreg }
        };
                var imageResources = new Dictionary<string, Image>
        {
            { "大地图.bmp", 大话助手2._0.Properties.Resources.大地图 },
            { "怨气.bmp", 大话助手2._0.Properties.Resources.怨气 },
            { "水墨条.bmp", 大话助手2._0.Properties.Resources.水墨条 },
            { "红木条.bmp", 大话助手2._0.Properties.Resources.红木条 },
            { "归队.bmp", 大话助手2._0.Properties.Resources.归队 },
            { "归队h.bmp", 大话助手2._0.Properties.Resources.归队h },
            { "医宝宝.bmp", 大话助手2._0.Properties.Resources.医宝宝 },
            { "医宝宝h.bmp", 大话助手2._0.Properties.Resources.医宝宝h },
            { "解冻.bmp", 大话助手2._0.Properties.Resources.解冻 },
            { "解冻h.bmp", 大话助手2._0.Properties.Resources.解冻h },
            { "领双.bmp", 大话助手2._0.Properties.Resources.领双 },
            { "领双h.bmp", 大话助手2._0.Properties.Resources.领双h },
            { "修装备.bmp", 大话助手2._0.Properties.Resources.修装备 },
            { "修装备h.bmp", 大话助手2._0.Properties.Resources.修装备h },
            { "修装备_算了.bmp", 大话助手2._0.Properties.Resources.修装备_算了 },
            { "修装备_算了h.bmp", 大话助手2._0.Properties.Resources.修装备_算了h },
            { "组队.bmp", 大话助手2._0.Properties.Resources.组队 },
            { "组队h.bmp", 大话助手2._0.Properties.Resources.组队h },
            { "车夫_取消.bmp", 大话助手2._0.Properties.Resources.车夫_取消 },
            { "车夫_取消h.bmp", 大话助手2._0.Properties.Resources.车夫_取消h },
            { "副本同意.bmp", 大话助手2._0.Properties.Resources.副本同意 },
            { "副本同意h.bmp", 大话助手2._0.Properties.Resources.副本同意h }
        };

                // 4. 释放DLL文件
                foreach (var file in resourceFiles)
                {
                    string filePath = Path.Combine(baseDir, file.Key);
                    // 先删除（如果存在且不是正在使用）
                    if (File.Exists(filePath))
                    {
                        try { File.Delete(filePath); }
                        catch (IOException) { AddLog($"警告：{file.Key} 被占用，跳过删除"); }
                    }
                    // 写入文件
                    File.WriteAllBytes(filePath, file.Value);
                    //AddLog($"已释放：{filePath}");
                }

                // 5. 释放图片资源
                foreach (var img in imageResources)
                {
                    string imgPath = Path.Combine(imgDir, img.Key);
                    // 图片需要先释放原有文件
                    if (File.Exists(imgPath))
                    {
                        try { File.Delete(imgPath); }
                        catch (IOException) { AddLog($"警告：{img.Key} 被占用，跳过删除"); }
                    }
                    img.Value.Save(imgPath);
                    //AddLog($"已释放图片：{imgPath}");
                }
            }
            catch (Exception ex)
            {
                AddLog($"资源解压失败：{ex.Message}");
                MessageBox.Show($"资源解压失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox_hp_SelectedIndexChanged(object sender, EventArgs e)
        {
            hp阈值 = int.Parse(comboBox_hp.Text);
        }

        private void comboBox_mp_SelectedIndexChanged(object sender, EventArgs e)
        {
            mp阈值 = int.Parse(comboBox_mp.Text);
        }

        private void comboBox_pethp_SelectedIndexChanged(object sender, EventArgs e)
        {
            pethp阈值 = int.Parse(comboBox_pethp.Text);
        }

        private void comboBox_petmp_SelectedIndexChanged(object sender, EventArgs e)
        {
            petmp阈值 = int.Parse(comboBox_petmp.Text);
        }


        void 保存配置()
        {
            try
            {
                var settings = 大话助手2._0.Properties.Settings.Default;
                // 复选框配置
                settings.ck_无限自动 = ck_无限自动.Checked;
                settings.ck_自动归队 = ck_自动归队.Checked;
                settings.ck_自动领双 = ck_自动领双.Checked;
                settings.ck_副本同意 = ck_副本同意.Checked;
                settings.ck_修装备 = ck_修装备.Checked;
                settings.ck_医宝宝 = ck_医宝宝.Checked;
                settings.ck_自动进队 = ck_自动进队.Checked;
                settings.ck_角色血法 = ck_角色血法.Checked;
                settings.ck_宝宝血法 = ck_宝宝血法.Checked;

                // ComboBox配置（校验索引有效性）
                settings.comboBox_hp = comboBox_hp.SelectedIndex >= 0 ? comboBox_hp.SelectedIndex : 0;
                settings.comboBox_mp = comboBox_mp.SelectedIndex >= 0 ? comboBox_mp.SelectedIndex : 0;
                settings.comboBox_pethp = comboBox_pethp.SelectedIndex >= 0 ? comboBox_pethp.SelectedIndex : 0;
                settings.comboBox_petmp = comboBox_petmp.SelectedIndex >= 0 ? comboBox_petmp.SelectedIndex : 0;

                // 文本框配置
                settings.textBox_注册码 = textBox_注册码.Text.Trim();
                settings.用户登录状态 = string.IsNullOrWhiteSpace(用户登录状态) ? "0" : 用户登录状态;

                // 保存配置（自动创建备份）
                settings.Save();
                AddLog("配置已保存");
            }
            catch (Exception ex)
            {
                AddLog($"保存配置失败：{ex.Message}");
                MessageBox.Show($"保存配置失败：{ex.Message}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            保存配置();
            isRunning = false;
            UnregisterHotKey(this.Handle, HotKeyID);
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var settings = 大话助手2._0.Properties.Settings.Default;
                // 读取复选框
                ck_无限自动.Checked = settings.ck_无限自动;
                ck_自动归队.Checked = settings.ck_自动归队;
                ck_自动领双.Checked = settings.ck_自动领双;
                ck_副本同意.Checked = settings.ck_副本同意;
                ck_修装备.Checked = settings.ck_修装备;
                ck_医宝宝.Checked = settings.ck_医宝宝;
                ck_自动进队.Checked = settings.ck_自动进队;
                ck_角色血法.Checked = settings.ck_角色血法;
                ck_宝宝血法.Checked = settings.ck_宝宝血法;

                // 读取ComboBox（校验索引范围）
                comboBox_hp.SelectedIndex = Math.Max(0, Math.Min(settings.comboBox_hp, comboBox_hp.Items.Count - 1));
                comboBox_mp.SelectedIndex = Math.Max(0, Math.Min(settings.comboBox_mp, comboBox_mp.Items.Count - 1));
                comboBox_pethp.SelectedIndex = Math.Max(0, Math.Min(settings.comboBox_pethp, comboBox_pethp.Items.Count - 1));
                comboBox_petmp.SelectedIndex = Math.Max(0, Math.Min(settings.comboBox_petmp, comboBox_petmp.Items.Count - 1));

                // 读取注册码
                textBox_注册码.Text = settings.textBox_注册码 ?? "";

                // 自动登录
                string 登录状态 = settings.用户登录状态 ?? "0";
                if (登录状态 == "1" && !string.IsNullOrWhiteSpace(textBox_注册码.Text))
                {
                    AddLog("自动登录中...");
                    await 执行登录();
                }
                else
                {
                    btnStart.Enabled = false; // 未登录禁用启动按钮
                }
            }
            catch (Exception ex)
            {
                AddLog($"加载配置失败：{ex.Message}，使用默认配置");
                // 重置默认配置
                comboBox_hp.SelectedIndex = 0;
                comboBox_mp.SelectedIndex = 0;
                comboBox_pethp.SelectedIndex = 0;
                comboBox_petmp.SelectedIndex = 0;
            }
        }


        private static void 文心初始化()
        {

            string softid = "5E7S5I1O4P8J5P1G";
            string 加密参数 = "1V0F1Q0H6B0F";
            string 解密参数 = "9H2B4X1A5Q1D";

            int 线路 = 0;
            验证.验证初始化(softid, 加密参数, 1, 解密参数, 1, 线路);//参数改成自己的.


        }

        private async Task 执行登录()
        {
            if ((DateTime.Now - lastClickTime_login).TotalSeconds < 3)
            {
                AddLog("登录操作频繁，请3秒后再试");
                return;
            }
            lastClickTime_login = DateTime.Now;

            string 卡密 = textBox_注册码.Text.Trim();
            if (string.IsNullOrWhiteSpace(卡密))
            {
                
                return;
            }

            try
            {
                AddLog("正在验证注册码...");
                string MAC地址 = 验证.GetLocalMac();
                string ret = await 验证.卡密登录(MAC地址, 卡密, "1.0");

                if (ret.Length == 16)
                {
                    文心token = ret;
                    注册码 = 卡密;
                    到期时间 = await 验证.取到期时间(注册码);
                    验证.监视用户状态(注册码, 文心token);

                    // 更新登录状态
                    用户登录状态 = "1";
                    button_登录.Enabled = false;
                    btnStart.Enabled = true;
                    显示到期时间(到期时间);

                    AddLog($"登录成功！到期时间：{到期时间}");
                    await 验证.到期自动结束();
                }
                else
                {
                    string 错误信息 = 验证.错误码对照(ret);
                    AddLog($"登录失败：{错误信息}");
                   
                }
            }
            catch (Exception ex)
            {
                AddLog($"登录过程异常：{ex.Message}");
               
            }
        }

        private async void button_登录_Click(object sender, EventArgs e)
        {
           await 执行登录();
        }

        private async void button_解绑_Click(object sender, EventArgs e)
        {
            string MAC地址 = 验证.GetLocalMac();
            string ret = await 验证.用户转绑(MAC地址, textBox_注册码.Text, "1");
            if (ret == "1")
            {
                MessageBox.Show("解绑成功,可以登录了");
            }
            else
            {                
                AddLog(验证.错误码对照(ret));                
            }
        }
       

        private void 显示到期时间(string time)
        {
            
            DateTime.TryParse(time, out  endTime);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            if (now >= endTime)
            {
                
                timer1.Stop();
                return;
            }

            TimeSpan ts = endTime - now;
            // 显示：剩余 天 时:分:秒
            label_剩余时间.Text = $"剩余时间：{ts.Days}天 {ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
        }
    }


}
    
