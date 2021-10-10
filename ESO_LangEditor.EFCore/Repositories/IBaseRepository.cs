using ESO_LangEditor.Core.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.EFCore.Repositories
{
    public interface IBaseRepository<T>
    {
        Task<PagedList<T>> GetAllAsync(PageParameters pageParameters);
        Task<PagedList<T>> GetByConditionAsync(Expression<Func<T, bool>> expression, PageParameters pageParameters);
        Task<IQueryable<TType>> SelectByConditionWithDistinctAsync<TType>(Expression<Func<T, TType>> expression);
        void Create(T entity);
        void CreateList(List<T> entity);
        void Update(T entity);
        void UpdateList(List<T> entity);
        void Delete(T entity);
        void DeleteList(List<T> entity);
        Task<bool> SaveAsync();
    }
}
