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
    public partial class stuAmbForm : Form
    {
        List<string> samb;

        public stuAmbForm(List<string> stuAmbList)
        {
            InitializeComponent();
            samb = stuAmbList;
            richTextBox1.Text = String.Join("\n\n", samb);
        }

        private void stuAmbForm_Load(object sender, EventArgs e)
        {

        }
    }
}
