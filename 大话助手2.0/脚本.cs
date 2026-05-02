using System;
using System.Threading;
using System.Windows.Forms;
using 游戏脚本;

public class 脚本
{
    private 大漠中文 dm;
    private bool isRunning;
    private Thread thread;
    private readonly int Hwnd;
    private readonly int 序号;


    public 脚本(大漠中文 dm, int hwnd, int 序号)
    {
        this.dm = dm;
        Hwnd = hwnd;
        this.序号 = 序号;
        dm.BindWindow(Hwnd, "gdi", "windows", "windows", 0);
        dm.SetPath(@"C:/Users/Public/dm/img/");
        dm.SetShowErrorMsg(0);
    }

    public void Start(bool is无限自动, bool is自动归队, bool is自动领双, bool is医宝宝, bool is修装备, bool is自动进队, bool is副本同意, bool is角色血法, bool is宝宝血法)
    {
        isRunning = true;
        bool 血未操作 = false;
        bool 蓝未操作 = false;
        bool 血未操作p = false;
        bool 蓝未操作p = false;
        thread = new Thread(() =>
        {
            dm.取窗口尺寸(Hwnd,out int winx,out _);
            while (isRunning)
            {
                监听线程();
                if (is无限自动) 无限自动();
                if (is自动归队) 自动归队();
                if (is自动领双) 自动领双();
                if (is医宝宝) 医宝宝();
                if (is修装备) 修装备();
                if (is自动进队) 自动进队();
                if (is副本同意) 副本同意();
                if (is角色血法)
                {
                   
                    if (是否战斗() == 0)
                    {
                       血未操作 = true;
                       蓝未操作 = true;
                    }
                    角色血法(winx ,Form1.Instance.hp阈值,Form1.Instance.mp阈值,ref 血未操作,ref 蓝未操作);
                }
                if (is宝宝血法)
                {
                    
                    if (是否战斗() == 0)
                    {
                        血未操作p = true;
                        蓝未操作p = true;
                    }
                    宝宝血法(winx,Form1.Instance.pethp阈值, Form1.Instance.petmp阈值, ref 血未操作p, ref 蓝未操作p);
                }

                Thread.Sleep(1000);
            }
        })
        { IsBackground = true };
        thread.Start();
    }

    public void Stop()
    {
        isRunning = false;
    }

    public void 监听线程()
    {

        int winState = dm.GetWindowState(Hwnd, 0);

        if(winState == 0)
        {
            isRunning = false;
            
        }
    }


    private void 无限自动()
    {
        dm.KeyDown(18);
        dm.KeyPress(56);
        dm.KeyUp(18);
    }



