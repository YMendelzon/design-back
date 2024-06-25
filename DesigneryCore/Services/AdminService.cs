using DesigneryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCore.Services
{
    public class AdminService : IAdminService
    {
        public int Get()
        {
            try
            {
                return 1;
            }
            catch (Exception ex)
            {
                //write to logger
                throw;
            }
        }
    }
}
