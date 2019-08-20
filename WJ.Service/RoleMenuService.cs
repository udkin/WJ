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
    public class RoleMenuService : DbContext<WJ_T_RoleMenu>
    {
        #region 单列模式
        private static RoleMenuService _instance = null;

        private RoleMenuService() { }

        public static RoleMenuService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("RoleMenuService")
                        if (_instance == null)
                            _instance = new RoleMenuService();

                return _instance;
            }
        }
        #endregion

        #region 删除角色菜单信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(int roleId, SqlSugarClient db = null)
        {
            try
            {
                if (db == null)
                {
                    using (var db1 = DbInstance)
                    {
                        return db1.Deleteable<WJ_T_RoleMenu>(p => p.RoleId == roleId).ExecuteCommand() > 0;
                    }
                }
                else
                {
                    return db.Deleteable<WJ_T_RoleMenu>(p => p.RoleId == roleId).ExecuteCommand() > 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }
            return false;
        }
        #endregion

        #region 角色菜单树
        /// <summary>
        /// 返回前端格式的角色菜单树
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetRoleMenu(int roleId)
        {
            try
            {
                using (var db = DbInstance)
                {
                    List<WJ_V_RoleMenu> roleMenuList = db.Queryable<WJ_T_Menu, WJ_T_RoleMenu>((m, rm) => new object[] {
                        JoinType.Left,m.Id==rm.MenuId && rm.RoleId == roleId})
                       .Select((m, rm) => new WJ_V_RoleMenu { Id = m.Id, Menu_Name = m.Menu_Name, Menu_Level = m.Menu_Level, RoleId = rm.RoleId, Menu_Type = m.Menu_Type }).ToList();

                    List<dynamic> menuList = new List<dynamic>();

                    var topMenuList = roleMenuList.Where(p => p.Menu_Level.Length == 3).ToList();
                    foreach (var item in topMenuList)
                    {
                        List<dynamic> subMenuList = GetSubRoleMenu(roleMenuList, item.Menu_Level);
                        if (subMenuList.Count == 0)
                        {
                            menuList.Add(new { id = item.Id, title = item.Menu_Name, type = item.Menu_Type, spread = true });
                        }
                        else
                        {
                            menuList.Add(new { id = item.Id, title = item.Menu_Name, type = item.Menu_Type, spread = true, children = subMenuList });
                        }
                    }
                    return menuList;
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 返回角色子菜单
        /// </summary>
        /// <param name="allMenuList"></param>
        /// <param name="levelCode"></param>
        /// <returns></returns>
        public List<dynamic> GetSubRoleMenu(List<WJ_V_RoleMenu> allMenuList, string levelCode)
        {
            List<dynamic> menuList = new List<dynamic>();
            var subUserMenuList = allMenuList.Where(p => p.Menu_Level.StartsWith(levelCode) && p.Menu_Level.Length == levelCode.Length + 3).ToList();

            foreach (var item in subUserMenuList)
            {
                List<dynamic> subMenuList = GetSubRoleMenu(allMenuList, item.Menu_Level);
                if (subMenuList.Count == 0)
                {
                    menuList.Add(new { id = item.Id, title = item.Menu_Name, Checked = item.RoleId != null, type = item.Menu_Type, spread = true });
                }
                else
                {
                    menuList.Add(new { id = item.Id, title = item.Menu_Name, Checked = item.RoleId != null, type = item.Menu_Type, spread = true, children = subMenuList });
                }
            }
            return menuList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetRoleMenu1()
        {
            try
            {
                using (var db = DbInstance)
                {
                    var allMenuList = db.Queryable<WJ_T_Menu>().Where(p => p.Menu_State == 1).OrderBy(p => p.Menu_Level).ToList();

                    List<dynamic> menuList = new List<dynamic>();
                    var topMenuList = allMenuList.Where(p => p.Menu_Level.Length == 3).ToList();
                    foreach (var item in topMenuList)
                    {
                        List<dynamic> subMenuList = GetSubRoleMenu1(allMenuList, item.Menu_Level);
                        if (subMenuList.Count == 0)
                        {
                            menuList.Add(new { id = item.Id, title = item.Menu_Name, type = item.Menu_Type, spread = true });
                        }
                        else
                        {
                            menuList.Add(new { id = item.Id, title = item.Menu_Name, type = item.Menu_Type, spread = true, children = subMenuList });
                        }
                    }
                    return menuList;
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
        /// <param name="allMenuList"></param>
        /// <param name="levelCode"></param>
        /// <returns></returns>
        public List<dynamic> GetSubRoleMenu1(List<WJ_T_Menu> allMenuList, string levelCode)
        {
            List<dynamic> menuList = new List<dynamic>();
            var subUserMenuList = allMenuList.Where(p => p.Menu_Level.StartsWith(levelCode) && p.Menu_Level.Length == levelCode.Length + 3).ToList();

            foreach (var item in subUserMenuList)
            {
                List<dynamic> subMenuList = GetSubRoleMenu1(allMenuList, item.Menu_Level);
                if (subMenuList.Count == 0)
                {
                    menuList.Add(new { id = item.Id, title = item.Menu_Name, type = item.Menu_Type, spread = true });
                }
                else
                {
                    menuList.Add(new { id = item.Id, title = item.Menu_Name, type = item.Menu_Type, spread = true, children = subMenuList });
                }
            }
            return menuList;
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string GetRoleMenuId(int roleId)
        {
            try
            {
                using (var db = DbInstance)
                {
                    var idList = db.Queryable<WJ_T_Menu, WJ_T_RoleMenu>((m, rm) => new object[] { JoinType.Inner, m.Id == rm.MenuId && rm.RoleId == roleId && m.Menu_Type == 1 })
                        .Select((m, rm) => new { m.Id }).ToList();

                    string result = "";
                    foreach (var item in idList)
                    {
                        result += item.Id + ",";
                    }
                    return result.TrimEnd(',');
                }
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
