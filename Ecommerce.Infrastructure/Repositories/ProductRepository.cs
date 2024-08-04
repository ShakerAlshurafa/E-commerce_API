using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext dbContext;

        public ProductRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProductsByCategory(int categoryId)
        {
            //// [ eager loading ]
            ////
            //var products = (IEnumerable<Product>)await dbContext.products.Include(p => p.Category)
            //    .Where(c => c.CategoryId == categoryId)
            //    .ToListAsync();
            //return products;


            //// [ Explicit Loading ]
            ///
            //var products = await dbContext.products
            //    .Where(c => c.CategoryId == categoryId)
            //    .ToListAsync();
            //foreach (var product in products)
            //{
            //    await dbContext.Entry(product).Reference(r => r.Category).LoadAsync();
            //}
            //return products;


            //// [Lazy Loading]
            //// download lazy pakage --> proxies
            //// in program.cs add (UseLazyLoadingProxies) after "DefaultConnection"
            //// add virtual before navigational property

            var products = await dbContext.Products
                .Where(c => c.CategoryId == categoryId)
                .ToArrayAsync();
            return products;
        }
    }
}
