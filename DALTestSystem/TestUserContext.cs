using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystemLibrary;

namespace DALTestSystem
{
    public class TestUserContext: DbContext
    {
        public TestUserContext(string conStr) : base(conStr)
        {

        }

        static TestUserContext()
        {
            Database.SetInitializer<TestUserContext>(new TestUserInitializeDB());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<TestDB> TestsDB { get; set; }
        public DbSet<QuestionDB> QuestionsDB { get; set; }
        public DbSet<AnswerDB> AnswersDB { get; set; }
        public DbSet<TestUsers> TestUsers { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TestDB>().HasKey(x => x.Id);
            //modelBuilder.Entity<QuestionDB>().HasKey(x => x.Id);
            //modelBuilder.Entity<AnswerDB>().HasKey(x => x.Id);
            modelBuilder.Entity<TestDB>().HasMany(x => x.QuestionDBs).WithRequired(y => y.TestDB);
            modelBuilder.Entity<QuestionDB>().HasMany(x => x.AnswerDBs).WithRequired(y => y.QuestionDB);
            modelBuilder.Entity<User>().HasMany(x => x.Groups).WithMany(y => y.Users);
            modelBuilder.Entity<AnswerDB>().HasMany(x => x.TestUsers).WithMany(y => y.AnswerDBs);
        }
    }
}
