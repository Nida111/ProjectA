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
    public partial class AssignProjectDetails : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-VFGDRDF\NIDA;Initial Catalog=ProjectA;Integrated Security=True");
        public AssignProjectDetails()
        {
            
            InitializeComponent();
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);
            conn.Open();
            String cmd = "SELECT  Project.[Title] as Title, Project.Description as Description, CONCAT(Person.FirstName, ' ', Person.LastName) AS Name ,(SELECT Value FROM Lookup WHERE Category = 'ADVISOR_ROLE' AND Id = AdvisorRole ) AS [Advisor Role] , (SELECT Value FROM Lookup WHERE Category = 'DESIGNATION' AND Id = Designation) AS [Desgination] ,ProjectAdvisor.AssignmentDate AS [Assignment Date] FROM [dbo].[Project] JOIN [dbo].[ProjectAdvisor] ON Project.Id = ProjectAdvisor.ProjectId JOIN [dbo].[Advisor] ON  ProjectAdvisor.AdvisorId = Advisor.Id  JOIN [dbo].[Person] ON Advisor.Id = Person.Id";

            SqlCommand command = new SqlCommand(cmd, conn);

            {
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = command;
                DataTable dbadataset = new DataTable();
                sda.Fill(dbadataset);
                BindingSource bsource = new BindingSource();
                bsource.DataSource = dbadataset;
                dataGridView1.DataSource = bsource;
                sda.Update(dbadataset);
                conn.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AssignProjectDetails_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            assignproject f = new assignproject();
            f.Show();
        }
    }
}
