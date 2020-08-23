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
    public partial class copEmpForm : Form
    {
        List<string> coplist;

        public copEmpForm(List<string> coEmpList)
        {
            InitializeComponent();
            coplist = coEmpList;
            richTextBox1.Text = String.Join("\n\n", coplist);
        }

        private void copEmpForm_Load(object sender, EventArgs e)
        {

        }
    }
}
