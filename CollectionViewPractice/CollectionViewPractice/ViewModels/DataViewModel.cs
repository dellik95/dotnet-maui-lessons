using CollectionViewPractice.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionViewPractice.ViewModels
{
    public class DataViewModel
    {
        private readonly IProductRepository productRepository;

        public ObservableCollection<Product> Products { get; set; }

        public DataViewModel(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
            this.productRepository.GetProducts().ContinueWith(p =>
            {
                if (this.Products == null)
                {
                    this.Products = new ObservableCollection<Product>(p.Result);
                    return;
                }
            });
        }
    }
}
