﻿using Newtonsoft.Json;
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
    public class UserService : DbContext<WJ_T_User>
    {
        #region 单列模式
        private static UserService _instance = null;

        private UserService() { }

        public static UserService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("UserService")
                        if (_instance == null)
                            _instance = new UserService();

                return _instance;
            }
        }
        #endregion

        #region 用户登录、退出
        /// <summary>
        /// 用户便当
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public int UserLogin(string loginName, string password)
        {
            int userId = -1;
            try
            {
                var user = DbInstance.Queryable<WJ_T_User>().Where(p => p.User_LoginName == loginName && p.User_State == 1).First();
                if (user != null && user.User_Password == password)
                {
                    return user.Id;
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }
            return userId;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool LogOut(string token)
        {
            try
            {
                return DbInstance.Updateable<WJ_T_User>(p => new WJ_T_User() { User_Token = "" }).Where(p => p.User_Token == token).ExecuteCommand() > 0;
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }
            return false;
        }
        #endregion

        #region 使用Token获取用户ID
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public WJ_V_User GetUserByToken(string token)
        {
            try
            {
                return DbInstance.Queryable<WJ_V_User>().Where(p => p.User_Token == token).First();
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion

        #region 更新用户Token
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public bool UpdateUserToken(int userId, string token)
        {
            try
            {
                return DbInstance.Updateable<WJ_T_User>().SetColumns(p => new WJ_T_User() { User_Token = token }).Where(p => p.Id == userId).ExecuteCommand() > 0;
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return false;
            }
        }
        #endregion

        #region 更新用户密码
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="data"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SetUserPassword(int userId, JObject data, ref string errorMsg)
        {
            try
            {
                using (var db = DbInstance)
                {
                    string oldPassword = data["oldPassword"].ToString();
                    string password = data["password"].ToString();
                    return db.Updateable<WJ_T_User>(p => p.User_Password == password).Where(p => p.Id == userId && p.User_Password == oldPassword).ExecuteCommand() > 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                errorMsg = ex.Message;
                return false;
            }
        }
        #endregion

        #region 管理员
        #region 获取后台管理员列表信息
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<WJ_V_User> GetManagerList(JObject data, ref int totalCount)
        {
            try
            {
                int pageIndex = data["page"].ToObject<int>();
                int pageSize = data["limit"].ToObject<int>();
                string userName = (data["username"] == null ? "" : data["username"].ToString().Trim());
                string telphone = (data["telphone"] == null ? "" : data["telphone"].ToString().Trim());
                int roleId = (data["role"] == null ? 0 : data["role"].ToObject<int>());
                //PageModel page = new PageModel();
                //page.PageIndex = pageIndex;
                //page.PageSize = limit;
                //managerList = new DbContext<WJ_V_User>().GetList(p => p.User_Name.Contains(""), page);

                //拼接拉姆达
                //var exp = Expressionable.Create<WJ_V_User>()
                //  .OrIF(1 == 1, it => it.Id == 11)
                //  .And(it => it.Id == 1)
                //  .AndIF(2 == 2, it => it.Id == 1)
                //  .Or(it => it.User_Name == "a1").ToExpression();//拼接表达式
                //var list = DbInstance.Queryable<WJ_V_User>().Where(exp).ToList();

                var queryable = DbInstance.Queryable<WJ_V_User>().Where(p => p.Id == 1 || p.User_Type <= 2 && p.User_State == 1)
                    .WhereIF(!string.IsNullOrWhiteSpace(userName), p => p.User_Name.Contains(userName))
                    .WhereIF(!string.IsNullOrWhiteSpace(telphone), p => p.User_Name.Contains(telphone))
                    .WhereIF(roleId > 0, p => p.RoleId == data["role"].ToObject<int>())
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

        #region 增加管理员信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">提交的表单数据</param>
        /// <param name="errorMsg">错误信息</param>
        /// <returns></returns>
        public bool AddManager(JObject data, ref string errorMsg)
        {
            try
            {
                if (IsExits(p => p.User_LoginName == data["loginname"].ToString().Trim() && p.User_Type == 2 && p.User_State == 1))
                {
                    errorMsg = "存在相同登录用户名";
                }
                else
                {
                    WJ_T_User user = new WJ_T_User();
                    user.User_LoginName = data["loginname"].ToString();
                    user.User_Password = data["password"].ToString();
                    user.DeptId = data["dept"].ToObject<int>();
                    user.TitleId = data["title"].ToObject<int>();
                    user.User_Name = data["username"].ToString();
                    user.User_Head = "";
                    user.User_Sex = data["sex"].ToObject<int>();
                    user.User_Phone = data["telphone"].ToString();
                    user.User_Type = 2;
                    user.User_CreateTime = DateTime.Now;
                    user.User_State = 1;

                    int roleId = data["role"].ToObject<int>();

                    BeginTran();
                    int userId = AddReturnIdentity(user);

                    if (userId > 0)
                    {
                        WJ_T_UserRole userRole = new WJ_T_UserRole();
                        userRole.UserId = userId;
                        userRole.RoleId = roleId;

                        if (UserRoleService.Instance.Add(userRole))
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
                errorMsg = ex.Message;
            }

            RollbackTran();
            return false;
        }
        #endregion

        #region 更新管理员信息
        /// <summary>
        /// 更新管理员信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateManager(JObject data, ref string errorMsg)
        {
            try
            {
                int id = data["Id"].ToObject<int>();
                if (IsExits(p => p.Id != id && p.User_LoginName == data["loginname"].ToString().Trim() && p.User_Type == 2 && p.User_State == 1))
                {
                    errorMsg = "存在相同登录用户名";
                }
                else
                {
                    WJ_T_User user = GetSingle(p => p.Id == id);
                    user.User_LoginName = data["loginname"].ToString();
                    user.User_Password = data["password"].ToString();
                    user.DeptId = data["dept"].ToObject<int>();
                    user.TitleId = data["title"].ToObject<int>();
                    user.User_Name = data["username"].ToString();
                    user.User_Head = "";
                    user.User_Sex = data["sex"].ToObject<int>();
                    user.User_Phone = data["telphone"].ToString();

                    BeginTran();
                    bool flag = Update(user);

                    if (flag)
                    {
                        int roleId = data["role"].ToObject<int>();
                        WJ_T_UserRole userRole = UserRoleService.Instance.GetSingle(p => p.UserId == user.Id);
                        userRole.RoleId = roleId;

                        if (UserRoleService.Instance.Update(userRole))
                        {
                            CommitTran();
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
            }

            RollbackTran();
            return false;
        }
        #endregion

        #region 删除管理员信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool DeleteManager(int id, ref string errorMsg)
        {
            try
            {
                return DbInstance.Updateable<WJ_T_User>(p => new WJ_T_User() { User_State = -1 }).Where(p => p.User_Type == 2 && p.Id == id).ExecuteCommand() > 0;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                LogHelper.DbServiceLog(ex.Message);
                return false;
            }
        }
        #endregion

        #region 重置管理员密码
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ResetManagerPassword(int id)
        {
            try
            {
                return DbInstance.Ado.ExecuteCommand(string.Format(@"update WJ_T_User set User_Password = (select SystemMap_Value from wj_t_systemmap where SystemMap_Type = 'InitPassword') where Id = {0}"
                                        , id)) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return false;
            }
        }
        #endregion
        #endregion

        #region 操作用户
        #region 获取操作员列表信息
        /// <summary>
        /// 获取前台操作员列表信息
        /// </summary>
        /// <returns></returns>
        public List<WJ_V_User> GetUserList(JObject data, ref int totalCount)
        {
            try
            {
                int pageIndex = data["page"].ToObject<int>();
                int pageSize = data["limit"].ToObject<int>();
                string name = (data["User_Name"] == null ? "" : data["User_Name"].ToString().Trim());
                string telphone = (data["telphone"] == null ? "" : data["telphone"].ToString().Trim());

                var queryable = DbInstance.Queryable<WJ_V_User>().Where(p => p.User_Type > 2 && p.User_State == 1)
                    .WhereIF(!string.IsNullOrWhiteSpace(name), p => p.User_Name.Contains(name))
                    .WhereIF(!string.IsNullOrWhiteSpace(telphone), p => p.User_Name.Contains(telphone))
                    .OrderBy(p => p.User_Type)
                    .OrderBy(p => p.User_CreateTime)
                    .ToPageList(pageIndex, pageSize, ref totalCount);

                return queryable;

                //return DbInstance.Queryable<WJ_V_User>().Where(p => p.User_Type > 1 && p.User_State == 1).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion

        #region 增加操作用户信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">提交的表单数据</param>
        /// <param name="errorMsg">错误信息</param>
        /// <returns></returns>
        public bool AddUser(JObject data, ref string errorMsg)
        {
            try
            {
                if (IsExits(p => p.User_LoginName == data["loginname"].ToString().Trim() && p.User_Type == 4 && p.User_State == 1))
                {
                    errorMsg = "存在相同登录用户名";
                }
                else
                {
                    WJ_T_User user = new WJ_T_User();
                    user.User_LoginName = data["loginname"].ToString();
                    user.User_Password = data["password"].ToString();
                    user.DeptId = data["dept"].ToObject<int>();
                    user.TitleId = data["title"].ToObject<int>();
                    user.User_Name = data["username"].ToString();
                    user.User_Head = "";
                    user.User_Sex = data["sex"].ToObject<int>();
                    user.User_Phone = data["telphone"].ToString();
                    user.User_Type = 4;
                    user.User_CreateTime = DateTime.Now;
                    user.User_State = 1;

                    return Add(user);
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                errorMsg = ex.Message;
            }

            return false;
        }
        #endregion

        #region 更新操作用户信息
        /// <summary>
        /// 更新操作用户信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateUser(JObject data, ref string errorMsg)
        {
            try
            {
                int id = data["Id"].ToObject<int>();
                if (IsExits(p => p.Id != id && p.User_LoginName == data["loginname"].ToString().Trim() && p.User_Type == 4 && p.User_State == 1))
                {
                    errorMsg = "存在相同登录用户名";
                }
                else
                {
                    WJ_T_User user = GetSingle(p => p.Id == id);
                    user.User_LoginName = data["loginname"].ToString();
                    user.User_Password = data["password"].ToString();
                    user.DeptId = data["dept"].ToObject<int>();
                    user.TitleId = data["title"].ToObject<int>();
                    user.User_Name = data["username"].ToString();
                    user.User_Head = "";
                    user.User_Sex = data["sex"].ToObject<int>();
                    user.User_Phone = data["telphone"].ToString();

                    return Update(user);
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

        #region 删除操作用户信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rimaryList"></param>
        /// <returns></returns>
        public bool DeleteUser(List<int> rimaryList)
        {
            using (var db = DbInstance)
            {
                try
                {
                    db.Ado.BeginTran();
                    db.Updateable<WJ_T_UserApp>(p => p.UserApp_State == -1).Where(p => rimaryList.Contains<int>(p.UserId)).ExecuteCommand();
                    if (db.Updateable<WJ_T_User>(p => p.User_State == -1).Where(p => p.User_Type == 4 && rimaryList.Contains<int>(p.Id)).ExecuteCommand() > 0)
                    {
                        db.CommitTran();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    LogHelper.DbServiceLog(ex.Message);
                }

                db.Ado.RollbackTran();
                return false;
            }
        }
        #endregion
        #endregion
    }
}
