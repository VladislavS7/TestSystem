using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystemLibrary
{
    public class Test
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int CountOfQuestions { get; set; }
        public int MinimumPassingPercent { get; set; }
        public string Img { get; set; }
        public List<Question> questions { get; set; }
        public Test()
        {
            questions = new List<Question>();
        }
    }
}
