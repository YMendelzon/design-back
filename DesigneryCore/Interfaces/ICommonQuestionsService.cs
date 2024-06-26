using DesigneryCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCore.Interfaces
{
    public interface ICommonQuestionsService
    {
        List<CommonQuestions> GetAllQuestions(int langId);
        bool ChangeRating(int cqId, int rating);
    }
}