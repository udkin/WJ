using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;

namespace WJ.DataService
{
    public class WebSocketController
    {
        private bool wsStatus = false;
        private WebSocketServer server = null;
        private IWebSocketConnection cartSockets = null;
        private IWebSocketConnection shoppingSockets = null;
        private IWebSocketConnection gameSockets = null;
        #region 委托事件
        /// <summary>
        /// 游戏页面跳转委托
        /// </summary>
        /// <param name="orderNo"></param>
        public delegate void SendGameEventHandler(string orderNo, string gameLevel, string gamePatternQuantity, string gameBlockQuantity);
        /// <summary>
        /// 游戏页面跳转委托
        /// </summary>
        public event SendGameEventHandler SendGameEvent;
        #endregion
        #region 单例模式
        private static WebSocketController _instance = null;

        public static WebSocketController Instance
        {
            get
            {
                if (_instance == null)
                    lock ("WebSocketController")
                        if (_instance == null)
                            _instance = new WebSocketController();
                return _instance;
            }
        }

        private WebSocketController()
        {
            server = new WebSocketServer("ws://127.0.0.1:8088");
        }
        #endregion

        #region 启动服务
        /// <summary>
        /// 启用服务
        /// </summary>
        public void Start()
        {
            FleckLog.Level = LogLevel.Info;
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    wsStatus = true;
                };
                socket.OnClose = () =>
                {
                    wsStatus = false;
                    if (cartSockets != null)
                    {
                        cartSockets.Close();
                        cartSockets = null;
                    }
                    if(shoppingSockets != null)
                    {
                        shoppingSockets.Close();
                        shoppingSockets = null;
                    }
                    if (gameSockets != null)
                    {
                        gameSockets.Close();
                        gameSockets = null;
                    }
                };
                socket.OnMessage = message =>
                {
                    if (message.Substring(0,1) == "{")
                    {
                        dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(message);

                        string action = jsonObj.Action;// 任务编号
                        switch (action)
                        {
                            case "CreateOrder":
                                shoppingSockets = socket;
                                BusinessController.Instance.CreateServerOrder(jsonObj);
                                break;
                            case "CreateGameOrder":
                                gameSockets = socket;
                                BusinessController.Instance.CreateServerGameOrder(jsonObj);
                                break;
                            case "GameWin":
                                BusinessController.Instance.GameWin(jsonObj);
                                break;
                        }
                        gameSockets = socket;
                    }
                    else
                    {
                        string msg = string.Empty;
                        switch (message)
                        {
                            case "FaceTest":
                                msg = FaceTest();
                                break;
                            case "Remote":
                                ActivateRemote();
                                break;
                            case "Reset":
                                System.Windows.Forms.Application.Exit();
                                break;
                            case "VipCoupon":
                                cartSockets = socket;
                                break;
                            case "Game":// 游戏
                                gameSockets = socket;
                                break;
                        }
                        socket.Send(msg);
                    }
                };
                #region 接收二进制流
                //socket.OnBinary = file =>
                //{
                //    string path = ("D:/test.txt");
                //    //创建一个文件流
                //    FileStream fs = new FileStream(path, FileMode.Create);
                //    //将byte数组写入文件中
                //    fs.Write(file, 0, file.Length);
                //    //所有流类型都要关闭流，否则会出现内存泄露问题
                //    fs.Close();
                //}; 
                #endregion
            });
        }
        #endregion

        #region 会员登录
        /// <summary>
        /// 向购物车页面发送会员登录成功结果
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public void CartWebSocketSend(string text)
        {
            try
            {
                if (cartSockets != null && text != null && text != "")
                {
                    cartSockets.Send(text);
                }
            }
            catch { }
        }
        #endregion

        #region 皮肤测试
        /// <summary>
        /// 皮肤测试
        /// </summary>
        public string FaceTest()
        {
            try
            {
                string vid = Config.FaceVid;
                string imageFolder = Config.GetConfigValue("FaceTemp");

                float[] mf = null;//皮肤值肤色值，（肤色值、水分值、油分值、纹理值、皱纹值、毛孔值）
                int vague = 0;//皮肤图片是否模糊，1清晰；0模糊
                bool flag = TesterController.Instance.Analyze(imageFolder, ref vague, ref mf);

                if (flag && vague == 1)
                {
                    return mf[0] + "," + mf[1] + "," + mf[2] + "," + mf[3] + "," + mf[4];
                }
            }
            catch (Exception ex)
            {
                LogHelper.Debuglog("调用皮肤测试仪异常：" + ex.ToString(), LogType.Controller);
            }
            return "";
        }
        #endregion

        #region 启动远程协助程序
        public void ActivateRemote()
        {
            string filePath = @"D:\面膜机\TeamViewer.exe";
            Process[] proc = Process.GetProcessesByName("TeamViewer");
            if (proc.Length == 0 && System.IO.File.Exists(filePath))
            {
                var p = Process.Start(filePath);     //要开启的进程（或要启用的程序），括号内为绝对路径
                if (p.Start())
                {
                    SetForegroundWindow(p.MainWindowHandle);
                }
            }
            else
            {
                SetForegroundWindow(proc[0].MainWindowHandle);
            }
        }

        /// <summary>
        /// 设置程序为置顶程序
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion

        #region 正常购买
        /// <summary>
        /// 订单支付二维码
        /// </summary>
        /// <param name="qrCode"></param>
        public void ShoppingRCode(string qrCode)
        {
            try
            {
                if (shoppingSockets != null && string.IsNullOrWhiteSpace(qrCode) == false)
                {
                    shoppingSockets.Send(qrCode);
                }
            }
            catch { }
        }
        #endregion

        #region 游戏赢取
        /// <summary>
        /// 显示游戏支付二维码
        /// </summary>
        /// <param name="qrCode"></param>
        public void GameQRCode(string qrCode)
        {
            try
            {
                if (gameSockets != null && string.IsNullOrWhiteSpace(qrCode) == false)
                {
                    gameSockets.Send(qrCode);
                }
            }
            catch { }
        }

        /// <summary>
        /// 游戏支付结果，默认成功，否则不回调
        /// </summary>
        public void GamePayResult(string result)
        {
            try
            {
                dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(result);
                if (jsonObj.Status == true)
                {
                    string gamePatternQuantity = jsonObj.GamePatternQuantity;// 游戏使用几组图案
                    string gameLevel = jsonObj.GameLevell;// 消除方块，0随机，1最少
                    string gameBlockQuantity = jsonObj.GameBlockQuantity;// 显示几种方块，默认7
                    string orderNo = jsonObj.OrderNo.ToString();
                    SendGameEvent(orderNo, gameLevel, gamePatternQuantity, gameBlockQuantity);
                }else{
                    gameSockets.Send(result);
                }
            }
            catch (Exception ex){ 
                
            }
        }
        #endregion
    }
}
