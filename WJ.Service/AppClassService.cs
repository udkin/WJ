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
    public class AppClassService : DbContext<WJ_T_AppClass>
    {
        #region 单列模式
        private static AppClassService _instance = null;

        private AppClassService() { }

        public static AppClassService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("AppDataService")
                        if (_instance == null)
                            _instance = new AppClassService();

                return _instance;
            }
        }
        #endregion

        #region 获取应用分类下拉列表
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic GetAppClass()
        {
            try
            {
                using (var db = DbInstance)
                {
                    return db.Queryable<WJ_T_AppClass>().Where(p => p.AppClass_State == 1).OrderBy(p => p.AppClass_Sort).Select(f => new { f.Id, f.AppClass_Name }).ToList();
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取应用分类列表信息
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<WJ_T_AppClass> GetList(JObject data, ref int totalCount)
        {
            try
            {
                int pageIndex = data["page"].ToObject<int>();
                int pageSize = data["limit"].ToObject<int>();
                string name = (data["AppClass_Name"] == null ? "" : data["AppClass_Name"].ToString().Trim());

                var queryable = DbInstance.Queryable<WJ_T_AppClass>().Where(p => p.AppClass_State == 1)
                    .WhereIF(!string.IsNullOrWhiteSpace(name), p => p.AppClass_Name.Contains(name))
                    .OrderBy(p => p.AppClass_Sort)
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

        #region 增加应用分类信息
        /// <summary>
        /// 增加应用分类信息
        /// </summary>
        /// <param name="data">提交的表单数据</param>
        /// <param name="errorMsg">错误信息</param>
        /// <returns></returns>
        public bool Add(int userId, JObject data, ref string errorMsg)
        {
            try
            {
                if (IsExits(p => p.AppClass_Name == data["AppClass_Name"].ToString().Trim() && p.AppClass_State == 1))
                {
                    errorMsg = "存在相同应用分类名称";
                }
                else
                {
                    WJ_T_AppClass appClass = new WJ_T_AppClass();
                    appClass.AppClass_Name = data["AppClass_Name"] != null ? data["AppClass_Name"].ToString() : "";
                    appClass.AppClass_Icon = data["AppClass_Icon"] != null ? data["AppClass_Icon"].ToString() : "";
                    appClass.AppClass_Sort = data["AppClass_Sort"] != null ? data["AppClass_Sort"].ToObject<int>() : 1;
                    appClass.AppClass_Remark = data["AppClass_Remark"] != null ? data["AppClass_Remark"].ToString() : "";
                    appClass.AppClass_Creator = userId;
                    appClass.AppClass_CreateTime = DateTime.Now;
                    appClass.AppClass_State = 1;

                    if (Add(appClass))
                    {
                        return true;
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

        #region 更新应用分类信息
        /// <summary>
        /// 更新应用分类信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(JObject data, ref string errorMsg)
        {
            try
            {
                int id = data["Id"].ToObject<int>();
                if (IsExits(p => p.Id != id && p.AppClass_Name == data["AppClass_Name"].ToString().Trim() && p.AppClass_State == 1))
                {
                    errorMsg = "存在相同应用分类名称";
                }
                else
                {
                    WJ_T_AppClass appClass = GetSingle(p => p.Id == id);
                    appClass.AppClass_Name = data["AppClass_Name"] != null ? data["AppClass_Name"].ToString() : "";
                    appClass.AppClass_Icon = data["AppClass_Icon"] != null ? data["AppClass_Icon"].ToString() : "";
                    appClass.AppClass_Remark = data["AppClass_Remark"] != null ? data["AppClass_Remark"].ToString() : "";
                    appClass.AppClass_Sort = data["AppClass_Sort"] != null ? data["AppClass_Sort"].ToObject<int>() : 1;

                    return Update(appClass);
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }
            return false;
        }
        #endregion

        #region 删除应用分类信息
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
                    if (db.Queryable<WJ_T_App>().Any(p => rimaryList.Contains<int>(p.AppClassId) && p.App_State == 1))
                    {
                        errorMsg = "应用分类已被使用不能删除";
                    }
                    else
                    {
                        return db.Updateable<WJ_T_AppClass>(p => p.AppClass_State == -1).Where(p => rimaryList.Contains<int>(p.Id)).ExecuteCommand() > 0;
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
