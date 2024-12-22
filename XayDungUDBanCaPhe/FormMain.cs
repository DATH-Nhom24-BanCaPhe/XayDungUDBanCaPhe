using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XayDungUDBanCaPhe
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void quanLyNVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNV f = new FormNV();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void quảnLýLoạiSảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLN f = new FormLN();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void quảnLýSảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSP f = new FormSP();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void quảnLýHóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHD f = new FormHD();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void quảnLýThốngKêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTK f = new FormTK();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void quảnLýHóaĐơnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormLapHD f = new FormLapHD();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            DialogResult ketqua = MessageBox.Show("Bạn muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ketqua == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
