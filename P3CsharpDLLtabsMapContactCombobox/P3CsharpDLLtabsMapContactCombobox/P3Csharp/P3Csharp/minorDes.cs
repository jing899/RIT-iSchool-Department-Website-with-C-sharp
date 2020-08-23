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
    public partial class minorDes : Form
    {
        Form parent = new Form();
        Object id;
        Minors mn;
        public minorDes(Object id, Minors mn, Form that)
        {
            InitializeComponent();
            this.parent = that;
            this.id = id;
            this.mn = mn;

            int TagNum = Int32.Parse(id.ToString()) - 1;  //subtract one as the array is 0 based
            minName.Text = mn.UgMinors[TagNum].name;
            minTitle.Text = mn.UgMinors[TagNum].title;
            minDes.Text = mn.UgMinors[TagNum].description;
            minNote.Text = mn.UgMinors[TagNum].note;

            grid_course.BackgroundColor = Color.White;
            grid_course.RowHeadersVisible = false;
            for (int i = 0; i < mn.UgMinors[TagNum].courses.Count; i++)
            {
                grid_course.Columns.Add(null, mn.UgMinors[TagNum].courses[i]);
            }
        }

            private void minorDes_Load(object sender, EventArgs e)
        {

        }

        private void btn_close_underGradDetails_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
