using AdminLTE.Demo.Data.IRepositories;
using AdminLTE.Demo.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AdminLTE.Demo.Data.Repositories
{
    public class GenericRepository<TEntity>
        : IGenericRepository<TEntity> where TEntity : Entity
    {
        internal DefaultContext _dbContext;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(DefaultContext context)
        {
            this._dbContext = context;
            this.dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            dbSet.Remove(entityToDelete);
        }

        public void DeleteMuti(List<object> ids)
        {
            List<TEntity> entitiesToDelete = dbSet.Where(p => ids.Contains(ids)).ToList();
            dbSet.RemoveRange(entitiesToDelete);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
		public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }


    }
}
