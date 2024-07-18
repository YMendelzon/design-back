using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCommon.Models
{
    public class Categories
    {
        public int CategoryID { get; set; }
        public string NameHe { get; set; }
        public string DescriptionHe { get; set; }
        public string nameEn;
        public string NameEn {
            get { return nameEn; }
            set
            {
                nameEn = value !=  "" ? value :NameHe;
            }
        }


        public string descriptionEn;
        public string DescriptionEn
        {
            get { return descriptionEn; }
            set
            {
                descriptionEn = value != "" ? value : DescriptionHe;
            }
        }

        public int UpCategory { get; set; }
        public DateTime CreateAt { get; set; }
        // מקבל שמות ען עברית אנגלית


        public string? ImageURL { get; set; }
        public IFormFile Image { get; set; }
    }
}
