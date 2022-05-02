using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestSystemLibrary;

namespace TestConstructor
{
    public partial class AddAnswer : Form
    {
        Answer answer;

        public AddAnswer(Answer answer, bool isEdit)
        {
            InitializeComponent();
            this.answer = answer;
            if (isEdit)
            {
                fill_filds();
            }
        }

        private void fill_filds()
        {
            this.richTextBox1.Text = answer.Text;
            this.checkBox1.Checked = answer.IsTrue;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Передає варіант відповіді
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.richTextBox1.Text.Length == 0)
            {
                MessageBox.Show("Текст відповіді пустий", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                answer.Text = this.richTextBox1.Text;
                answer.IsTrue = this.checkBox1.Checked;

                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
        }
    }
}
