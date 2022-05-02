using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystemLibrary;

namespace DALTestSystem
{
    public class QuestionDB
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Img { get; set; }
        public int Points { get; set; }
        public int CountOfAnswers { get; set; }
        public ICollection<AnswerDB> AnswerDBs { get; set; }
        public TestDB TestDB { get; set; }
        public QuestionDB()
        {
            AnswerDBs = new List<AnswerDB>();
        }
    }
}
