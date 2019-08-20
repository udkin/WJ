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
    public class UserAppService : DbContext<WJ_T_UserApp>
    {
        #region 单列模式
        private static UserAppService _instance = null;

        private UserAppService() { }

        public static UserAppService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("UserAppService")
                        if (_instance == null)
                            _instance = new UserAppService();

                return _instance;
            }
        }
        #endregion

        #region 对外接口服务
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public List<WJ_V_UserApp> GetUserAppList(int userId)
        {
            try
            {
                using (var db = DbInstance)
                {
                    return db.Queryable<WJ_V_UserApp>().Where(p => p.UserId == userId).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public dynamic GetUserAppDynamic(int userId)
        {
            try
            {
                using (var db = DbInstance)
                {
                    return db.Queryable<WJ_V_UserApp>().Where(p => p.UserId == userId).Select(f => new { f.AppClassId, f.AppId, f.App_Name, f.App_Icon, f.App_BrowserType, f.App_Type, f.App_Flag }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取用户APP信息
        /// <summary>
        /// 获取用户APP信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<WJ_V_UserApp> GetList(JObject data, ref int totalCount)
        {
            try
            {
                using (var db = DbInstance)
                {
                    int userId = data["UserId"].ToObject<int>();
                    int pageIndex = data["page"].ToObject<int>();
                    int pageSize = data["limit"].ToObject<int>();
                    return db.Queryable<WJ_V_UserApp>().Where(p => p.UserId == userId).ToPageList(pageIndex, pageSize, ref totalCount);
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion

        #region 设置用户APP信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">提交的表单数据</param>
        /// <param name="errorMsg">错误信息</param>
        /// <returns></returns>
        public bool SetUserApp(JObject data, ref string errorMsg)
        {
            using (SqlSugar.SqlSugarClient db = DbInstance)
            {
                try
                {
                    int userId = data["UserId"].ToObject<int>();

                    db.Ado.BeginTran();

                    if (userId > 0)
                    {
                        List<int> oldIdList = new List<int>();
                        List<int> newIdList = new List<int>();
                        foreach (var item in data["AppList"].ToArray())
                        {
                            newIdList.Add(item["AppId"].ToObject<int>());
                        }

                        var oldAppList = db.Queryable<WJ_T_UserApp>().Where(p => p.UserId == userId).ToList();

                        foreach (var oldApp in oldAppList)
                        {
                            oldIdList.Add(oldApp.AppId);
                            if (!newIdList.Contains(oldApp.AppId) && oldApp.UserApp_State != -1)
                            {
                                oldApp.UserApp_State = -1;
                                Update(oldApp);
                            }
                        }

                        foreach (var id in newIdList)
                        {
                            // 原有APP列表中已经存在
                            var oldApp = oldAppList.FirstOrDefault(p => p.AppId == id);
                            if (oldApp != null)
                            {
                                if (oldApp.UserApp_State == -1)
                                {
                                    oldApp.UserApp_State = 1;
                                    Update(oldApp);
                                }
                            }
                            else
                            {
                                WJ_T_UserApp userApp = new WJ_T_UserApp();
                                userApp.UserId = userId;
                                userApp.AppId = id;
                                userApp.UserApp_State = 1;

                                if (Add(userApp))
                                {
                                    db.Ado.RollbackTran();
                                    break;
                                }
                            }
                        }

                        db.Ado.CommitTran();
                        return true;
                    }
                    else
                    {
                        db.Ado.RollbackTran();
                    }
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    LogHelper.DbServiceLog(ex.Message);
                    errorMsg = ex.Message;
                }

                return false;
            }
        }
        #endregion

        #region 更新操作用户APP信息
        /// <summary>
        /// 更新操作用户信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(JObject data, ref string errorMsg)
        {
            try
            {
                int userId = data["UserId"].ToObject<int>();
                int appId = data["AppId"].ToObject<int>();

                WJ_T_UserApp userApp = GetSingle(p => p.UserId == userId && p.AppId == appId);
                if (userApp != null)
                {
                    userApp.UserApp_LoginName = data["UserApp_LoginName"].ToString();
                    userApp.UserApp_Password = data["UserApp_Password"].ToString();

                    return Update(userApp);
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.ControllerErrorLog(ex.Message);
            }
            return false;
        }
        #endregion
    }
}
