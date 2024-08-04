using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext dbContext;

        public GenericRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Create(T model)
        {
            await dbContext.Set<T>().AddAsync(model);
        }

        public bool Delete(int id)
        {
            var model = dbContext.Set<T>().Find(id);
            if (model != null)
            {
                dbContext.Set<T>().Remove(model);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<T>> GetAll(int page_size = 2, int page_number = 1, string? includeProperty = null, Expression<Func<T,bool>> filter = null)
        {
            //if (typeof(T) == typeof(Product))
            //{
            //    var model = await dbContext.products.Include(x => x.Category).ToListAsync();
            //    return (IEnumerable<T>) model;
            //}
            //return await dbContext.Set<T>().ToListAsync();

            IQueryable<T> query = dbContext.Set<T>();
            if(filter != null)
            {
                query = query.Where(filter); 
            }
            if(includeProperty != null)
            {
                foreach (var property in includeProperty.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            if(page_size > 0)
            {
                if(page_size > 4)
                {
                    page_size = 4;
                }
                query = query.Skip(page_size * (page_number - 1)).Take(page_size);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T model)
        {
            dbContext.Set<T>().Update(model);
        }
    }
}
