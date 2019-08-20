using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WJ.Common;

namespace WJ.Service
{
    public class DbContext<T> where T : class, new()
    {
        public DbContext()
        {

        }

        //注意：不能写成静态的
        public SqlSugarClient DbInstance//用来处理事务多表查询和复杂的操作
        {
            get
            {
                SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Defalut"].ConnectionString,
                    DbType = DbType.SqlServer,
                    InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                    IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了
                    IsShardSameThread = true//设为true相同线程是同一个SqlConnection
                });
                //调式代码 用来打印SQL 
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    LogHelper.DbSqlLog(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                };
                return db;
            }
        }

        #region 公用方法
        #region 事务控制方法
        public void BeginTran()
        {
            DbInstance.Ado.BeginTran();
        }
        public void CommitTran()
        {
            DbInstance.Ado.CommitTran();
        }
        public void RollbackTran()
        {
            DbInstance.Ado.RollbackTran();
        }
        #endregion

        #region 新增
        public virtual bool Add(T obj)
        {
            return DbInstance.Insertable(obj).ExecuteReturnIdentity() > 0;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int AddReturnIdentity(T obj)
        {
            return DbInstance.Insertable(obj).ExecuteReturnIdentity();
        }
        #endregion

        #region 删除
        /// <summary>
        /// 根据实体对象删除
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Delete(T obj)
        {
            return DbInstance.Deleteable<T>().Where(obj).ExecuteCommand() > 0;
        }

        public bool Delete(Expression<Func<T, bool>> whereExpression)
        {
            return DbInstance.Deleteable<T>().Where(whereExpression).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool DeleteById(dynamic id)
        {
            return DbInstance.Deleteable<T>().In(id).ExecuteCommand() > 0;
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Update(T obj)
        {
            return DbInstance.Updateable(obj).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommand() > 0;
        }

        public virtual bool Update(dynamic updateObj)
        {
            return DbInstance.Updateable<T>(updateObj).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public bool UpdateEx(Expression<Func<T, T>> columns, Expression<Func<T, bool>> whereExpression)
        {
            return DbInstance.Updateable<T>().SetColumns(columns).Where(whereExpression).ExecuteCommand() > 0;
        }

        public bool UpdateEx(Expression<Func<T, T>> columns, Expression<Func<T, bool>> setValueExpression, Expression<Func<T, bool>> whereExpression)
        {
            return DbInstance.Updateable<T>().SetColumns(columns).ReSetValue(setValueExpression).Where(whereExpression).ExecuteCommand() > 0;
        }
        #endregion

        /// <summary>
        /// 根据ID查询返回实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(int id)
        {
            return DbInstance.Queryable<T>().InSingle(id);
        }

        /// <summary>
        /// 根据参数值查询返回实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetSingle(Expression<Func<T, bool>> whereExpression)
        {
            return DbInstance.Queryable<T>().Single(whereExpression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetList()
        {
            return DbInstance.Queryable<T>().ToList();
        }

        public List<T> GetList(Expression<Func<T, bool>> whereExpression)
        {
            return DbInstance.Queryable<T>().Where(whereExpression).ToList();
        }

        /// <summary>
        /// 根据条件获取列表
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetList(Expression<Func<T, bool>> whereExpression, PageModel page)
        {
            int count = 0;
            var result = DbInstance.Queryable<T>().Where(whereExpression).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public bool IsExits(Expression<Func<T, bool>> whereExpression)
        {
            using (var db = DbInstance)
            {
                return db.Queryable<T>().Any(whereExpression);
            }
        }
        #endregion
    }
}
