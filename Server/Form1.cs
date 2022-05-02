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
                if(user != null)
                {
                    
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void Users_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedTab.Text)
            {
                case "Tests":

                    break;
                case "Users":
                    List<User> Users = new List<User>();
                    Users = repositoryUser.GetAll().ToList();
                    foreach (User item in Users)
                    {
                        dataGridView1.Rows.Add(item.Id, item.FirstName, item.LastName, item.Login, item.Password, item.isAdmin);
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
    }
}
