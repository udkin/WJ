using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///应用表
    ///</summary>
    public partial class WJ_T_App
    {
           public WJ_T_App(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Id {get;set;}

           /// <summary>
           /// Desc:应用分类ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int AppClassId {get;set;}

           /// <summary>
           /// Desc:应用名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string App_Name {get;set;}

           /// <summary>
           /// Desc:应用图标
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string App_Image {get;set;}

           /// <summary>
           /// Desc:应用链接URL
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string App_Url {get;set;}

           /// <summary>
           /// Desc:应用登录用户名
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string App_LoginUser {get;set;}

           /// <summary>
           /// Desc:应用登录密码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string App_Password {get;set;}

           /// <summary>
           /// Desc:浏览器类型，1：IE，2：Chrome
           /// Default:1
           /// Nullable:False
           /// </summary>           
           public int App_BrowserType {get;set;}

           /// <summary>
           /// Desc:应用状态，-1：废弃，1：正常
           /// Default:1
           /// Nullable:True
           /// </summary>           
           public int? App_State {get;set;}

    }
}
