using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionViewPractice.Models
{

    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
    }
}
