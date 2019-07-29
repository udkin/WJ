using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJ.Common;
using WJ.Entity;

namespace WJ.Service
{
    public class AppService : DbContext<WJ_T_App>
    {
        #region 单列模式
        private static AppService _instance = null;

        private AppService() { }

        public static AppService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("AppService")
                        if (_instance == null)
                            _instance = new AppService();

                return _instance;
            }
        }
        #endregion

        #region 获取应用下拉列表
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic GetApp()
        {
            try
            {
                return DbInstance.Queryable<WJ_T_App>().Where(p => p.App_State == 1).OrderBy(p => p.App_Sort).Select(f => new { f.Id, f.App_Name }).ToList();
            }
            catch (Exception ex)
            {
                Common.LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取应用列表信息
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<WJ_V_App> GetList(JObject data, ref int totalCount)
        {
            try
            {
                int pageIndex = data["page"].ToObject<int>();
                int pageSize = data["limit"].ToObject<int>();
                string name = (data["App_Name"] == null ? "" : data["App_Name"].ToString().Trim());

                var queryable = DbInstance.Queryable<WJ_V_App>().Where(p => p.App_State == 1)
                    .WhereIF(!string.IsNullOrWhiteSpace(name), p => p.App_Name.Contains(name))
                    .OrderBy(p => p.App_Sort)
                    .ToPageList(pageIndex, pageSize, ref totalCount);

                return queryable;
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion

        #region 增加应用信息
        /// <summary>
        /// 增加应用信息
        /// </summary>
        /// <param name="data">提交的表单数据</param>
        /// <param name="errorMsg">错误信息</param>
        /// <returns></returns>
        public bool Add(int userId, JObject data, ref string errorMsg)
        {
            try
            {
                if (IsExits(p => p.App_Name == data["App_Name"].ToString().Trim() && p.App_State == 1))
                {
                    errorMsg = "存在相同应用名称";
                }
                else
                {
                    WJ_T_App app = new WJ_T_App();
                    app.AppClassId = data["AppClassId"] != null ? data["AppClassId"].ToObject<int>() : 1;
                    app.App_Name = data["App_Name"] != null ? data["App_Name"].ToString() : "";
                    app.App_Icon = data["App_Icon"] != null ? data["App_Icon"].ToString() : "";
                    app.App_Type = data["App_Type"].ToObject<int>();
                    app.App_Flag = data["App_Flag"].ToObject<int>();
                    app.App_Sort = data["App_Sort"] != null ? data["App_Sort"].ToObject<int>() : 1;

                    if (data["App_Type"].ToObject<int>() == 0)
                    {
                        app.App_LoginUrl = data["App_LoginUrl"] != null ? data["App_LoginUrl"].ToString() : "";
                        app.App_HomeUrl = data["App_HomeUrl"] != null ? data["App_HomeUrl"].ToString() : "";
                        app.App_Method = data["App_Method"] != null ? data["App_Method"].ToString() : "";
                        app.App_Paramater = data["App_Paramater"] != null ? data["App_Paramater"].ToString() : "";
                        app.App_Form = data["App_Form"] != null ? data["App_Form"].ToString() : "";
                        app.App_LoginName = data["App_LoginName"] != null ? data["App_LoginName"].ToString() : "";
                        app.App_Password = data["App_Password"] != null ? data["App_Password"].ToString() : "";
                        app.App_BrowserType = data["App_BrowserType"] != null ? data["App_BrowserType"].ToObject<int>() : 2;
                    }

                    app.App_Creator = userId;
                    app.App_CreateTime = DateTime.Now;
                    app.App_State = 1;

                    if (Add(app))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }
            return false;
        }
        #endregion

        #region 更新应用信息
        /// <summary>
        /// 更新应用信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(JObject data, ref string errorMsg)
        {
            try
            {
                int id = data["Id"].ToObject<int>();
                if (IsExits(p => p.Id != id && p.App_Name == data["App_Name"].ToString().Trim() && p.App_State == 1))
                {
                    errorMsg = "存在相同应用名称";
                }
                else
                {
                    WJ_T_App app = GetSingle(p => p.Id == id);
                    app.AppClassId = data["AppClassId"] != null ? data["AppClassId"].ToObject<int>() : 1;
                    app.App_Name = data["App_Name"] != null ? data["App_Name"].ToString() : "";
                    app.App_Icon = data["App_Icon"] != null ? data["App_Icon"].ToString() : "";
                    app.App_Type = data["App_Type"].ToObject<int>();
                    app.App_Flag = data["App_Flag"].ToObject<int>();
                    app.App_Sort = data["App_Sort"] != null ? data["App_Sort"].ToObject<int>() : 1;

                    if (data["App_Type"].ToObject<int>() == 0)
                    {
                        app.App_LoginUrl = data["App_LoginUrl"].ToString();
                        app.App_HomeUrl = data["App_HomeUrl"].ToString();
                        app.App_Method = data["App_Method"].ToString();
                        app.App_Paramater = data["App_Paramater"].ToString();
                        app.App_Form = data["App_Form"].ToString();
                        app.App_LoginName = data["App_LoginName"].ToString();
                        app.App_Password = data["App_Password"].ToString();
                        app.App_BrowserType = data["App_BrowserType"].ToObject<int>();
                    }

                    return Update(app);
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }
            return false;
        }
        #endregion

        #region 删除应用信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(int id, ref string errorMsg)
        {
            try
            {
                using (SqlSugarClient db = DbInstance)
                {
                    if (db.Queryable<WJ_T_UserApp>().Any(p => p.AppId == id && p.UserApp_State == 1))
                    {
                        errorMsg = "应用已被用户使用不能删除";
                    }
                    else
                    {
                        return db.Updateable<WJ_T_App>(p => p.App_State == -1).Where(p => p.Id == id).ExecuteCommand() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rimaryList"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(List<int> rimaryList, ref string errorMsg)
        {
            try
            {
                using (SqlSugarClient db = DbInstance)
                {
                    if (db.Queryable<WJ_T_UserApp>().Any(p => rimaryList.Contains<int>(p.AppId) && p.UserApp_State == 1))
                    {
                        errorMsg = "应用已被用户使用不能删除";
                    }
                    else
                    {
                        return db.Updateable<WJ_T_App>(p => p.App_State == -1).Where(p => rimaryList.Contains<int>(p.Id)).ExecuteCommand() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }
            return false;
        }
        #endregion
    }
}
