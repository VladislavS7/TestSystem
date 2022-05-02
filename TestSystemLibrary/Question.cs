using System.Collections.Generic;

namespace TestSystemLibrary
{
    public class Question
    {
        public string Text { get; set; }
        public string Img { get; set; }
        public int Points { get; set; }
        public int CountOfAnswers { get; set; }
        public List<Answer> Answers { get; set; }
        public Question()
        {
            Answers = new List<Answer>();
        }
    }
}