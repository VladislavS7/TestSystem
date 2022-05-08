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
    public partial class AddGroup : Form
    {
        string conStr = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
        GenericUnitOfWork work;
        IGenericRepository<User> repositoryUser;
        IGenericRepository<Group> repositoryGroup;
        Group group;
        bool isEdit;
        DataGridView DataGridView;
        int GroupId;
        public AddGroup(bool isEdit, DataGridView gridView)
        {
            InitializeComponent();
            this.isEdit = isEdit;
            DataGridView = gridView;
            fillAllUsers();
            if (isEdit)
            {
                work = new GenericUnitOfWork(new TestUserContext(conStr));
                repositoryGroup = work.Repository<Group>();
                group = repositoryGroup.FindById(gridView.CurrentRow.Cells[0].Value);
                redactFilling(group);
                GroupId = group.Id;
                work.Dispose();
            }
        }

        private void redactFilling(Group group)
        {
            List<int> ids = group.Users.Select(x => x.Id).ToList();

            textBox1.Text = group.Name;
            foreach (int id in ids)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if(Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value) == id)
                    {
                        dataGridView1.Rows[i].Cells[0].Value = true;
                        break;
                    }
                }
            }
        }

        private void fillAllUsers()
        {
            work = new GenericUnitOfWork(new TestUserContext(conStr));
            repositoryUser = work.Repository<User>();
            List<User> users = repositoryUser.GetAll().ToList();
            foreach (User user in users)
            {
                int rowIndex = dataGridView1.Rows.Add();

                var row = this.dataGridView1.Rows[rowIndex];

                row.Cells[1].Value = user.Id;
                row.Cells[2].Value = user.FirstName;
                row.Cells[3].Value = user.LastName;
            }

            work.Dispose();
        }

        //Відміна
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        //OK
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0)
            {
                work = new GenericUnitOfWork(new TestUserContext(conStr));
                repositoryGroup = work.Repository<Group>();
                repositoryUser = work.Repository<User>();
                if (!isEdit)
                {
                    Group group = new Group();
                    group.Name = textBox1.Text;
                    List<int> ids = new List<int>();
                    foreach (DataGridViewRow item in dataGridView1.Rows)
                    {
                        if (Convert.ToBoolean(item.Cells[0].Value))
                        {
                            ids.Add(Convert.ToInt32(item.Cells[1].Value));
                        }
                    }
                    foreach (int id in ids)
                    {
                        group.Users.Add(repositoryUser.FindById(id));
                    }
                    repositoryGroup.Add(group);
                    work.SaveChanges();
                    DataGridView.Rows.Add(group.Id, group.Name);
                }
                else
                {
                    if(textBox1.Text.Length > 0)
                    {
                        Group group = repositoryGroup.FindById(GroupId);
                        group.Name = textBox1.Text;
                        group.Users.Clear();
                        List<int> ids = new List<int>();
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)//ID всіх відмічених користувачів
                        {
                            if(Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true)
                            {
                                ids.Add(Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value));
                            }
                        }
                        foreach (int id in ids)
                        {
                            group.Users.Add(repositoryUser.FindById(id));
                        }
                        work.SaveChanges();
                    }
                }
                work.Dispose();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
