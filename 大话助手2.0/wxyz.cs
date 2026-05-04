using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Management.Instrumentation;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 游戏脚本;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
//using static System.Net.Mime.MediaTypeNames;

namespace 大话助手2._0
{
    public class wxyz
    {
        private RC4Encryption RC4 = new RC4Encryption();
        private int 线路 = 0;
        private string jxm = "";
        private string jem = "";
        private int jm类型 = 0;
        private int je类型 = 0;
        private string 地址 = "http://api.1wxyun.com/?type=";
        private string 地址2 = "http://api2.1wxyun.com/?type=";
        private string Softid = "";

        /// <summary>
        /// 软件首次启动需要初始化,配置要和软件后台设置一致
        /// </summary>
        /// <param name="softid">软件ID</param>
        /// <param name="jm">加密参数,如无加密,可以为空</param>
        /// <param name="jmtype">加密类型,1 RC4加密 其他数字不加密</param>
        /// <param name="je">解密参数,如无加密,可以为空</param>
        /// <param name="jetype">1 RC4解密 其他数字不解密</param>
        /// <param name="默认线路">0 默认为0,其他数字为1</param>

        //主窗口需要调用的初始化方法,如果不想在主窗口调用,可以把这个方法放在静态构造函数里,或者放在一个单独的类里,只要保证软件启动时调用一次即可.

        public void 验证初始化(string softid, string jm, int jmtype, string je, int jetype, int 默认线路)
        {
            if (jmtype == 1)
            {
                jxm = jm;
                jm类型 = jmtype;
            }
            if (jetype == 1)
            {
                jem = je;
                je类型 = jetype;
            }
            if (默认线路 != 0)
            {
                线路 = 1;
            }
            Softid = "Softid=" + softid;
        }
        /// <summary>
        /// 获取公告
        /// </summary>
        /// <returns>登录成功返回公告内容，失败返回错误码，根据错误代码查看错误原因即可</returns>

        private string 加密参(string 参数)
        {
            if (jm类型 == 1)
            {
                string 返回 = RC4.加密rc4(jxm, 参数);
                返回 = 返回.Replace("=", "%3D").Replace("+", "%2B");
                return 返回;
            }
            else
            {
                return 参数;
            }
        }



        public async Task<string> 取最新版本号()
        {
            string type = "3";
            string post = Softid;
            string 返回 = await 提交数据(type, post);
            return 返回;
        }

        public async Task<string> 取下载地址()
        {
            string type = "6";
            string post = Softid;
            string 返回 = await 提交数据(type, post);
            return 返回;
        }

        /// <summary>
        /// 检测用户状态
        /// </summary>
        /// <param name="UserName">用户名或卡密</param>
        /// <param name="Token">登录成功后返回的一串Token</param>
        /// <returns>成功状态返回1，失败返回错误码，根据错误代码查看错误原因即可。(循环调用接口检测,检测周期不能小于3分钟,用于检测账号是否正常登陆,是否被踢下线,是否超过指定多开数量)</returns>
        public async Task<string> 检测用户状态(string UserName, string Token)
        {
            string type = "8";
            UserName = 加密参(UserName);
            Token = 加密参(Token);
            string post = $"{Softid}&UserName={UserName}&Token={Token}";
            string 返回 = await 提交数据(type, post);
            return 返回;
        }
        public void 监视用户状态(string UserName, string Token)
        {
            int 错误计次 = 0;
            Task.Run(async () =>
            {
                while (true)
                {
                    string 状态 = await 检测用户状态(UserName, Token);
                    Form1.Instance.用户登录状态 = 状态;
                    if (状态 == "")
                    {
                        错误计次++;
                        if (错误计次 >= 5)
                        {
                            Application.Exit();
                        }
                        //接口调用失败,可以选择继续监视或者停止监视,这里选择继续监视
                        await Task.Delay(TimeSpan.FromMinutes(3)); // 每3分钟检测一次
                        continue;
                    }
                    if (状态 != "1")
                    {
                        MessageBox.Show(错误码对照(状态));
                        Application.Exit();

                    }
                    await Task.Delay(TimeSpan.FromMinutes(3)); // 每3分钟检测一次

                }
            });
        }

