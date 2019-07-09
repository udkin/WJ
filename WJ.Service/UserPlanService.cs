using SqlSugar;
using System;
using System.Collections.Generic;
using WJ.Common;
using WJ.Entity;

namespace WJ.Service
{
    public class UserPlanService : DbContext<WJ_T_UserPlan>
    {
        #region 单列模式
        private static UserPlanService _instance = null;

        private UserPlanService() { }

        public static UserPlanService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("PlanService")
                        if (_instance == null)
                            _instance = new UserPlanService();

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
        public bool AddUserPlan(int userId, dynamic jsonObj)
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
        /// 更新方案信息
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

        /// <summary>
        /// 更新方案活动标志
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userPandId"></param>
        /// <returns></returns>
        public bool UpdateUserPlanActivate(int userId, int userPandId)
        {
            using (SqlSugarClient db = DbHelper.GetInstance())
            {
                try
                {
                    db.BeginTran();
                    db.Updateable<WJ_T_UserPlan>().SetColumns(p => new WJ_T_UserPlan() { UserPlan_Activate = 0 }).Where(p => p.UserId == userId && p.UserPlan_Activate == 1).ExecuteCommand();
                    db.Updateable<WJ_T_UserPlan>().SetColumns(p => new WJ_T_UserPlan() { UserPlan_Activate = 1 }).Where(p => p.UserId == userId && p.Id == userPandId).ExecuteCommand();
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

        #region 查询
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        public List<WJ_T_UserPlan> GetUserPlan(int userId, dynamic jsonObj)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    return db.Queryable<WJ_T_UserPlan>().Where(p => p.UserId == userId).ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.ErrorLog(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
