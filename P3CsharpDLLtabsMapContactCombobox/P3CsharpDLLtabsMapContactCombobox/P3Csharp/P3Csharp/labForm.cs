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
    public partial class labForm : Form
    {
        List<string> lab;

        public labForm(List<string> labInfoList)
        {
            InitializeComponent();
            lab = labInfoList;
            richTextBox1.Text = String.Join("\n\n", lab);
        }

        private void labForm_Load(object sender, EventArgs e)
        {

        }
    }
}