        public async Task 到期自动结束()
        {
            while (true)
            {
                // 把字符串时间 转换成 可比较的时间
                if (DateTime.TryParse(Form1.Instance.到期时间, out DateTime 到期时间))
                {
                    // 获取当前电脑时间
                    DateTime 当前时间 = DateTime.Now;


                    if (到期时间 < 当前时间)
                    {

                        
                        for (int i = 20; i > 0; i--) 
                        {
                            await Task.Delay(1000);
                            Form1.Instance.AddLog($"注册码已经到期, 软件将在{i}秒后关闭");

                        }
                        
                        Application.Exit();
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1)); // 每1分钟检测一次
            }
        }



        public async Task<string> 卡密登录(string Mac, string Card, string version)

        {

            string type = "17";
            Card = 加密参(Card);
            version = 加密参(version);
            Mac = 加密参(Mac);
            string post = $"{Softid}&Card={Card}&Version={version}&Mac={Mac}";
            string 返回 = await 提交数据(type, post);
            return 返回;
        }

        public async Task<string> 用户转绑(string Mac, string Card, string Type)

        {

            string type = "14";
            Card = 加密参(Card);
            Type = 加密参(Type);
            Mac = 加密参(Mac);
            string post = $"{Softid}&UserName={Card}&UserPwd={""}&Type={Type}&Mac={Mac}";

            string 返回 = await 提交数据(type, post);
            return 返回;
        }


        /// 获取成功返回对应到期时间，失败返回错误码，根据错误代码查看错误原因即可</returns>
        public async Task<string> 取到期时间(string UserName)
        {
            string type = "24";
            UserName = 加密参(UserName);
            string post = $"{Softid}&UserName={UserName}&UserPwd=";
            string 返回 = await 提交数据(type, post);
            return 返回;
        }

        //生成机器码
        public string GetLocalMac()
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                // 1. 获取CPU序列号
                ManagementClass cpuClass = new ManagementClass("Win32_Processor");
                ManagementObjectCollection cpuColl = cpuClass.GetInstances();
                foreach (ManagementObject mo in cpuColl)
                {
                    sb.Append(mo["ProcessorId"]?.ToString() ?? "");
                    break;
                }

                // 2. 获取主板序列号
                ManagementClass boardClass = new ManagementClass("Win32_BaseBoard");
                ManagementObjectCollection boardColl = boardClass.GetInstances();
                foreach (ManagementObject mo in boardColl)
                {
                    sb.Append(mo["SerialNumber"]?.ToString() ?? "");
                    break;
                }

                // 3. 获取第一个可用MAC地址
                var mac = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(nic => nic.OperationalStatus == OperationalStatus.Up
                    && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback);

                if (mac != null)
                {
                    var phyAddr = mac.GetPhysicalAddress();
                    sb.Append(BitConverter.ToString(phyAddr.GetAddressBytes()));
                }

