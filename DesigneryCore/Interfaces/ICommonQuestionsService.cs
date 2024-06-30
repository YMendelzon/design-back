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
        List<CommonQuestions> GetAllQuestions();
        bool PutCommonQuestions(int cqId, CommonQuestions c);
        bool PostCommonQuestions(CommonQuestions c);
    }
}