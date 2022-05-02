using System.Data.Entity;

namespace DALTestSystem
{
    public class TestUserInitializeDB : CreateDatabaseIfNotExists<TestUserContext>
    {
        protected override void Seed(TestUserContext context)
        {
            Group group = new Group() { Name = "GR1" };
            User user = new User() { FirstName = "VL", LastName = "S", Login = "a", Password = "a", isAdmin = true };
            user.Groups.Add(group);
            group.Users.Add(user);
            context.Users.Add(user);
            context.Groups.Add(group);
            context.SaveChanges();
        }
    }
}