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
    public partial class studentServiceForm : Form
    {
        List<string> sv;
        public studentServiceForm(List<string> StudentSerList)
        {
            InitializeComponent();

            sv = StudentSerList;
            richTextBox1.Text = String.Join("\n\n", sv);
        }

        private void studentServiceForm_Load(object sender, EventArgs e)
        {

        }
    }
}
