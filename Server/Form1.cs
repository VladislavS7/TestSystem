using DALTestSystem;
using RepositoryLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        string conStr = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
        GenericUnitOfWork work;
        IGenericRepository<User> repositoryUser;
        IGenericRepository<Group> repositoryGroup;
        public Form1()
        {
            InitializeComponent();
            work = new GenericUnitOfWork(new TestUserContext(conStr));
            //repositoryUser = work.Repository<User>();
            //repositoryGroup = work.Repository<Group>();
        }

        //Перевірка входу на сервер
        private void Form1_Load(object sender, EventArgs e)
        {
            work = new GenericUnitOfWork(new TestUserContext(conStr));
            repositoryUser = work.Repository<User>();
            StringBuilder log = new StringBuilder();
            StringBuilder pass = new StringBuilder();
            SignIn signIn = new SignIn(log, pass);
            DialogResult result = signIn.ShowDialog();
            if(result == DialogResult.OK)
            {
                string lg = log.ToString();
                string ps = pass.ToString();
                repositoryUser = work.Repository<User>();
                User user = repositoryUser.FindAll(u => u.Login == lg && u.Password == ps).FirstOrDefault();
                if(user == null || user.isAdmin == false)
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }


        //Перехід між вкладками 
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            work = new GenericUnitOfWork(new TestUserContext(conStr));
            repositoryUser = work.Repository<User>();
            repositoryGroup = work.Repository<Group>();
            List<User> users = new List<User>();
            List<Group> groups = new List<Group>();
            switch (tabControl1.SelectedTab.Text)
            {
                case "Tests":

                    break;
                case "Users":
                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.Rows.Clear();
                        dataGridView2.Rows.Clear();
                    }
                        repositoryUser = work.Repository<User>();
                        users = repositoryUser.GetAll().ToList();
                        foreach (User item in users)
                        {
                            dataGridView1.Rows.Add(item.Id, item.FirstName, item.LastName, item.Login, item.Password, item.isAdmin);
                        }
                        User currentUser = users[0];
                        groups = currentUser.Groups.ToList();
                        foreach (Group item in groups)
                        {
                            dataGridView2.Rows.Add(item.Id, item.Name);
                        }
                    
                    break;
                case "Groups":
                    if (dataGridView3.Rows.Count > 0)
                    {
                        dataGridView3.Rows.Clear();
                        dataGridView4.Rows.Clear();
                    }
                        groups = repositoryGroup.GetAll().ToList();
                    if(groups.Count > 0)
                    {
                        foreach (Group group in groups)
                        {
                            dataGridView3.Rows.Add(group.Id, group.Name);
                        }
                        if(groups[0].Users.Count > 0)
                        {
                            foreach (User user in groups[0].Users)
                            {
                                dataGridView4.Rows.Add(user.Id, user.FirstName, user.LastName);
                            }
                        }
                    }
                        
                    
                    break;
                case "Results":

                    break;
                case "Connections":

                    break;
            }
            work.Dispose();
        }

        //Додати користувача
        private void button1_Click(object sender, EventArgs e)
        {
            User user = new User();
            AddUser addUser = new AddUser(user, false);
            DialogResult result = addUser.ShowDialog();
            if(result == DialogResult.OK)
            {
                dataGridView1.Rows.Add(user.Id, user.FirstName, user.LastName, user.Login, user.Password, user.isAdmin);
            }
        }

        //Підвантаження груп в яких є користувач
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                work = new GenericUnitOfWork(new TestUserContext(conStr));
                repositoryUser = work.Repository<User>();
                dataGridView2.Rows.Clear();
                repositoryUser = work.Repository<User>();
                User user = repositoryUser.FindById(dataGridView1.CurrentRow.Cells[0].Value);
                foreach (Group item in user.Groups)
                {
                    dataGridView2.Rows.Add(item.Id, item.Name);
                }
            }
        }

        //Редагувати користувача
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                work = new GenericUnitOfWork(new TestUserContext(conStr));
                repositoryUser = work.Repository<User>();
                User user = repositoryUser.FindById(dataGridView1.CurrentRow.Cells[0].Value);

                AddUser addUser = new AddUser(user, true);
                DialogResult result = addUser.ShowDialog();
                if (result == DialogResult.OK)
                {
                    work = new GenericUnitOfWork(new TestUserContext(conStr));
                    repositoryUser = work.Repository<User>();
                    User user1 = repositoryUser.FindById(dataGridView1.CurrentRow.Cells[0].Value);
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value = user1.Id;
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value = user1.FirstName;
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value = user1.LastName;
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value = user1.Login;
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[4].Value = user1.Password;
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[5].Value = user1.isAdmin;

                    dataGridView2.Rows.Clear();

                    foreach (Group item in user1.Groups)
                    {
                        dataGridView2.Rows.Add(item.Id, item.Name);
                    }
                    work.Dispose();
                }
            }
        }

        //Видалити користувача
        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Do you really want to delete this user?", "Information", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    work = new GenericUnitOfWork(new TestUserContext(conStr));
                    repositoryUser = work.Repository<User>();
                    User user = repositoryUser.FindById(dataGridView1.CurrentRow.Cells[0].Value);
                    repositoryUser.Remove(user);
                    work.SaveChanges();


                    dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                    if (dataGridView1.Rows.Count > 0)
                    {
                        User user1 = repositoryUser.FindById(dataGridView1.Rows[0].Cells[0].Value);
                        dataGridView2.Rows.Clear();
                        foreach (Group item in user1.Groups)
                        {
                            dataGridView2.Rows.Add(item.Id, item.Name);
                        }
                    }
                    work.Dispose();
                }
            }
        }

        //Додати групу
        private void button4_Click(object sender, EventArgs e)
        {
            work = new GenericUnitOfWork(new TestUserContext(conStr));
            repositoryGroup = work.Repository<Group>();
            AddGroup addGroup = new AddGroup(false, dataGridView3);
            DialogResult result = addGroup.ShowDialog();
            if(result == DialogResult.OK)
            {
                int Id = Convert.ToInt32(dataGridView3.Rows[dataGridView3.Rows.Count - 1].Cells[0].Value);
                Group group1 = repositoryGroup.FindById(Id);
                dataGridView4.Rows.Clear();
                foreach (User user in group1.Users)
                {
                    dataGridView4.Rows.Add(user.Id, user.FirstName, user.LastName);
                }
            }
            work.Dispose();
        }

        //Підвантаження користувачів у групі
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            work = new GenericUnitOfWork(new TestUserContext(conStr));
            repositoryGroup = work.Repository<Group>();
            int Id = Convert.ToInt32(dataGridView3.Rows[e.RowIndex].Cells[0].Value);
            Group group = repositoryGroup.FindById(Id);
            List<User> users = group.Users.ToList();
            dataGridView4.Rows.Clear();
            foreach (User user in users)
            {
                dataGridView4.Rows.Add(user.Id, user.FirstName, user.LastName);
            }
            work.Dispose();
        }

        //Редагувати групу
        private void button5_Click(object sender, EventArgs e)
        {
            if(dataGridView3.Rows.Count > 0)
            {
                work = new GenericUnitOfWork(new TestUserContext(conStr));
                repositoryGroup = work.Repository<Group>();
                Group group = repositoryGroup.FindById(dataGridView3.CurrentRow.Cells[0].Value);
                AddGroup addGroup = new AddGroup( true, dataGridView3);
                DialogResult result = addGroup.ShowDialog();
                if (result == DialogResult.OK)
                {
                    work = new GenericUnitOfWork(new TestUserContext(conStr));
                    repositoryGroup = work.Repository<Group>();
                    int id = group.Id;
                    Group group1 = repositoryGroup.FindById(id);
                    for (int i = 0; i < dataGridView3.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(dataGridView3.Rows[i].Cells[0].Value) == id)
                        {
                            dataGridView3.Rows[i].Cells[0].Value = group1.Id;
                            dataGridView3.Rows[i].Cells[1].Value = group1.Name;
                            break;
                        }
                    }
                    dataGridView4.Rows.Clear();
                    foreach (User user in group1.Users)
                    {
                        dataGridView4.Rows.Add(user.Id, user.FirstName, user.LastName);
                    }
                    work.Dispose();
                }
                work.Dispose();
            }
        }

        //Видалити групу
        private void button6_Click(object sender, EventArgs e)
        {
            if(dataGridView3.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Do you really want to delete this group?", "Information", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    work = new GenericUnitOfWork(new TestUserContext(conStr));
                    repositoryGroup = work.Repository<Group>();
                    Group group = repositoryGroup.FindById(dataGridView3.CurrentRow.Cells[0].Value);
                    repositoryGroup.Remove(group);
                    work.SaveChanges();

                    dataGridView3.Rows.Remove(dataGridView3.CurrentRow);
                    dataGridView4.Rows.Clear();
                    if (dataGridView3.Rows.Count > 0)
                    {
                        Group group1 = repositoryGroup.FindById(dataGridView3.Rows[0].Cells[0].Value);
                        foreach (User user in group1.Users)
                        {
                            dataGridView4.Rows.Add(user.Id, user.FirstName, user.LastName);
                        }
                    }
                    work.Dispose();
                }
            }
        }
    }
}
