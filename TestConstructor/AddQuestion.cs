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
using TestSystemLibrary;

namespace TestConstructor
{
    public partial class AddQuestion : Form
    {
        Question question;
        Answer answer;
        string img_path;
        bool isEdit;
        public AddQuestion(Question question, bool isEdit)
        {
            InitializeComponent();
            this.question = question;
            this.isEdit = isEdit;
            if (this.isEdit)
            {
                filiEdit();
            }
        }
        private void filiEdit()
        {
            this.textBox1.Text = this.question.Text;
            this.numericUpDown1.Value = this.question.Points;
            if(this.question.Img != null)
            {
                byte[] blob = Convert.FromBase64String(this.question.Img);
                Bitmap bitmap;
                using (MemoryStream ms = new MemoryStream(blob))
                {
                    bitmap = new Bitmap(ms);
                }
                this.pictureBox1.Image = bitmap;
            }
            
            foreach (Answer answer in this.question.Answers)
            {
                this.dataGridView1.Rows.Add(answer.Text, answer.IsTrue);
            }
        }

        //Додати варіант відповіді
        private void button5_Click(object sender, EventArgs e)
        {
            answer = new Answer();
            AddAnswer addAnswer = new AddAnswer(answer, false);
            DialogResult result = addAnswer.ShowDialog();
            if(result == DialogResult.Yes)
            {
                this.dataGridView1.Rows.Add(answer.Text, answer.IsTrue);
            }
        }

        //Редагувати відповідь
        private void button6_Click(object sender, EventArgs e)
        {
            if(this.dataGridView1.Rows.Count == 0)
            {
                return;
            }
            answer = new Answer();
            answer.Text = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
            answer.IsTrue = Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells[1].Value);
            AddAnswer addAnswer = new AddAnswer(answer, true);
            DialogResult result = addAnswer.ShowDialog();
            if(result == DialogResult.Yes)
            {
                this.dataGridView1.CurrentRow.Cells[0].Value = answer.Text;
                this.dataGridView1.CurrentRow.Cells[1].Value = answer.IsTrue;
            }
        }

        //Видалити відповідь
        private void button7_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count == 0)
            {
                return;
            }
            this.dataGridView1.Rows.RemoveAt(this.dataGridView1.CurrentRow.Index);
        }

        //Додати фото для запитання
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                this.img_path = openFileDialog.FileName;
                this.pictureBox1.Image = new Bitmap(openFileDialog.FileName);
            }
        }

        //Очистити фото
        private void button2_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image = null;
            this.img_path = null;
        }

        //Відмінити питання
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Зберегти питання
        private void button3_Click(object sender, EventArgs e)
        {
                if (this.textBox1.Text.Length > 0 && this.dataGridView1.Rows.Count > 2)
                {
                    this.question.Text = this.textBox1.Text;
                    this.question.Points = Convert.ToInt32(this.numericUpDown1.Value);
                    this.question.CountOfAnswers = this.dataGridView1.Rows.Count;

                    if (this.img_path != null)
                    {
                        byte[] blob = File.ReadAllBytes(this.img_path);
                        string img = Convert.ToBase64String(blob);
                        this.question.Img = img;
                    }

                    if(question.Answers.Count != 0)
                    {
                        question.Answers.Clear();
                    }

                    for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                    {
                        answer = new Answer();
                        answer.Text = this.dataGridView1.Rows[i].Cells[0].Value.ToString();
                        answer.IsTrue = Convert.ToBoolean(this.dataGridView1.Rows[i].Cells[1].Value);
                        this.question.Answers.Add(answer);
                    }

                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не всі поля заповнені або небостатньо варіантів для відповіді");
                }           
        }

        private void from_DataGrid_to_List()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                Answer answer = new Answer();
                answer.Text = row.Cells[0].Value.ToString();
                answer.IsTrue = Convert.ToBoolean(row.Cells[1].Value);
                question.Answers.Add(answer);
            }
        }
    }
}
