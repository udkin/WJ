using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJ.Common;
using WJ.Entity;

namespace WJ.Service
{
    public class AuditService : DbContext<WJ_T_Audit>
    {
        #region 单列模式
        private static AuditService _instance = null;

        private AuditService() { }

        public static AuditService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("AuditService")
                        if (_instance == null)
                            _instance = new AuditService();

                return _instance;
            }
        }
        #endregion

        #region 获取待审核信息列表
        /// <summary>
        /// 获取待审核信息列表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<WJ_V_Audit> GetUnAuditList(JObject data, ref int totalCount)
        {
            try
            {
                int pageIndex = data["page"].ToObject<int>();
                int pageSize = data["limit"].ToObject<int>();

                var queryable = DbInstance.Queryable<WJ_V_Audit>().Where(p => p.Audit_State == 0)
                    .WhereIF(data["App_Name"] != null && data["App_Name"].ToString().Trim() != "", p => p.App_Name.Contains(data["App_Name"].ToString()))
                    .WhereIF(data["Start_Date"] != null && data["Start_Date"].ToString().Trim() != "", p => p.Audit_ApplyTime >= data["Start_Date"].ToObject<DateTime>())
                    .WhereIF(data["End_Date"] != null && data["End_Date"].ToString().Trim() != "", p => p.Audit_ApplyTime < data["End_Date"].ToObject<DateTime>().AddDays(1))
                    .OrderBy(p => p.Audit_ApplyTime, OrderByType.Desc)
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

        #region 获取已审核信息列表
        /// <summary>
        /// 获取已审核信息列表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<WJ_V_Audit> GetAuditList(JObject data, ref int totalCount)
        {
            try
            {
                int pageIndex = data["page"].ToObject<int>();
                int pageSize = data["limit"].ToObject<int>();

                var queryable = DbInstance.Queryable<WJ_V_Audit>().Where(p => p.Audit_State > 0)
                    .WhereIF(data["App_Name"] != null && data["App_Name"].ToString().Trim() != "", p => p.App_Name.Contains(data["App_Name"].ToString()))
                    .WhereIF(data["Start_Date"] != null && data["Start_Date"].ToString().Trim() != "", p => p.Audit_ApplyTime >= data["Start_Date"].ToObject<DateTime>())
                    .WhereIF(data["End_Date"] != null && data["End_Date"].ToString().Trim() != "", p => p.Audit_ApplyTime < data["End_Date"].ToObject<DateTime>().AddDays(1))
                    .OrderBy(p => p.Audit_ApplyTime, OrderByType.Desc)
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

        #region 通过应用审核
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public bool PassAudit(int userId, int auditId)
        {
            using (var db = DbInstance)
            {
                try
                {
                    BeginTran();
                    bool flag = false;

                    var audit = GetSingle(p => p.Id == auditId);
                    WJ_T_AppTemp appTemp = null;
                    WJ_T_App app = null;

                    if (audit.Audit_Type == 1)
                    {
                        app = new WJ_T_App();
                    }
                    else
                    {
                        app = AppService.Instance.GetById(audit.AppId.Value);
                    }

                    if (audit.Audit_Type <= 2)
                    {
                        appTemp = AppTempService.Instance.GetSingle(p => p.Id == audit.AppTempId.Value);
                        app.AppClassId = appTemp.AppClassId;
                        app.App_Name = appTemp.AppTemp_Name;
                        app.App_Icon = appTemp.AppTemp_Icon.Replace("t_", "");
                        app.App_Type = appTemp.AppTemp_Type;
                        app.App_Flag = appTemp.AppTemp_Flag;

                        if (app.App_Type != 0)
                        {
                            app.App_LoginUrl = appTemp.AppTemp_LoginUrl;
                            app.App_HomeUrl = appTemp.AppTemp_HomeUrl;
                            app.App_Method = appTemp.AppTemp_Method;
                            app.App_Paramater = appTemp.AppTemp_Paramater;
                            app.App_Form = appTemp.AppTemp_Form;
                            app.App_LoginName = appTemp.AppTemp_LoginName;
                            app.App_Password = appTemp.AppTemp_Password;
                            app.App_BrowserType = appTemp.AppTemp_BrowserType;
                        }
                        else
                        {
                            app.App_LoginUrl = "";
                            app.App_HomeUrl = "";
                            app.App_Method = "";
                            app.App_Paramater = "";
                            app.App_Form = "";
                            app.App_LoginName = "";
                            app.App_Password = "";
                            app.App_BrowserType = null;
                        }

                        app.App_Sort = appTemp.AppTemp_Sort;

                        if (audit.Audit_Type == 1)
                        {
                            app.App_Creator = appTemp.AppTemp_Creator;
                            app.App_CreateTime = DateTime.Now;
                        }

                        //flag = AppTempService.Instance.Update(new { AppTemp_State = 1, Id = audit.AppTempId.Value });
                        flag = AppTempService.Instance.UpdateEx(p => new WJ_T_AppTemp() { AppTemp_State = 1 }, p => p.Id == appTemp.Id && p.AppTemp_State == 0);
                    }
                    else
                    {
                        flag = true;
                    }

                    app.App_AuditState = 1;
                    app.App_State = audit.Audit_Type * 10;

                    if (audit.Audit_Type == 1)
                    {
                        flag = flag && AppService.Instance.Add(app);
                    }
                    else
                    {
                        if (audit.Audit_Type == 2)
                        {
                            flag = flag && AppService.Instance.Update(app);
                        }
                        else
                        {
                            flag = flag && AppService.Instance.Update(new { App_AuditState = app.App_AuditState, App_State = app.App_State, Id = app.Id });
                        }
                    }

                    if (flag && UpdateEx(p => new WJ_T_Audit() { Audit_State = 1, Audit_Approver = userId, Audit_Approval_Time = DateTime.Now }, p => p.Id == auditId && p.Audit_State == 0))
                    {
                        if (!string.IsNullOrWhiteSpace(app.App_Icon))
                        {
                            string oldFileName = AppDomain.CurrentDomain.BaseDirectory + "Store\\Image\\" + "t_" + app.App_Icon;
                            string newFileName = AppDomain.CurrentDomain.BaseDirectory + "Store\\Image\\" + app.App_Icon;
                            FileInfo fi = new FileInfo(oldFileName);
                            fi.MoveTo(newFileName);
                        }

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
        }
        #endregion

        #region 驳回应用审核
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public bool RejectAudit(int userId, int auditId)
        {
            try
            {
                BeginTran();
                bool flag = false;

                var audit = GetSingle(p => p.Id == auditId);

                if (audit.Audit_Type > 1)
                {
                    flag = AppService.Instance.Update(new { App_AuditState = 1, Id = audit.AppId.Value });
                }
                else
                {
                    flag = true;
                }

                if (audit.AppTempId != null)
                {
                    flag = flag && AppTempService.Instance.Update(new { AppTemp_State = 1, Id = audit.AppTempId.Value });
                }

                if (flag && UpdateEx(p => new WJ_T_Audit() { Audit_State = 2, Audit_Approver = userId, Audit_Approval_Time = DateTime.Now }, p => p.Id == auditId && p.Audit_State == 0))
                {
                    CommitTran();
                    return true;
                }
                else
                {
                    RollbackTran();
                    return false;
                }
            }
            catch (Exception ex)
            {
                RollbackTran();
                LogHelper.DbServiceLog(ex.Message);
                return false;
            }
        }
        #endregion

        #region 返回首页最新应用操作日志
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<WJ_V_Audit> GetTopAuditList(int top)
        {
            try
            {
                return DbInstance.Queryable<WJ_V_Audit>()
                .OrderBy(p => p.Audit_ApplyTime, OrderByType.Desc)
                .Take(top)
                .ToList();
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
