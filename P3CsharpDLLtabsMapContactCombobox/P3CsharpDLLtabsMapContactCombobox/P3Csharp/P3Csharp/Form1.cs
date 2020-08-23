using Newtonsoft.Json.Linq;
using RESTaccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P3Csharp
{
    public partial class Form1 : Form
    {
        private RESTapi rest = null;
        Stopwatch sw = new Stopwatch();
        List<string> facultyNames = new List<string>();
        List<string> staffNames = new List<string>();
        public List<string> NewsTitle = new List<string>();
        List<string> IntAreaNames = new List<string>();
        List<string> IntFacNames = new List<string>();
        public List<string> StudyAbList = new List<string>();
        public List<string> StudentSerList = new List<string>();
        public List<string> labInfoList = new List<string>();
        public List<string> stuAmbList = new List<string>();
        public List<string> formList = new List<string>();
        public List<string> coEmpList = new List<string>();

        News onenew;

        public Form1()
        {
            InitializeComponent();
            Populate();
        }

        public void Populate()
        {
            rest = new RESTapi("http://ist.rit.edu/api");
            // Get About information
            string jsonAbout = rest.getRESTData("/about/");

            // Convert the JSON to an About C# object
            // use http://json2csharp.com
            // enter http://ist.rit.edu/api/about 

            About about = JToken.Parse(jsonAbout).ToObject<About>();

            lbl_aboutTitle.Text = about.title;
            richTextBox1.Text = about.description;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            lbl_about_quoteAuthor.Text = about.quoteAuthor;
            lbl_about_quoteAuthor.TextAlign = ContentAlignment.MiddleCenter;
            textBox1.Text = about.quote;
            textBox1.TextAlign = HorizontalAlignment.Center;

        } // Populate()

        private void Btn_people_Enter(object sender, EventArgs e)
        {
            string jsonPeople = rest.getRESTData("/people/");

            // cast to a people object
            People people = JToken.Parse(jsonPeople).ToObject<People>();

            // loop through JSON array of faculty
            foreach (Faculty thisfac in people.faculty)
            {
                comboBoxFaculty.Items.Add(thisfac.name);
                facultyNames.Add(thisfac.username);
            }

            foreach (Staff thisStaff in people.staff)
            {
                comboBoxStaff.Items.Add(thisStaff.name);
                staffNames.Add(thisStaff.username);
            }
        }

        // display image and information when clicked on a specific faculty
        private void ComboBoxFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get what got us here
            ComboBox comboBox = (ComboBox)sender;

            // save the selected employee's name, to lookup the index in the combobox list
            // to get the username from the other list.
            string selectedEmployee = (string)comboBoxFaculty.SelectedItem;

            int resultIndex = -1;   // where in list is this person. let it error if not found

            // find the first instance of the selected employee
            resultIndex = comboBoxFaculty.FindStringExact(selectedEmployee);

            string jsonPeople = rest.getRESTData("/people/faculty/username=" + facultyNames[resultIndex]);

            Faculty onefac = JToken.Parse(jsonPeople).ToObject<Faculty>();

            facName.Text = selectedEmployee;
            facTitle.Text = onefac.title;
            facOffice.Text = "Office: " + onefac.office;
            facPhone.Text = "Phone: " + onefac.phone;
            facEmail.Text = "Email: " + onefac.email;

            facPic.Size = new Size(200, 200);
            facPic.Load(onefac.imagePath);
        } // end ComboBoxFaculty_SelectedIndexChanged

        // display image and information when clicked on a specific staff
        private void ComboBoxStaff_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedEmployee = (string)comboBoxStaff.SelectedItem;
            int resultIndex = -1;
            resultIndex = comboBoxStaff.FindStringExact(selectedEmployee);
            string jsonPeople = rest.getRESTData("/people/staff/username=" + staffNames[resultIndex]);
            Staff oneStaff = JToken.Parse(jsonPeople).ToObject<Staff>();

            stfName.Text = selectedEmployee;
            stfTitle.Text = oneStaff.title;
            stfOffice.Text = "Office: " + oneStaff.office;
            stfPhone.Text = "Phone: " + oneStaff.phone;
            stfEmail.Text = "Email: " + oneStaff.email;

            staffPic.Size = new Size(200, 200);
            staffPic.Load(oneStaff.imagePath);
        } // end ComboBoxStaff_SelectedIndexChanged


        private void Form1_Load(object sender, EventArgs e)
        {
            /*       // dynamically add a tab
                   string title = "News";
                   TabPage newstabpage = new TabPage(title);
                   tab.TabPages.Insert(6, newstabpage);

                   TextBox tb = new TextBox();
                   tb.BackColor = SystemColors.Info;
                   tb.Location = new Point(50, 10);
                   tb.Size = new Size(200, 30);
                   tb.TabIndex = 1;
                   tb.Text = "Enter your name...";

                   // add the news text box to the News tab page
                   newstabpage.Controls.Add(tb);
                   */
        }


        private void btn_loaddata_Click(object sender, EventArgs e)
        {
            string jsonEmp = rest.getRESTData("/employment/");

            Employment employment =
                JToken.Parse(jsonEmp).ToObject<Employment>();

            sw.Reset();
            sw.Start();

            for (int i = 0; i < employment.coopTable.coopInformation.Count; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value =
                    employment.coopTable.coopInformation[i].employer;
                dataGridView1.Rows[i].Cells[1].Value =
                    employment.coopTable.coopInformation[i].degree;
                dataGridView1.Rows[i].Cells[2].Value =
                    employment.coopTable.coopInformation[i].city;
                dataGridView1.Rows[i].Cells[3].Value =
                    employment.coopTable.coopInformation[i].term;

            }

            sw.Stop();
            Console.WriteLine("Data: " + sw.ElapsedMilliseconds.ToString());

        } // btn_loaddata

        private void tabCoop_Enter(object sender, EventArgs e)
        {
            // create the columns for ListView
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            // add column headers and widths
            listView1.Width = 450;
            listView1.Columns.Add("Employer", 100);
            listView1.Columns.Add("Degree", 100);
            listView1.Columns.Add("City", 100);
            listView1.Columns.Add("Term", 100);

        } // tabCoop_Enter

        private void btn_loadlist_Click(object sender, EventArgs e)
        {
            string jsonEmp = rest.getRESTData("/employment/");
            Employment employment =
                JToken.Parse(jsonEmp).ToObject<Employment>();

            sw.Reset();
            sw.Start();

            // Populate the List View
            ListViewItem item;

            for (int i = 0; i < employment.coopTable.coopInformation.Count; i++)
            {
                // build the row to display
                item = new ListViewItem(
                    new String[]
                    {
                        employment.coopTable.coopInformation[i].employer,
                        employment.coopTable.coopInformation[i].degree,
                        employment.coopTable.coopInformation[i].city,
                        employment.coopTable.coopInformation[i].term
                    }
                );

                // Append this list item to the row
                listView1.Items.Add(item);

            } // for

            sw.Stop();
            Console.WriteLine("List: " + sw.ElapsedMilliseconds.ToString());
        }

        // display contact page from api
        private void TabContact_Enter(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("http://ist.rit.edu/api/contactForm.php");
        } // end TabContact_Enter

        // display employment map when click on the show map button
        private void BtnEmploymentMap_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://ist.rit.edu/api/map.php");
        } // end tnEmploymentMap_Click


        // degrees page
        private void tabDegrees_Enter(object sender, EventArgs e)
        {
            int pointX = 40; int pointY = 40; int pointZ = 40; int pointJ = 40;
            int pointA = 40; int pointB = 40; int pointC = 40; int pointD = 40;

            string jsonDeg = rest.getRESTData("/Degrees/");

            Degrees ungra =
                JToken.Parse(jsonDeg).ToObject<Degrees>();

            // undergraduate degree title
            Label Title = new Label();
            Title.AutoSize = true;
            Title.Font = new Font("Arial", 14);
            Title.ForeColor = Color.Tomato;
            Title.Location = new Point(335, 0);
            Title.Text = "Undergraduate Degrees";
            Degree_panel.Controls.Add(Title);


            // Undergrduate title
            foreach (Undergraduate thisunder in ungra.undergraduate)
            {
                Label UnderTitle = new Label();
                UnderTitle.AutoSize = true;
                UnderTitle.MaximumSize = new Size(180, 0);
                UnderTitle.Font = new Font("Arial", 11, FontStyle.Bold);
                UnderTitle.Location = new Point(pointX, 40);
                UnderTitle.Text = thisunder.title;
                UnderTitle.TextAlign = ContentAlignment.MiddleCenter;
                Degree_panel.Controls.Add(UnderTitle);
                pointX += 290;
            }

            // undergraduate description
            foreach (Undergraduate thisunder in ungra.undergraduate)
            {
                Label UnderDes = new Label();
                UnderDes.AutoSize = true;
                UnderDes.MaximumSize = new Size(170, 0);
                UnderDes.Font = new Font("Arial", 9);
                UnderDes.Location = new Point(pointY, 100);
                UnderDes.Text = thisunder.description;
                UnderDes.TextAlign = ContentAlignment.MiddleCenter;
                Degree_panel.Controls.Add(UnderDes);
                pointY += 290;
            }

            // undergraduate concentrations title
            foreach (Undergraduate thisunder in ungra.undergraduate)
            {
                Label UnderConTitle = new Label();
                UnderConTitle.AutoSize = true;
                UnderConTitle.Font = new Font("Arial", 9, FontStyle.Bold);
                UnderConTitle.Location = new Point(pointJ, 220);
                UnderConTitle.Text = "Concentrations: ";
                Degree_panel.Controls.Add(UnderConTitle);
                pointJ += 300;
            }

            // undergraduate concentrations
            foreach (Undergraduate thisunder in ungra.undergraduate)
            {
                ListBox UnderCon = new ListBox();
                UnderCon.BorderStyle = BorderStyle.None;
                UnderCon.SelectionMode = SelectionMode.None;
                UnderCon.AutoSize = true;
                UnderCon.Location = new Point(pointZ, 245);
                Degree_panel.Controls.Add(UnderCon);
                pointZ += 300;

                if (thisunder.concentrations.Count == 4)
                {
                    var myItems = new List<string> { thisunder.concentrations[0], thisunder.concentrations[1], thisunder.concentrations[2], thisunder.concentrations[3] };
                    UnderCon.DataSource = myItems;
                }
                if (thisunder.concentrations.Count == 5)
                {
                    var myItems = new List<string> { thisunder.concentrations[0], thisunder.concentrations[1], thisunder.concentrations[2], thisunder.concentrations[3], thisunder.concentrations[4] };
                    UnderCon.DataSource = myItems;
                }
                if (thisunder.concentrations.Count == 6)
                {
                    var myItems = new List<string> { thisunder.concentrations[0], thisunder.concentrations[1], thisunder.concentrations[2], thisunder.concentrations[3], thisunder.concentrations[4], thisunder.concentrations[5] };
                    UnderCon.DataSource = myItems;
                }
            }

            Degrees gra =
                JToken.Parse(jsonDeg).ToObject<Degrees>();

            //graduate degree title
            Label gTitle = new Label();
            gTitle.AutoSize = true;
            gTitle.Font = new Font("Arial", 14);
            gTitle.ForeColor = Color.Tomato;
            gTitle.Location = new Point(335, 340);
            gTitle.Text = "Graduate Degrees";
            Degree_panel.Controls.Add(gTitle);

            // Grduate title
            foreach (Graduate thisgra in gra.graduate)
            {
                Label GraTitle = new Label();
                GraTitle.AutoSize = true;
                GraTitle.MaximumSize = new Size(180, 0);
                GraTitle.Font = new Font("Arial", 12, FontStyle.Bold);
                GraTitle.Location = new Point(pointA, 380);
                GraTitle.Text = thisgra.title;
                GraTitle.TextAlign = ContentAlignment.MiddleCenter;
                Degree_panel.Controls.Add(GraTitle);
                pointA += 290;
            }
            // undergraduate description
            foreach (Graduate thisgra in gra.graduate)
            {
                Label graDes = new Label();
                graDes.AutoSize = true;
                graDes.MaximumSize = new Size(190, 0);
                graDes.Font = new Font("Arial", 9);
                graDes.Location = new Point(pointB, 430);
                graDes.Text = thisgra.description;
                graDes.TextAlign = ContentAlignment.MiddleCenter;
                Degree_panel.Controls.Add(graDes);
                pointB += 280;
            }

            // undergraduate concentrations title
            //    foreach (Graduate thisgra in gra.graduate)
            for (int m = 0; m <= 2; m++)
            {
                Label gradConTitle = new Label();
                gradConTitle.AutoSize = true;
                gradConTitle.Font = new Font("Arial", 9, FontStyle.Bold);
                gradConTitle.Location = new Point(pointC, 550);
                gradConTitle.Text = "Concentrations: ";
                Degree_panel.Controls.Add(gradConTitle);
                pointC += 300;
            }

            // undergraduate concentrations
            foreach (Graduate thisgra in gra.graduate)
            {
                ListBox graCon = new ListBox();
                graCon.BorderStyle = BorderStyle.None;
                graCon.AutoSize = true;
                graCon.SelectionMode = SelectionMode.None;
                graCon.Location = new Point(pointD, 580);
                Degree_panel.Controls.Add(graCon);
                pointD += 300;

                if (thisgra.concentrations == null)
                {
                    var myItems = new List<string> { thisgra.availableCertificates[0], thisgra.availableCertificates[1] };
                    graCon.DataSource = myItems;
                    graCon.Visible = false;
                }

                else
                {
                    if (thisgra.concentrations.Count == 3)
                    {
                        var myItems = new List<string> { thisgra.concentrations[0], thisgra.concentrations[1], thisgra.concentrations[2] };
                        graCon.DataSource = myItems;
                    } // end if

                    if (thisgra.concentrations.Count == 6)
                    {
                        var myItems = new List<string> { thisgra.concentrations[0], thisgra.concentrations[1], thisgra.concentrations[2], thisgra.concentrations[3], thisgra.concentrations[4], thisgra.concentrations[5] };
                        graCon.DataSource = myItems;
                    } // end if
                } // end else

                // text for graduate available certificates
                Degrees graCert = JToken.Parse(jsonDeg).ToObject<Degrees>();

                degNameCert.Text = graCert.graduate[3].degreeName;
                deCert1.Text = graCert.graduate[3].availableCertificates[0];
                deCert2.Text = graCert.graduate[3].availableCertificates[1];
            }
        } // end Degrees_Enter

        private void tabEmployment_Enter(object sender, EventArgs e)
        {
            string jsonEmp = rest.getRESTData("/Employment/");
            Employment emp = JToken.Parse(jsonEmp).ToObject<Employment>();

            empInTitle.Text = emp.introduction.title;
            empConTitle1.Text = emp.introduction.content[0].title;
            empConTitle2.Text = emp.introduction.content[1].title;
            empConTitle1_des.Text = emp.introduction.content[0].description;
            empConTitle2_des.Text = emp.introduction.content[1].description;
            empDegTitle.Text = emp.degreeStatistics.title;
            empStatDes1.Text = emp.degreeStatistics.statistics[0].description;
            empStatDes2.Text = emp.degreeStatistics.statistics[1].description;
            empStatDes3.Text = emp.degreeStatistics.statistics[2].description;
            empStatDes4.Text = emp.degreeStatistics.statistics[3].description;
            empStatValue1.Text = emp.degreeStatistics.statistics[0].value;
            empStatValue2.Text = emp.degreeStatistics.statistics[1].value;
            empStatValue3.Text = emp.degreeStatistics.statistics[2].value;
            empStatValue4.Text = emp.degreeStatistics.statistics[3].value;
            empEmpTitle.Text = emp.employers.title;
            empCarTitle.Text = emp.careers.title;
            empEmp1.Text = emp.employers.employerNames[0];
            empEmp2.Text = emp.employers.employerNames[1];
            empEmp3.Text = emp.employers.employerNames[2];
            empEmp4.Text = emp.employers.employerNames[3];
            empEmp5.Text = emp.employers.employerNames[4];
            empEmp6.Text = emp.employers.employerNames[5];
            empCar1.Text = emp.careers.careerNames[0];
            empCar2.Text = emp.careers.careerNames[1];
            empCar3.Text = emp.careers.careerNames[2];
            empCar4.Text = emp.careers.careerNames[3];
            empCar5.Text = emp.careers.careerNames[4];
            empCar6.Text = emp.careers.careerNames[5];

        }

        private void tabResearch_Enter(object sender, EventArgs e)
        {
            string jsonRes = rest.getRESTData("/research/");
            Research research = JToken.Parse(jsonRes).ToObject<Research>();

            // loop through JSON array of faculty
            foreach (ByInterestArea thisIntArea in research.byInterestArea)
            {
                comboBoxIntArea.Items.Add(thisIntArea.areaName);
                IntAreaNames.Add(thisIntArea.areaName);
            }

            foreach (ByFaculty thisIntFac in research.byFaculty)
            {
                comboBoxIntFac.Items.Add(thisIntFac.facultyName);
                IntFacNames.Add(thisIntFac.username);
            }
        }

        private void comboBoxIntFac_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedIntFac = (string)comboBoxIntFac.SelectedItem;
            int resultIndex = -1;   // where in list is this person. let it error if not found

            resultIndex = comboBoxIntFac.FindStringExact(selectedIntFac);
            string jsonResour = rest.getRESTData("/research/byFaculty/username=" + IntFacNames[resultIndex]);
            ByFaculty asd = JToken.Parse(jsonResour).ToObject<ByFaculty>();

            Label[] thisPCitation;
            thisPCitation = new Label[asd.citations.Count];
            List<string> thisPersonCitation = new List<string>();
            int num = 0;

            while (num < asd.citations.Count)
            {
                thisPCitation[num] = new Label();
                thisPCitation[num].Text = asd.citations[num].ToString();
                thisPersonCitation.Add(asd.citations[num]);
                num++;
            }

            resByFacPanel.Controls.Add(ResByFacTextBox);
            resByFacPanel.Controls.AddRange(thisPCitation);
            ResByFacTextBox.Text = String.Join("\n\n", thisPersonCitation);
        } // end ComboBoxFaculty_SelectedIndexChanged

        private void comboBoxIntArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedIntArea = (string)comboBoxIntArea.SelectedItem;
            int resultIndex = -1;   // where in list is this person. let it error if not found

            resultIndex = comboBoxIntArea.FindStringExact(selectedIntArea);
            string jsonResourInt = rest.getRESTData("/research/byInterestArea/areaName=" + IntAreaNames[resultIndex]);
            ByInterestArea bia = JToken.Parse(jsonResourInt).ToObject<ByInterestArea>();

            Label[] thisIntCitation;
            thisIntCitation = new Label[bia.citations.Count];
            List<string> thisIntAreaCitation = new List<string>();
            int num = 0;

            while (num < bia.citations.Count)
            {
                thisIntCitation[num] = new Label();
                thisIntCitation[num].Text = bia.citations[num].ToString();
                thisIntAreaCitation.Add(bia.citations[num]);
                num++;
            }

            resByIntAreaPanel.Controls.Add(ResByFacTextBox);
            resByIntAreaPanel.Controls.AddRange(thisIntCitation);
            resByIntAreaTextBox.Text = String.Join("\n\n", thisIntAreaCitation);
        } // end ComboBoxFaculty_SelectedIndexChanged




        // footer
        private void tabFooter_Enter(object sender, EventArgs e)
        {
            string jsonFoo = rest.getRESTData("/Footer/");
            Footer ft = JToken.Parse(jsonFoo).ToObject<Footer>();

            FtTitle.Text = ft.social.title;
            FtTweet.Text = ft.social.tweet;
            FtBy.Text = ft.social.by;
            applylink.Text = ft.quickLinks[0].title;
            aboutLink.Text = ft.quickLinks[1].title;
            supportLink.Text = ft.quickLinks[2].title;
            labLink.Text = ft.quickLinks[3].title;
            webBrowser2.DocumentText = ft.copyright.html;

            int pointX = 30;
            int pointY = 40;

            string jsonNew = rest.getRESTData("/news/");

            onenew = JToken.Parse(jsonNew).ToObject<News>();
            //    int numCount = -1;

            foreach (Older thisnew in onenew.older)
            {
                Button NewstitleButton = new Button();
                NewstitleButton.Location = new Point(pointX, pointY);
                NewstitleButton.Text = thisnew.title;
                NewstitleButton.AutoSize = true;
                NewstitleButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
                NewstitleButton.Click += new EventHandler(this.NewstitleButton_Click);
                pointY += 40;

                // numCount += 1;
                NewsTitle.Add(thisnew.title);
                NewsTitle.Add(thisnew.description);
                //  NewsTitleCount.Add(numCount.ToString());
                // MessageBox.Show(thisnew.description);
                //   MessageBox.Show(thisnew.description);
                //      MessageBox.Show(numCount.ToString());
            }
        }// end footer

        private void tweeter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://twitter.com/istatrit");
            tweeter.LinkVisited = true;
        }

        private void Facebook_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/ISTatRIT");
            Facebook.LinkVisited = true;
        }

        private void ApplyNow_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.rit.edu/admission.html");
            applylink.LinkVisited = true;
        }

        private void aboutLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ist.rit.edu/assets/includes/calls/calls.php?area=aboutSite");
            aboutLink.LinkVisited = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ist.rit.edu/assets/includes/calls/calls.php?area=supportIST");
            supportLink.LinkVisited = true;
        }

        private void labLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ist.rit.edu/assets/includes/resources/calls.php?area=tutors");
            labLink.LinkVisited = true;
        }

        private void NewstitleButton_Click(object sender, EventArgs e)
        {
            //  MessageBox.Show(thisnew.description);
            Form f2 = new NewsForm(NewsTitle);
            f2.Show();
        }


        private void tabResource_Enter(object sender, EventArgs e)
        {
            string jsonResource = rest.getRESTData("/Resources/");
            Resources res = JToken.Parse(jsonResource).ToObject<Resources>();

            resourseTitle.Text = res.title;
            reSub.Text = res.subTitle;

            resouStudyAbroad.Text = res.studyAbroad.title;
            StudyAbList.Add(res.studyAbroad.title);
            StudyAbList.Add(res.studyAbroad.description);
            StudyAbList.Add(res.studyAbroad.places[0].nameOfPlace);
            StudyAbList.Add(res.studyAbroad.places[0].description);
            StudyAbList.Add(res.studyAbroad.places[1].nameOfPlace);
            StudyAbList.Add(res.studyAbroad.places[1].description);

            StudentService_Title.Text = res.studentServices.title;
            StudentSerList.Add(res.studentServices.title);
            StudentSerList.Add(res.studentServices.academicAdvisors.title);
            StudentSerList.Add(res.studentServices.academicAdvisors.description);
            StudentSerList.Add(res.studentServices.academicAdvisors.faq.title);
            StudentSerList.Add(res.studentServices.academicAdvisors.faq.contentHref);
            StudentSerList.Add(res.studentServices.professonalAdvisors.title);
            StudentSerList.Add(res.studentServices.professonalAdvisors.advisorInformation[0].name);
            StudentSerList.Add(res.studentServices.professonalAdvisors.advisorInformation[0].department);
            StudentSerList.Add(res.studentServices.professonalAdvisors.advisorInformation[0].email);
            StudentSerList.Add(res.studentServices.professonalAdvisors.advisorInformation[1].name);
            StudentSerList.Add(res.studentServices.professonalAdvisors.advisorInformation[1].department);
            StudentSerList.Add(res.studentServices.professonalAdvisors.advisorInformation[1].email);
            StudentSerList.Add(res.studentServices.professonalAdvisors.advisorInformation[2].name);
            StudentSerList.Add(res.studentServices.professonalAdvisors.advisorInformation[2].department);
            StudentSerList.Add(res.studentServices.professonalAdvisors.advisorInformation[2].email);
            StudentSerList.Add(res.studentServices.facultyAdvisors.title);
            StudentSerList.Add(res.studentServices.facultyAdvisors.description);
            StudentSerList.Add(res.studentServices.istMinorAdvising.title);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[0].title);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[0].advisor);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[0].email);            
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[1].title);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[1].advisor);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[1].email);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[2].title);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[2].advisor);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[2].email);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[3].title);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[3].advisor);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[3].email);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[4].title);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[4].advisor);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[4].email);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[5].title);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[5].advisor);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[5].email);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[6].title);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[6].advisor);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[6].email);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[7].title);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[7].advisor);
            StudentSerList.Add(res.studentServices.istMinorAdvising.minorAdvisorInformation[7].email);

            LabInfo_Title.Text = res.tutorsAndLabInformation.title;
            labInfoList.Add(res.tutorsAndLabInformation.title);
            labInfoList.Add(res.tutorsAndLabInformation.description);
            labInfoList.Add(res.tutorsAndLabInformation.tutoringLabHoursLink);

            stu_amb_title.Text = res.studentAmbassadors.title;
            stuAmbList.Add(res.studentAmbassadors.title);
            stuAmbList.Add(res.studentAmbassadors.ambassadorsImageSource);
            stuAmbList.Add(res.studentAmbassadors.subSectionContent[0].title);
            stuAmbList.Add(res.studentAmbassadors.subSectionContent[0].description);
            stuAmbList.Add(res.studentAmbassadors.subSectionContent[1].title);
            stuAmbList.Add(res.studentAmbassadors.subSectionContent[1].description);
            stuAmbList.Add(res.studentAmbassadors.subSectionContent[2].title);
            stuAmbList.Add(res.studentAmbassadors.subSectionContent[2].description);
            stuAmbList.Add(res.studentAmbassadors.subSectionContent[3].title);
            stuAmbList.Add(res.studentAmbassadors.subSectionContent[3].description);
            stuAmbList.Add(res.studentAmbassadors.subSectionContent[4].title);
            stuAmbList.Add(res.studentAmbassadors.subSectionContent[4].description);
            stuAmbList.Add(res.studentAmbassadors.subSectionContent[5].title);
            stuAmbList.Add(res.studentAmbassadors.subSectionContent[5].description);
            stuAmbList.Add(res.studentAmbassadors.subSectionContent[6].title);
            stuAmbList.Add(res.studentAmbassadors.subSectionContent[6].description);
            stuAmbList.Add(res.studentAmbassadors.applicationFormLink);
            stuAmbList.Add(res.studentAmbassadors.note);

            formList.Add(res.forms.graduateForms[0].formName);
            formList.Add(res.forms.graduateForms[0].href);
            formList.Add(res.forms.graduateForms[1].formName);
            formList.Add(res.forms.graduateForms[1].href);
            formList.Add(res.forms.graduateForms[2].formName);
            formList.Add(res.forms.graduateForms[2].href);
            formList.Add(res.forms.graduateForms[3].formName);
            formList.Add(res.forms.graduateForms[3].href);
            formList.Add(res.forms.graduateForms[4].formName);
            formList.Add(res.forms.graduateForms[4].href);
            formList.Add(res.forms.graduateForms[5].formName);
            formList.Add(res.forms.graduateForms[5].href);            
            formList.Add(res.forms.graduateForms[6].formName);
            formList.Add(res.forms.graduateForms[6].href);
            formList.Add(res.forms.undergraduateForms[0].formName);
            formList.Add(res.forms.undergraduateForms[0].href);

            copEmpTitle.Text = res.coopEnrollment.title;
            coEmpList.Add(res.coopEnrollment.title);

            coEmpList.Add(res.coopEnrollment.enrollmentInformationContent[0].title);
            coEmpList.Add(res.coopEnrollment.enrollmentInformationContent[0].description);
            coEmpList.Add(res.coopEnrollment.enrollmentInformationContent[1].title);
            coEmpList.Add(res.coopEnrollment.enrollmentInformationContent[1].description);
            coEmpList.Add(res.coopEnrollment.enrollmentInformationContent[2].title);
            coEmpList.Add(res.coopEnrollment.enrollmentInformationContent[2].description);
            coEmpList.Add(res.coopEnrollment.enrollmentInformationContent[3].title);
            coEmpList.Add(res.coopEnrollment.enrollmentInformationContent[3].description);
            coEmpList.Add(res.coopEnrollment.RITJobZoneGuidelink);
        }

        // call studyAbroad form when clicked
        private void resouStudyAbout_Click(object sender, EventArgs e)
        {
            Form abform = new StudyAbroadForm(StudyAbList);
            abform.Show();
        }

        // call studentService form when clicked
        private void StudentService_Title_Click(object sender, EventArgs e)
        {
            Form svform = new studentServiceForm(StudentSerList);
            svform.Show();
        }

        // call labInfo form when clicked
        private void LabInfo_Title_Click(object sender, EventArgs e)
        {
            Form labform = new labForm(labInfoList);
            labform.Show();
        }

        // call studentAmbassadors form when clicked
        private void stu_amb_title_Click(object sender, EventArgs e)
        {
            Form stAmbForm = new labForm(stuAmbList);
            stAmbForm.Show();
            
        }

        // call forms form when clicked
        private void Forms_Click(object sender, EventArgs e)
        {
            Form fForm = new FormsForm(formList);
            fForm.Show();
        }

        // call coopEmployment form when clicked
        private void copEmpTitle_Click(object sender, EventArgs e)
        {
            Form copForm = new FormsForm(coEmpList);
            copForm.Show();
        }

        Minors Minor;
        private void tabMiner_Enter(object sender, EventArgs e)
        {
            string jsonMinor = rest.getRESTData("/minors/");
            string jsoncourse = rest.getRESTData("/courses/");
            //Minors Minor = JToken.Parse(jsonMinor).ToObject<Minors>();
            Minor = JToken.Parse(jsonMinor).ToObject<Minors>();
            // Course courses = JToken.Parse(jsoncourse).ToObject<Course>();
            min1.Text = Minor.UgMinors[0].title;
            min2.Text = Minor.UgMinors[1].title;
            min3.Text = Minor.UgMinors[2].title;
            min4.Text = Minor.UgMinors[3].title;
            min5.Text = Minor.UgMinors[4].title;
            min6.Text = Minor.UgMinors[5].title;
            min7.Text = Minor.UgMinors[6].title;
            min8.Text = Minor.UgMinors[7].title;

        }

        public void openMinor(object id)
        {
            //    this.Visible = false;
            minorDes minorDescription = new minorDes(id, Minor, this);
            minorDescription.Visible = true;
        }

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {
            openMinor(min1.Tag);
        }

        private void flowLayoutPanel2_Click(object sender, EventArgs e)
        {
            openMinor(min2.Tag);
        }

        private void flowLayoutPanel3_Click(object sender, EventArgs e)
        {
            openMinor(min3.Tag);
        }

        private void flowLayoutPanel4_Click(object sender, EventArgs e)
        {
            openMinor(min4.Tag);
        }

        private void flowLayoutPanel5_Click(object sender, EventArgs e)
        {
            openMinor(min5.Tag);
        }

        private void flowLayoutPanel6_Click(object sender, EventArgs e)
        {
            openMinor(min6.Tag);
        }

        private void flowLayoutPanel7_Click(object sender, EventArgs e)
        {
            openMinor(min7.Tag);
        }

        private void flowLayoutPanel8_Click(object sender, EventArgs e)
        {
            openMinor(min8.Tag);
        }
    } // end Form1


}
