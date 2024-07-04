using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesigneryCommon.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public string QuestionHe { get; set; }
        public string AnswerHe { get; set; }

        //ברירת מחדל - אם אין מלל באנגלית נראה את העברית במקום
        private string questionEn;

        public string QuestionEn
        {
            get { return questionEn; }
            set
            {
                questionEn = value != "" ? value : QuestionHe;
            }
        }
        private string answerEn;

        public string AnswerEn
        {
            get { return answerEn; }
            set
            {
                answerEn = value != "" ? value : AnswerHe;
            }
        }

        public int Rating { get; set; }


    }
}
