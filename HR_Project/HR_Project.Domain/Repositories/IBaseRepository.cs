using HR_Project.Domain.Entities.Abstract;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Domain.Repositories
{
    public interface IBaseRepository<T> where T : class,IBaseEntity
    {
        Task Create(T entity);

        Task Update(T entity);

        Task Delete(T entity); //NOT: Veritabanında silme işlemi yapmayacağız, statüsünü pasife çekeceğiz

        Task<bool> Any(Expression<Func<T, bool>> predicate); 

        Task<T> GetDefault(Expression<Func<T, bool>> expression); 

        Task<List<T>> GetDefaults(Expression<Func<T, bool>> expression); 

        Task<TResult> GetFilteredFirstOrDefault<TResult>
            (Expression<Func<T, TResult>> select,//select
            Expression<Func<T, bool>> where, //koşul
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, //sıralama
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);//join

        //Çoklu dönecek aşağıda!
        Task<List<TResult>> GetFilteredList<TResult>(
            Expression<Func<T, TResult>> select,//select
            Expression<Func<T, bool>> where, //koşul
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, //sıralama
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);//join


    }
}
