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
    public partial class Group : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-VFGDRDF\NIDA;Initial Catalog=ProjectA;Integrated Security=True");
        int IDD;
        int idd;
        
        int panelWidth;
        bool Hidden;
        public Group()
        {
            InitializeComponent();
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);
            panel5.Hide();
            dataGridView2.Hide();
            panelWidth = PanelSlide.Width;
            Hidden = false;
            conn.Open();
            try
            {


                String query = "SELECT GroupStudent.GroupId AS [Group Id] , GroupStudent.StudentId ,Student.RegistrationNo AS [Roll No], (SELECT Value FROM Lookup WHERE Category = 'STATUS' AND Id = Status ) AS [Status] , AssignmentDate  FROM [dbo].[GroupStudent]  JOIN  [dbo].[Student] ON GroupStudent.StudentId = Student.Id"; ;

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

                this.dataGridView1.Columns["StudentId"].Visible = false;


                query = "SELECT Id, Created_On From [dbo].[Group]"; ;

                command = new SqlCommand(query, conn);

                sda = new SqlDataAdapter();
                sda.SelectCommand = command;
                dbadataset = new DataTable();
                sda.Fill(dbadataset);
                bsource = new BindingSource();
                bsource.DataSource = dbadataset;
                dataGridView2.DataSource = bsource;


                deleteButton = new DataGridViewButtonColumn();
                deleteButton.Name = "Delete";
                deleteButton.HeaderText = "Delete";
                deleteButton.Text = "Delete";
                deleteButton.UseColumnTextForButtonValue = true;
                this.dataGridView2.Columns.Add(deleteButton);
                conn.Close();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                conn.Close();
            }

        }

        private void Group_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            
            DateTime dt = DateTime.Today;
            try
            {
                String insert1 = "insert into [dbo].[Group] values('" + dt + "' ) ";
                SqlCommand sql1 = new SqlCommand(insert1, conn);
                int i1 = sql1.ExecuteNonQuery();

                // to check if the student is registered or not
                if (i1 >= 1)
                {
                    MessageBox.Show("Group Created");
                    this.Refresh();
                }
                else
                {
                    { MessageBox.Show(" Group Not Created"); }
                }


                conn.Close();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                conn.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel5.Show();
            button5.Hide();
            button6.Show();
            SqlCommand cmd;
            SqlDataAdapter da;
            DataSet ds;
            cmd = new SqlCommand("SELECT Id FROM [dbo].[Group]", conn);

            da = new SqlDataAdapter(cmd);
            ds = new DataSet();

            da.Fill(ds);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

            {
                comboBox3.Items.Add(ds.Tables[0].Rows[i][0]);
            }

            cmd = new SqlCommand("select RegistrationNo from Student", conn);

            da = new SqlDataAdapter(cmd);
            ds = new DataSet();

            da.Fill(ds);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

            {
                comboBox2.Items.Add(ds.Tables[0].Rows[i][0]);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            conn.Open();
            string advisor = comboBox1.SelectedItem.ToString();
            string proadd = "SELECT Id FROM Lookup WHERE Category= 'STATUS' AND Value ='" + advisor + "' ";
            SqlCommand InvAmount = new SqlCommand(proadd, conn);
            int advisorvalu = Convert.ToInt32(InvAmount.ExecuteScalar());

            int ad = Convert.ToInt32(comboBox3.SelectedItem.ToString());
            string reg = Convert.ToString(comboBox2.SelectedItem.ToString());
            
            string prooo = "SELECT Id FROM Student  WHERE RegistrationNo='" + reg + "' ";
            SqlCommand InvAmou = new SqlCommand(prooo, conn);
            int proid = Convert.ToInt32(InvAmou.ExecuteScalar());
            
            DateTime dt = DateTime.Today;
            try
            {
                String insert1 = "insert into GroupStudent  values('" + ad + "','" + proid + "','" + advisorvalu + "','" + dt + "' ) ";
                SqlCommand sql1 = new SqlCommand(insert1, conn);
                int i1 = sql1.ExecuteNonQuery();

                // to check if the student is registered or not
                if (i1 >= 1)
                {
                    MessageBox.Show("Student Added ");
                    this.Refresh();
                }
                else
                {
                    { MessageBox.Show("Student not Added"); }
                }
                this.Refresh();
                Refresh();
                this.Hide();
                Group ss = new Group();
                ss.Show();

                conn.Close();
            }
            catch (Exception a)
            {
                MessageBox.Show("Already Added in a group");
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
                IDD = Convert.ToInt32(dataGridView1.Rows[index].Cells[2].Value.ToString());
                idd = Convert.ToInt32(dataGridView1.Rows[index].Cells[3].Value.ToString());
                try
                {
                    string delete1 = String.Format("DELETE FROM GroupStudent WHERE GroupId = '" + IDD + "' AND StudentId='" + idd + "'");

                    SqlCommand de11 = new SqlCommand(delete1, conn);
                    int i = de11.ExecuteNonQuery();

                    if (i >= 1)
                    {
                        MessageBox.Show("  Deletion Succesfull ");
                        this.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("  Deletion Failed");
                    }
                    this.Refresh();
                    Refresh();
                    this.Hide();
                    Group ss = new Group();
                    ss.Show();
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                    
                }
            }
           

            else if (e.ColumnIndex == dataGridView1.Columns["EditButton"].Index)
            {
                panel5.Show();
                button6.Hide();
                button5.Show();
                label5.Hide();
                label2.Hide();
                comboBox2.Hide();
                comboBox3.Hide();
                int indexx = e.RowIndex;
                IDD = Convert.ToInt32(dataGridView1.Rows[indexx].Cells[2].Value.ToString());
                idd = Convert.ToInt32(dataGridView1.Rows[indexx].Cells[3].Value.ToString());
              
                SqlCommand cmd;
                SqlDataAdapter da;
                DataSet ds;
                cmd = new SqlCommand("select Id from [dbo].[Group]", conn);

                da = new SqlDataAdapter(cmd);
                ds = new DataSet();

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                {
                    comboBox3.Items.Add(ds.Tables[0].Rows[i][0]);
                }

                cmd = new SqlCommand("select RegistrationNo from Student", conn);

                da = new SqlDataAdapter(cmd);
                ds = new DataSet();

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                {
                    comboBox2.Items.Add(ds.Tables[0].Rows[i][0]);
                }

            }

            

            conn.Close();
        }
        // when edit button in pressed
        private void button5_Click(object sender, EventArgs e)
        {
            conn.Open();
            string advisor = comboBox1.SelectedItem.ToString();
            string proadd = "SELECT Id FROM Lookup WHERE Category= 'STATUS' AND Value ='" + advisor + "' ";
            SqlCommand InvAmount = new SqlCommand(proadd, conn);
            int advisorvalu = Convert.ToInt32(InvAmount.ExecuteScalar());
            
            DateTime dt = DateTime.Today;
            try
            {
                String insert1 = "update  GroupStudent set  GroupId='" + IDD + "',StudentId='" + idd + "',Status='" + advisorvalu + "',AssignmentDate='" + dt + "' WHERE  GroupId = '" + IDD + "' AND StudentId='" + idd + "'";
                SqlCommand sql1 = new SqlCommand(insert1, conn);
                int i1 = sql1.ExecuteNonQuery();

                // to check if the student is registered or not
                if (i1 >= 1)
                {
                    MessageBox.Show("Updation Sucessful");
                    this.Refresh();
                }
                else
                {
                    { MessageBox.Show("Updation failed"); }
                }
                this.Refresh();
                Refresh();
                this.Hide();
                Group ss = new Group();
                ss.Show();
                conn.Close();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                conn.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Refresh();
            Refresh();
            this.Hide();
            Group ss = new Group();
            ss.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 ff = new Form2();
            ff.Show();
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

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel5.Hide();
            dataGridView1.Hide();
            dataGridView2.Show();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            conn.Open();

            if (e.RowIndex == dataGridView2.NewRowIndex || e.RowIndex < 0)
                return;

            //Check if click is on specific column 
            if (e.ColumnIndex == dataGridView2.Columns["Delete"].Index)
            {
                int index = e.RowIndex;
                int IDDD = Convert.ToInt32(dataGridView2.Rows[index].Cells[1].Value.ToString());
                try
                {
                    string delete2 = String.Format("DELETE FROM [dbo].[GroupEvaluation] WHERE GroupId='" + IDDD + "'");
                    SqlCommand de12 = new SqlCommand(delete2, conn);
                    int j = de12.ExecuteNonQuery();

                    string delete3 = String.Format("DELETE FROM [dbo].[GroupStudent] WHERE GroupId='" + IDDD + "'");
                    SqlCommand de13 = new SqlCommand(delete3, conn);
                    int k = de13.ExecuteNonQuery();

                    string delete1 = String.Format("DELETE FROM [dbo].[Group] WHERE Id='" + IDDD + "'");
                    SqlCommand de11 = new SqlCommand(delete1, conn);
                    int i = de11.ExecuteNonQuery();

                    if (i >= 1)
                    {
                        MessageBox.Show("  Deletion Succesfull ");
                        this.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("  Deletion Failed");
                    }
                    this.Refresh();
                    Refresh();
                    this.Hide();
                    Group ss = new Group();
                    ss.Show();
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                    
                }

            }
            conn.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
