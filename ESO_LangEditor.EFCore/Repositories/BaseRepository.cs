using ESO_LangEditor.Core.RequestParameters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.EFCore.Repositories
{
    public class BaseRepository<T, TId> : IBaseRepository<T>, IBaseRepository2<T, TId> where T : class
    {
        public DbContext DbContext { get; set; }
        public BaseRepository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<T> GetByIdAsync(TId id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public async Task<bool> IsExistAsync(TId id)
        {
            return await DbContext.Set<T>().FindAsync(id) != null;
        }

        public async Task<PagedList<T>> GetAllAsync(PageParameters pageParameters)
        {
            var items = await DbContext.Set<T>()
                .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                .Take(pageParameters.PageSize)
                .ToListAsync();

            var count = await DbContext.Set<T>().CountAsync();

            return new PagedList<T>(items, count, pageParameters.PageNumber, pageParameters.PageSize);


            //return Task.FromResult();
            //return Task.FromResult(DbContext.Set<T>().AsEnumerable());
        }

        public async Task<PagedList<T>> GetByConditionAsync(Expression<Func<T, bool>> expression, PageParameters pageParameters)
        {
            var items = await DbContext.Set<T>()
                .Where(expression)
                .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                .Take(pageParameters.PageSize)
                .ToListAsync();

            var count = await DbContext.Set<T>().CountAsync();

            return new PagedList<T>(items, count, pageParameters.PageNumber, pageParameters.PageSize);

            //return Task.FromResult(DbContext.Set<T>().Where(expression));
            //return Task.FromResult(DbContext.Set<T>().Where(expression).AsEnumerable());
        }

        public Task<IQueryable<TType>> SelectByConditionWithDistinctAsync<TType>(Expression<Func<T, TType>> expression)
        {
            return Task.FromResult(DbContext.Set<T>().Select(expression).Distinct());
            //return Task.FromResult(DbContext.Set<T>().Select(expression).Distinct().AsEnumerable());
        }

        public async Task<bool> SaveAsync()
        {
            return await DbContext.SaveChangesAsync() > 0;
        }

        public void Update(T entity)
        {
            DbContext.Set<T>().Update(entity);
        }

        public void Create(T entity)
        {
            DbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }

        public void CreateList(List<T> entity)
        {
            DbContext.Set<T>().AddRange(entity);
        }

        public void UpdateList(List<T> entity)
        {
            DbContext.Set<T>().UpdateRange(entity);
            //DbContext.SaveChangesAsync();
        }

        public void DeleteList(List<T> entity)
        {
            DbContext.Set<T>().RemoveRange(entity);
        }

        
    }

}
