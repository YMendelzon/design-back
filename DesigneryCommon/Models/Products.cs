using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCommon.Models
{
    public class Products
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? ImageURL { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal? SalePrice { get; set; }
    }
}
