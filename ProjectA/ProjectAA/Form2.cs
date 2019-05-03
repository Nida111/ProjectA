using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectA
{
    public partial class Form2 : Form
    {
        int panelWidth;
        bool Hidden;
        public Form2()
        {
            InitializeComponent();
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);
            panelWidth = PanelSlide.Width;
            Hidden = false;

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PanelSlide_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {

            this.Hide();
            Evaluate ee = new Evaluate();
            ee.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Hide();
            GroupEvaluation ee = new GroupEvaluation();
            ee.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Hide();
            GroupProject ee = new GroupProject();
            ee.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Group ee = new Group();
            ee.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            this.Hide();
            assignproject ee = new assignproject();
            ee.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {

            this.Hide();
            advisor f1 = new advisor();
            f1.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {

            this.Hide();
            projectadd f1 = new projectadd();
            f1.Show();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            this.Hide();
            student f2 = new student();
            f2.Show();
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

        private void button5_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form1 f1 = new Form1();
            f1.Show();
        }
    }
}
