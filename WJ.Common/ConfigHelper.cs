using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;

namespace WJ.Common
{
    public class ConfigHelper
    {
        #region 单列模式
        private static ConfigHelper _instance = null;

        private ConfigHelper() { }

        public static ConfigHelper Instance
        {
            get
            {
                if (_instance == null)
                    lock ("SiteConfigService")
                        if (_instance == null)
                            _instance = new ConfigHelper();

                return _instance;
            }
        }
        #endregion

        #region 变量
        private const string _xmlPath = "/App_Data/WebSiteConfig.xml";
        #endregion

        #region 属性
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> WebSiteConfig { set; get; }
        #endregion

        #region 填充网站配置信息
        /// <summary>
        /// 填充回复用户消息
        /// </summary>
        /// <param name="dictUserMessage"></param>
        public void FillWebSiteConfig()
        {
            WebSiteConfig = null;
            try
            {
                Cache cache = System.Web.HttpRuntime.Cache;
                if (cache["WebSiteConfig"] == null)
                {
                    lock ("WebSiteConfig")
                    {
                        if (cache["WebSiteConfig"] == null)
                        {
                            WebSiteConfig = new Dictionary<string, string>();
                            string filePath = HttpContext.Current.Server.MapPath(_xmlPath);
                            XElement xml = XElement.Load(filePath);
                            foreach (XElement ele in xml.Elements("Config").Elements())
                            {
                                string key = ele.Attribute("key").Value;
                                string value = ele.Attribute("value").Value;

                                if (!WebSiteConfig.ContainsKey(key))
                                {
                                    WebSiteConfig.Add(key, value);
                                }
                            }

                            CacheDependency cdy = new CacheDependency(filePath, DateTime.Now);    // 跟踪文件缓存，如果文件更改自动更新缓存
                            System.Web.HttpRuntime.Cache.Insert("WebSiteConfig", WebSiteConfig, cdy);
                            //System.Web.HttpRuntime.Cache.Insert("UserMessage", dictUserMessage, cdy, DateTime.Now.AddMonths(1), TimeSpan.Zero, CacheItemPriority.Low, RemoveCallback);
                        }
                        else
                        {
                            WebSiteConfig = cache["WebSiteConfig"] as Dictionary<string, string>;
                        }
                    }
                }
                else
                {
                    WebSiteConfig = cache["WebSiteConfig"] as Dictionary<string, string>;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Instance.Debuglog("【网站配置文件异常】：" + ex.Message, "_WebSiteConfig.txt");
            }
        }

        #region 暂未实现方法
        /// <summary>
        /// 缓存删除之前调用方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reason"></param>
        /// <param name="expensiveObject"></param>
        /// <param name="dependency"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        public void UpdateCallback(string key, CacheItemUpdateReason reason, out object expensiveObject, out CacheDependency dependency, out DateTime absoluteExpiration, out TimeSpan slidingExpiration)
        {
            // key:要从缓存中移除的项的标识符。
            // reason:要从缓存中移除项的原因。
            // expensiveObject:此方法返回时，包含含有更新的缓存项对象。
            // dependency:此方法返回时，包含定义项对象和文件、缓存键、文件或缓存键的数组或另一个 System.Web.Caching.CacheDependency 对象之间的依赖项的对象。
            // absoluteExpiration:此方法返回时，包含对象的到期时间。
            // slidingExpiration:此方法返回时，包含对象的上次访问时间和对象的到期时间之间的时间间隔。
            expensiveObject = null;
            dependency = null;
            absoluteExpiration = DateTime.Now;
            slidingExpiration = TimeSpan.Zero;
            Console.WriteLine("缓存马上被移除!");
            //但是现在还有没有呢？输出试试
            Console.WriteLine(HttpRuntime.Cache["DD"]);
            Console.WriteLine(reason.ToString());
        }

        /// <summary>
        /// 缓存删除之后调用方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="reason"></param>
        public void RemoveCallback(string key, object value, CacheItemRemovedReason reason)
        {

        }
        #endregion
        #endregion
    }
}