using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DALTestSystem
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public Group()
        {
            Users = new List<User>();
        }
    }
}