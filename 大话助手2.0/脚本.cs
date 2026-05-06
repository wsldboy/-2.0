using System;
using System.Threading;
using System.Windows.Forms;
using 游戏脚本;

public class 脚本
{
    // ====================== 【常量统一管理，方便修改】 ======================
    private const string ImgPath = @"C:/Users/Public/dm/img/";
    private const int DefaultSleep = 500;
    private const int ClickSleep = 20;
    private readonly int[] SupportRes = { 1024, 1366 };

    // ====================== 全局对象 ======================
    private readonly 大漠中文 dm;
    private readonly int hwnd;
    private readonly int 序号;
    private Thread runThread;
    private bool isRunning;

    // ====================== 构造 ======================
    public 脚本(大漠中文 dm, int hwnd, int 序号)
    {
        this.dm = dm ?? throw new ArgumentNullException(nameof(dm));
        this.hwnd = hwnd;
        this.序号 = 序号;

        // 初始化大漠
        dm.BindWindow(hwnd, "gdi", "windows", "windows", 0);
        dm.SetPath(ImgPath);
        dm.SetShowErrorMsg(0);
    }

    // ====================== 启动 ======================
    public void Start(bool is无限自动, bool is自动归队, bool is自动领双, bool is医宝宝,
                     bool is修装备, bool is自动进队, bool is副本同意, bool is角色血法, bool is宝宝血法)
    {
        if (isRunning) return;

        isRunning = true;
        bool 血未操作 = false, 蓝未操作 = false, 血未操作p = false, 蓝未操作p = false;

        runThread = new Thread(() =>
        {
            try
            {
                dm.取窗口尺寸(hwnd, out int winx, out _);
                while (isRunning)
                {
                    监听退出();
                    if (!isRunning) break;

                    if (is无限自动) 无限自动();
                    if (is自动归队) 自动归队();
                    if (is自动领双) 自动领双();
                    if (is医宝宝) 医宝宝();
                    if (is修装备) 修装备();
                    if (is自动进队) 自动进队();
                    if (is副本同意) 副本同意();

                    int inBattle = 是否战斗();
                    if (is角色血法 && inBattle != -1) 角色血法(winx, inBattle, ref 血未操作, ref 蓝未操作);
                    if (is宝宝血法 && inBattle != -1) 宝宝血法(winx, inBattle, ref 血未操作p, ref 蓝未操作p);

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                安全日志($"【异常】脚本出错：{ex.Message}");
            }
            finally
            {
                isRunning = false;
            }
        })
        { IsBackground = true, Priority = ThreadPriority.AboveNormal };
        runThread.Start();
    }

    // ====================== 停止 ======================
    public void Stop()
    {
        isRunning = false;
        if (runThread != null && runThread.IsAlive)
            runThread.Join(1000); // 等待线程安全退出
    }

    // ====================== 窗口监听 ======================
    private void 监听退出()
    {
        if (dm.GetWindowState(hwnd, 0) == 0)
            isRunning = false;
    }

    // ====================== 通用工具方法（消除大量重复） ======================
    private bool 查找点击(string pic, int x1, int y1, int x2, int y2, string desc, int delay = DefaultSleep)
    {
        dm.FindPic(x1, y1, x2, y2, pic, "202020", 0.9, 0, out int x, out int y);
        if (x <= 0) return false;

        移动点击(x + 5, y + 5, delay);
        安全日志($"{getID()} {desc}");
        return true;
    }

    private void 移动点击(int x, int y, int delay = DefaultSleep)
    {
        dm.MoveTo(x, y);
        Thread.Sleep(ClickSleep);
        dm.LeftClick();
        Thread.Sleep(delay);
        dm.MoveTo(900, 500);
    }

    private void 移动右键(int x, int y, int delay = DefaultSleep)
    {
        dm.MoveTo(x, y);
        Thread.Sleep(ClickSleep);
        dm.RightClick();
        Thread.Sleep(delay);
        dm.MoveTo(900, 500);
    }

    // 【跨线程安全写UI】不会再报错/卡死
    private void 安全设置UI(int row, string col, string value)
    {
        if (Form1.Instance.dataGridView1.InvokeRequired)
        {
            Form1.Instance.dataGridView1.Invoke(new Action(() => 安全设置UI(row, col, value)));
            return;
        }
        Form1.Instance.dataGridView1.Rows[row].Cells[col].Value = value;
    }

    private void 安全日志(string msg)
    {
        if (Form1.Instance.InvokeRequired)
        {
            Form1.Instance.Invoke(new Action(() => 安全日志(msg)));
            return;
        }
        Form1.Instance.AddLog(msg);
    }

    // ====================== 功能模块 ======================
    private void 无限自动()
    {
        dm.KeyDown(18);
        dm.KeyPress(56);
        dm.KeyUp(18);
    }

    private void 自动归队() 
    {
        dm.FindPic(285,0,854,86, "归队.bmp|归队h.bmp","202020",0.9,0,out int x,out int y);
        if(x > 0)
        {
            Thread.Sleep(3000);
            移动点击(x + 5, y + 5);
            安全日志($"{getID()} 自动归队");
        }
    } 
    private void 自动领双()
    {
        查找点击("解冻.bmp|解冻h.bmp", 246, 337, 516, 397, "解冻双倍");
        查找点击("领双.bmp|领双h.bmp", 246, 337, 516, 397, "领取1小时双倍");
    }
    private void 医宝宝() => 查找点击("医宝宝.bmp|医宝宝h.bmp", 250, 321, 515, 378, "医宝宝");
    private void 修装备() => 查找点击("修装备.bmp|修装备h.bmp|修装备_算了.bmp|修装备_算了h.bmp", 250, 342, 501, 453, "修理装备");
    private void 自动进队() => 查找点击("组队.bmp|组队h.bmp", 415, 398, 659, 453, "进入队伍");
    private void 副本同意() => 查找点击("副本同意.bmp|副本同意h.bmp", 300, 360, 670, 500, "同意进入副本");

    // ====================== 角色血法 ======================
    private void 角色血法(int winx, int inBattle, ref bool 血未操作, ref bool 蓝未操作)
    {
        if (inBattle == 0)
        {
            血未操作 = true;
            蓝未操作 = true;
            //return;
        }

        int hp = 100, mp = 100;
        if (winx == 1024)
        {
            dm.FindPic(929, 23, 1017, 35, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int hpx, out _);
            if(hpx> 0)
            {
               hp = (int)Math.Round((hpx - 929) / 88f * 100);
                
                if (hp < Form1.Instance.hp阈值 && 血未操作 && inBattle == 1)
                { 移动右键(1000, 28); 血未操作 = false; 安全日志($"{getID()}角色使用血药"); }
            }
                
            dm.FindPic(929, 39, 1017, 50, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int mpx, out _);
            if(mpx> 0)
            {
               mp = (int)Math.Round((mpx - 929) / 88f * 100);
                
                if (mp < Form1.Instance.mp阈值 && 蓝未操作 && inBattle == 1)
                { 移动右键(1000, 45); 蓝未操作 = false; 安全日志($"{getID()}角色使用法药"); }
            }                  
            
        }
        else if (winx == 1366)
        {
            dm.FindPic(1259, 25, 1358, 38, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int hpx, out _);
            if (hpx > 0)
            {
                hp = (int)Math.Round((hpx - 1259) / 99f * 100);
                
                if (hp < Form1.Instance.hp阈值 && 血未操作 && inBattle == 1)
                { 移动右键(1300, 31); 血未操作 = false; 安全日志($"{getID()}角色使用血药"); }
            }
            dm.FindPic(1259, 42, 1358, 55, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int mpx, out _);
            if (mpx > 0)
            {
                mp = (int)Math.Round((mpx - 1259) / 99f * 100);
                
                if (mp < Form1.Instance.mp阈值 && 蓝未操作 && inBattle == 1)
                { 移动右键(1300, 48); 蓝未操作 = false; 安全日志($"{getID()}角色使用法药"); }
            }
            
        }
        安全设置UI(序号, "col3", hp.ToString());
        安全设置UI(序号, "col4", mp.ToString());

    }

    // ====================== 宝宝血法（修复了BUG） ======================
    private void 宝宝血法(int winx, int inBattle, ref bool 血未操作p, ref bool 蓝未操作p)
    {
        if (inBattle == 0)
        {
            血未操作p = true;
            蓝未操作p = true;
            //return;
        }

        int pethp = 100, petmp = 100;
        if (winx == 1024)
        {
            dm.FindPic(797, 20, 860, 32, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int hpx, out _);
            if(hpx > 0)
            {
                pethp = (int)Math.Round((hpx - 797) / 63f * 100);
                
                if (pethp < Form1.Instance.pethp阈值 && 血未操作p && inBattle == 1)
                { 移动右键(820, 26); 血未操作p = false; 安全日志($"{getID()}宝宝使用血药"); }
            }

            dm.FindPic(797, 34, 860, 46, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int mpx, out _);
            if (mpx > 0)
            {
                petmp = (int)Math.Round((mpx - 797) / 63f * 100);
                
                if (petmp < Form1.Instance.petmp阈值 && 蓝未操作p && inBattle == 1) 
                { 移动右键(820, 40); 蓝未操作p = false; 安全日志($"{getID()}宝宝使用法药"); }
            }
            

        }
        else if (winx == 1366)
        {
            dm.FindPic(1110, 22, 1181, 35, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int hpx, out _);
            if (hpx > 0)
            {
                pethp = (int)Math.Round((hpx - 1110) / 71f * 100);
                
                if (pethp < Form1.Instance.pethp阈值 && 血未操作p && inBattle == 1)
                { 移动右键(1150, 29); 血未操作p = false; 安全日志($"{getID()}宝宝使用血药"); }
            }
            dm.FindPic(1110, 37, 1181, 50, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int mpx, out _);
            if (mpx > 0)
            {
                petmp = (int)Math.Round((mpx - 1110) / 71f * 100);
                
                if (petmp < Form1.Instance.petmp阈值 && 蓝未操作p && inBattle == 1)
                { 移动右键(1150, 44); 蓝未操作p = false; 安全日志($"{getID()}宝宝使用法药"); }
            }
            
        }
        安全设置UI(序号, "col5", pethp.ToString());
        安全设置UI(序号, "col6", petmp.ToString());
    }

    // ====================== 工具 ======================
    private string getID()
    {
        try { return dm.取窗口标题(hwnd)?.Split('-')[2] ?? "未知"; }
        catch { return "未知"; }
    }

  
    // -1=不在战斗，游戏不做处理,0=战斗界面，1=大地图界面   
    private int 是否战斗() => dm.FindPic(0, 0, 663, 30, "大地图.bmp|怨气.bmp", "202020", 0.9, 0, out _, out _);


}