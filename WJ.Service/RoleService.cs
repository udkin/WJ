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
    public class RoleService : DbContext<WJ_T_Role>
    {
        #region 单列模式
        private static RoleService _instance = null;

        private RoleService() { }

        public static RoleService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("RoleService")
                        if (_instance == null)
                            _instance = new RoleService();

                return _instance;
            }
        }
        #endregion

        #region 角色下拉列表
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic GetRole()
        {
            try
            {
                using (SqlSugarClient db = DbInstance)
                {
                    return db.Queryable<WJ_T_Role>().Where(p => p.Role_State == 1).OrderBy(p => p.Role_Sort).Select(f => new { f.Id, f.Role_Name }).ToList();
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion

        #region 角色增、删、改、查
        #region 获取角色列表信息
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<WJ_V_Role> GetList(JObject data, ref int totalCount)
        {
            try
            {
                int pageIndex = data["page"].ToObject<int>();
                int pageSize = data["limit"].ToObject<int>();
                string roleName = data["Role_Name"] != null ? data["Role_Name"].ToString() : "";

                var queryable = DbInstance.Queryable<WJ_V_Role>().Where(p => p.Role_State == 1)
                    .WhereIF(!string.IsNullOrWhiteSpace(roleName), p => p.Role_Name.Contains(roleName))
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

        #region 增加角色信息
        /// <summary>
        /// 增加角色信息
        /// </summary>
        /// <param name="data">提交的表单数据</param>
        /// <param name="errorMsg">错误信息</param>
        /// <returns></returns>
        public bool Add(int userId, JObject data, ref string errorMsg)
        {
            using (SqlSugar.SqlSugarClient db = DbInstance)
            {
                try
                {
                    if (IsExits(p => p.Role_Name == data["Role_Name"].ToString().Trim() && p.Role_State == 1))
                    {
                        errorMsg = "存在相同角色名称";
                    }
                    else
                    {
                        WJ_T_Role role = new WJ_T_Role();
                        role.Role_Name = data["Role_Name"].ToString();
                        role.Role_Sort = data["Role_Sort"].ToObject<int>();
                        role.Role_Creator = userId;
                        role.Role_CreateTime = DateTime.Now;
                        role.Role_State = 1;

                        string menuIds = GetMenuId(data["Role_Menu"]);
                        if (string.IsNullOrWhiteSpace(menuIds))
                        {
                            errorMsg = "";
                            return false;
                        }

                        db.BeginTran();
                        int roleId = Add(role, db);

                        if (roleId > 0)
                        {
                            foreach (var menuId in menuIds.TrimEnd(',').Split(','))
                            {
                                WJ_T_RoleMenu roleMneu = new WJ_T_RoleMenu();
                                roleMneu.RoleId = roleId;
                                roleMneu.MenuId = Convert.ToInt32(menuId);

                                if (RoleMenuService.Instance.Add(roleMneu, db) <= 0)
                                {
                                    db.RollbackTran();
                                    return false;
                                }
                            }

                            return true;
                        }
                        else
                        {
                            db.RollbackTran();
                        }
                    }
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    LogHelper.DbServiceLog(ex.Message);
                    errorMsg = ex.Message;
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public string GetMenuId(JToken menu)
        {
            string menuId = "";
            foreach (var item in menu.ToArray())
            {
                menuId += item["id"].ToObject<int>() + ",";
                if (item["children"] != null)
                {
                    menuId += GetMenuId(item["children"]);
                }
            }
            return menuId;
        }
        #endregion

        #region 更新角色信息
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(JObject data, ref string errorMsg)
        {
            using (SqlSugar.SqlSugarClient db = DbInstance)
            {
                try
                {
                    int roleId = data["Id"].ToObject<int>();
                    if (IsExits(p => p.Role_Name == data["Role_Name"].ToString().Trim() && p.Id != roleId && p.Role_State == 1))
                    {
                        errorMsg = "存在相同角色名称";
                    }
                    else
                    {
                        string menuIds = GetMenuId(data["Role_Menu"]);
                        if (string.IsNullOrWhiteSpace(menuIds))
                        {
                            errorMsg = "";
                            return false;
                        }

                        db.BeginTran();

                        WJ_T_Role role = GetSingle(p => p.Id == roleId);
                        role.Role_Name = data["Role_Name"].ToString();
                        role.Role_Sort = data["Role_Sort"].ToObject<int>();
                        bool flag = Update(role, db);

                        if (flag)
                        {
                            //string oldMenuIds = RoleMenuService.Instance.GetRoleMenuId(id);

                            RoleMenuService.Instance.Delete(roleId, db); //删除旧的角色树记录

                            foreach (var menuId in menuIds.TrimEnd(',').Split(','))
                            {
                                WJ_T_RoleMenu roleMneu = new WJ_T_RoleMenu();
                                roleMneu.RoleId = roleId;
                                roleMneu.MenuId = Convert.ToInt32(menuId);

                                if (RoleMenuService.Instance.Add(roleMneu, db) <= 0)
                                {
                                    db.RollbackTran();
                                    return false;
                                }
                            }

                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    LogHelper.DbServiceLog(ex.Message);
                    errorMsg = ex.Message;
                }
                return false;
            }
        }
        #endregion

        #region 删除角色信息
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
                    if (db.Queryable<WJ_T_UserRole>().Any(p => rimaryList.Contains<int>(p.RoleId)))
                    {
                        errorMsg = "角色已被用户使用不能删除";
                    }
                    else
                    {
                        return db.Updateable<WJ_T_Role>(p => p.Role_State == -1).Where(p => rimaryList.Contains<int>(p.Id)).ExecuteCommand() > 0;
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
        #endregion
    }
}
