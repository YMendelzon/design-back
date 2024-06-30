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
        public string NameHe { get; set; }
        public string? DescriptionHe { get; set; }
        public decimal ?Price { get; set; }
        public string? ImageURL { get; set; }
        public DateTime ?CreatedAt { get; set; }
        public decimal? SalePrice { get; set; }
        private string nameEn;

        public string NameEn
        {
            get { return nameEn; }
            set
            {
                nameEn = value != "" ? value : NameHe;
            }
        }
        private string? descriptionEN;

        public string ?DescriptionEN 
        {
            get { return descriptionEN; }
            set { descriptionEN
                    
                    = value != "" ? value : DescriptionHe; }
        }


    }
}
