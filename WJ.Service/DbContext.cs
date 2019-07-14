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
                });
                //调式代码 用来打印SQL 
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    LogHelper.DbSqlLog(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                };
                return db;
            }
        }
        //public SimpleClient<Student> StudentDb { get { return new SimpleClient<Student>(Db); } }//用来处理Student表的常用操作
        //public SimpleClient<School> SchoolDb { get { return new SimpleClient<School>(Db); } }//用来处理School表的常用操作
        public SimpleClient<T> CurrentDb { get { return new SimpleClient<T>(DbInstance); } }//用来处理T表的常用操作

        #region 公用方法
        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Add(T obj)
        {
            return CurrentDb.Insert(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public virtual int Add(T obj, SqlSugarClient db)
        {
            return db.Insertable(obj).ExecuteReturnIdentity();
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
            return CurrentDb.Delete(obj);
        }

        /// <summary>
        /// 根据实体对象删除
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Delete(T obj, SqlSugarClient db)
        {
            return db.Deleteable<T>().Where(obj).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool DeleteById(dynamic id)
        {
            return CurrentDb.DeleteById(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public virtual bool DeleteById(dynamic id, SqlSugarClient db)
        {
            return db.Deleteable<T>().In(id).ExecuteCommand() > 0;
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Update(T obj, bool isNullColumn = false)
        {
            if (isNullColumn)
            {
                return DbInstance.Updateable(obj).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommand() > 0;
            }
            else
            {
                return CurrentDb.Update(obj);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Update(T obj, SqlSugarClient db)
        {
            return db.Updateable(obj).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommand() > 0;
        }
        #endregion

        /// <summary>
        /// 根据ID查询返回实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(int id)
        {
            return CurrentDb.GetById(id);
        }

        /// <summary>
        /// 根据ID查询返回实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(int id, SqlSugarClient db)
        {
            return db.Queryable<T>().InSingle(id);
        }

        /// <summary>
        /// 根据参数值查询返回实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetSingle(Expression<Func<T, bool>> whereExpression)
        {
            return CurrentDb.GetSingle(whereExpression);
        }

        /// <summary>
        /// 根据条件获取列表
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetList(Expression<Func<T, bool>> whereExpression, PageModel page = null)
        {
            return CurrentDb.GetPageList(whereExpression, page);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public bool IsExits(Expression<Func<T, bool>> whereExpression)
        {
            using (SqlSugarClient db = DbInstance)
            {
                return db.Queryable<T>().Any(whereExpression);
            }
        }
        #endregion
    }
}
