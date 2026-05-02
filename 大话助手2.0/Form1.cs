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
        #region
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
        #endregion

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


        //表格初始化,日志输出,获取游戏角色
        #region 
        /// <summary>
        /// 初始化表格
        /// </summary>
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
        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="msg">要输出的内容</param>
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
        

        
        // 自动查找游戏窗口
        private void 获取角色()
        {
            try
            {
                dmList.Clear();
                hwndlist.Clear();
                dataGridView1.Rows.Clear();
                scriptList.Clear();
                大漠中文 dmTemp = new 大漠中文();                

                // 这里修改为你的游戏窗口标题
                string ret = dmTemp.枚举窗口(0, "$Revision", "Win32Window", 3);
                //string ret = dmTemp.枚举窗口(0, "$Revision", "Notepad", 3);
                if (string.IsNullOrWhiteSpace(ret)) return;
                //arr是枚举出来的文本型句柄数组
                string[] arr = ret.Split(',');
                //根据句柄数组的长度计次循环
                for (int i = 0; i < arr.Length; i++)
                {
                    //文本句柄变成整数加入到全局变量list<hwnds>里面
                    int hwndID = int.Parse(arr[i]);
                    hwndlist.Add(hwndID);
                    //根据单个句柄取到的标题,分割出来取到ID
                    string title = dmTemp.取窗口标题(hwndID);
                    string ID = title.Split('-') is var a && a.Length >= 3 ? a[2] : "未知";
                    //添加到表格里
                    dataGridView1.Rows.Add(i + 1, ID);                  

                }

                AddLog($"检测到 {hwndlist.Count} 个游戏窗口");
                dmTemp.释放();
            }
            catch (Exception ex)
            {
                AddLog("错误：" + ex.Message);
            }
        }
        #endregion

        // 启动/停止按钮
        #region
        private void btnStart_Click(object sender, EventArgs e)
        {
            // 间隔是否小于3秒
            if ((DateTime.Now - lastClickTime).TotalMilliseconds < 4000)
            {
                return; // 3秒内重复点击直接返回
            }

            // 记录本次点击时间
            lastClickTime = DateTime.Now;
            if (!isRunning)
            {
                if (hwndlist.Count == 0)
                {
                    MessageBox.Show("没有角色");
                    return;
                }

                isRunning = true;
                btnStart.Text = "■ 停止脚本/Home";
                btnStart.BackColor = System.Drawing.Color.Red;
                启动脚本线程();
                AddLog("已启动所有角色脚本");
            }
            else
            {
                isRunning = false;
                btnStart.Text = "▶ 启动脚本/Home";
                btnStart.BackColor = System.Drawing.Color.LimeGreen;
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



        // 后台脚本线程
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
        #endregion


        //按钮刷新角色,删除角色
        #region  
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
        #endregion


        //解压资源,初始化选择框
        #region
        private void 解压资源() 
        {
            // 1. 创建目录（不存在则自动创建）
            string dmDir = Path.GetDirectoryName(@"C:/Users/Public/dm/img/");
            if (!Directory.Exists(dmDir))
            {
                Directory.CreateDirectory(dmDir);
            }

            // 2. 如果已存在 dm.dll，先删除（避免覆盖失败）

            if (File.Exists(@"C:/Users/Public/dm/dm.dll"))
            {
                File.Delete(@"C:/Users/Public/dm/dm.dll");
            }
            if (File.Exists(@"C:/Users/Public/dm/dmreg.dll"))
            {
                File.Delete(@"C:/Users/Public/dm/dmreg.dll");
            }


            // 3. 从项目资源中读取 dm.dll 字节流
            byte[] dmBytes = 大话助手2._0.Properties.Resources.dm;
            byte[] dmregBytes = 大话助手2._0.Properties.Resources.dmreg;

            // 4. 写入文件到 C:\Users\Public\dm\dm.dll
            File.WriteAllBytes(@"C:/Users/Public/dm/dm.dll", dmBytes);
            File.WriteAllBytes(@"C:/Users/Public/dm/dmreg.dll", dmregBytes);

            //导出资源里的图片
            大话助手2._0.Properties.Resources.大地图.Save(Path.Combine(@"C:/Users/Public/dm/img/", "大地图.bmp"));
            大话助手2._0.Properties.Resources.怨气.Save(Path.Combine(@"C:/Users/Public/dm/img/", "怨气.bmp"));
            大话助手2._0.Properties.Resources.水墨条.Save(Path.Combine(@"C:/Users/Public/dm/img/", "水墨条.bmp"));
            大话助手2._0.Properties.Resources.红木条.Save(Path.Combine(@"C:/Users/Public/dm/img/", "红木条.bmp"));
            大话助手2._0.Properties.Resources.归队.Save(Path.Combine(@"C:/Users/Public/dm/img/" ,"归队.bmp"));
            大话助手2._0.Properties.Resources.归队h.Save(Path.Combine(@"C:/Users/Public/dm/img/", "归队h.bmp"));
            大话助手2._0.Properties.Resources.医宝宝.Save(Path.Combine(@"C:/Users/Public/dm/img/", "医宝宝.bmp"));
            大话助手2._0.Properties.Resources.医宝宝h.Save(Path.Combine(@"C:/Users/Public/dm/img/", "医宝宝h.bmp"));
            大话助手2._0.Properties.Resources.解冻.Save(Path.Combine(@"C:/Users/Public/dm/img/", "解冻.bmp"));
            大话助手2._0.Properties.Resources.解冻h.Save(Path.Combine(@"C:/Users/Public/dm/img/", "解冻h.bmp"));
            大话助手2._0.Properties.Resources.领双.Save(Path.Combine(@"C:/Users/Public/dm/img/", "领双.bmp"));
            大话助手2._0.Properties.Resources.领双h.Save(Path.Combine(@"C:/Users/Public/dm/img/", "领双h.bmp"));
            大话助手2._0.Properties.Resources.修装备.Save(Path.Combine(@"C:/Users/Public/dm/img/", "修装备.bmp"));
            大话助手2._0.Properties.Resources.修装备h.Save(Path.Combine(@"C:/Users/Public/dm/img/", "修装备h.bmp"));
            大话助手2._0.Properties.Resources.修装备_算了.Save(Path.Combine(@"C:/Users/Public/dm/img/", "修装备_算了.bmp"));
            大话助手2._0.Properties.Resources.修装备_算了h.Save(Path.Combine(@"C:/Users/Public/dm/img/", "修装备_算了h.bmp"));
            大话助手2._0.Properties.Resources.组队.Save(Path.Combine(@"C:/Users/Public/dm/img/", "组队.bmp"));
            大话助手2._0.Properties.Resources.组队h.Save(Path.Combine(@"C:/Users/Public/dm/img/", "组队h.bmp"));
            大话助手2._0.Properties.Resources.车夫_取消.Save(Path.Combine(@"C:/Users/Public/dm/img/", "车夫_取消.bmp"));
            大话助手2._0.Properties.Resources.车夫_取消h.Save(Path.Combine(@"C:/Users/Public/dm/img/", "车夫_取消h.bmp"));
            大话助手2._0.Properties.Resources.医宝宝.Save(Path.Combine(@"C:/Users/Public/dm/img/", "医宝宝.bmp"));
            大话助手2._0.Properties.Resources.医宝宝h.Save(Path.Combine(@"C:/Users/Public/dm/img/", "医宝宝h.bmp"));
            大话助手2._0.Properties.Resources.副本同意.Save(Path.Combine(@"C:/Users/Public/dm/img/", "副本同意.bmp"));
            大话助手2._0.Properties.Resources.副本同意h.Save(Path.Combine(@"C:/Users/Public/dm/img/", "副本同意h.bmp"));
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
        #endregion
        #region//配置相关
        void 保存配置()
        {
            // 保存复选框勾选状态
            大话助手2._0.Properties.Settings.Default.ck_无限自动 = ck_无限自动.Checked;
            大话助手2._0.Properties.Settings.Default.ck_自动归队 = ck_自动归队.Checked;
            大话助手2._0.Properties.Settings.Default.ck_自动领双 = ck_自动领双.Checked;
            大话助手2._0.Properties.Settings.Default.ck_副本同意 = ck_副本同意.Checked;
            大话助手2._0.Properties.Settings.Default.ck_修装备 = ck_修装备.Checked;
            大话助手2._0.Properties.Settings.Default.ck_医宝宝 = ck_医宝宝.Checked;
            大话助手2._0.Properties.Settings.Default.ck_自动进队 = ck_自动进队.Checked;
            大话助手2._0.Properties.Settings.Default.ck_角色血法 = ck_角色血法.Checked;
            大话助手2._0.Properties.Settings.Default.ck_宝宝血法 = ck_宝宝血法.Checked;

            // 保存ComboBox选中索引
            大话助手2._0.Properties.Settings.Default.comboBox_hp = comboBox_hp.SelectedIndex;
            大话助手2._0.Properties.Settings.Default.comboBox_mp = comboBox_mp.SelectedIndex;
            大话助手2._0.Properties.Settings.Default.comboBox_pethp = comboBox_pethp.SelectedIndex;
            大话助手2._0.Properties.Settings.Default.comboBox_petmp = comboBox_petmp.SelectedIndex;

            //保存编辑框textBox_注册码
            大话助手2._0.Properties.Settings.Default.textBox_注册码 = textBox_注册码.Text;

            //保存用户登录状态
            大话助手2._0.Properties.Settings.Default.用户登录状态 = 用户登录状态;
            string aa = 用户登录状态;
            Debug.WriteLine(aa);

            // 必须调用，才会写入本地配置
            大话助手2._0.Properties.Settings.Default.Save();
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            保存配置();
            isRunning = false;
            UnregisterHotKey(this.Handle, HotKeyID);
        }

        
        private async void Form1_Load(object sender, EventArgs e)
        {
            // 读取
            ck_无限自动.Checked = 大话助手2._0.Properties.Settings.Default.ck_无限自动;
            ck_自动归队.Checked = 大话助手2._0.Properties.Settings.Default.ck_自动归队;
            ck_自动领双.Checked = 大话助手2._0.Properties.Settings.Default.ck_自动领双;
            ck_副本同意.Checked = 大话助手2._0.Properties.Settings.Default.ck_副本同意;
            ck_修装备.Checked = 大话助手2._0.Properties.Settings.Default.ck_修装备;
            ck_医宝宝.Checked = 大话助手2._0.Properties.Settings.Default.ck_医宝宝;
            ck_自动进队.Checked = 大话助手2._0.Properties.Settings.Default.ck_自动进队;
            ck_角色血法.Checked = 大话助手2._0.Properties.Settings.Default.ck_角色血法;
            ck_宝宝血法.Checked = 大话助手2._0.Properties.Settings.Default.ck_宝宝血法;
            //还原ComboBox
            comboBox_hp.SelectedIndex = 大话助手2._0.Properties.Settings.Default.comboBox_hp;
            comboBox_mp.SelectedIndex = 大话助手2._0.Properties.Settings.Default.comboBox_mp;
            comboBox_pethp.SelectedIndex = 大话助手2._0.Properties.Settings.Default.comboBox_pethp;
            comboBox_petmp.SelectedIndex = 大话助手2._0.Properties.Settings.Default.comboBox_petmp;
            //还原textBox_注册码
            textBox_注册码.Text = 大话助手2._0.Properties.Settings.Default.textBox_注册码;
            //如果上次登录状态=="1",则本次直接登录
            string 登录状态 = 大话助手2._0.Properties.Settings.Default.用户登录状态;

            if (登录状态 == "1")
            {

                await 执行登录();

            }
        }

        #endregion

        ////文心验证相关
        #region

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
            // 间隔是否小于3秒
            if ((DateTime.Now - lastClickTime_login).TotalMilliseconds < 3000)
            {
                return; // 3秒内重复点击直接返回
            }

            // 记录本次点击时间
            lastClickTime_login = DateTime.Now;
            string MAC地址 = 验证.GetLocalMac();
            // 直接 await 调用
            string ret = await 验证.卡密登录(MAC地址, textBox_注册码.Text, "1.0");
            if (ret.Length == 16)
            {
                文心token = ret;
                注册码 = textBox_注册码.Text;
                到期时间 = await 验证.取到期时间(注册码);
                验证.监视用户状态(注册码, 文心token);
                button_登录.Enabled = false;
                btnStart.Enabled = true;
                显示到期时间(到期时间);
                await 验证.到期自动结束();
            }
            else
            {
                MessageBox.Show("登录失败：" + 验证.错误码对照(ret));

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

                //LogForm.日志输出(ret);
                MessageBox.Show(验证.错误码对照(ret));
            }
        }
        #endregion

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
    
