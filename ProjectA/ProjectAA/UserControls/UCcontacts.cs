using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkypeDemo.UserControls
{
    public partial class UCcontacts : UserControl
    {
        public UCcontacts()
        {
            InitializeComponent();
        }
        public Image ProfilePic
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }
        public string txtName
        {
            get { return labelName.Text; }
            set { labelName.Text = value; }
        }
        public string txtStatus
        {
            get { return labelStatus.Text; }
            set { labelStatus.Text = value; }
        }
    }
}
