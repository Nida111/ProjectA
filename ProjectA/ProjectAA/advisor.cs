using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace ProjectA
{
    public partial class advisor : Form
    {
        int IDD;
        int panelWidth;
        bool Hidden;
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-VFGDRDF\NIDA;Initial Catalog=ProjectA;Integrated Security=True");
        public advisor()
        {
            InitializeComponent();
            panelWidth = PanelSlide.Width;
            Hidden = false;
            panel5.Hide();
            button6.Hide();
            dataGridView1.ForeColor = Color.Black;

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);

            conn.Open();
            try
            {


                String query = "SELECT Person.Id AS Id, FirstName as [First Name], Advisor.Designation as Designation FROM [dbo].[Person] JOIN [dbo].[Advisor] ON Advisor.Id = Person.Id "; ;

                SqlCommand command = new SqlCommand(query, conn);

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = command;
                DataTable dbadataset = new DataTable();
                sda.Fill(dbadataset);
                BindingSource bsource = new BindingSource();
                bsource.DataSource = dbadataset;
                dataGridView1.DataSource = bsource;

                // adding edit and delete button on grid
                var EditButton = new DataGridViewButtonColumn();
                EditButton.Name = "EditButton";
                EditButton.HeaderText = "Edit";
                EditButton.Text = "Edit";
                EditButton.UseColumnTextForButtonValue = true;
                this.dataGridView1.Columns.Add(EditButton);

                var deleteButton = new DataGridViewButtonColumn();
                deleteButton.Name = "DeleteButton";
                deleteButton.HeaderText = "Delete";
                deleteButton.Text = "Delete";
                deleteButton.UseColumnTextForButtonValue = true;
                this.dataGridView1.Columns.Add(deleteButton);

                var Details = new DataGridViewButtonColumn();
                Details.Name = "DetailsButton";
                Details.HeaderText = "Details";
                Details.Text = "Details";
                Details.UseColumnTextForButtonValue = true;
                this.dataGridView1.Columns.Add(Details);


                this.dataGridView1.Columns["Id"].Visible = false;
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                
            }
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Hidden)
            {
                PanelSlide.Width = PanelSlide.Width + 10;
                if (PanelSlide.Width >= panelWidth)
                {
                    timer1.Stop();
                    Hidden = false;
                    this.Refresh();
                }
            }
            else
            {
                PanelSlide.Width = PanelSlide.Width - 10;
                if (PanelSlide.Width <= 0)
                {
                    timer1.Stop();
                    Hidden = true;
                    this.Refresh();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel5.Show();
            button6.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f1 = new Form2();
            f1.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Refresh();
            Refresh();
            this.Hide();
            advisor ss = new advisor();
            ss.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            conn.Open();
            string fn = textBox1.Text;
            string ln = textBox2.Text;
            string con = textBox3.Text;
            string email = textBox4.Text;
            string salary = textBox6.Text;


            if (string.IsNullOrEmpty(fn) || !Regex.IsMatch(textBox1.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid First name. Should be letters ");
                textBox1.BackColor = Color.DodgerBlue;
                textBox1.Clear();
                conn.Close();
                return;
            }
            textBox1.BackColor = Color.White;


            if (string.IsNullOrEmpty(ln) || !Regex.IsMatch(textBox2.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid Last name. Should be letters");
                textBox2.BackColor = Color.DodgerBlue;
                textBox2.Clear();
                conn.Close();
                return; ;

            }
            textBox2.BackColor = Color.White;
            if (string.IsNullOrEmpty(con) || !Regex.IsMatch(textBox3.Text, @"^[0-9]{11}$"))
            {
                MessageBox.Show("Invalid contact number.Should be digits");
                textBox3.BackColor = Color.DodgerBlue;
                textBox3.Clear();
                conn.Close();
                return;

            }
            textBox3.BackColor = Color.White;

            SqlCommand cmd = new SqlCommand("Select count(*) from Person where Email= @email", conn);
            cmd.Parameters.AddWithValue("@email", this.textBox4.Text);

            int result = Convert.ToInt32(cmd.ExecuteScalar());

            if (result != 0)
            {
                MessageBox.Show("Email Exists");
                conn.Close();
                return;
            }
            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(textBox4.Text, @"^([\w]+)@([\w]+)\.([\w]+)$"))
            {
                MessageBox.Show("Invalid email ");
                textBox4.BackColor = Color.DodgerBlue;
                textBox4.Clear();
                conn.Close();
                return;

            }
            textBox4.BackColor = Color.White;
            if (string.IsNullOrEmpty(salary) || !Regex.IsMatch(textBox6.Text, @"^[0-9]{0,18}$"))
            {
                MessageBox.Show("Invalid salary.Should be digits ");
                textBox6.BackColor = Color.DodgerBlue;
                textBox6.Clear();
                conn.Close();
                return;

            }
            textBox6.BackColor = Color.White;
            DateTime dt = dateTimePicker1.Value;
            string design = comboBox2.SelectedItem.ToString();
            //getting designation
            try
            {
                string designn = "SELECT Id FROM Lookup WHERE Category= 'DESIGNATION' AND Value ='" + design + "' ";
                SqlCommand InvAmountcom = new SqlCommand(designn, conn);
                int desigination = Convert.ToInt32(InvAmountcom.ExecuteScalar().ToString());

                string gen = comboBox1.SelectedItem.ToString();
                //getting gender value from lookup
                string genvalue = "SELECT Id FROM Lookup WHERE Category= 'GENDER' AND Value ='" + gen + "' ";
                SqlCommand genint = new SqlCommand(genvalue, conn);
                genint.ExecuteNonQuery();
                int gender = Convert.ToInt32(genint.ExecuteScalar());

                String insert1 = "insert into Person  values('" + fn + "','" + ln + "','" + con + "','" + email + "','" + dt + "','" + gender + "' ) ";
                SqlCommand sql1 = new SqlCommand(insert1, conn);
                int i1 = sql1.ExecuteNonQuery();

                //getting id from person for advisor
                string sid = String.Format("SELECT Id FROM Person Where Email ='{0}'", email);
                SqlCommand stuid = new SqlCommand(sid, conn);
                int id = Convert.ToInt32(stuid.ExecuteScalar());

                String insert2 = "insert into Advisor values('" + id + "','" + desigination + "','" + salary + "')";
                SqlCommand sql2 = new SqlCommand(insert2, conn);
                int i2 = sql2.ExecuteNonQuery();

                // to check if the student is registered or not
                if (i1 >= 1 && i2 >= 1)
                {
                    MessageBox.Show("Advisor Registered ");
                    this.Refresh();
                }
                else
                {
                    { MessageBox.Show(" Advisor not Registered :"); }
                }

                this.Refresh();
                Refresh();
                this.Hide();
                advisor ss = new advisor();
                ss.Show();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                
            }
            conn.Close();


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            conn.Open();

            if (e.RowIndex == dataGridView1.NewRowIndex || e.RowIndex < 0)
                return;

            //Check if click is on specific column 
            if (e.ColumnIndex == dataGridView1.Columns["DeleteButton"].Index)
            {
                int index = e.RowIndex;
                int ID = Convert.ToInt32(dataGridView1.Rows[index].Cells[3].Value.ToString());
                try
                {
                    string delete3 = String.Format("DELETE FROM ProjectAdvisor WHERE AdvisorId = '{0}'", ID);
                    SqlCommand de13 = new SqlCommand(delete3, conn);
                    int k = de13.ExecuteNonQuery();

                    string delete1 = String.Format("DELETE FROM Advisor WHERE Id = '{0}'", ID);
                    SqlCommand de11 = new SqlCommand(delete1, conn);
                    int i = de11.ExecuteNonQuery();


                    string delete2 = string.Format("DELETE FROM Person WHERE Id = '{0}'", ID);
                    SqlCommand del2 = new SqlCommand(delete2, conn);
                    int j = del2.ExecuteNonQuery();



                    // to check if the student is registered or not
                    if (i >= 1 && j >= 1)
                    {
                        MessageBox.Show(" Advisor Deleted ");
                        this.Refresh();
                    }
                    else
                    {
                        MessageBox.Show(" Advisor not Deleted");
                    }
                    this.Refresh();
                    Refresh();
                    this.Hide();
                    advisor ss = new advisor();
                    ss.Show();
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                }

            }


            else if (e.ColumnIndex == dataGridView1.Columns["EditButton"].Index)
            {

                int indexx = e.RowIndex;
                IDD = Convert.ToInt32(dataGridView1.Rows[indexx].Cells[3].Value.ToString());
                panel5.Show();
                button5.Hide();
                button6.Show();
                try
                {
                    string fn = "SELECT FirstName FROM Person WHERE Id = '" + IDD + "' ";
                    SqlCommand fnn = new SqlCommand(fn, conn);
                    fnn.ExecuteNonQuery();
                    string first = Convert.ToString(fnn.ExecuteScalar());
                    textBox1.Text = first;

                    string ln = "SELECT LastName FROM Person WHERE Id = '" + IDD + "' ";
                    SqlCommand lnn = new SqlCommand(ln, conn);
                    lnn.ExecuteNonQuery();
                    string last = Convert.ToString(lnn.ExecuteScalar());
                    textBox2.Text = last;


                    string con = "SELECT Contact FROM Person WHERE Id = '" + IDD + "' ";
                    SqlCommand cont = new SqlCommand(con, conn);
                    cont.ExecuteNonQuery();
                    string contact = Convert.ToString(cont.ExecuteScalar());
                    textBox3.Text = contact;


                    string em = "SELECT Email FROM Person WHERE Id = '" + IDD + "' ";
                    SqlCommand emi = new SqlCommand(em, conn);
                    emi.ExecuteNonQuery();
                    string email = Convert.ToString(emi.ExecuteScalar());
                    textBox4.Text = email;

                    string sal = "SELECT Salary FROM Advisor WHERE Id = '" + IDD + "' ";
                    SqlCommand sall = new SqlCommand(sal, conn);
                    sall.ExecuteNonQuery();
                    string salary = Convert.ToString(sall.ExecuteScalar());
                    textBox6.Text = salary;

                    string gen = "SELECT Gender FROM Person WHERE Id = '" + IDD + "' ";
                    SqlCommand gender = new SqlCommand(gen, conn);
                    gender.ExecuteNonQuery();
                    int genn = Convert.ToInt32(gender.ExecuteScalar());

                    string designn = "SELECT Value FROM Lookup WHERE Category= 'GENDER' AND Id ='" + genn + "' ";
                    SqlCommand InvAmountcom = new SqlCommand(designn, conn);
                    string mf = Convert.ToString(InvAmountcom.ExecuteScalar().ToString());
                    comboBox1.Text = mf;

                    string de = "SELECT Designation FROM Advisor WHERE Id = '" + IDD + "' ";
                    SqlCommand des = new SqlCommand(de, conn);
                    des.ExecuteNonQuery();
                    int desg = Convert.ToInt32(des.ExecuteScalar());
                    string design = "SELECT Value FROM Lookup WHERE Category= 'DESIGNATION' AND Id ='" + desg + "' ";
                    SqlCommand InvAmount = new SqlCommand(design, conn);
                    string mff = Convert.ToString(InvAmount.ExecuteScalar().ToString());
                    comboBox2.Text = mff;

                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                    
                }

            }

            else if (e.ColumnIndex == dataGridView1.Columns["DetailsButton"].Index)
            {
                this.Hide();
                advidetails s1 = new advidetails();
                s1.Show();

            }

            conn.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            conn.Open();
            string fn = textBox1.Text;
            string ln = textBox2.Text;
            string con = textBox3.Text;
            string email = textBox4.Text;
            string salary = textBox6.Text;

            if (string.IsNullOrEmpty(fn) || !Regex.IsMatch(textBox1.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid First Name.Should be letters");
                textBox1.BackColor = Color.DodgerBlue;
                textBox1.Clear();
                conn.Close();
                return;
            }
            textBox1.BackColor = Color.White;


            if (string.IsNullOrEmpty(ln) || !Regex.IsMatch(textBox2.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid last Name.Should be letters");
                textBox2.BackColor = Color.DodgerBlue;
                textBox2.Clear();
                conn.Close();
                return; ;

            }
            textBox2.BackColor = Color.White;
            if (string.IsNullOrEmpty(con) || !Regex.IsMatch(textBox3.Text, @"^[0-9]{10,12}$"))
            {
                MessageBox.Show("Invalid contact ,should be 11 digits");
                textBox3.BackColor = Color.DodgerBlue;
                textBox3.Clear();
                conn.Close();
                return;

            }
            textBox3.BackColor = Color.White;


            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(textBox4.Text, @"^([\w]+)@([\w]+)\.([\w]+)$"))
            {
                MessageBox.Show("Invalid email ");
                textBox4.BackColor = Color.DodgerBlue;
                textBox4.Clear();
                conn.Close();
                return;

            }
            textBox4.BackColor = Color.White;
            if (string.IsNullOrEmpty(salary) || !Regex.IsMatch(textBox6.Text, @"^[0-9]*$"))
            {
                MessageBox.Show("Invalid salary,should be digits ");
                textBox6.BackColor = Color.DodgerBlue;
                textBox6.Clear();
                conn.Close();
                return;

            }
            textBox6.BackColor = Color.White;
            DateTime dt = dateTimePicker1.Value;
            string design = comboBox2.SelectedItem.ToString();
            try
            {
                //getting designation
                string designn = "SELECT Id FROM Lookup WHERE Category= 'DESIGNATION' AND Value ='" + design + "' ";
                SqlCommand InvAmountcom = new SqlCommand(designn, conn);
                int desigination = Convert.ToInt32(InvAmountcom.ExecuteScalar().ToString());

                string gen = comboBox1.SelectedItem.ToString();
                //getting gender value from lookup
                string genvalue = "SELECT Id FROM Lookup WHERE Category= 'GENDER' AND Value ='" + gen + "' ";
                SqlCommand genint = new SqlCommand(genvalue, conn);
                genint.ExecuteNonQuery();
                int gender = Convert.ToInt32(genint.ExecuteScalar());

                //inserting value in person
                String insert1 = "update  Person set FirstName = '" + fn + "',LastName='" + ln + "',Contact='" + con + "',Email='" + email + "',DateOfBirth='" + dt + "',Gender='" + gender + "'where Id='" + IDD + "'";
                SqlCommand sql1 = new SqlCommand(insert1, conn);
                int i1 = sql1.ExecuteNonQuery();


                //getting id from person for advisor
                string sid = String.Format("SELECT Id FROM Person Where Email ='{0}'", email);
                SqlCommand stuid = new SqlCommand(sid, conn);
                int id = Convert.ToInt32(stuid.ExecuteScalar());

                String insert2 = "update  Advisor set Designation = '" + desigination + "',Salary='" + salary + "'where Id='" + IDD + "'";
                SqlCommand sql2 = new SqlCommand(insert2, conn);
                int i2 = sql2.ExecuteNonQuery();

                // to check if the student is registered or not
                if (i1 >= 1 && i2 >= 1)
                {
                    MessageBox.Show("Edited Succesfully");
                    this.Refresh();
                }
                else
                {
                    { MessageBox.Show("Edition failed"); }
                }


                this.Refresh();
                Refresh();
                this.Hide();
                advisor ss = new advisor();
                ss.Show();
                conn.Close();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                conn.Close();
            }




        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
