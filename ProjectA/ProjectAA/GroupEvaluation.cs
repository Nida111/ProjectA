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
    public partial class GroupEvaluation : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-VFGDRDF\NIDA;Initial Catalog=ProjectA;Integrated Security=True");
        int panelWidth;
        bool Hidden;
        int IDD;
        int idd;
        public GroupEvaluation()
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
            conn.Open();
            try
            {
                String query = "SELECT GroupEvaluation.GroupId AS [Group Id] , GroupEvaluation.EvaluationId as [EVA] ,Evaluation.Name as [Name], GroupEvaluation.ObtainedMarks AS [Obtained marks],  GroupEvaluation.EvaluationDate AS [Evaluation Date] FROM [dbo].[Group]  JOIN  [dbo].[GroupEvaluation] ON [dbo].[Group].Id =  GroupEvaluation.GroupId JOIN  [dbo].[Evaluation] ON GroupEvaluation.EvaluationId = Evaluation.Id";

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

                this.dataGridView1.Columns["EVA"].Visible = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            panel5.Show();
            button5.Hide();
            conn.Open();
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

            cmd = new SqlCommand("select Name from Evaluation", conn);

            da = new SqlDataAdapter(cmd);
            ds = new DataSet();

            da.Fill(ds);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

            {
                comboBox2.Items.Add(ds.Tables[0].Rows[i][0]);
            }
            conn.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
           conn.Open();
            
            int ad = Convert.ToInt32(comboBox3.SelectedItem.ToString());    //group id
            string reg = Convert.ToString(comboBox2.SelectedItem.ToString()); //evaluation name

            string prooo = "SELECT Id FROM Evaluation WHERE Name='" + reg + "' ";
            SqlCommand InvAmou = new SqlCommand(prooo, conn);
            int pro = Convert.ToInt32(InvAmou.ExecuteScalar());
           string mark = textBox1.Text.ToString();

            if (string.IsNullOrEmpty(mark) || !Regex.IsMatch(textBox1.Text, @"^[0-9]{1,3}$"))
            {
                MessageBox.Show("Invalid marks");
                textBox1.Clear();
                textBox1.BackColor = Color.DodgerBlue;
                conn.Close();
                return;
            }
            int marks = Convert.ToInt32(textBox1.Text.ToString());
            DateTime dt = DateTime.Today;

            try
            {
                String insert1 = "insert into GroupEvaluation  values('" + ad + "','" + pro + "','" + marks + "','" + dt + "' ) ";

                SqlCommand sql1 = new SqlCommand(insert1, conn);
                int i1 = sql1.ExecuteNonQuery();

                // to check if the student is registered or not
                if (i1 >= 1)
                {
                    MessageBox.Show("Evaluation is marked against a  group");
                    this.Refresh();
                }
                else
                {
                    {
                        MessageBox.Show("Evaluation Not Marked");
                    }
                }
                this.Refresh();
                Refresh();
                this.Hide();
                GroupEvaluation ss = new GroupEvaluation();
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
                IDD = Convert.ToInt32(dataGridView1.Rows[index].Cells[2].Value.ToString());
                idd = Convert.ToInt32(dataGridView1.Rows[index].Cells[3].Value.ToString());
                try
                {


                    string delete1 = String.Format("DELETE FROM GroupEvaluation WHERE GroupId = '" + IDD + "' AND EvaluationId='" + idd + "'");

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
                    GroupEvaluation ss = new GroupEvaluation();
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
                label2.Hide();
                label5.Hide();
                comboBox2.Hide();
                comboBox3.Hide();
                int indexx = e.RowIndex;
                IDD = Convert.ToInt32(dataGridView1.Rows[indexx].Cells[2].Value.ToString());
                idd = Convert.ToInt32(dataGridView1.Rows[indexx].Cells[3].Value.ToString());

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

                cmd = new SqlCommand("select Name from Evaluation", conn);

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

        private void button5_Click(object sender, EventArgs e)
        {
            conn.Open();
            int marks = Convert.ToInt32(textBox1.Text.ToString());
            DateTime dt = DateTime.Today;
            try
            {
                String insert1 = "update  GroupEvaluation set  GroupId='" + IDD + "',EvaluationId='" + idd + "',ObtainedMarks='" + marks + "',EvaluationDate='" + dt + "' WHERE  GroupId = '" + IDD + "' AND EvaluationId='" + idd + "'";
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
                GroupEvaluation ss = new GroupEvaluation();
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
