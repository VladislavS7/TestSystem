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
using System.Xml.Serialization;
using TestSystemLibrary;

namespace TestConstructor
{
    public partial class ExistingTests : Form
    {
        public StringBuilder name;
        public ExistingTests(StringBuilder name)
        {
            InitializeComponent();
            this.name = name;
            LoadNames();
        }

        private void LoadNames()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));

            List<string> d = new List<string>();
            //Serializer serializer = new Serializer();
            //string xmlDoc = File.ReadAllText("TestsNames.xml");
            //d = serializer.Deserialize<List<string>>(xmlDoc);
            using (FileStream fs = new FileStream("TestsNames.xml", FileMode.Open))
            {
                d = (List<string>)serializer.Deserialize(fs);
            }

            foreach (string item in d)
            {
                listBox1.Items.Add(item);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            name.Append(listBox1.SelectedItem.ToString());
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
