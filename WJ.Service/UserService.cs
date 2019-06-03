﻿using Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJ.Service
{
    public class UserService
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public int UserLogin(string loginName, string password)
        {
            int userId = -1;
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    var user = db.Queryable<WJ_T_User>().Where(p => p.User_LoginName == loginName && p.User_State == 1).First();
                    if (user != null && user.User_Password == password)
                    {
                        return user.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return userId;
        }

        #region 超级管理员
        /// <summary>
        /// 返回超级管理员菜单
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetSuperAdminMenu()
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    var allMenuList = db.Queryable<WJ_T_Menu>().OrderBy(p => p.Menu_Level).ToList();

                    List<dynamic> menuList = new List<dynamic>();
                    var firstMenuList = allMenuList.Where(p => p.Menu_Level.Length == 3);
                    foreach (var item in firstMenuList)
                    {
                        List<dynamic> subMenuLit = GetSubMenu(allMenuList, item.Menu_Level);
                        if (item.Menu_Url != null)
                        {
                            menuList.Add(new { title = item.Menu_Name, icon = item.Menu_Ico, jump = item.Menu_Url, list = subMenuLit });
                        }
                        else
                        {
                            menuList.Add(new { title = item.Menu_Name, icon = item.Menu_Ico, list = subMenuLit });
                        }
                    }
                    return menuList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetSuperAdminControllerName()
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    return db.Queryable<WJ_V_UserRoleMenu>().Select(f => f.Menu_Control.Substring(0, 1)).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion

        #region 用户所有权限菜单
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<dynamic> GetUserRoleMenu(int userId)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    var userRoleMenuList = db.Queryable<WJ_V_UserRoleMenu>().Where(p => p.UserId == userId).OrderBy(p => p.Menu_Level).ToList();

                    List<dynamic> menuList = new List<dynamic>();
                    var firstMenuList = userRoleMenuList.Where(p => p.Menu_Level.Length == 3);
                    foreach (var item in firstMenuList)
                    {
                        List<dynamic> subMenuLit = GetUserSubMenu(userRoleMenuList, item.Menu_Level);
                        if (item.Menu_Url.Contains("/"))
                        {
                            menuList.Add(new { title = item.Menu_Name, icon = item.Menu_Ico, jump = item.Menu_Url, list = subMenuLit });
                        }
                        else
                        {
                            menuList.Add(new { name = item.Menu_Url, title = item.Menu_Name, icon = item.Menu_Ico, list = subMenuLit });
                        }
                    }
                    return menuList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 返回用户可以访问的控制器名称列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<string> GetUserControllerName(int userId)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    return db.Queryable<WJ_V_UserRoleMenu>().Where(p => p.UserId == userId).Select(f => f.Menu_Control.ToLower()).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public bool AuthorizeController(string controllerName)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    return db.Queryable<WJ_T_Menu>().Any(p => p.Menu_Control == controllerName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region 返回子菜单
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userMenuList"></param>
        /// <param name="levelCode"></param>
        /// <returns></returns>
        public List<dynamic> GetSubMenu(List<WJ_T_Menu> userMenuList, string levelCode)
        {
            List<dynamic> menuList = new List<dynamic>();
            var subUserMenuList = userMenuList.Where(p => p.Menu_Level.Substring(0, levelCode.Length) == levelCode && p.Menu_Level.Length == levelCode.Length + 3);

            try
            {
                foreach (var item in subUserMenuList)
                {
                    List<dynamic> subMenuList = GetSubMenu(userMenuList, item.Menu_Level);
                    menuList.Add(new { jump = item.Menu_Url, title = item.Menu_Name, list = subMenuList });
                }
            }
            catch { }
            return menuList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userMenuList"></param>
        /// <param name="levelCode"></param>
        /// <returns></returns>
        public List<dynamic> GetUserSubMenu(List<WJ_V_UserRoleMenu> userMenuList, string levelCode)
        {
            List<dynamic> menuList = new List<dynamic>();
            var subUserMenuList = userMenuList.Where(p => p.Menu_Level.Substring(0, levelCode.Length) == levelCode && p.Menu_Level.Length == levelCode.Length + 3);

            try
            {
                foreach (var item in subUserMenuList)
                {
                    List<dynamic> subMenuList = GetUserSubMenu(userMenuList, item.Menu_Level);
                    menuList.Add(new { jump = item.Menu_Url, title = item.Menu_Name, list = subMenuList });
                }
            }
            catch { }
            return menuList;
        }
        #endregion

        public List<dynamic> GetUserList()
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    //return db.Queryable<WJ_T_User>().Where(p => p.User_Type == 1 && p.User_State == 1).ToList();
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
