using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALTestSystem
{
    public class TestUsers
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int ResultPoints { get; set; }
        public bool isPassed { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public User User { get; set; }
        public TestDB TestDB { get; set; }
        public ICollection<AnswerDB> AnswerDBs { get; set; }
        public TestUsers()
        {
            AnswerDBs = new List<AnswerDB>();
        }
    }
}
