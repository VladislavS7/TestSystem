using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class SignIn : Form
    {
        StringBuilder log;
        StringBuilder pass;
        public SignIn(StringBuilder log, StringBuilder pass)
        {
            InitializeComponent();
            this.log = log;
            this.pass = pass;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            log.Append(textBox1.Text);
            pass.Append(textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
