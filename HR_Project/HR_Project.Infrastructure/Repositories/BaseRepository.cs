using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Repositories;
using HR_Project.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;


namespace HR_Project.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity, new() //abstract olamasın diye new yazdım
    {
        private readonly AppDbContext _context;
        protected DbSet<T> table;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            table= _context.Set<T>();
        }

        

        public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            return await table.AnyAsync(predicate);
        }

        public async Task Create(T entity)
        {
            await table.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetDefault(Expression<Func<T, bool>> expression)
        {
            return await table.FirstOrDefaultAsync(expression);
        }

        public async Task<List<T>> GetDefaults(Expression<Func<T, bool>> expression)
        {
            return await table.Where(expression).ToListAsync();
        }

        public async Task<TResult> GetFilteredFirstOrDefault<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = table;

            if (where != null)
                query = query.Where(where);
            if (include != null)
                query = include(query);
            if (orderBy != null)
                return await orderBy(query).Select(select).FirstOrDefaultAsync();
            else
                return await query.Select(select).FirstOrDefaultAsync();
        }

        public async Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = table;

            if (where != null)
                query = query.Where(where);
            if (include != null)
                query = include(query);
            if (orderBy != null)
                return await orderBy(query).Select(select).ToListAsync();
            else
                return await query.Select(select).ToListAsync();
        }

        public async Task Update(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
