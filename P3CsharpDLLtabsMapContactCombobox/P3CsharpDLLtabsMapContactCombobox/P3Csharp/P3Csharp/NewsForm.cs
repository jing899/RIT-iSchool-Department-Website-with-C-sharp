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
    public partial class NewsForm : Form
    {
       // Form1 main = new Form1();
       
        List<string> news;

        public NewsForm(List<string> NewsTitle)
        {
            InitializeComponent();
            news = NewsTitle;

            richTextBox1.Text = String.Join("\n\n", news);
     //       richTextBox1.Text = String.Join(Environment.NewLine, news);
        }

    }
}
