using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCommon.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string? NameHe { get; set; }
        public string? DescriptionHe { get; set; }
        public decimal? Price { get; set; }
        public string? ImageURL { get; set; }
        public DateTime? CreatedAt { get; set; }
        public decimal? SalePrice { get; set; }
        public bool? IsRecommended { get; set; }
        private string nameEn;
        
        private string? descriptionEn;

        public string? NameEn
        {
            get { return nameEn; }
            set
            {
                nameEn = value != "" ? value : NameHe;
            }
        }
      

        public string? DescriptionEn
        {
            get { return descriptionEn; }
            set
            {
                descriptionEn = value != "" ? value : DescriptionHe;
            }
        }

        //public IFormFile Image { get; set; }


    }
}
