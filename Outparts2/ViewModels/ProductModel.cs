using Outparts2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Outparts2.ViewModels
{
    public class ProductModel
    {
        public List<Product> _products { get; set; }
        public List<Product> findAll()
        {
            _products = new List<Product>{new Product()
            {
                Id = "1", Name = "Backup Camera", Photo = "backupcam.jpg", Price = 2.81
            },
            new Product()
            {
                Id = "2",Name="Cable Switch",Photo="cableswitch.jpg", Price=3.80
            },
            new Product()
            {
                Id = "3",Name="CBS",Photo="CBS.jpg", Price=3.83
            },
              new Product()
            {
                Id = "4",Name="CD",Photo="cd.jpg", Price=3.63
            },
            new Product()
            {
                Id = "5",Name="Cell Booster",Photo="cellbooster.png", Price=6.89
            },
            new Product()
            {
                Id = "6",Name="GPS",Photo="gps.jpg", Price=1.68
            },
              new Product()
            {
                Id = "7",Name="Hazard Radio",Photo="hazard radio.jpg", Price=3.83
            },
              new Product()
            {
                Id = "8",Name="Outdoor Switch",Photo="outdoor.jpg", Price=3.45
            },
            new Product()
            {
                Id = "9",Name="Radio",Photo="radios.jpg", Price=3.86
            },
              new Product()
            {
                Id = "10",Name="Tailgater",Photo="tailgater.jpg", Price=3.89
            }
            };
            return _products;
        }
        public Product find(string id)
        {
            List<Product> products = findAll(); //findAll() initializes a list of products setting values to the properties of the product model. it then returns the list. 
            var prod = products.Where(a => a.Id == id).FirstOrDefault();
            return prod; //returns a product with id it recieves as method parameter
        }
    }
}
