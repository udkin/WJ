using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WJ.Common;
using WJ.Entity;

namespace WJ.Service
{
    public class UserPlanAppService : DbContext<WJ_T_UserPlanApp>
    {
        #region 单列模式
        private static UserPlanAppService _instance = null;

        private UserPlanAppService() { }

        public static UserPlanAppService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("PlanService")
                        if (_instance == null)
                            _instance = new UserPlanAppService();

                return _instance;
            }
        }
        #endregion

        #region 增加
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        public bool AddUserPlanApp(int userId, dynamic jsonObj)
        {
            using (SqlSugarClient db = DbHelper.GetInstance())
            {
                try
                {
                    db.BeginTran();

                    WJ_T_UserPlan plan = new WJ_T_UserPlan()
                    {
                        UserId = userId,
                        UserPlan_Name = jsonObj.PlanName.ToString(),
                        UserPlan_Layout = jsonObj.PlanLayout.ToString(),
                        UserPlan_State = 1,
                        UserPlan_CreateTime = DateTime.Now,
                        UserPlan_UseCount = 0
                    };

                    int planId = db.Insertable(plan).ExecuteReturnIdentity();

                    if (planId <= 0)
                    {
                        db.RollbackTran();
                        return false;
                    }

                    List<WJ_T_UserPlanApp> userPlanAppList = new List<WJ_T_UserPlanApp>();

                    foreach (var item in jsonObj.AppList)
                    {
                        WJ_T_UserPlanApp userPlanApp = new WJ_T_UserPlanApp()
                        {
                            UserPlanId = planId,
                            AppId = Convert.ToInt32(item.AppId),
                            UserPlanApp_Location = item.AppLocation.ToString(),
                            UserPlanApp_State = 1
                        };
                        userPlanAppList.Add(userPlanApp);
                    }

                    if (db.Insertable(userPlanAppList.ToArray()).ExecuteCommand() == userPlanAppList.Count)
                    {
                        db.CommitTran();
                        return true;
                    }
                    else
                    {
                        db.RollbackTran();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    LogHelper.ErrorLog(ex.Message);
                    db.RollbackTran();
                    return false;
                }
            }

        }
        #endregion

        #region 更新
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        public bool UpdateUserPlan(int userId, dynamic jsonObj)
        {
            using (SqlSugarClient db = DbHelper.GetInstance())
            {
                try
                {
                    db.BeginTran();

                    int userPlanId = Convert.ToInt32(jsonObj.PlanId);
                    if (userPlanId <= 0)
                    {
                        db.RollbackTran();
                        return false;
                    }

                    db.Deleteable<WJ_T_UserPlanApp>().Where(it => it.UserPlanId == userPlanId).ExecuteCommand();//删除所有方案应用

                    WJ_T_UserPlan plan = new WJ_T_UserPlan()
                    {
                        Id = userPlanId,
                        UserId = userId,
                        UserPlan_Name = jsonObj.PlanName.ToString(),
                        UserPlan_Layout = jsonObj.PlanLayout.ToString()
                    };

                    //db.Updateable(plan).Where(p => p.Id == userPlanId).ExecuteCommand();//更新用户方案信息
                    db.Updateable(plan).ExecuteCommand();//更新用户方案信息

                    List<WJ_T_UserPlanApp> userPlanAppList = new List<WJ_T_UserPlanApp>();

                    foreach (var item in jsonObj.AppList)
                    {
                        WJ_T_UserPlanApp userPlanApp = new WJ_T_UserPlanApp()
                        {
                            UserPlanId = userPlanId,
                            AppId = Convert.ToInt32(item.AppId),
                            UserPlanApp_Location = item.AppLocation.ToString(),
                            UserPlanApp_State = 1
                        };
                        userPlanAppList.Add(userPlanApp);
                    }

                    if (db.Insertable(userPlanAppList.ToArray()).ExecuteCommand() == userPlanAppList.Count)
                    {
                        db.CommitTran();
                        return true;
                    }
                    else
                    {
                        db.RollbackTran();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    LogHelper.ErrorLog(ex.Message);
                    db.RollbackTran();
                    return false;
                }
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        public bool DeleteUserPlan(int userId, dynamic jsonObj)
        {
            using (SqlSugarClient db = DbHelper.GetInstance())
            {
                try
                {
                    db.BeginTran();

                    int userPlanId = Convert.ToInt32(jsonObj.PlanId);
                    if (userPlanId <= 0)
                    {
                        db.RollbackTran();
                        return false;
                    }

                    db.Updateable<WJ_T_UserPlan>().SetColumns(p => new WJ_T_UserPlan() { UserPlan_State = -1 }).Where(p => p.Id == userPlanId).ExecuteCommand();//更新用户方案信息
                    db.Updateable<WJ_T_UserPlanApp>().SetColumns(p => new WJ_T_UserPlanApp() { UserPlanApp_State = -1 }).Where(p => p.UserPlanId == userPlanId && p.UserPlanApp_State == 1).ExecuteCommand();//更新用户方案信息

                    db.CommitTran();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    LogHelper.ErrorLog(ex.Message);
                    db.RollbackTran();
                    return false;
                }
            }
        }
        #endregion

        public List<T> GetList<T>(Expression<Func<T, bool>> whereExpression)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    return db.Queryable<T>().Where(whereExpression).ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.ErrorLog(ex.Message);
                return null;
            }
        }
    }
}
