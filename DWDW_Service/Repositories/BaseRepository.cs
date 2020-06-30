using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DWDW_Service.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        Task<TEntity> FindAsync(object Id);
        TEntity Find(object Id);
        IEnumerable<TEntity> GetAll();
    }
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected DbContext dbContext { get; set; }

        public BaseRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        public async Task AddAsync(TEntity entity)
        {
            await dbContext.AddAsync(entity);
            dbContext.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
            dbContext.SaveChanges();
        }

        public async Task<TEntity> FindAsync(object Id)
        {
            return await dbContext.Set<TEntity>().FindAsync(Id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbContext.Set<TEntity>();
        }

        public void Update(TEntity entity)
        {
            dbContext.Set<TEntity>().Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void Add(TEntity entity)
        {
            dbContext.Add(entity);
            dbContext.SaveChanges();
        }

        public TEntity Find(object Id)
        {
            return dbContext.Set<TEntity>().Find(Id);
        }
    }
}
