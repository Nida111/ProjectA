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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Rectangle = System.Drawing.Rectangle;

namespace ProjectA
{
    public partial class ProjectReport : Form
    {
        public ProjectReport()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-VFGDRDF\NIDA;Initial Catalog=ProjectA;Integrated Security=True");
            InitializeComponent();
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);
            conn.Open();
            String cmd = "Select  pr.Title  AS [PROJECT TITLE] ,s.RegistrationNo  AS [REGISTRATION NO], CONCAT(p.FirstName+ ' ', + p.LastName)  AS [ADVISOR NAME] FROM Student s JOIN GroupStudent gs ON s.Id = gs.StudentId JOIN [dbo].[Group] g ON gs.GroupId = g.Id JOIN GroupProject gp ON g.Id = gp.GroupId JOIN Project pr ON gp.ProjectId = pr.Id JOIN ProjectAdvisor pa ON pr.Id = pa.ProjectId JOIN Advisor a ON pa.AdvisorId = a.Id JOIN Person p ON a.Id = p.Id";
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
                this.dataGridView1.AllowUserToAddRows = false;
                conn.Close();
            }
        }

        private void ProjectReport_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 f1 = new Form3();
            f1.Show();
        }

        public void exportgridtopdf(DataGridView dqw, string filename)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            PdfPTable pdftable = new PdfPTable(dqw.Columns.Count);
            pdftable.DefaultCell.Padding = 3;
            pdftable.WidthPercentage = 100;
            pdftable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftable.DefaultCell.BorderWidth = 1;

            iTextSharp.text.Font text = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);
            //add header
            foreach (DataGridViewColumn column in dqw.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, text));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                pdftable.AddCell(cell);
            }

            //add datarow

            foreach (DataGridViewRow row in dqw.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    pdftable.AddCell(new Phrase(cell.Value.ToString(), text));
                }
            }
            var savefiledialoge = new SaveFileDialog();
            savefiledialoge.FileName = filename;
            savefiledialoge.DefaultExt = ".pdf";
            if (savefiledialoge.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefiledialoge.FileName, FileMode.Create))
                {
                    Document pdfdoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfdoc, stream);
                    pdfdoc.Open();
                    pdfdoc.Add(pdftable);
                    pdfdoc.Close();
                    stream.Close();
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            exportgridtopdf(dataGridView1, "Project_Report");
        }
    }
}
