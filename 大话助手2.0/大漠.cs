using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

public class 大漠中文
{
    // ======================== 核心 API ========================
    [DllImport(@"C:\Users\Public\dm\dmreg.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    private static extern int SetDllPathA(string path, int mode);



    public object dm;

    public 大漠中文()
    {
        try
        {


            // 1. 从 绝对路径 加载 dll

            int result = SetDllPathA(@"C:\Users\Public\dm\dm.dll", 0);
            if (result == 0)
                throw new Exception("SetDllPathA 设置失败，检查 dm.dll 是否存在或被杀毒拦截");

            // 2. 创建大漠对象（成功！）
            Type type = Type.GetTypeFromProgID("dm.dmsoft", true);
            dm = Activator.CreateInstance(type);
        }
        catch (Exception ex)
        {
            // 拼接详细错误，方便你定位问题
            string errorMsg = $"【大漠加载失败】\r\n错误信息：{ex.Message}\r\n解决：1.确保dm.dll正确 2.程序编译x86 3.关闭杀毒";
            throw new Exception("加载失败：" + errorMsg);
        }
    }



    // 绑定窗口
    public int BindWindow(int hwnd, string d, string m, string k, int mode)
    {
        return (int)dm.GetType().InvokeMember("BindWindow", System.Reflection.BindingFlags.InvokeMethod, null, dm, new object[] { hwnd, d, m, k, mode });
    }

    // 解绑
    public int UnBindWindow()
    {
        return (int)dm.GetType().InvokeMember("UnBindWindow", System.Reflection.BindingFlags.InvokeMethod, null, dm, null);
    }
    //枚举窗口
    public string 枚举窗口(int parent, string title, string class_name, int filter)
    {
        return (string)dm.GetType().InvokeMember("EnumWindow", System.Reflection.BindingFlags.InvokeMethod, null, dm, new object[] { parent, title, class_name, filter });
    }
    public string 取窗口标题(int hwnd)
    {
        return (string)dm.GetType().InvokeMember("GetWindowTitle", System.Reflection.BindingFlags.InvokeMethod, null, dm, new object[] { hwnd });
    }

    public int SetPath(string path)
    {
        return (int)dm.GetType().InvokeMember("SetPath", System.Reflection.BindingFlags.InvokeMethod, null, dm, new object[] { path });
    }
    public int MoveTo(int x, int y)
    {
        return (int)dm.GetType().InvokeMember("MoveTo", System.Reflection.BindingFlags.InvokeMethod, null, dm, new object[] { x, y });
    }

    public int LeftClick()
    {
        return (int)dm.GetType().InvokeMember("LeftClick", System.Reflection.BindingFlags.InvokeMethod, null, dm, null);
    }
    public int RightClick()
    {
        return (int)dm.GetType().InvokeMember("RightClick", System.Reflection.BindingFlags.InvokeMethod, null, dm, null);
    }

    public int KeyPress(int vk_code)
    {
        return (int)dm.GetType().InvokeMember("KeyPress", System.Reflection.BindingFlags.InvokeMethod, null, dm, new object[] { vk_code });
    }
    public int KeyDown(int vk_code)
    {
        return (int)dm.GetType().InvokeMember("KeyDown", System.Reflection.BindingFlags.InvokeMethod, null, dm, new object[] { vk_code });
    }

    public int KeyUp(int vk_code)
    {
        return (int)dm.GetType().InvokeMember("KeyUp", System.Reflection.BindingFlags.InvokeMethod, null, dm, new object[] { vk_code });
    }
    public int SetShowErrorMsg(int show)
    {
        return (int)dm.GetType().InvokeMember("SetShowErrorMsg", System.Reflection.BindingFlags.InvokeMethod, null, dm, new object[] { show });
    }

    public int GetWindowState(int hwnd,int flag)
    {
        return (int)dm.GetType().InvokeMember("GetWindowState", System.Reflection.BindingFlags.InvokeMethod, null, dm, new object[] { hwnd,flag });
    }

    public int FindPic(int x1, int y1, int x2, int y2, string pic_name, string delta_color, double sim, int dir, out int x, out int y)
    {
        object[] args = new object[10];
        object result;
        ParameterModifier[] mods = new ParameterModifier[1];

        mods[0] = new ParameterModifier(10);
        mods[0][8] = true;
        mods[0][9] = true;

        args[0] = x1;
        args[1] = y1;
        args[2] = x2;
        args[3] = y2;
        args[4] = pic_name;
        args[5] = delta_color;
        args[6] = sim;
        args[7] = dir;

        result = dm.GetType().InvokeMember("FindPic", BindingFlags.InvokeMethod, null, dm, args, mods, null, null);
        x = (int)args[8];
        y = (int)args[9];
        return (int)result;
    }
    public int 取窗口尺寸(int hwnd, out int width, out int height)
    {
        object[] args = new object[3];
        object result;
        ParameterModifier[] mods = new ParameterModifier[1];

        mods[0] = new ParameterModifier(3);
        mods[0][1] = true;
        mods[0][2] = true;

        args[0] = hwnd;

        result = dm.GetType().InvokeMember("GetClientSize", BindingFlags.InvokeMethod, null, dm, args, mods, null, null);
        width = (int)args[1];
        height = (int)args[2];
        return (int)result;
    }
    public void 释放()
    {
        if (dm != null)
        {
            // 1. 先解绑窗口（非常重要）
            dm.GetType().InvokeMember("UnBindWindow",
                BindingFlags.InvokeMethod, null, dm, null);

            // 2. 释放COM组件
            Marshal.ReleaseComObject(dm);

            // 3. 置空
            dm = null;

            // 4. 强制垃圾回收（彻底释放）
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

}