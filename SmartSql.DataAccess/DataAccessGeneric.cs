﻿using System;
using System.Collections.Generic;
using System.Text;
using SmartSql.Abstractions;
using SmartSql.DataAccess.Abstractions;
using Microsoft.Extensions.Logging;

namespace SmartSql.DataAccess
{
    /// <summary>
    /// 泛型数据库访问对象
    /// 提供常规方法
    /// </summary>
    /// <typeparam name="TEntity">Table Entity</typeparam>
    public class DataAccessGeneric<TEntity> : DataAccess, IReadDataAccess<TEntity>, IWriteDataAccess<TEntity>
        where TEntity : class
    {
        public DataAccessGeneric(String smartSqlMapConfigPath = "SmartSqlMapConfig.xml") : base(smartSqlMapConfigPath)
        {

        }
        public DataAccessGeneric(ISmartSqlMapper smartSqlMapper) : base(smartSqlMapper)
        {

        }
        protected override void InitScope()
        {
            Scope = typeof(TEntity).Name;
        }
        #region Read

        public virtual TEntity GetEntity<TPrimary>(TPrimary Id, DataSourceChoice sourceChoice = DataSourceChoice.Read)
        {

            return SqlMapper.QuerySingle<TEntity>(new RequestContext
            {
                Scope = this.Scope,
                SqlId = DefaultSqlId.GetEntity,
                Request = new { Id = Id }
            }, sourceChoice);
        }

        public virtual IEnumerable<TResponse> GetList<TResponse>(object paramObj, DataSourceChoice sourceChoice = DataSourceChoice.Read)
        {
            return SqlMapper.Query<TResponse>(new RequestContext
            {
                Scope = this.Scope,
                SqlId = DefaultSqlId.GetList,
                Request = paramObj
            }, sourceChoice);
        }

        public virtual IEnumerable<TResponse> GetListByPage<TResponse>(object paramObj, DataSourceChoice sourceChoice = DataSourceChoice.Read)
        {
            return SqlMapper.Query<TResponse>(new RequestContext
            {
                Scope = this.Scope,
                SqlId = DefaultSqlId.GetListByPage,
                Request = paramObj
            }, sourceChoice);
        }

        public virtual int GetRecord(object paramObj, DataSourceChoice sourceChoice = DataSourceChoice.Read)
        {
            return SqlMapper.QuerySingle<int>(new RequestContext
            {
                Scope = this.Scope,
                SqlId = DefaultSqlId.GetRecord,
                Request = paramObj
            }, sourceChoice);
        }

        public virtual bool IsExist(object paramObj, DataSourceChoice sourceChoice = DataSourceChoice.Read)
        {
            return SqlMapper.QuerySingle<int>(new RequestContext
            {
                Scope = this.Scope,
                SqlId = DefaultSqlId.GetRecord,
                Request = paramObj
            }, sourceChoice) > 0;
        }

        #endregion
        #region Write
        public virtual TPrimary Insert<TPrimary>(TEntity entity)
        {
            bool isNoneIdentity = typeof(TPrimary) == typeof(NoneIdentity);
            if (!isNoneIdentity)
            {
                return SqlMapper.ExecuteScalar<TPrimary>(new RequestContext
                {
                    Scope = this.Scope,
                    SqlId = DefaultSqlId.Insert,
                    Request = entity
                });
            }
            else
            {
                SqlMapper.Execute(new RequestContext
                {
                    Scope = this.Scope,
                    SqlId = DefaultSqlId.Insert,
                    Request = entity
                });
                return default(TPrimary);
            }
        }
        public virtual int Delete<TPrimary>(TPrimary Id)
        {
            return SqlMapper.Execute(new RequestContext
            {
                Scope = this.Scope,
                SqlId = DefaultSqlId.Delete,
                Request = new { Id = Id }
            });
        }

        public virtual int Update(TEntity entity)
        {
            return SqlMapper.Execute(new RequestContext
            {
                Scope = this.Scope,
                SqlId = DefaultSqlId.Update,
                Request = entity
            });
        }
        #endregion
    }

}
