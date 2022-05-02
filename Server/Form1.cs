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
        GenericUnitOfWork work = new GenericUnitOfWork(new TestUserContext(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString));
        IGenericRepository<User> repositoryUser;
        IGenericRepository<Group> repositoryGroup;
        public Form1()
        {
            InitializeComponent();
            repositoryUser = work.Repository<User>();
            repositoryGroup = work.Repository<Group>();
        }

        //Перевірка входу на сервер
        private void Form1_Load(object sender, EventArgs e)
        {
            StringBuilder log = new StringBuilder();
            StringBuilder pass = new StringBuilder();
            SignIn signIn = new SignIn(log, pass);
            DialogResult result = signIn.ShowDialog();
            if(result == DialogResult.OK)
            {
                string lg = log.ToString();
                string ps = pass.ToString();
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
            switch (tabControl1.SelectedTab.Text)
            {
                case "Tests":

                    break;
                case "Users":
                    dataGridView1.Rows.Clear();
                    dataGridView2.Rows.Clear();
                    List<User> Users = new List<User>();
                    Users = repositoryUser.GetAll().ToList();
                    foreach (User item in Users)
                    {
                        dataGridView1.Rows.Add(item.Id, item.FirstName, item.LastName, item.Login, item.Password, item.isAdmin);
                    }
                    User currentUser = Users[0];
                    List<Group> groups = currentUser.Groups.ToList();
                    foreach (Group item in groups)
                    {
                        dataGridView2.Rows.Add(item.Id, item.Name);
                    }
                    break;
                case "Groups":

                    break;
                case "Results":

                    break;
                case "Connections":

                    break;
            }
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
                dataGridView2.Rows.Clear();
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
            User user = repositoryUser.FindById(dataGridView1.CurrentRow.Cells[0].Value);
            user.LastName = "SUSID";
            List<Group> groups = new List<Group>();
            foreach (Group item in user.Groups)
            {
                groups.Add(item);
            }
            user.Groups.Clear();
            user.Groups = groups;
            work.SaveChanges();






            //user.Id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            //user.FirstName = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            //user.LastName = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            //user.Login = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            //user.Password = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            //user.isAdmin = Convert.ToBoolean(dataGridView1.CurrentRow.Cells[5].Value);
            //if(dataGridView2.Rows.Count > 0)
            //{
            //    foreach (DataGridViewRow item in dataGridView2.Rows)
            //    {
            //        Group group = repositoryGroup.FindById(item.Cells[0].Value);
            //        user.Groups.Add(group);
            //    }
            //}
            //AddUser addUser = new AddUser(user, true);
            //DialogResult result = addUser.ShowDialog();
            //if(result == DialogResult.OK)
            //{
               // repositoryUser.Update(user);
                //work.SaveChanges();
            //}

        }
    }
}