                // MD5哈希 生成固定32位
                using (MD5 md5 = MD5.Create())
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(sb.ToString());
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    StringBuilder machineCode = new StringBuilder();
                    foreach (byte b in hashBytes)
                    {
                        machineCode.Append(b.ToString("X2"));
                    }
                    // 固定返回32位大写
                    return machineCode.ToString();
                }
            }
            catch
            {
                return "00000000000000000000000000000000";
            }
        }

        /// <summary>
        /// 如果错误返回错误对应的说明
        /// </summary>
        /// <param name="erroID">错误ID</param>
        /// <returns>返回错误说明,没有错误返回空文本</returns>
        public string 错误码对照(string erroID)
        {
            string 错误说明 = erroID;
            switch (erroID)
            {
                case "-71001":
                    错误说明 = "无网络连接，请检查您的网络设置.";
                    break;
                case "-71002":
                    错误说明 = "网络连接异常.";
                    break;
                case "-81001":
                    错误说明 = "WebApi接口不存在.";
                    break;
                case "-81003":
                    错误说明 = "软件已被强制关闭.";
                    break;
                case "-81004":
                    错误说明 = "软件不存在.";
                    break;
                case "-81005":
                    错误说明 = "软件已停用.";
                    break;
                case "-81006":
                    错误说明 = "版本号不存在.";
                    break;
                case "-81007":
                    错误说明 = "程序接口密码错误.";
                    break;
                case "-81008":
                    错误说明 = "程序不允许用户转绑.";
                    break;
                case "-81009":
                    错误说明 = "软件已开启收费模式,当前版本为免费版本.";
                    break;
                case "-81010":
                    错误说明 = "版本号已停用.";
                    break;
                case "-81011":
                    错误说明 = "软件版本必须为数字.";
                    break;
                case "-81012":
                    错误说明 = "当前软件未开启试用功能.";
                    break;
                case "-81015":
                    错误说明 = "频繁调用,请等待10分钟后再做尝试.";
                    break;
                case "-81016":
                    错误说明 = "单IP频繁访问限制.";
                    break;
                case "-81017":
                    错误说明 = "此接口开启了算法,但是提交时未加密数据再提交.";
                    break;
                case "-81018":
                    错误说明 = "变量数据不存在.";
                    break;
                case "-81019":
                    错误说明 = "变量ID必须为数字.";
                    break;
                case "-81020":
                    错误说明 = "变量别名只能是数字或者字母或者数字字母混合.";
                    break;
                case "-81021":
                    错误说明 = "错误的调用方式,请查看后台接口的调用方式和参数.";
                    break;
                case "-81022":
                    错误说明 = "机器码填写错误，长度必须是不能超过32位的字母或数字.";
                    break;
                case "-81023":
                    错误说明 = "注册码在其他地方登录.";
                    break;
                case "-81024":
                    错误说明 = "扣点数量必须大于0.";
                    break;
                case "-81025":
                    错误说明 = "软件扣费模式和卡类不符.";
                    break;
                case "-81026":
                    错误说明 = "当前扣费模式为时间计费,无法使用扣点功能.";
                    break;
                case "-81027":
                    错误说明 = "黑名单已存在.";
                    break;
                case "-81028":
                    错误说明 = "当前用户在别的机器登陆使用,暂时无法登陆 请等10分钟.";
                    break;
                case "-81029":
                    错误说明 = "当前用户在别的IP登陆使用,暂时无法登陆 请等10分钟.";
                    break;
                case "-81030":
                    错误说明 = "当前用户已超过最大登陆数量,暂时无法登陆 请等10分钟.";
                    break;
                case "-81031":
                    错误说明 = "用户已在别的地方登陆.";
                    break;
                case "-81032":
                    错误说明 = "token填写错误,长度必须是16位字母或数字.";
                    break;
                case "-81033":
                    错误说明 = "用户名或卡密或试用特征填写错误,长度必须是6-16位的字母或数字.";
                    break;
                case "-81035":
                    错误说明 = "解绑类型只能为数字并且只能为1或者2";
                    break;
                case "-81036":
                    错误说明 = "用户名或卡密或试用特征使用点数不足.";
                    break;
                case "-81037":
                    错误说明 = "当前版本已被停止使用.";
                    break;
                case "-81039":
                    错误说明 = "用户电脑特征已被列入黑名单.";
                    break;
                case "-81040":
                    错误说明 = "检测用户状态过于频繁，请把检测周期设置大于等于3分钟.";
                    break;
                case "-81042":
                    错误说明 = "用户或卡密提交的云数据超过最大长度.";
                    break;
                case "-81043":
                    错误说明 = "用户或卡密提交的封停原因超过最大长度.";
                    break;
                case "-81044":
                    错误说明 = "提交的加入黑名单原因超过最大长度.";
                    break;
                case "-81045":
                    错误说明 = "VMP授权密钥设置错误,请查看管理端对应的设置要求进行设置.";
                    break;
                case "-81047":
                    错误说明 = "提交的VMP账号格式错误,长度不能小于10且不能大于200";
                    break;
                case "-81048":
                    错误说明 = "VMP授权端校验失败";
                    break;
                case "-81049":
                    错误说明 = "取用户指定类型数据错误";
                    break;
                case "-83001":
                    错误说明 = "注册码不存在.";
                    break;
                case "-83002":
                    错误说明 = "注册码填写错误,长度应16位.";
                    break;
                case "-83003":
                    错误说明 = "注册码已禁用.";
                    break;
                case "-83004":
                    错误说明 = "注册码类型和软件扣费模式不符.";
                    break;
                case "-83005":
                    错误说明 = "该注册码所属卡类为单次卡类每个电脑只能登陆使用一张.";
                    break;
                case "-83006":
                    错误说明 = "注册码已到期.";
                    break;
                case "-83007":
                    错误说明 = "卡密使用点数不足.";
                    break;
                case "-83008":
                    错误说明 = "注册码已经绑定在其他账号上.";
                    break;
                case "-83009":
                    错误说明 = "注册码未在绑定的IP地址登陆.";
                    break;
                case "-83011":
                    错误说明 = "注册码重绑次数超过限制.";
                    break;
                case "-83012":
                    错误说明 = "卡密积分不足.";
                    break;
                case "-83013":
                    错误说明 = "卡密未激活.";
                    break;
                case "-83014":
                    错误说明 = "注册码和IP一致无需转绑即可登陆.";
                    break;
                case "-83015":
                    错误说明 = "卡密和账号一致无需转绑即可登陆.";
                    break;
                case "-83016":
                    错误说明 = "注册码转绑后将到期";
                    break;
                case "-83017":
                    错误说明 = "卡密转绑后点数小于0";
                    break;
                case "-82001":
                    错误说明 = "用户不存在.";
                    break;
                case "-82002":
                    错误说明 = "用户名填写错误,长度必须是6-16位字母或数字.";
                    break;
                case "-82003":
                    错误说明 = "用户密码填写错误,长度必须是6-16位字母或数字.";
                    break;
                case "-82004":
                    错误说明 = "用户超级密码填写错误,长度必须是6-16位字母或数字.";
                    break;
                case "-82005":
                    错误说明 = "用户名已存在.";
                    break;
                case "-82006":
                    错误说明 = "用户已被锁定.";
                    break;
                case "-82007":
                    错误说明 = "用户已到期.";
                    break;
                case "-82008":
                    错误说明 = "用户使用点数不足.";
                    break;
                case "-82009":
                    错误说明 = "用户未在绑定的电脑上登陆.";
                    break;
                case "-82010":
                    错误说明 = "用户未在绑定的IP地址登陆.";
                    break;
                case "-82011":
                    错误说明 = "用户注册次数超过限制.";
                    break;
                case "-82012":
                    错误说明 = "用户重绑次数超过限制.";
                    break;
                case "-82013":
                    错误说明 = "用户积分不足.";
                    break;
                case "-82014":
                    错误说明 = "用户IP一致无需转绑即可登陆.";
                    break;
                case "-82015":
                    错误说明 = "用户账号一致无需转绑即可登陆.";
                    break;
                case "-82016":
                    错误说明 = "用户注册失败，程序停止新用户注册.";
                    break;
                case "-82017":
                    错误说明 = "用户注册失败，已开启卡密注册.";
                    break;
                case "-82018":
                    错误说明 = "用户超级密码错误.";
                    break;
                case "-82019":
                    错误说明 = "用户转绑后将到期";
                    break;
                case "-82020":
                    错误说明 = "用户转绑后点数小于0";
                    break;
                case "-82021":
                    错误说明 = "用户密码错误.";
                    break;
                case "-82022":
                    错误说明 = "推荐人填写错误,长度必须是6-16位字母或数字.";
                    break;
                case "-82023":
                    错误说明 = "超过最大用户数量，联系管理员升级.";
                    break;
                case "-84001":
                    错误说明 = "充值卡密填写错误,长度要等于16位.";
                    break;
                case "-84002":
                    错误说明 = "充值卡密不存在.";
                    break;
                case "-84003":
                    错误说明 = "充值卡密已被使用.";
                    break;
                case "-84004":
                    错误说明 = "充值卡密已被锁定.";
                    break;
                case "-84005":
                    错误说明 = "该卡密所属卡类为单次卡类每个用户只能充值一张.";
                    break;
                case "-85001":
                    错误说明 = "试用积分不足.";
                    break;
                case "-85002":
                    错误说明 = "试用账号不存在.";
                    break;
                case "-85003":
                    错误说明 = "试用账号已锁定.";
                    break;
                case "-85004":
                    错误说明 = "试用账号无此数据.";
                    break;
                case "-85005":
                    错误说明 = "试用已到期.";
                    break;
                case "-85006":
                    错误说明 = "试用账号使用点数不足.";
                    break;
                case "-85007":
                    错误说明 = "试用账号填写错误,长度必须是6-16位字母或数字.";
                    break;
                case "-81060":
                    错误说明 = "超过最大可激活用户数.";
                    break;
                case "-83018":
                    错误说明 = "注册码不存在或试用账号不存在或此前已被封禁.";
                    break;
                case "-82024":
                    错误说明 = "用户不存在或者密码错误或此前已被封禁.";
                    break;
            }

            if (错误说明 == erroID)
            {
                错误说明 = "";
            }
            return 错误说明;
        }
        private async Task<string> Post提交(string url, string post)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // 设置内容类型为 application/x-www-form-urlencoded  
                    using (HttpContent content = new StringContent(post, Encoding.UTF8, "application/x-www-form-urlencoded"))
                    {
                        // 发送 POST 请求  
                        using (HttpResponseMessage response = await client.PostAsync(url, content))
                        {
                            // 确保 HTTP 成功状态值  
                            response.EnsureSuccessStatusCode();

                            // 读取响应内容  
                            string pageData = await response.Content.ReadAsStringAsync();
                            return pageData;
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    // 捕获 HttpRequestException 异常，这通常在没有网络连接时抛出  
                    Debug.WriteLine($"网络请求异常: {e.Message}");
                    return "-71001";//自定义数据
                }
                catch (Exception e)
                {
                    // 捕获其他可能的异常  
                    Debug.WriteLine($"其他异常: {e.Message}");
                    return "-71002";//自定义数据
                }

            }
        }
        private async Task<string> 提交数据(string type, string post)
        {
            string url = 地址 + type;
            string 返回 = await Post提交(url, post);
            if (返回 == "-71001" || 返回 == "-71002")
            {
                if (线路 == 0)//网络异常的时候进行线路切换,只切换一次
                {
                    地址 = 地址2;
                    url = 地址 + type;
                    线路 = 1;
                    返回 = await Post提交(url, post);
                }
            }
            if (jm类型 == 1)
            {
                //if( 返回[..1] != "-" )//取左边第一位 .net8.0可以用
                //{
                //    返回 = RC4.解密rc4( jem , 返回 );
                //}

                if (返回.Length > 0 && 返回[0] != '-')
                {
                    返回 = RC4.解密rc4(jem, 返回);
                }
            }
            return 返回;
        }

    }
    class RC4Encryption
    {
        public string 加密rc4(string pwd, string data)
        {
            // 初始化密钥和置换盒数组
            int[] key = new int[256];
            int[] box = new int[256];
            for (int i = 0; i < 256; i++)
            {
                key[i] = pwd[i % pwd.Length];
                key[i] = key[i] << 8 | key[i]; // 将char转换为int，并左移8位
                box[i] = i;
            }

            // 置换盒置换
            for (int j = 0, i = 0; i < 256; i++)
            {
                j = (j + box[i] + key[i]) % 256;
                int tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }

            // 将UTF8字符串转换为字节数组
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] cipherBytes = new byte[dataBytes.Length];

            // RC4加密过程
            for (int a = 0, j = 0, i = 0; i < dataBytes.Length; i++)
            {
                a = (a + 1) % 256;
                j = (j + box[a]) % 256;
                int tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                int k = box[(box[a] + box[j]) % 256];

                // 加密每个字节
                cipherBytes[i] = (byte)(dataBytes[i] ^ k);
            }

            // 将加密后的字节数组转换为Base64编码的字符串
            return Convert.ToBase64String(cipherBytes);
        }
        public string 解密rc4(string pwd, string data)
        {
            // 初始化密钥和置换盒数组
            int[] key = new int[256];
            int[] box = new int[256];
            for (int i = 0; i < 256; i++)
            {
                key[i] = pwd[i % pwd.Length];
                key[i] = key[i] << 8 | key[i]; // 将char转换为int，并左移8位
                box[i] = i;
            }

            // 置换盒置换,与加密时相同
            for (int j = 0, i = 0; i < 256; i++)
            {
                j = (j + box[i] + key[i]) % 256;
                int tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }

            // 将Base64编码字符串转换为字节数组
            byte[] cipherBytes = Convert.FromBase64String(data);//这里转成字节
            byte[] dataBytes = new byte[cipherBytes.Length];

            // RC4解密过程（与加密过程相同，因为异或是对称的）
            for (int a = 0, j = 0, i = 0; i < cipherBytes.Length; i++)
            {
                a = (a + 1) % 256;
                j = (j + box[a]) % 256;
                int tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                int k = box[(box[a] + box[j]) % 256];

                // 解密每个字节（与加密时的异或操作相同）
                dataBytes[i] = (byte)(cipherBytes[i] ^ k);
            }

            // 将解密后的字节数组转换为UTF8编码的字符串
            return Encoding.UTF8.GetString(dataBytes);//在这里进行了base64解密
        }
        public string Base64加密(string data)
        {
            // 进行Base64编码
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            string base64EncodedData = Convert.ToBase64String(dataBytes);
            return base64EncodedData;
        }
        public string Base64解密(string data)
        {
            // 进行Base64解码
            byte[] base64DecodedBytes = Convert.FromBase64String(data);
            string decodedData = Encoding.UTF8.GetString(base64DecodedBytes);
            return decodedData;
        }
    }
}
