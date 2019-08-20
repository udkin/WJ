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
                using (var db = DbInstance)
                {
                    return db.Queryable<WJ_T_App>().Where(p => p.App_State == 1).OrderBy(p => p.App_Sort).Select(f => new { f.Id, f.App_Name }).ToList();
                }
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
        /// <param name="data"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<WJ_V_App> GetList(JObject data, ref int totalCount)
        {
            try
            {
                using (var db = DbInstance)
                {
                    int pageIndex = data["page"].ToObject<int>();
                    int pageSize = data["limit"].ToObject<int>();
                    string appClassName = data["AppClass_Name"] == null ? "" : data["AppClass_Name"].ToString().Trim();
                    string appName = data["App_Name"] == null ? "" : data["App_Name"].ToString().Trim();
                    int appState = data["App_State"] == null ? 0 : data["App_State"].ToObject<int>();

                    var queryable = db.Queryable<WJ_V_App>()
                        .WhereIF(!string.IsNullOrWhiteSpace(appClassName), p => p.AppClass_Name.Contains(appClassName))
                        .WhereIF(!string.IsNullOrWhiteSpace(appName), p => p.App_Name.Contains(appName))
                        .WhereIF(appState == 0, p => p.App_State != 30)
                        .WhereIF(appState > 0, p => p.App_State == appState)
                        .OrderBy(p => p.App_Sort)
                        .ToPageList(pageIndex, pageSize, ref totalCount);

                    return queryable;
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取应用详细信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public WJ_V_App GetAppInfo(int appId)
        {
            try
            {
                return DbInstance.Queryable<WJ_V_App>().Single(p => p.Id == appId);
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion

        #region 更新应用信息
        /// <summary>
        /// 更新应用信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Update(int userId, JObject data, ref string errorMsg)
        {
            try
            {
                BeginTran();

                int appId = data["Id"].ToObject<int>();
                string appName = data["App_Name"].ToString().Trim();

                if (AppTempService.Instance.IsExits(p => p.AppTemp_Name == appName && p.AppTemp_State == 0) || IsExits(p => p.Id != appId && p.App_Name == appName && p.App_State != 30))
                {
                    errorMsg = "存在相同应用名称";
                    RollbackTran();
                }
                else
                {
                    WJ_T_AppTemp appTemp = new WJ_T_AppTemp();
                    appTemp.AppId = appId;
                    appTemp.AppClassId = data["AppClassId"] != null ? data["AppClassId"].ToObject<int>() : 1;
                    appTemp.AppTemp_Name = data["App_Name"] != null ? data["App_Name"].ToString() : "";
                    appTemp.AppTemp_Icon = data["App_Icon"] != null ? data["App_Icon"].ToString() : "";
                    appTemp.AppTemp_Type = data["App_Type"].ToObject<int>();
                    appTemp.AppTemp_Flag = data["App_Flag"].ToObject<int>();
                    appTemp.AppTemp_LoginUrl = data["App_LoginUrl"].ToString();
                    appTemp.AppTemp_HomeUrl = data["App_HomeUrl"].ToString();
                    appTemp.AppTemp_Method = data["App_Method"].ToString();
                    appTemp.AppTemp_Paramater = data["App_Paramater"].ToString();
                    appTemp.AppTemp_Form = data["App_Form"].ToString();
                    appTemp.AppTemp_LoginName = data["App_LoginName"].ToString();
                    appTemp.AppTemp_Password = data["App_Password"].ToString();
                    appTemp.AppTemp_BrowserType = data["App_BrowserType"].ToObject<int>();
                    appTemp.AppTemp_Sort = data["App_Sort"] != null ? data["App_Sort"].ToObject<int>() : 1;
                    appTemp.AppTemp_Creator = userId;
                    appTemp.AppTemp_CreateTime = DateTime.Now;
                    appTemp.AppTemp_DataType = 1;
                    appTemp.AppTemp_State = 0;

                    int appTempId = AppTempService.Instance.AddReturnIdentity(appTemp);

                    if (appTempId > 0)
                    {
                        WJ_T_Audit audit = new WJ_T_Audit();
                        audit.App_Name = appTemp.AppTemp_Name;
                        audit.AppId = appId;
                        audit.AppTempId = appTempId;
                        audit.Audit_Applicant = userId;
                        audit.Audit_ApplyTime = DateTime.Now;
                        audit.Audit_Type = 2;
                        audit.Audit_State = 0;

                        bool flag = AuditService.Instance.Add(audit);
                        if (flag && Update(new { App_AuditState = 0, Id = appId }))
                        {
                            CommitTran();
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }

            RollbackTran();
            return false;
        }
        #endregion

        #region 删除应用信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int userId, JObject data)
        {
            try
            {
                BeginTran();

                int appId = data["Id"].ToObject<int>();
                var app = GetSingle(p => p.Id == appId);
                bool flag = Update(new { App_AuditState = 0, Id = app.Id });

                WJ_T_Audit audit = new WJ_T_Audit();
                audit.App_Name = app.App_Name;
                audit.AppId = app.Id;
                audit.Audit_Applicant = userId;
                audit.Audit_ApplyTime = DateTime.Now;
                audit.Audit_Type = 3;
                audit.Audit_State = 0;

                if (flag && AuditService.Instance.Add(audit))
                {
                    CommitTran();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }

            RollbackTran();
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
                using (var db = DbInstance)
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

        #region 上架应用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public bool AppUp(int userId, int appId)
        {
            try
            {
                BeginTran();

                var app = GetSingle(p => p.Id == appId);

                WJ_T_Audit audit = new WJ_T_Audit();
                audit.App_Name = app.App_Name;
                audit.AppId = app.Id;
                audit.Audit_Applicant = userId;
                audit.Audit_ApplyTime = DateTime.Now;
                audit.Audit_Type = 4;
                audit.Audit_State = 0;

                bool flag = Update(new { App_AuditState = 0, Id = app.Id });

                if (flag && AuditService.Instance.Add(audit))
                {
                    CommitTran();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }

            RollbackTran();
            return false;
        }
        #endregion

        #region 下架应用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public bool AppDown(int userId, int appId)
        {
            try
            {
                BeginTran();

                var app = GetSingle(p => p.Id == appId);

                WJ_T_Audit audit = new WJ_T_Audit();
                audit.App_Name = app.App_Name;
                audit.AppId = app.Id;
                audit.Audit_Applicant = userId;
                audit.Audit_ApplyTime = DateTime.Now;
                audit.Audit_Type = 5;
                audit.Audit_State = 0;

                bool flag = Update(new { App_AuditState = 0, Id = app.Id });

                if (flag && AuditService.Instance.Add(audit))
                {
                    CommitTran();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }

            RollbackTran();
            return false;
        }
        #endregion
    }
}
