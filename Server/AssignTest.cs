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
    public partial class AssignTest : Form
    {
        string conStr = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
        GenericUnitOfWork work;
        IGenericRepository<User> repositoryUser;
        IGenericRepository<Group> repositoryGroup;
        IGenericRepository<TestDB> repositoryTestDB;
        IGenericRepository<TestUsers> repositoryTestUsers;
        List<TestDB> testDBs;
        DataGridView dataGridView;
        public AssignTest(DataGridView dataGridView)
        {
            this.dataGridView = dataGridView;
            InitializeComponent();
            LoadTests();
        }

        private void LoadTests()
        {
            work = new GenericUnitOfWork(new TestUserContext(conStr));
            repositoryTestDB = work.Repository<TestDB>();
            testDBs = repositoryTestDB.GetAll().ToList();
            foreach (TestDB item in testDBs)
            {
                comboBox1.Items.Add("[" + item.Id + "]" + item.Title);
            }
            work.Dispose();
        }

        //Cancel
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        //Assign
        private void button2_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem != null)
            {
                work = new GenericUnitOfWork(new TestUserContext(conStr));
                repositoryTestUsers = work.Repository<TestUsers>();
                repositoryGroup = work.Repository<Group>();
                if (radioButton1.Checked)
                {
                    TestUsers testUsers = new TestUsers();
                    testUsers.DateTime = dateTimePicker1.Value;
                    testUsers.ResultPoints = 0;
                    testUsers.isPassed = false;

                    string Testid = comboBox1.SelectedItem.ToString();
                    int i = Testid.IndexOf(']');
                    Testid = Testid.Remove(i);
                    Testid = Testid.Remove(0, 1);

                    testUsers.TestId = Convert.ToInt32(Testid);
                    testUsers.UserId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

                    repositoryTestUsers.Add(testUsers);
                    work.SaveChanges();

                    testUsers = repositoryTestUsers.GetAll().Last();
                    dataGridView.Rows.Add(testUsers.Id, testUsers.TestId, testUsers.UserId, testUsers.DateTime);
                }
                if (radioButton2.Checked)
                {
                    int idGroup = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                    Group group = repositoryGroup.FindById(idGroup);
                    List<int> usersIds = new List<int>();
                    foreach (User item in group.Users)
                    {
                        usersIds.Add(item.Id);
                    }
                    List<TestUsers> testUsers = new List<TestUsers>();

                    string Testid = comboBox1.SelectedItem.ToString();
                    int index = Testid.IndexOf(']');
                    Testid = Testid.Remove(index);
                    Testid = Testid.Remove(0, 1);

                    for (int i = 0; i < usersIds.Count; i++)
                    {
                        testUsers.Add(
                            new TestUsers() { DateTime = dateTimePicker1.Value, ResultPoints = 0, isPassed = false,
                            TestId = Convert.ToInt32(Testid), UserId = usersIds[i]}
                            );
                    }

                    foreach (TestUsers item in testUsers)
                    {
                        repositoryTestUsers.Add(item);
                    }
                    work.SaveChanges();
                    testUsers.Clear();
                    testUsers = repositoryTestUsers.GetAll().ToList();
                    testUsers.Reverse();

                    for (int i = 0; i < usersIds.Count; i++)
                    {
                        dataGridView.Rows.Add(testUsers[i].Id, testUsers[i].TestId, testUsers[i].UserId, testUsers[i].DateTime);
                    }
                }
                this.Close();
                work.Dispose();
            }
        }

        //Коли вибирається тест
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = comboBox1.SelectedItem.ToString();
            int i = id.IndexOf(']');
            id = id.Remove(i);
            id = id.Remove(0, 1);
            TestDB testDB = testDBs.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
            textBox1.Text = testDB.Id.ToString();
            textBox2.Text = testDB.Title;
            textBox3.Text = testDB.Author;
            textBox4.Text = testDB.CountOfQuestions.ToString();
            textBox5.Text = testDB.MinimumPassingPercent.ToString();
        }

        //Для користувача
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            work = new GenericUnitOfWork(new TestUserContext(conStr));
            repositoryUser = work.Repository<User>();
            List<User> users = repositoryUser.GetAll().ToList();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Add("ID", "ID");
            dataGridView1.Columns.Add("FN", "Name");
            dataGridView1.Columns.Add("SN", "Surname");
            dataGridView1.Columns.Add("IA", "isAdmin");

            foreach (DataGridViewColumn item in dataGridView1.Columns)
            {
                item.ReadOnly = true;
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            foreach (User item in users)
            {
                dataGridView1.Rows.Add(item.Id, item.FirstName, item.LastName, item.isAdmin);
            }

            work.Dispose();
        }

        //Для групи
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            work = new GenericUnitOfWork(new TestUserContext(conStr));
            repositoryGroup = work.Repository<Group>();
            List<Group> groups  = repositoryGroup.GetAll().ToList();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Add("ID", "ID");
            dataGridView1.Columns.Add("N", "Name");
            foreach (DataGridViewColumn item in dataGridView1.Columns)
            {
                item.ReadOnly = true;
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            foreach (Group item in groups)
            {
                dataGridView1.Rows.Add(item.Id, item.Name);
            }
            work.Dispose();
        }
    }
}
