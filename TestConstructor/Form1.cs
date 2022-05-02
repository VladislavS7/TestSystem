using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using TestSystemLibrary;

namespace TestConstructor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Instruments.IsFormEmpty = true;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            Instruments.isSaved = true;
            Instruments.test = new Test();
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            using (FileStream fs = new FileStream("TestsNames.xml", FileMode.Open))
            {
                Instruments.testsNames = (List<string>)serializer.Deserialize(fs);
            }
        }

        //Створення Тесту
        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Instruments.IsFormEmpty || Instruments.isSaved)
            {
                Instruments.test = new Test();
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                richTextBox1.Text = "";
                numericUpDown3.Value = 0;
                pictureBox1.Image = null;
                pictureBox2.Image = null;
                createToolStripMenuItem.Enabled = false;
                 
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                DialogResult result = folderBrowserDialog.ShowDialog();//Вибір папки куди зберегти
                if (result == DialogResult.OK)
                {
                    Instruments.pathToFolder = folderBrowserDialog.SelectedPath;
                    Instruments.isSaved = false;//Тест при створенні не збережені
                    Instruments.isOpened = false;
                    Instruments.IsFormEmpty = false;
                }
            }
            else
            {
                DialogResult dialog = MessageBox.Show("Ви не зберегли тест. Зберегти?", "Warning", MessageBoxButtons.YesNo);
                if(dialog == DialogResult.Yes)//Зберегти 
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
                Instruments.isSaved = true;
                createToolStripMenuItem_Click(sender, e);
            }
        }

        //Додати питання 
        private void button2_Click(object sender, EventArgs e)
        {
            Question question = new Question();
            AddQuestion addQuestion = new AddQuestion(question, false);
            DialogResult result = addQuestion.ShowDialog();
            if(result == DialogResult.Yes)
            {
                this.dataGridView2.Rows.Clear();//Очищення таблиці відповідей
                this.dataGridView1.Rows.Add(question.Text, question.Points, question.CountOfAnswers);
                if(question.Img != null)
                {
                    byte[] blob = Convert.FromBase64String(question.Img);
                    Bitmap bitmap;
                    using (MemoryStream ms = new MemoryStream(blob))
                    {
                        bitmap = new Bitmap(ms);
                    }
                    this.pictureBox2.Image = bitmap;
                }
                foreach (Answer answer in question.Answers)
                {
                    this.dataGridView2.Rows.Add(answer.Text, answer.IsTrue);
                }
                this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Selected = true;//Вибрати останній доданий рядок
                Instruments.test.questions.Add(question);
                Instruments.test.CountOfQuestions++;
                textBox3.Text = Instruments.test.CountOfQuestions.ToString();
                Instruments.isSaved = false;
            }
            
        }

        //Картинка для теста
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;...";
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if(dialogResult == DialogResult.OK)
            {
                
                pictureBox1.Image = new Bitmap(openFileDialog.FileName);
                byte[] blob = File.ReadAllBytes(openFileDialog.FileName);
                string img = Convert.ToBase64String(blob);
                Instruments.test.Img = img;
            }
            Instruments.isSaved = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                this.dataGridView2.Rows.Clear();
                foreach (Answer answer in Instruments.test.questions[e.RowIndex].Answers)
                {
                    this.dataGridView2.Rows.Add(answer.Text, answer.IsTrue);
                }
                if (Instruments.test.questions[e.RowIndex].Img != null)
                {
                    byte[] blob = Convert.FromBase64String(Instruments.test.questions[e.RowIndex].Img);
                    Bitmap bitmap;
                    using (MemoryStream ms = new MemoryStream(blob))
                    {
                        bitmap = new Bitmap(ms);
                    }
                    this.pictureBox2.Image = bitmap;
                }
                else
                {
                    this.pictureBox2.Image = null;
                }
            }
                
        }

        //Редагувати питання
        private void button3_Click(object sender, EventArgs e)
        {
            if(this.dataGridView1.Rows.Count > 0)
            {
                AddQuestion addQuestion = new AddQuestion(Instruments.test.questions[this.dataGridView1.CurrentRow.Index], true);
                DialogResult result = addQuestion.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    this.dataGridView1.CurrentRow.Cells[0].Value = Instruments.test.questions[this.dataGridView1.CurrentRow.Index].Text;
                    this.dataGridView1.CurrentRow.Cells[1].Value = Instruments.test.questions[this.dataGridView1.CurrentRow.Index].Points;
                    this.dataGridView1.CurrentRow.Cells[2].Value = Instruments.test.questions[this.dataGridView1.CurrentRow.Index].CountOfAnswers;

                    this.dataGridView2.Rows.Clear();
                    foreach (Answer answer in Instruments.test.questions[this.dataGridView1.CurrentRow.Index].Answers)
                    {
                        this.dataGridView2.Rows.Add(answer.Text, answer.IsTrue);
                    }
                    if (Instruments.test.questions[this.dataGridView1.CurrentRow.Index].Img != null)
                    {
                        byte[] blob = Convert.FromBase64String(Instruments.test.questions[this.dataGridView1.CurrentRow.Index].Img);
                        Bitmap bitmap;
                        using (MemoryStream ms = new MemoryStream(blob))
                        {
                            bitmap = new Bitmap(ms);
                        }
                        this.pictureBox2.Image = bitmap;
                    }
                    Instruments.isSaved = false; //Після редагування зміни не збережені
                }
            }
            
        }

        //Видалення питання
        private void button4_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count == 0)
            {
                return;
            }
            Instruments.test.questions.RemoveAt(dataGridView1.CurrentRow.Index);
            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);

            dataGridView2.Rows.Clear();
            pictureBox2.Image = null;
            Instruments.isSaved = false;
            
        }

        private void AddTestNameToTests(string name)
        {
            XmlDocument document = new XmlDocument();
            document.Load("TestsNames.xml");
            XmlNode path = document.CreateElement("string");
            XmlNode nameText = document.CreateTextNode(Instruments.pathToFolder + "\\" + name);
            path.AppendChild(nameText);
            document.DocumentElement.AppendChild(path);
            document.Save("TestsNames.xml");
        }

        //Зберегти тест
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Instruments.test.Title = textBox1.Text;
            Instruments.test.Author = textBox2.Text;
            Instruments.test.Description = richTextBox1.Text;
            Instruments.test.CountOfQuestions = Convert.ToInt32(textBox3.Text);
            Instruments.test.MinimumPassingPercent = Convert.ToInt32(numericUpDown3.Value);

            if (Instruments.test.Title != null && Instruments.test.MinimumPassingPercent > 0 &&
                Instruments.test.Author != null && Instruments.test.CountOfQuestions > 0 &&
                Instruments.test.Img != null) 
            {
                if (!Instruments.isOpened)
                {
                    AddTestNameToTests(Instruments.test.Title);//Додати шлях до тесту в файл де зберігаються шляхи до всіх тестів
                    Serializer serializer = new Serializer();
                    string xml = serializer.Serialize<Test>(Instruments.test);
                    string path = Instruments.pathToFolder + "\\" + Instruments.test.Title;
                    File.WriteAllText(path, xml);

                    Instruments.isSaved = true;
                    createToolStripMenuItem.Enabled = true;
                }
                else
                {
                    Serializer serializer = new Serializer();
                    string xml = serializer.Serialize<Test>(Instruments.test);
                    string path = Instruments.currentPathToTest;
                    File.WriteAllText(path, xml);

                    Instruments.isSaved = true;
                    createToolStripMenuItem.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Не всі поля теста заповнені", "Error", MessageBoxButtons.OK);
            }
        }

        //Закрити конструктор тестів
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Instruments.isSaved == false)
            {
                DialogResult dialog = MessageBox.Show("Ви не зберегли тест. Зберегти?", "Warning", MessageBoxButtons.YesNo);
            
                if(dialog == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
            }
            this.Close();
        }

        //Відкрити тест
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Instruments.isSaved == false && Instruments.IsFormEmpty == false)
            {
                DialogResult dialogResult = MessageBox.Show("Ви не зберегли тест. Зберегти?", "Попередження", MessageBoxButtons.YesNo);
                if(dialogResult == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
            }
            StringBuilder name = new StringBuilder();
            ExistingTests existingTests = new ExistingTests(name);
            DialogResult result = existingTests.ShowDialog();
            if(result == DialogResult.OK)
            {
                Instruments.currentPathToTest = name.ToString();
                Instruments.isOpened = true;
                Instruments.IsFormEmpty = false;
                Instruments.isSaved = true;
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                richTextBox1.Text = "";
                numericUpDown3.Value = 0;
                this.pictureBox1.Image = null;
                this.pictureBox2.Image = null;
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                Instruments.test = new Test();
                Serializer serializer = new Serializer();
                string xmlDoc = File.ReadAllText(name.ToString());
                Instruments.test = serializer.Deserialize<Test>(xmlDoc);
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;

                textBox1.Text = Instruments.test.Title;
                textBox2.Text = Instruments.test.Author;
                richTextBox1.Text = Instruments.test.Description;
                textBox3.Text = Instruments.test.CountOfQuestions.ToString();
                numericUpDown3.Value = Instruments.test.MinimumPassingPercent;
                byte[] blob = Convert.FromBase64String(Instruments.test.Img);
                Bitmap bitmap;
                using (MemoryStream ms = new MemoryStream(blob))
                {
                    bitmap = new Bitmap(ms);
                }
                this.pictureBox1.Image = bitmap;

                foreach (Question question in Instruments.test.questions)
                {
                    this.dataGridView1.Rows.Add(question.Text, question.Points, question.CountOfAnswers);
                }
                dataGridView1.Rows[0].Selected = true;
                foreach (Answer answer in Instruments.test.questions[0].Answers)
                {
                    this.dataGridView2.Rows.Add(answer.Text, answer.IsTrue);
                }
                if (Instruments.test.questions[0].Img != null)
                {
                    blob = Convert.FromBase64String(Instruments.test.questions[0].Img);
                    using (MemoryStream ms = new MemoryStream(blob))
                    {
                        bitmap = new Bitmap(ms);
                    }
                    this.pictureBox2.Image = bitmap;
                }
                else
                {
                    this.pictureBox2.Image = null;
                }
            }
        }

    }
}
