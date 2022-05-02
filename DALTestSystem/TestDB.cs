using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystemLibrary;

namespace DALTestSystem
{
    public class TestDB
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int CountOfQuestions { get; set; }
        public int MinimumPassingPercent { get; set; }
        public string Img { get; set; }

        public ICollection<QuestionDB> QuestionDBs{ get; set; }
        public TestDB()
        {
            QuestionDBs = new List<QuestionDB>();
        }
    }
}
