using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using WR.Utils;
using WR.WCF.DataContract;

namespace WR.DAL.EF
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DatabaseType
    {
        Oracle,
        Mysql,
        SqlServer
    }

    /// <summary>
    /// 数据库操作
    /// </summary>
    public class BFdbContext : DbContext
    {
        public BFdbContext()
            : base("name=DBString")
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 500;

            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;

            if (objectContext.Connection.ConnectionString.Contains("MySql.Data.MySqlClient"))
                DatabaseType = DatabaseType.Mysql;
            else
                DatabaseType = DatabaseType.Oracle;
        }

        public DatabaseType DatabaseType { get; set; }

        #region 数据操作
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update<T>(T entity) where T : class
        {
            var set = this.Set<T>();
            set.Attach(entity);
            this.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
            return this.SaveChanges();
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert<T>(T entity) where T : class
        {
            this.Set<T>().Add(entity);
            return this.SaveChanges();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public int Delete<T>(T entity) where T : class
        {
            this.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
            return this.SaveChanges();
        }

        /// <summary>
        /// 提交变更
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.GetBaseException().Message);
            }
            catch (DbEntityValidationException ex)
            {
                var item = ex.EntityValidationErrors.First();
                throw new Exception(item.ValidationErrors.ToString());
            }
        }

        /// <summary>
        /// sql执行数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int ExecuteSqlCommand(string sql, params object[] parms)
        {
            if (DatabaseType == DatabaseType.Mysql)
                sql = sql.Replace("sysdate", "sysdate()").Replace("nvl(", "ifnull(").Replace("rownum","0");

            return Database.ExecuteSqlCommand(sql, parms);
        }

        #region 执行存储过程
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="proName"></param>
        /// <returns></returns>
        public int ExecuteProcedure(string proName)
        {
            var res = Database.SqlQuery<int>(string.Format("EXEC {0}", proName)).ToList();
            return res.FirstOrDefault();
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proName"></param>
        /// <returns></returns>
        public List<T> ExecuteProcedure<T>(string proName)
        {
            return Database.SqlQuery<T>(string.Format("EXEC {0}", proName)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int ExecuteProcedure(string proName, BFParameters parms)
        {
            string proc = proName;
            var ps = parms.GetDbParameters(ref proc);

            //var res = Database.SqlQuery<int>(proc, ps);
            //return res.FirstOrDefault();
            var res = Database.ExecuteSqlCommand(proc, ps);
            return res;
        }

        public List<T> ExecuteProcedure<T>(string proName, BFParameters parms)
        {
            string proc = proName;
            var ps = parms.GetDbParameters(ref proc);

            return Database.SqlQuery<T>(proc, ps).ToList();
        }
        #endregion

        #region 事务更新
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void TUpdate<T>(T entity) where T : class
        {
            var set = this.Set<T>();
            set.Attach(entity);
            this.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void TInsert<T>(T entity) where T : class
        {
            this.Set<T>().Add(entity);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void TDelete<T>(T entity) where T : class
        {
            this.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
        }
        #endregion

        /// <summary>
        /// 标记事务
        /// </summary>
        /// <returns></returns>
        public DbContextTransaction BeginTransaction()
        {
            DbContextTransaction tx = Database.BeginTransaction();
            return tx;
        }
        #endregion

        #region 数据查询
        /// <summary>
        /// 查找数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public T Find<T>(Expression<Func<T, bool>> conditions) where T : class
        {
            return this.Set<T>().FirstOrDefault(conditions);
        }

        /// <summary>
        /// 查找数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public T Find<T>(params object[] keyValues) where T : class
        {
            return this.Set<T>().Find(keyValues);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> FindAll<T>() where T : class
        {
            return this.Set<T>();
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IQueryable<T> FindAll<T>(Expression<Func<T, bool>> conditions) where T : class
        {
            return this.Set<T>().Where(conditions).AsQueryable();
        }

        /// <summary>
        /// sql查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public IQueryable<T> SqlQuery<T>(string sql, params object[] parms) where T : class
        {
            if (DatabaseType == DatabaseType.Mysql)
                sql = sql.Replace("sysdate", "sysdate()").Replace("nvl(", "ifnull(").Replace("rownum", "0");

            return Database.SqlQuery<T>(sql, parms).AsQueryable();
        }
        #endregion

        #region 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public PagedList<T> FindAllByPage<T, S>(Expression<Func<T, S>> orderBy, int pageIndex) where T : class
        {
            return this.Set<T>().OrderByDescending(orderBy).ToPagedList(pageIndex, 15);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="conditions"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public PagedList<T> FindAllByPage<T, S>(Expression<Func<T, bool>> conditions, Expression<Func<T, S>> orderBy, int pageIndex) where T : class
        {
            var queryList = this.Set<T>().Where(conditions) as IQueryable<T>;

            return queryList.OrderByDescending(orderBy).ToPagedList(pageIndex, 15);
        }

        /// <summary>
        /// sql查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public PagedList<T> SqlQuery<T, S>(string sql, int pageIndex, Expression<Func<T, S>> orderBy, params object[] parms) where T : class
        {
            return Database.SqlQuery<T>(sql, parms).AsQueryable().OrderByDescending(orderBy).ToPagedList(pageIndex, 15);
        }
        #endregion

        /// <summary>
        /// 设置数据库
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<BFdbContext>(null);
            //modelBuilder.Entity<bbs>().HasMany(e=>e.Poster).WithMany(e=>e.

            //设置Schema
            modelBuilder.HasDefaultSchema("WR_OPR");

            base.OnModelCreating(modelBuilder);
        }

        //注册表
        public DbSet<TBMENU> TBMENU { get; set; }
        public DbSet<TBROLE> TBROLE { get; set; }
        public DbSet<TBUSER> TBUSER { get; set; }
        public DbSet<TBUSERLOG> TBUSERLOG { get; set; }

        public DbSet<WMCLASSIFICATIONITEM> WMCLASSIFICATIONITEM { get; set; }
        public DbSet<WMCLASSIFICATIONSCHEME> WMCLASSIFICATIONSCHEME { get; set; }
        public DbSet<WMDEFECTLIST> WMDEFECTLIST { get; set; }
        public DbSet<WMDIELAYOUT> WMDIELAYOUT { get; set; }
        public DbSet<WMDIELAYOUTLIST> WMDIELAYOUTLIST { get; set; }
        public DbSet<WMIDENTIFICATION> WMIDENTIFICATION { get; set; }
        public DbSet<WMINSPECTEDDIELIST> WMINSPECTEDDIELIST { get; set; }
        public DbSet<WMINSPECTIONINFO> WMINSPECTIONINFO { get; set; }
        public DbSet<WMINSPECTIONPASS> WMINSPECTIONPASS { get; set; }
        public DbSet<WMWAFERRESULT> WMWAFERRESULT { get; set; }
        public DbSet<CMNDICT> CMNDICT { get; set; }
        public DbSet<CMNRULE> CMNRULE { get; set; }
        public DbSet<CMNRELATION> CMNRELATION { get; set; }

        public DbSet<EMCLASSIFICATIONMARK> EMCLASSIFICATIONMARK { get; set; }
        public DbSet<EMLIBRARY> EMLIBRARY { get; set; }
        //public DbSet<EMITEMRESULT> EMITEMRESULT { get; set; }
        public DbSet<EMEXAMRESULT> EMEXAMRESULT { get; set; }
        public DbSet<EMPLAN> EMPLAN { get; set; }
        public DbSet<WMYIELDSETTING> WMYIELDSETTING { get; set; }
    }
}
