using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesigneryCore.Interfaces;

namespace DesigneryCore.Services
{
    public class CommonQuestionsService : ICommonQuestionsService
    {
        public int getAllQuestions(int langId)
        {
            try
            {
                return 1;
            }
            catch { return 0; };
        }
    }
}
