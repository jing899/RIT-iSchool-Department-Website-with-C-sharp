using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P3Csharp
{
    public partial class FormsForm : Form
    {
        List<string> form;

        public FormsForm(List<string> formList)
        {
            InitializeComponent();
            form = formList;
            richTextBox1.Text = String.Join("\n\n", form);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
