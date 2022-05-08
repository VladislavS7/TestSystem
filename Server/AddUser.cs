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
    public partial class AddUser : Form
    {
        GenericUnitOfWork work = new GenericUnitOfWork(new TestUserContext(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString));
        IGenericRepository<User> repositoryUser;
        IGenericRepository<Group> repositoryGroup;
        List<string> logins = new List<string>();
        List<Group> groups = new List<Group>();
        User user;
        bool isEdit = false;
        public AddUser(User user, bool isEdit)
        {
            InitializeComponent();
            this.isEdit = isEdit;
            this.user = user;
            repositoryUser = work.Repository<User>();
            repositoryGroup = work.Repository<Group>();
            getAllGroups();
            if (isEdit)
            {
                editUserLoad();
                //this.user = new User();
                //this.user.Id = user.Id;
            }
            logins = repositoryUser.GetAll().ToList().Select(x => x.Login).ToList();
        }

        //Заповнення списку груп
        private void getAllGroups()
        {
            groups = repositoryGroup.GetAll().ToList();
            foreach (Group item in groups)
            {
                checkedListBox1.Items.Add(item.Name);
            }
        }

        private void editUserLoad()
        {
            textBox1.Text = user.FirstName;
            textBox2.Text = user.LastName;
            textBox3.Text = user.Login;
            textBox4.Text = user.Password;
            checkBox1.Checked = user.isAdmin;
            if(user.Groups.Count > 0)
            {
                foreach (Group item in user.Groups)
                {
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        if(item.Name == checkedListBox1.Items[i].ToString())
                        {
                            checkedListBox1.SetItemChecked(i, true);
                        }
                    }
                }
            }
        }

        //Показати пароль
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.CheckState == CheckState.Checked)
            {
                textBox4.PasswordChar = '\0';
            }
            else
            {
                textBox4.PasswordChar = '*';
            }
        }

        //Відмінити
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Додати користувача
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.ForeColor == Color.Red)
            {
                MessageBox.Show("This login has already been used!", "", MessageBoxButtons.OK);
                return;
            }
            else if (textBox1.Text == " " || textBox2.Text == " " || textBox3.Text == " " || textBox4.Text == " ")
            {
                MessageBox.Show("Incorrect data", "", MessageBoxButtons.OK);
                return;
            }
            
            List<Group> groups = new List<Group>();
            if (!isEdit)
            {

                user.FirstName = textBox1.Text;
                user.LastName = textBox2.Text;
                user.Login = textBox3.Text;
                user.Password = textBox4.Text;
                user.isAdmin = checkBox1.Checked;
                if (checkedListBox1.CheckedItems.Count > 0)//Заповнення груп користувача (якщо вибрані)
                {
                    foreach (string item in checkedListBox1.CheckedItems)
                    {
                        Group group = repositoryGroup.FindAll(x => x.Name == item).FirstOrDefault();
                        groups.Add(group);
                    }
                }
                user.Groups = groups;
                repositoryUser.Add(user);
                work.SaveChanges();
            }
            else
            {
                User newUser = repositoryUser.FindById(user.Id);
                newUser.FirstName = textBox1.Text;
                newUser.LastName = textBox2.Text;
                newUser.Login = textBox3.Text;
                newUser.Password = textBox4.Text;
                newUser.isAdmin = checkBox1.Checked;
                List<string> names = new List<string>();
                foreach (var item in checkedListBox1.CheckedItems)//Всі вибрані групи
                {
                    names.Add(item.ToString());
                }

                foreach (var item in names)
                {
                    Group group = new Group();
                    group = repositoryGroup.FindAll(x => x.Name == item).FirstOrDefault();
                    groups.Add(group);
                }

                newUser.Groups.Clear();
                newUser.Groups = groups;
                work.SaveChanges();
            }
            work.Dispose();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

 

        //Перевірка чи можна використовувати цей логін
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!isEdit)
            {
                if (logins.Contains(textBox3.Text))
                {
                    textBox3.ForeColor = Color.Red;
                }
                else
                {
                    textBox3.ForeColor = Color.Black;
                }
            }
            else
            {
                if(logins.Contains(textBox3.Text) && textBox3.Text != user.Login)
                {
                    textBox3.ForeColor = Color.Red;
                }
                else
                {
                    textBox3.ForeColor = Color.Black;
                }
            }
            
        }

    }
}
