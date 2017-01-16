using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sokoban_solver
{
    public partial class dimDialog : Form
    {
        public dimDialog()
        {
            InitializeComponent();
        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Form1 p = (Form1)this.Owner;
                p.indexEnd.X = Convert.ToInt16(maskedTextBox1.Text);
                p.indexEnd.Y = Convert.ToInt16(maskedTextBox2.Text);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(this,ex.Message);
            }
        }
    }
}
