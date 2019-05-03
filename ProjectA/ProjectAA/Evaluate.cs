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
    public partial class Evaluate : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-VFGDRDF\NIDA;Initial Catalog=ProjectA;Integrated Security=True");
        int IDD;
        int panelWidth;
        bool Hidden;
        public Evaluate()
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
            conn.Open();
            dataGridView1.ForeColor = Color.Black;
            String query = "SELECT Id , Name , TotalMarks as [Total Marks], TotalWeightage as [Total Weightage] FROM [dbo].[Evaluation] ";

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


            this.dataGridView1.Columns["Id"].Visible = false;
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

        private void button7_Click(object sender, EventArgs e)
        {
            this.Refresh();
            Refresh();
            this.Hide();
            Evaluate ss = new Evaluate();
            ss.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form2 f1 = new Form2();
            f1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel3.Show();
            button4.Show();
            button5.Hide();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            conn.Open();
            button5.Hide();
            string fn = textBox1.Text;
            string ln = textBox2.Text;
            string lnn = textBox3.Text;

            SqlCommand cmd = new SqlCommand("Select count(*) from Evaluation where Name= @fn", conn);
            cmd.Parameters.AddWithValue("@fn", this.textBox1.Text);

            int result = Convert.ToInt32(cmd.ExecuteScalar());

            if (result != 0)
            {
                MessageBox.Show("Name Exists");
                conn.Close();
                return;
            }
            if (string.IsNullOrEmpty(fn) || !Regex.IsMatch(textBox1.Text, @"[A-Za-z0-9]+$"))
            {
                MessageBox.Show("Invalid name type again");
                textBox1.Clear();
                textBox1.BackColor = Color.DodgerBlue;
                conn.Close();
                return;
            }
            textBox1.BackColor = Color.White;
            if (string.IsNullOrEmpty(ln) || !Regex.IsMatch(textBox2.Text, @"^[0-9]{1,3}$"))
            {
                MessageBox.Show("Invalid marks");
                textBox2.Clear();
                textBox2.BackColor = Color.DodgerBlue;
                conn.Close();
                return;
            }
            textBox2.BackColor = Color.White;
            if (string.IsNullOrEmpty(lnn) || !Regex.IsMatch(textBox3.Text, @"^[0-9]{1,3}$"))
            {
                MessageBox.Show("Invalid Total Weightage");
                textBox3.Clear();
                textBox3.BackColor = Color.DodgerBlue;
                conn.Close();
                return;
            }
            textBox3.BackColor = Color.White;
            try
            {
                String insert1 = "insert into Evaluation values('" + fn + "','" + ln + "','" + lnn + "' ) ";
                SqlCommand sql1 = new SqlCommand(insert1, conn);
                int i1 = sql1.ExecuteNonQuery();

                // to check if the student is registered or not
                if (i1 >= 1)
                {
                    MessageBox.Show("Numbers Added");
                    this.Refresh();
                }
                else
                {
                    { MessageBox.Show("Numbers Added"); }
                }

                this.Refresh();
                Refresh();
                this.Hide();
                Evaluate ss = new Evaluate();
                ss.Show();
                conn.Close();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                conn.Close();
            }
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
                int ID = Convert.ToInt32(dataGridView1.Rows[index].Cells[2].Value.ToString());
                try
                {
                    string delete2 = String.Format("DELETE FROM GroupEvaluation WHERE EvaluationId = '{0}'", ID);
                    SqlCommand de12 = new SqlCommand(delete2, conn);
                    int j = de12.ExecuteNonQuery();

                    string delete1 = String.Format("DELETE FROM Evaluation WHERE Id = '{0}'", ID);
                    SqlCommand de11 = new SqlCommand(delete1, conn);
                    int i = de11.ExecuteNonQuery();


                    // to check if the student is registered or not
                    if (i >= 1)
                    {
                        MessageBox.Show(" Evaluation Deleted ");
                        this.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("  Evaluation not Deleted ");
                    }

                    this.Refresh();
                    Refresh();
                    this.Hide();
                    Evaluate ss = new Evaluate();
                    ss.Show();
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                    conn.Close();
                }
            }

            else if (e.ColumnIndex == dataGridView1.Columns["EditButton"].Index)
            {

                int indexx = e.RowIndex;
                IDD = Convert.ToInt32(dataGridView1.Rows[indexx].Cells[2].Value.ToString());
                panel3.Show();
                
                button5.Show();
                try
                {
                    string fn = "SELECT Name FROM Evaluation WHERE Id = '" + IDD + "' ";
                    SqlCommand fnn = new SqlCommand(fn, conn);
                    fnn.ExecuteNonQuery();
                    string first = Convert.ToString(fnn.ExecuteScalar());
                    textBox1.Text = first;

                    string ln = "SELECT TotalMarks FROM Evaluation WHERE Id = '" + IDD + "' ";
                    SqlCommand lnn = new SqlCommand(ln, conn);
                    lnn.ExecuteNonQuery();
                    string last = Convert.ToString(lnn.ExecuteScalar());
                    textBox2.Text = last;


                    string con = "SELECT TotalWeightage FROM Evaluation WHERE Id = '" + IDD + "' ";
                    SqlCommand cont = new SqlCommand(con, conn);
                    cont.ExecuteNonQuery();
                    string contact = Convert.ToString(cont.ExecuteScalar());
                    textBox3.Text = contact;
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                    conn.Close();
                }
            }
            conn.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            conn.Open();
            button5.Hide();
            string fn = textBox1.Text;
            string ln = textBox2.Text;
            string lnn = textBox3.Text;

            if (string.IsNullOrEmpty(fn) || !Regex.IsMatch(textBox1.Text, @"[A-Za-z0-9]+$"))
            {
                MessageBox.Show("Invalid name type again");
                textBox1.Clear();
                textBox1.BackColor = Color.DodgerBlue;
                conn.Close();
                return;
            }
            textBox1.BackColor = Color.White;
            if (string.IsNullOrEmpty(ln) || !Regex.IsMatch(textBox2.Text, @"^[0-9]{1,3}$"))
            {
                MessageBox.Show("Invalid marks");
                textBox2.Clear();
                textBox2.BackColor = Color.DodgerBlue;
                conn.Close();
                return;
            }
            textBox2.BackColor = Color.White;
            if (string.IsNullOrEmpty(lnn) || !Regex.IsMatch(textBox3.Text, @"^[0-9]{1,3}$"))
            {
                MessageBox.Show("Invalid Total Weightage");
                textBox3.Clear();
                textBox3.BackColor = Color.DodgerBlue;
                conn.Close();
                return;
            }
            textBox3.BackColor = Color.White;

            try
            {
                String insert1 = "update  Evaluation set Name='" + fn + "',TotalMarks='" + ln + "',TotalWeightage='" + lnn + "'";
                SqlCommand sql1 = new SqlCommand(insert1, conn);
                int i1 = sql1.ExecuteNonQuery();
                // to check upation
                if (i1 >= 1)
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
                Evaluate ss = new Evaluate();
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
