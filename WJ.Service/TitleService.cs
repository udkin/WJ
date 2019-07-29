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
    public class TitleService : DbContext<WJ_T_Title>
    {
        #region 单列模式
        private static TitleService _instance = null;

        private TitleService() { }

        public static TitleService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("TitleService")
                        if (_instance == null)
                            _instance = new TitleService();

                return _instance;
            }
        }
        #endregion

        #region 职务下拉列表
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic GetTitle()
        {
            try
            {
                using (SqlSugarClient db = DbInstance)
                {
                    return db.Queryable<WJ_T_Title>().Where(p => p.Title_State == 1).OrderBy(p => p.Title_Sort).Select(f => new { f.Id, f.Title_Name }).ToList();
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取职务列表信息
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<WJ_T_Title> GetList(JObject data, ref int totalCount)
        {
            try
            {
                int pageIndex = data["page"].ToObject<int>();
                int pageSize = data["limit"].ToObject<int>();
                string titleName = (data["titlename"] == null ? "" : data["titlename"].ToString().Trim());

                var queryable = DbInstance.Queryable<WJ_T_Title>().Where(p => p.Title_State == 1)
                    .WhereIF(!string.IsNullOrWhiteSpace(titleName), p => p.Title_Name.Contains(titleName) || p.Title_FullName.Contains(titleName))
                    .OrderBy(p => p.Title_Sort)
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

        #region 增加职务信息
        /// <summary>
        /// 增加职务信息
        /// </summary>
        /// <param name="data">提交的表单数据</param>
        /// <param name="errorMsg">错误信息</param>
        /// <returns></returns>
        public bool Add(JObject data, ref string errorMsg)
        {
            using (SqlSugar.SqlSugarClient db = DbInstance)
            {
                try
                {
                    if (IsExits(p => p.Title_Name == data["Dept_Name"].ToString().Trim() && p.Title_State == 1))
                    {
                        errorMsg = "存在相同职务简称";
                    }
                    else if (data["Title_FullName"] != null && IsExits(p => p.Title_FullName == data["Title_FullName"].ToString().Trim() && p.Title_State == 1))
                    {
                        errorMsg = "存在相同职务全称";
                    }
                    else
                    {
                        WJ_T_Title title = new WJ_T_Title();
                        title.Title_Name = data["Title_Name"].ToString();
                        title.Title_FullName = data["Title_FullName"] != null ? data["Title_FullName"].ToString() : "";
                        title.Title_Code = data["Title_Code"].ToString();
                        title.Title_Sort = data["Title_Sort"].ToObject<int>();
                        title.Title_CreateTime = DateTime.Now;
                        title.Title_State = 1;

                        return Add(title);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.DbServiceLog(ex.Message);
                }
                return false;
            }
        }
        #endregion

        #region 更新职务信息
        /// <summary>
        /// 更新职务信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(JObject data, ref string errorMsg)
        {
            using (SqlSugar.SqlSugarClient db = DbInstance)
            {
                try
                {
                    int id = data["Id"].ToObject<int>();
                    if (IsExits(p => p.Id != id && p.Title_Name == data["Dept_Name"].ToString().Trim() && p.Title_State == 1))
                    {
                        errorMsg = "存在相同职务简称";
                    }
                    else if (data["Title_FullName"] != null && IsExits(p => p.Id != id && p.Title_FullName == data["Title_FullName"].ToString().Trim() && p.Title_State == 1))
                    {
                        errorMsg = "存在相同职务全称";
                    }
                    else
                    {
                        WJ_T_Title title = GetSingle(p => p.Id == id);
                        title.Title_Name = data["Title_Name"].ToString();
                        title.Title_FullName = data["Title_FullName"] != null ? data["Title_FullName"].ToString() : "";
                        title.Title_Code = data["Title_Code"].ToString();
                        title.Title_Sort = data["Title_Sort"].ToObject<int>();

                        return Update(title);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.DbServiceLog(ex.Message);
                }
                return false;
            }
        }
        #endregion

        #region 删除职务信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Delete(int id, ref string errorMsg)
        {
            try
            {
                using (SqlSugarClient db = DbInstance)
                {
                    if (db.Queryable<WJ_T_User>().Any(p => p.TitleId == id && p.User_State == 1))
                    {
                        errorMsg = "职务已被用户使用不能删除";
                    }
                    else
                    {
                        return db.Updateable<WJ_T_Title>(p => new WJ_T_Title() { Title_State = -1 }).Where(p => p.Id == id).ExecuteCommand() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }
            return false;
        }

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
                    if (db.Queryable<WJ_T_User>().Any(p => rimaryList.Contains<int>(p.TitleId) && p.User_State == 1))
                    {
                        errorMsg = "职务已被用户使用不能删除";
                    }
                    else
                    {
                        return db.Updateable<WJ_T_Title>(p => p.Title_State == -1).Where(p => rimaryList.Contains<int>(p.Id)).ExecuteCommand() > 0;
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