    private void 自动归队()
    {
        dm.FindPic(285, 0, 854, 86, "归队.bmp|归队h.bmp", "202020", 0.9, 0, out int x, out int y);
        if (x > 0)
        {
            Thread.Sleep(3000);
            dm.MoveTo(x + 5, y + 5);
            dm.LeftClick();
            Thread.Sleep(500);
            dm.MoveTo(900, 500);
            Form1.Instance.AddLog(getID() + "正在归队");
        }
    }
    private void 自动领双() {
        dm.FindPic(246, 337, 516, 397, "解冻.bmp|解冻h.bmp", "202020", 0.9, 0, out int x, out int y);
        if (x > 0)
        {
            dm.MoveTo(x + 5, y + 5);
            dm.LeftClick();
            Thread.Sleep(500);
            dm.LeftClick();
            Thread.Sleep(500);
            dm.MoveTo(900, 500);
            Form1.Instance.AddLog(getID() + " 解冻了双倍");

        }
        dm.FindPic(246, 337, 516, 397, "领双.bmp|领双h.bmp", "202020", 0.9, 0, out int x1, out int y1);
        if (x1 > 0)
        {
            dm.MoveTo(x1 + 5, y1 + 5);
            dm.LeftClick();
            Thread.Sleep(500);
            dm.LeftClick();
            Thread.Sleep(500);
            dm.MoveTo(x1 + 50, y1 + 70);
            dm.LeftClick();
            Thread.Sleep(500);
            dm.MoveTo(900, 500);
            Form1.Instance.AddLog(getID() + " 领取了1小时双倍");
        }
    }
    private void 医宝宝() {
        dm.FindPic(250, 321, 515, 378, "医宝宝.bmp|医宝宝h.bmp", "202020", 0.9, 0, out int x, out int y);
        if (x > 0)
        {
            dm.MoveTo(x + 5, y + 5);
            dm.LeftClick();
            Thread.Sleep(500);
            dm.MoveTo(900, 500);
            Form1.Instance.AddLog(getID() + " 医宝宝");
        }
    }
    private void 修装备() {
        dm.FindPic(250, 342, 501, 453, "修装备.bmp|修装备h.bmp|修装备_算了.bmp|修装备_算了h.bmp", "202020", 0.9, 0, out int x, out int y);
        if (x > 0)
        {
            dm.MoveTo(x + 5, y + 5);
            dm.LeftClick();
            Thread.Sleep(500);
            dm.MoveTo(900, 500);
            Form1.Instance.AddLog(getID() + " 修理装备");
        }
    }
    private void 自动进队() {
        dm.FindPic(415, 398, 659, 453, "组队.bmp|组队h.bmp", "202020", 0.9, 0, out int x, out int y);
        if (x > 0)
        {
            dm.MoveTo(x + 5, y + 5);
            dm.LeftClick();
            Thread.Sleep(500);
            dm.MoveTo(900, 500);
            Form1.Instance.AddLog(getID() + " 进入队伍");
        }
    }
    private void 副本同意() {
        dm.FindPic(300, 360, 670, 500, "副本同意.bmp|副本同意h.bmp", "202020", 0.9, 0, out int x, out int y);
        if (x > 0)
        {
            dm.MoveTo(x + 5, y + 5);
            dm.LeftClick();
            Thread.Sleep(500);
            dm.MoveTo(900, 500);
            Form1.Instance.AddLog(getID() + " 同意进入副本");
        }
    }
    private void 角色血法(int winx,int hp阈值,int mp阈值,ref bool  血未操作,ref bool 蓝未操作) {
        
        if (winx == 1024)
        {
            //找当前血量1024
            dm.FindPic(929, 23, 1017, 35, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int hpx, out _);
            //找当前法量1024
            dm.FindPic(929, 39, 1017, 50, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int mpx, out _);
            int hp = 100;
            int mp = 100;
            if (hpx > 0)
            {
                hp = (int)Math.Round((double)((hpx - 929) / 88f * 100));
            }

            if (mpx > 0)
            {
                mp = (int)Math.Round((double)((mpx - 929) / 88f * 100));
            }

            if (hp <hp阈值 && 血未操作 && 是否战斗() == 1)
            {
                dm.MoveTo(1000, 28);
                Thread.Sleep(20);
                dm.RightClick();
                Thread.Sleep(500);
                dm.MoveTo(900, 500);
                血未操作 = false;
                Form1.Instance.AddLog(getID() + "角色使用了血药");
            }

            if (mp <mp阈值 && 蓝未操作 && 是否战斗() == 1)
            {
                dm.MoveTo(1000, 45);
                Thread.Sleep(20);
                dm.RightClick();
                Thread.Sleep(500);
                dm.MoveTo(900, 500);
                蓝未操作 = false;
                Form1.Instance.AddLog(getID() + "角色使用了法药");
            }

            Form1.Instance.dataGridView1.Rows[序号].Cells["col3"].Value = hp.ToString();
            Form1.Instance.dataGridView1.Rows[序号].Cells["col4"].Value = mp.ToString();

        }
        else if (winx == 1366)
        {
            //找当前血量1366
            dm.FindPic(1259, 25, 1358, 38, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int hpx, out _);
            //找当前法量1366
            dm.FindPic(1259, 42, 1358, 55, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int mpx, out _);
            int hp = 100;
            int mp = 100;
            if (hpx > 0)
            {
                hp = (int)Math.Round((double)((hpx - 1259) / 99f * 100));
            }
            if (mpx > 0)
            {
                mp = (int)Math.Round((double)((mpx - 1259) / 99f * 100));
            }


            if (hp < hp阈值 && 血未操作 && 是否战斗() == 1)
            {
                dm.MoveTo(1300, 31);
                Thread.Sleep(20);
                dm.RightClick();
                Thread.Sleep(500);
                dm.MoveTo(900, 500);
                血未操作 = false;
                Form1.Instance.AddLog(getID() + "角色使用了血药");
            }
            if (mp <mp阈值 && 蓝未操作 && 是否战斗() == 1)
            {
                dm.MoveTo(1300, 48);
                Thread.Sleep(20);
                dm.RightClick();
                Thread.Sleep(500);
                dm.MoveTo(900, 500);
                蓝未操作 = false;
                Form1.Instance.AddLog(getID() + "角色使用了法药");
            }

            Form1.Instance.dataGridView1.Rows[序号].Cells["col3"].Value = hp.ToString();
            Form1.Instance.dataGridView1.Rows[序号].Cells["col4"].Value = mp.ToString();
        }
    }



