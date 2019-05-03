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

namespace ProjectA
{
    public partial class GroupProject : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-VFGDRDF\NIDA;Initial Catalog=ProjectA;Integrated Security=True");
        int panelWidth;
        bool Hidden;
        public GroupProject()
        {
            InitializeComponent();
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);
            panel5.Hide();
            panelWidth = PanelSlide.Width;
            Hidden = false;
            try
            {
                String query = "SELECT GroupProject.GroupId AS [Group Id]  , GroupProject.ProjectId AS [Project Id], Project.Title  FROM [dbo].[Project]  JOIN  [dbo].[GroupProject] ON Project.Id = GroupProject.ProjectId JOIN [dbo].[Group] ON GroupProject.GroupId = [dbo].[Group].Id"; ;

                SqlCommand command = new SqlCommand(query, conn);

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = command;
                DataTable dbadataset = new DataTable();
                sda.Fill(dbadataset);
                BindingSource bsource = new BindingSource();
                bsource.DataSource = dbadataset;
                dataGridView1.DataSource = bsource;


                var deleteButton = new DataGridViewButtonColumn();
                deleteButton.Name = "DeleteButton";
                deleteButton.HeaderText = "Delete";
                deleteButton.Text = "Delete";
                deleteButton.UseColumnTextForButtonValue = true;
                this.dataGridView1.Columns.Add(deleteButton);
                this.dataGridView1.Columns["Project Id"].Visible = false;
                conn.Close();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                conn.Close();
            }
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

        private void button7_Click(object sender, EventArgs e)
        {
            this.Refresh();
            Refresh();
            this.Hide();
            GroupProject ss = new GroupProject();
            ss.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel5.Show();
            try
            {

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

                cmd = new SqlCommand("select Title from Project", conn);

                da = new SqlDataAdapter(cmd);
                ds = new DataSet();

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                {
                    comboBox2.Items.Add(ds.Tables[0].Rows[i][0]);
                }
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                conn.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            conn.Open();
            int ad = Convert.ToInt32(comboBox3.SelectedItem.ToString());    //group id
            string reg = Convert.ToString(comboBox2.SelectedItem.ToString()); //project title
           

            string prooo = "SELECT Id FROM Project WHERE title='" + reg + "' ";
            SqlCommand InvAmou = new SqlCommand(prooo, conn);
            int pro = Convert.ToInt32(InvAmou.ExecuteScalar());

            DateTime dt = DateTime.Today;
            
            try
            {
                String insert1 = "insert into GroupProject  values('" + pro + "','" + ad + "','" + dt + "' ) "     ;

                SqlCommand sql1 = new SqlCommand(insert1, conn);
                int i1 = sql1.ExecuteNonQuery();

                // to check if the student is registered or not
                if (i1 >= 1)
                {
                    MessageBox.Show("project is assigned to group");
                    this.Refresh();
                }
                else
                {
                    {
                        MessageBox.Show("Project not assigned to group");
                    }
                }
                this.Refresh();
                Refresh();
                this.Hide();
                GroupProject ss = new GroupProject();
                ss.Show();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Already exists");
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
                int IDD = Convert.ToInt32(dataGridView1.Rows[index].Cells[1].Value.ToString());
                int idd = Convert.ToInt32(dataGridView1.Rows[index].Cells[2].Value.ToString());
                try
                {
                    string delete1 = String.Format("DELETE FROM GroupProject WHERE GroupId = '" + IDD + "' AND ProjectId='" + idd + "'");

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
                    GroupProject ss = new GroupProject();
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
}
