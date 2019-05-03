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
    public partial class assignproject : Form
    {
        int panelWidth;
        bool Hidden;
        int IDD;
        int idd;
        
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-VFGDRDF\NIDA;Initial Catalog=ProjectA;Integrated Security=True");
        public assignproject()
        {
            InitializeComponent();
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);
            panelWidth = PanelSlide.Width;
            Hidden = false;
            panel2.Hide();
            conn.Open();
            try
            {
                String query = "SELECT ProjectAdvisor.[AdvisorId] as [AID],ProjectAdvisor.[ProjectId] as [PID], Project.[Title] as Title, CONCAT(Person.FirstName, ' ', Person.LastName) AS Name FROM [dbo].[Project] JOIN [dbo].[ProjectAdvisor] ON Project.Id = ProjectAdvisor.ProjectId JOIN [dbo].[Advisor] ON  ProjectAdvisor.AdvisorId = Advisor.Id  JOIN [dbo].[Person] ON Advisor.Id = Person.Id";

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

                this.dataGridView1.Columns["PId"].Visible = false;
                this.dataGridView1.Columns["AId"].Visible = false;
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
            panel2.Show();
            button4.Hide();
            button5.Show();
            SqlCommand cmd;
            SqlDataAdapter da;
            DataSet ds;
            cmd = new SqlCommand("select  CONCAT(Person.[FirstName], ' ',Person.[LastName]) FROM [dbo].[Advisor] JOIN [dbo].[Person] ON Advisor.Id = Person.Id ", conn);

            da = new SqlDataAdapter(cmd);
            ds = new DataSet();

            da.Fill(ds);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

            {
                comboBox3.Items.Add(ds.Tables[0].Rows[i][0]);
            }

            cmd = new SqlCommand("select Title from Project", conn);

            da = new SqlDataAdapter(cmd);
            ds = new DataSet();

            da.Fill(ds);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

            {
                comboBox2.Items.Add(ds.Tables[0].Rows[i][0]);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            conn.Open();
            string advisor = comboBox1.SelectedItem.ToString();
            string proadd = "SELECT Id FROM Lookup WHERE Category= 'ADVISOR_ROLE' AND Value ='" + advisor + "' ";
            SqlCommand InvAmount = new SqlCommand(proadd, conn);
            int advisorvalu = Convert.ToInt32(InvAmount.ExecuteScalar());

            string adname = Convert.ToString(comboBox3.SelectedItem.ToString());
            string addd = "SELECT Advisor.Id FROM  [dbo].[Advisor] JOIN [dbo].[Person] ON Advisor.Id = Person.Id  WHERE Concat(FirstName, ' ' , LastName)='" +adname + "' ";
            SqlCommand InvAmoun = new SqlCommand(addd, conn);
            int adid = Convert.ToInt32(InvAmoun.ExecuteScalar());

            string pro = Convert.ToString(comboBox2.SelectedItem.ToString());
            string prooo = "SELECT Id FROM Project   WHERE Title='" + pro + "' ";
            SqlCommand InvAmou = new SqlCommand(prooo, conn);
            int proid = Convert.ToInt32(InvAmou.ExecuteScalar());
            DateTime dt = DateTime.Today;
            
            try
            {
                String insert1 = "insert into ProjectAdvisor  values('" + adid + "','" + proid + "','" + advisorvalu + "','" + dt + "' ) ";
                SqlCommand sql1 = new SqlCommand(insert1, conn);
                int i1 = sql1.ExecuteNonQuery();

                if (i1 >= 1)
                {
                    MessageBox.Show("Project Assigned ");
                    this.Refresh();
                }
                else
                {
                    { MessageBox.Show(" Project not Assigned"); }
                }
                this.Refresh();
                Refresh();
                this.Hide();
                assignproject ss = new assignproject();
                ss.Show();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Already assigned");
            }
            conn.Close();
        }
        // edited press
        private void button4_Click(object sender, EventArgs e)
        {

            conn.Open();
            string advisor = comboBox1.SelectedItem.ToString();
            try
            {
                string proadd = "SELECT Id FROM Lookup WHERE Category= 'ADVISOR_ROLE' AND Value ='" + advisor + "' ";
                SqlCommand InvAmount = new SqlCommand(proadd, conn);
                int advisorvalu = Convert.ToInt32(InvAmount.ExecuteScalar());
                string adname = Convert.ToString(comboBox3.SelectedItem.ToString());
                string addd = "SELECT Advisor.Id FROM  [dbo].[Advisor] JOIN [dbo].[Person] ON Advisor.Id = Person.Id  WHERE Concat(FirstName, ' ' , LastName)='" + adname + "' ";
                SqlCommand InvAmoun = new SqlCommand(addd, conn);
                int adid = Convert.ToInt32(InvAmoun.ExecuteScalar());

                string pro = Convert.ToString(comboBox2.SelectedItem.ToString());
                string prooo = "SELECT Id FROM Project   WHERE Title='" + pro + "' ";
                SqlCommand InvAmou = new SqlCommand(prooo, conn);
                int proid = Convert.ToInt32(InvAmou.ExecuteScalar());
                DateTime dt = DateTime.Today;


                String insert1 = "update ProjectAdvisor set AdvisorId = '" + adid + "',ProjectId='" + proid + "',AdvisorRole='" + advisorvalu + "',AssignmentDate='" + dt + "'";

                SqlCommand sql1 = new SqlCommand(insert1, conn);
                int i1 = sql1.ExecuteNonQuery();

                // to check if the student is registered or not
                if (i1 >= 1)
                {
                    MessageBox.Show("Updation Succesfull");
                    this.Refresh();
                }
                else

                { MessageBox.Show(" Updation Failed"); }


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
                IDD = Convert.ToInt32(dataGridView1.Rows[index].Cells[3].Value.ToString());
                idd = Convert.ToInt32(dataGridView1.Rows[index].Cells[4].Value.ToString());
                try
                {
                    string delete1 = String.Format("DELETE FROM ProjectAdvisor WHERE AdvisorId = '" + IDD + "' AND ProjectId='" + idd + "'");

                    SqlCommand de11 = new SqlCommand(delete1, conn);
                    int i = de11.ExecuteNonQuery();
                    // to check if the student is registered or not
                    if (i >= 1)
                    {
                        MessageBox.Show("  Deletion Succesfull ");
                        this.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("  Deletion Failed");
                    }
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                    
                }
            }


            else if (e.ColumnIndex == dataGridView1.Columns["EditButton"].Index)
            {
                panel2.Show();
                int indexx = e.RowIndex;
                IDD = Convert.ToInt32(dataGridView1.Rows[indexx].Cells[3].Value.ToString());
                idd = Convert.ToInt32(dataGridView1.Rows[indexx].Cells[4].Value.ToString());
                string tit = Convert.ToString(dataGridView1.Rows[indexx].Cells[5].Value.ToString());
                string advsor = Convert.ToString(dataGridView1.Rows[indexx].Cells[6].Value.ToString());
                panel3.Show();
                button5.Hide();
                button4.Show();
                
                SqlCommand cmd;
                SqlDataAdapter da;
                DataSet ds;
                cmd = new SqlCommand("select  CONCAT(Person.[FirstName], ' ',Person.[LastName]) FROM [dbo].[Advisor] JOIN [dbo].[Person] ON Advisor.Id = Person.Id ", conn);

                da = new SqlDataAdapter(cmd);
                ds = new DataSet();

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                {
                    comboBox3.Items.Add(ds.Tables[0].Rows[i][0]);
                }

                cmd = new SqlCommand("select Title from Project", conn);

                da = new SqlDataAdapter(cmd);
                ds = new DataSet();

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                {
                    comboBox2.Items.Add(ds.Tables[0].Rows[i][0]);
                }

            }

            else if (e.ColumnIndex == dataGridView1.Columns["DetailsButton"].Index)
            {
                this.Hide();
                AssignProjectDetails s1 = new AssignProjectDetails();
                s1.Show();

            }

            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 ff = new Form2();
            ff.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            this.Refresh();
            Refresh();
            this.Hide();
            assignproject ss = new assignproject();
            ss.Show();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