    private void 宝宝血法(int winx ,int pethp阈值, int petmp阈值, ref bool 血未操作p, ref bool 蓝未操作p) {
        
        if (winx == 1024)
        {
            //找当前血量1024
            dm.FindPic(797, 20, 860, 32, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int pethpx, out _);
            //找当前法量1024
            dm.FindPic(797, 34, 860, 46, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int petmpx, out _);
            int pethp = 100;
            int petmp = 100;
            if (pethpx > 0)
            {
                pethp = (int)Math.Round((double)((pethpx - 797) / 63f * 100));
            }

            if (petmpx > 0)
            {
                petmp = (int)Math.Round((double)((petmpx - 797) / 63f * 100));
            }

            if (pethp < pethp阈值 && 血未操作p && 是否战斗() == 1)
            {
                dm.MoveTo(820, 26);
                Thread.Sleep(20);
                dm.RightClick();
                Thread.Sleep(500);
                dm.MoveTo(900, 500);
                血未操作p = false;
                Form1.Instance.AddLog(getID() + "宝宝使用了血药");
            }

            if (petmp < petmp阈值 && 血未操作p && 是否战斗() == 1)
            {
                dm.MoveTo(820, 40);
                Thread.Sleep(20);
                dm.RightClick();
                Thread.Sleep(500);
                dm.MoveTo(900, 500);
                蓝未操作p = false;
                Form1.Instance.AddLog(getID() + "宝宝使用了法药");
            }

            Form1.Instance.dataGridView1.Rows[序号].Cells["col5"].Value = pethp.ToString();
            Form1.Instance.dataGridView1.Rows[序号].Cells["col6"].Value = petmp.ToString();

        }
        else if (winx == 1366)
        {
            //找当前血量1366
            dm.FindPic(1110, 22, 1181, 35, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int pethpx, out _);
            //找当前法量1366
            dm.FindPic(1110, 37, 1181, 50, "水墨条.bmp|红木条.bmp", "202020", 0.9, 0, out int petmpx, out _);
            int pethp = 100;
            int petmp = 100;

            if (pethpx > 0)
            {
                pethp = (int)Math.Round((double)((pethpx - 1110) / 71f * 100));
            }
            if (petmpx > 0)
            {
                petmp = (int)Math.Round((double)((petmpx - 1110) / 71f * 100));
            }


            if (pethp < pethp阈值 && 血未操作p && 是否战斗() == 1)
            {
                dm.MoveTo(1150, 29);
                Thread.Sleep(20);
                dm.RightClick();
                Thread.Sleep(500);
                dm.MoveTo(1150, 500);
                血未操作p = false;
                Form1.Instance.AddLog(getID() + "宝宝使用了血药");
            }
            if (petmp < petmp阈值 && 蓝未操作p && 是否战斗() == 1)
            {
                dm.MoveTo(1150, 44);
                Thread.Sleep(20);
                dm.RightClick();
                Thread.Sleep(500);
                dm.MoveTo(900, 500);
                蓝未操作p = false;
                Form1.Instance.AddLog(getID() + "宝宝使用了法药");
            }
            Form1.Instance.dataGridView1.Rows[序号].Cells["col5"].Value = pethp.ToString();
            Form1.Instance.dataGridView1.Rows[序号].Cells["col6"].Value = petmp.ToString();

        }

    }
    private string getID()
    {
        string title = dm.取窗口标题(Hwnd);
        string[] namearr = title.Split('-');
        return namearr[2];
    }
    //返回0在战斗外,返回1在战斗内
    private int 是否战斗()
    {
        int ret = dm.FindPic(0, 0, 663, 30, "大地图.bmp|怨气.bmp", "202020", 0.9, 0, out int x, out int y);

        return ret;
    }
}