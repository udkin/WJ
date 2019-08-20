using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJ.Common;
using WJ.Entity;

namespace WJ.Service
{
    public class AppTempService : DbContext<WJ_T_AppTemp>
    {
        #region 单列模式
        private static AppTempService _instance = null;

        private AppTempService() { }

        public static AppTempService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("AppTempService")
                        if (_instance == null)
                            _instance = new AppTempService();

                return _instance;
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
            using (var db = DbInstance)
            {
                try
                {
                    db.Ado.BeginTran();

                    string appName = data["App_Name"].ToString().Trim();
                    if (IsExits(p => p.AppTemp_Name == appName && p.AppTemp_State == 0) || AppService.Instance.IsExits(p=>p.App_Name == appName && p.App_State != 30))
                    {
                        errorMsg = "存在相同应用名称";
                    }
                    else
                    {
                        WJ_T_AppTemp app = new WJ_T_AppTemp();
                        app.AppClassId = data["AppClassId"] != null ? data["AppClassId"].ToObject<int>() : 1;
                        app.AppTemp_Name = data["App_Name"] != null ? data["App_Name"].ToString() : "";
                        app.AppTemp_Icon = data["App_Icon"] != null ? data["App_Icon"].ToString() : "";
                        app.AppTemp_Type = data["App_Type"].ToObject<int>();
                        app.AppTemp_Flag = data["App_Flag"].ToObject<int>();
                        app.AppTemp_Sort = data["App_Sort"] != null ? data["App_Sort"].ToObject<int>() : 1;

                        if (data["App_Type"].ToObject<int>() != 0)
                        {
                            app.AppTemp_LoginUrl = data["App_LoginUrl"] != null ? data["App_LoginUrl"].ToString() : "";
                            app.AppTemp_HomeUrl = data["App_HomeUrl"] != null ? data["App_HomeUrl"].ToString() : "";
                            app.AppTemp_Method = data["App_Method"] != null ? data["App_Method"].ToString() : "";
                            app.AppTemp_Paramater = data["App_Paramater"] != null ? data["App_Paramater"].ToString() : "";
                            app.AppTemp_Form = data["App_Form"] != null ? data["App_Form"].ToString() : "";
                            app.AppTemp_LoginName = data["App_LoginName"] != null ? data["App_LoginName"].ToString() : "";
                            app.AppTemp_Password = data["App_Password"] != null ? data["App_Password"].ToString() : "";
                            app.AppTemp_BrowserType = data["App_BrowserType"] != null ? data["App_BrowserType"].ToObject<int>() : 2;
                        }
                        else
                        {
                            app.AppTemp_LoginUrl = "";
                            app.AppTemp_HomeUrl = "";
                            app.AppTemp_Method = "";
                            app.AppTemp_Paramater = "";
                            app.AppTemp_Form = "";
                            app.AppTemp_LoginName = "";
                            app.AppTemp_Password = "";
                            app.AppTemp_BrowserType = null;
                        }

                        app.AppTemp_Creator = userId;
                        app.AppTemp_CreateTime = DateTime.Now;
                        app.AppTemp_DataType = 0;
                        app.AppTemp_State = 0;

                        int appTempId = AddReturnIdentity(app);
                        if (appTempId > 0)
                        {
                            WJ_T_Audit audit = new WJ_T_Audit();
                            audit.App_Name = app.AppTemp_Name;
                            audit.AppTempId = appTempId;
                            audit.Audit_Applicant = userId;
                            audit.Audit_ApplyTime = DateTime.Now;
                            audit.Audit_Type = 1;
                            audit.Audit_State = 0;

                            if(AuditService.Instance.Add(audit))
                            {
                                db.Ado.CommitTran();
                                return true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.DbServiceLog(ex.Message);
                }

                db.Ado.RollbackTran();
                return false;
            }
        }
        #endregion
    }
}
