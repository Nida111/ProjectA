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
    public partial class projectadd : Form
    {
        int IDD;
        int panelWidth;
        bool Hidden;
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-VFGDRDF\NIDA;Initial Catalog=ProjectA;Integrated Security=True");
        public projectadd()
        {
            InitializeComponent();
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);
            panelWidth = PanelSlide.Width;
            Hidden = false;
            panel5.Hide();
            dataGridView1.ForeColor = Color.Black;

            conn.Open();
            String query = "SELECT Id AS Id, Title as [Title], Description as Description FROM [dbo].[Project] "; ;

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

        private void button1_Click(object sender, EventArgs e)
        {
            panel5.Show();
            button4.Hide();

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

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            conn.Open();
            string title = textBox2.Text;

            string des = textBox1.Text;

            SqlCommand cmd = new SqlCommand("Select count(*) from Project where Title= @title", conn);
            cmd.Parameters.AddWithValue("@title", this.textBox2.Text);

            int result = Convert.ToInt32(cmd.ExecuteScalar());

            if (result != 0)
            {
                MessageBox.Show("Name Exists");
                conn.Close();
                return;
            }
            try
            {


                String insert1 = "insert into Project  values('" + title + "','" + des + "' )";
                SqlCommand sql1 = new SqlCommand(insert1, conn);
                int i1 = sql1.ExecuteNonQuery();
                if (i1 >= 1)
                {
                    MessageBox.Show("Project added");
                    this.Refresh();
                }
                else
                {
                    { MessageBox.Show("Project Not Added"); }
                }

                this.Refresh();
                Refresh();
                this.Hide();
                projectadd ss = new projectadd();
                ss.Show();
                
            }
            catch (Exception a)
            {

                MessageBox.Show(a.Message);
                
            }
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 dd = new Form2();
            dd.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Refresh();
            Refresh();
            this.Hide();
            projectadd ss = new projectadd();
            ss.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            conn.Open();

            if (e.RowIndex == dataGridView1.NewRowIndex || e.RowIndex < 0)
                return;
            try
            {


                //Check if click is on specific column 
                if (e.ColumnIndex == dataGridView1.Columns["DeleteButton"].Index)
                {
                    int index = e.RowIndex;
                    int ID = Convert.ToInt32(dataGridView1.Rows[index].Cells[2].Value.ToString());

                    string delete2 = String.Format("DELETE FROM ProjectAdvisor WHERE ProjectId = '{0}'", ID);
                    SqlCommand de12 = new SqlCommand(delete2, conn);
                    int j = de12.ExecuteNonQuery();

                    string delete3 = String.Format("DELETE FROM GroupProject WHERE GroupId = '{0}'", ID);
                    SqlCommand de13 = new SqlCommand(delete3, conn);
                    int k = de13.ExecuteNonQuery();

                    string delete1 = String.Format("DELETE FROM Project WHERE Id = '{0}'", ID);
                    SqlCommand de11 = new SqlCommand(delete1, conn);
                    int i = de11.ExecuteNonQuery();

                    if (i >= 1)
                    {
                        MessageBox.Show(" Project Deleted ");
                        this.Refresh();
                    }
                    else
                    {
                        MessageBox.Show(" Project not Deleted");
                    }

                    this.Refresh();
                    Refresh();
                    this.Hide();
                    projectadd ss = new projectadd();
                    ss.Show();
                }
                else if (e.ColumnIndex == dataGridView1.Columns["EditButton"].Index)
                {
                    int indexx = e.RowIndex;
                    IDD = Convert.ToInt32(dataGridView1.Rows[indexx].Cells[2].Value.ToString());
                    panel5.Show();
                    button5.Hide();
                    button4.Show();

                    string fn = "SELECT Title FROM Project WHERE Id = '" + IDD + "' ";
                    SqlCommand fnn = new SqlCommand(fn, conn);
                    fnn.ExecuteNonQuery();
                    string first = Convert.ToString(fnn.ExecuteScalar());
                    textBox2.Text = first;

                    string ln = "SELECT Description FROM Project WHERE Id = '" + IDD + "' ";
                    SqlCommand lnn = new SqlCommand(ln, conn);
                    lnn.ExecuteNonQuery();
                    string last = Convert.ToString(lnn.ExecuteScalar());
                    textBox1.Text = last;
                    conn.Close();

                }

            }
            catch (Exception a)
            {

                MessageBox.Show(a.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            conn.Open();
            string title = textBox2.Text;
            string des = textBox1.Text;
            try
            {

                String insert1 = "update  Project set Title = '" + title + "',Description='" + des + "'where Id='" + IDD + "'";
                SqlCommand sql1 = new SqlCommand(insert1, conn);
                int i1 = sql1.ExecuteNonQuery();
                if (i1 >= 1)
                {
                    MessageBox.Show("Project edited");
                    this.Refresh();
                }
                else
                {
                    { MessageBox.Show("Project Not edited"); }
                }

                this.Refresh();
                Refresh();
                this.Hide();
                projectadd ss = new projectadd();
                ss.Show();
                conn.Close();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                conn.Close();
            }

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    
}
