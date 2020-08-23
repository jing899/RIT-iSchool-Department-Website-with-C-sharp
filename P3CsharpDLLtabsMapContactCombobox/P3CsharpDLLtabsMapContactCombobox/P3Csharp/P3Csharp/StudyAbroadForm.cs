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
    public partial class StudyAbroadForm : Form
    {
        List<string> studys;

        public StudyAbroadForm(List<string> StudyAbList)
        {
            InitializeComponent();
            studys = StudyAbList;

            richTextBox1.Text = String.Join("\n\n", studys);
        }

    }
}

