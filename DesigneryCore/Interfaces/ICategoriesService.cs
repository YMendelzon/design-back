using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesigneryCommon.Models;

namespace DesigneryCore.Interfaces
{
    public interface ICategoriesService
    {
        List<Categories> GetAllCategories();
        bool postCategories(Categories c);
        bool PutCategories(int cId, Categories c);
        Categories GetCategoryById(int cId);
        //List<Categories> GetUpCategoriesByCategoryID(int categoryID);
        List<Categories> GetSubcategories(int categoryId);

    }
}
