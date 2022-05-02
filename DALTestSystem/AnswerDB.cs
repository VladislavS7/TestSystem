using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystemLibrary;

namespace DALTestSystem
{
    public class AnswerDB
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsTrue { get; set; }
        public QuestionDB QuestionDB{ get; set; }
        public ICollection<TestUsers> TestUsers { get; set; }
        public AnswerDB()
        {
            TestUsers = new List<TestUsers>();
        }
    }
}
