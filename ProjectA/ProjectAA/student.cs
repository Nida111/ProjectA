using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ProjectA
{
    public partial class student : Form
    {
        int IDD;
        int panelWidth;
        bool Hidden;

        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-VFGDRDF\NIDA;Initial Catalog=ProjectA;Integrated Security=True");
        
        public student()
        {
            InitializeComponent();
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);
            panelWidth = PanelSlide.Width;
            Hidden = false;
            panel3.Hide();
            button6.Hide();

            conn.Open();

            String query = "SELECT Person.Id AS Id, FirstName as [First Name], Student.[RegistrationNo] as [Registration No] FROM [dbo].[Person] JOIN [dbo].[Student] ON Student.Id = Person.Id "; ;
            try
            {
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
                conn.Close();

            }

            catch (Exception a)
            {

                MessageBox.Show(a.Message);
            }
            



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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide() ;
            Form2 f1 = new Form2();
            f1.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlCommand com = new SqlCommand();
            com.Connection = conn;
            conn.Open();
            string fn = textBox1.Text;
            string ln = textBox2.Text;
            string con = textBox3.Text;
            string email = textBox4.Text;
            string rollno = textBox5.Text;
            

            if (string.IsNullOrEmpty(fn) || !Regex.IsMatch(textBox1.Text, @"[A-Za-z]+$"))
             {
                MessageBox.Show("Invalid first name should be letters ");
                textBox1.Clear();
                textBox1.BackColor = Color.DodgerBlue;
                conn.Close();
                return;
            }
            textBox1.BackColor = Color.White;
            if (string.IsNullOrEmpty(ln) || !Regex.IsMatch(textBox2.Text, @"[A-Za-z]+$"))
             {
                MessageBox.Show("Invalid last name should be letters");
                textBox2.Clear();
                textBox2.BackColor = Color.DodgerBlue;
                conn.Close();
                return;
            }
            textBox2.BackColor = Color.White;
            if (string.IsNullOrEmpty(con) || !Regex.IsMatch(textBox3.Text, @"^[0-9]{11}$") )
            {
                MessageBox.Show("Invalid contact.Should be 11 digits");
                textBox2.Clear();
                textBox3.BackColor= Color.DodgerBlue; ;
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
                MessageBox.Show("Invalid email type ");
                textBox4.BackColor = Color.DodgerBlue;
                textBox4.Clear();
                conn.Close();
                return;

            }
            textBox4.BackColor = Color.White;

            cmd = new SqlCommand("Select count(*) from Student where RegistrationNo= @rollno", conn);
            cmd.Parameters.AddWithValue("@rollno", this.textBox5.Text);

            result = Convert.ToInt32(cmd.ExecuteScalar());
            if (result != 0)
            {
                MessageBox.Show("roll no  Exists");
                conn.Close();
                return;
            }

            if (string.IsNullOrEmpty(rollno) || !Regex.IsMatch(textBox5.Text, @"^\d{4}(-[A-Za-z][A-Za-z])(-\d{2,3})"))
            {
                MessageBox.Show("Invalid roll Number .The format should be e.g 2016-CE-63");
                textBox5.BackColor = Color.DodgerBlue;
                textBox5.Clear();
                conn.Close();
                return;
            }
            textBox5.BackColor = Color.White;

            DateTime dt = dateTimePicker1.Value;
            string gen = comboBox1.SelectedItem.ToString();
            //getting gender value from lookup
            string genvalue = "SELECT Id FROM Lookup WHERE Category= 'GENDER' AND Value ='" + gen + "' ";
            SqlCommand genint = new SqlCommand(genvalue, conn);
            genint.ExecuteNonQuery();
            int gender = Convert.ToInt32(genint.ExecuteScalar());
            try
            {
                //inserting value in person
                String insert1 = "insert into Person  values('" + fn + "','" + ln + "','" + con + "','" + email + "','" + dt + "','" + gender + "' ) ";
                SqlCommand sql1 = new SqlCommand(insert1, conn);
                int i1 = sql1.ExecuteNonQuery();

                //getting id from person for student
                string sid = String.Format("SELECT Id FROM Person Where Email ='{0}'", email);
                SqlCommand stuid = new SqlCommand(sid, conn);
                int id = Convert.ToInt32(stuid.ExecuteScalar());

                String insert2 = "insert into Student values('" + id + "','" + rollno + "')";
                SqlCommand sql2 = new SqlCommand(insert2, conn);
                int i2 = sql2.ExecuteNonQuery();

                // to check if the student is registered or not
                if (i1 >= 1 && i2 >= 1)
                {
                    MessageBox.Show(i1 + " Student Registered ");
                    this.Refresh();
                }
                else
                {
                    { MessageBox.Show(" Student not Registered :"); }
                }

                this.Refresh();
                Refresh();
                this.Hide();
                student ss = new student();
                ss.Show();

                conn.Close();
            }
            catch (Exception a)
            {

                MessageBox.Show(a.Message);
            }




        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel3.Show();
            button6.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            studetails s1 = new studetails();
            s1.Show();
        }
        // edit delete
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                conn.Open();

                if (e.RowIndex == dataGridView1.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == dataGridView1.Columns["DeleteButton"].Index)
                {
                    int index = e.RowIndex;
                    int ID = Convert.ToInt32(dataGridView1.Rows[index].Cells[3].Value.ToString());

                    string delete3 = string.Format("DELETE FROM GroupStudent WHERE StudentId = '{0}'", ID);
                    SqlCommand del3 = new SqlCommand(delete3, conn);
                    int k = del3.ExecuteNonQuery();

                    string delete1 = String.Format("DELETE FROM Student WHERE Id = '{0}'", ID);
                    SqlCommand de11 = new SqlCommand(delete1, conn);
                    int i = de11.ExecuteNonQuery();

                    string delete2 = string.Format("DELETE FROM Person WHERE Id = '{0}'", ID);
                    SqlCommand del2 = new SqlCommand(delete2, conn);
                    int j = del2.ExecuteNonQuery();

                    // to check if the student is registered or not
                    if (i >= 1 && j >= 1)
                    {
                        MessageBox.Show(" Student Deleted ");
                        this.Refresh();
                    }
                    else
                    {
                        MessageBox.Show(" Student not Deleted ");
                    }

                    this.Refresh();
                    Refresh();
                    this.Hide();
                    student ss = new student();
                    ss.Show();
                }


                else if (e.ColumnIndex == dataGridView1.Columns["EditButton"].Index)
                {

                    int indexx = e.RowIndex;
                    IDD = Convert.ToInt32(dataGridView1.Rows[indexx].Cells[3].Value.ToString());
                    panel3.Show();
                    button5.Hide();
                    button6.Show();

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

                    string regno = "SELECT RegistrationNo FROM Student WHERE Id = '" + IDD + "' ";
                    SqlCommand reg = new SqlCommand(regno, conn);
                    reg.ExecuteNonQuery();
                    string rollno = Convert.ToString(reg.ExecuteScalar());
                    textBox5.Text = rollno;

                    string gen = "SELECT Gender FROM Person WHERE Id = '" + IDD + "' ";
                    SqlCommand gender = new SqlCommand(gen, conn);
                    gender.ExecuteNonQuery();
                    int genn = Convert.ToInt32(gender.ExecuteScalar());

                    string designn = "SELECT Value FROM Lookup WHERE Category= 'GENDER' AND Id ='" + genn + "' ";
                    SqlCommand InvAmountcom = new SqlCommand(designn, conn);
                    string mf = Convert.ToString(InvAmountcom.ExecuteScalar().ToString());

                    comboBox1.Text = mf;
                }

                else if (e.ColumnIndex == dataGridView1.Columns["DetailsButton"].Index)
                {
                    this.Hide();
                    studetails s1 = new studetails();
                    s1.Show();

                }

                conn.Close();
            }
            catch (Exception a)
            {

                MessageBox.Show(a.Message);
            }


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void ReloadForm()
        {
            
            dataGridView1.Update();
            //and how many controls or settings you want, just add them here
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Refresh();
            Refresh();
            this.Hide();
            student ss = new student();
            ss.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {

                conn.Open();
                string fn = textBox1.Text;
                string ln = textBox2.Text;
                string con = textBox3.Text;
                string email = textBox4.Text;
                string rollno = textBox5.Text;

                if (string.IsNullOrEmpty(fn) || !Regex.IsMatch(textBox1.Text, @"[A-Za-z]+$"))
                {
                    MessageBox.Show("Invalid First name. Should be letters");
                    textBox1.Clear();
                    textBox1.BackColor = Color.DodgerBlue;
                    conn.Close();
                    return;
                }
                textBox1.BackColor = Color.White;
                if (string.IsNullOrEmpty(ln) || !Regex.IsMatch(textBox2.Text, @"[A-Za-z]+$"))
                {
                    MessageBox.Show("Invalid Last name. Should be letters");
                    textBox2.Clear();
                    textBox2.BackColor = Color.DodgerBlue;
                    conn.Close();
                    return;
                }
                textBox2.BackColor = Color.White;
                if (string.IsNullOrEmpty(con) || !Regex.IsMatch(textBox3.Text, @"^[0-9]{11}$"))
                {
                    MessageBox.Show("Invalid contact.It should be 11 digits");
                    textBox3.Clear();
                    textBox3.BackColor = Color.DodgerBlue; ;
                    conn.Close();
                    return;

                }
                textBox3.BackColor = Color.White;


                if (string.IsNullOrEmpty(email) || !Regex.IsMatch(textBox4.Text, @"^([\w]+)@([\w]+)\.([\w]+)$"))
                {
                    MessageBox.Show("Invalid email.");
                    textBox4.BackColor = Color.DodgerBlue;
                    textBox4.Clear();
                    conn.Close();
                    return;

                }
                textBox4.BackColor = Color.White;


                if (string.IsNullOrEmpty(rollno) || !Regex.IsMatch(textBox5.Text, @"^\d{4}(-[A-Za-z][A-Za-z])(-\d{2,3})"))
                {
                    MessageBox.Show("Invalid roll numbetr. The format should be e.g 2016-CE-63");
                    textBox5.BackColor = Color.DodgerBlue;
                    textBox5.Clear();
                    conn.Close();
                    return;
                }
                textBox5.BackColor = Color.White;
                DateTime dt = dateTimePicker1.Value;
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

                //getting id from person for student

                String insert2 = "update  Student set RegistrationNo = '" + rollno + "'where Id='" + IDD + "'";
                SqlCommand sql2 = new SqlCommand(insert2, conn);
                int i2 = sql2.ExecuteNonQuery();

                // to check if the student is registered or not
                if (i1 >= 1 && i2 >= 1)
                {
                    MessageBox.Show("Edited Succesfully ");

                }
                else
                {
                    { MessageBox.Show(" Try Again"); }
                }

                this.Refresh();
                Refresh();
                this.Hide();
                student ss = new student();
                ss.Show();
                conn.Close();
            }
            catch (Exception a)
            {

                MessageBox.Show(a.Message);
                conn.Close();
            }



        }
    }
}
