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
    public class DeptService : DbContext<WJ_T_Dept>
    {
        #region 单列模式
        private static DeptService _instance = null;

        private DeptService() { }

        public static DeptService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("DeptService")
                        if (_instance == null)
                            _instance = new DeptService();

                return _instance;
            }
        }
        #endregion

        #region 获取部门下拉列表
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic GetDept()
        {
            try
            {
                using (var db = DbInstance)
                {
                    return db.Queryable<WJ_T_Dept>().Where(p => p.Dept_State == 1).OrderBy(p => p.Dept_Sort).Select(f => new { f.Id, f.Dept_Name }).ToList();
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取部门列表信息
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<WJ_T_Dept> GetDeptList(JObject data, ref int totalCount)
        {
            try
            {
                int pageIndex = data["page"].ToObject<int>();
                int pageSize = data["limit"].ToObject<int>();
                string deptName = (data["deptname"] == null ? "" : data["deptname"].ToString().Trim());

                var queryable = DbInstance.Queryable<WJ_T_Dept>().Where(p => p.Dept_State == 1)
                    .WhereIF(!string.IsNullOrWhiteSpace(deptName), p => p.Dept_Name.Contains(deptName))
                    .OrderBy(p => p.Dept_Sort)
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
        public bool Add(JObject data, ref string errorMsg)
        {
            try
            {
                if (IsExits(p => p.Dept_Name == data["Dept_Name"].ToString().Trim() && p.Dept_State == 1))
                {
                    errorMsg = "存在相同部门简称";
                }
                else if (data["Dept_FullName"] != null && IsExits(p => p.Dept_Name == data["Dept_FullName"].ToString().Trim() && p.Dept_State == 1))
                {
                    errorMsg = "存在相同部门全称";
                }
                else
                {
                    WJ_T_Dept dept = new WJ_T_Dept();
                    dept.Dept_Name = data["Dept_Name"].ToString();
                    dept.Dept_Icon = "";
                    dept.Dept_Code = data["Dept_Code"] != null ? data["Dept_Code"].ToString() : "";
                    dept.Dept_Lever = data["Dept_Lever"].ToString();
                    dept.Dept_Sort = data["Dept_Sort"].ToObject<int>();
                    dept.Dept_CreateTime = DateTime.Now;
                    dept.Dept_State = 1;

                    return Add(dept);
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }
            return false;
        }
        #endregion

        #region 更新管理员信息
        /// <summary>
        /// 更新管理员信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(JObject data, ref string errorMsg)
        {
            try
            {
                int id = data["Id"].ToObject<int>();
                if (IsExits(p => p.Id != id && p.Dept_Name == data["Dept_Name"].ToString().Trim() && p.Dept_State == 1))
                {
                    errorMsg = "存在相同部门名称";
                }
                else
                {
                    WJ_T_Dept dept = GetSingle(p => p.Id == id);
                    dept.Dept_Name = data["Dept_Name"].ToString();
                    //dept.Dept_Icon = "";
                    dept.Dept_Code = data["Dept_Code"].ToString();
                    dept.Dept_Lever = data["Dept_Lever"].ToString();
                    dept.Dept_Sort = data["Dept_Sort"].ToObject<int>();

                    return Update(dept);
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }
            return false;
        }
        #endregion

        #region 删除部门信息
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
                    if (db.Queryable<WJ_T_User>().Any(p => rimaryList.Contains<int>(p.DeptId) && p.User_State == 1))
                    {
                        errorMsg = "部门已被用户使用不能删除";
                    }
                    else
                    {
                        return db.Updateable<WJ_T_Dept>(p => p.Dept_State == -1).Where(p => rimaryList.Contains<int>(p.Id)).ExecuteCommand() > 0;
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
    }
}
