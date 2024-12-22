using QuanLyBanCaPhe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XayDungUDBanCaPhe
{
    public partial class FormLN : Form
    {
        SqlConnection connection;
        SqlCommand command;
        string str = "Data Source = TRUCLY; Initial Catalog = csdl; Integrated Security = True;";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        private List<QuanLyLoaiNuoc> qlln = new List<QuanLyLoaiNuoc>();

        void loaddata()
        {
            command = connection.CreateCommand();
            command.CommandText = "select * from LoaiNuoc";
            adapter.SelectCommand = command;
            table.Clear();
            adapter.Fill(table);
            dgvLoaiDoUong.DataSource = table;

        }

        public FormLN()
        {
            InitializeComponent();
        }
        private void HienThiDanhSachLoaiNuoc(DataGridView dgv, List<QuanLyLoaiNuoc> ln)
        {
            dgv.DataSource = ln.ToList();


        }
        private void btn_Them_Click(object sender, EventArgs e)
        {
            DanhSachLoaiNuoc ds = new DanhSachLoaiNuoc(connection);
            QuanLyLoaiNuoc ln = new QuanLyLoaiNuoc(txtMaLoai.Text, txtTenLoai.Text);
            ds.Them(ln);
            loaddata();
            txtMaLoai.Clear();
            txtTenLoai.Clear();
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            DanhSachLoaiNuoc ds = new DanhSachLoaiNuoc(connection);
            string maLoai = txtMaLoai.Text;
            ds.Xoa(maLoai);
            loaddata();
            txtMaLoai.Clear();
            txtTenLoai.Clear();
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            DanhSachLoaiNuoc ds = new DanhSachLoaiNuoc(connection);
            QuanLyLoaiNuoc ln = new QuanLyLoaiNuoc(txtMaLoai.Text, txtTenLoai.Text);
            ds.Sua(ln);
            loaddata();
            txtMaLoai.Clear();
            txtTenLoai.Clear();
        }

        private void btn_Tim_Click(object sender, EventArgs e)
        {
            if (radTimMa.Checked)
            {
                DanhSachLoaiNuoc ds = new DanhSachLoaiNuoc(connection);
                List<QuanLyLoaiNuoc> ketqua = ds.TimTheoMa(txtTimMa.Text);

                if (ketqua.Count > 0)
                {
                    HienThiDanhSachLoaiNuoc(dgvLoaiDoUong, ketqua);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy loại nước!", "Thông báo", MessageBoxButtons.OK);
                }
            }
            if (radTimTen.Checked)
            {
                DanhSachLoaiNuoc ds = new DanhSachLoaiNuoc(connection);
                List<QuanLyLoaiNuoc> ketqua = ds.TimTheoTen(txtTimTen.Text);

                if (ketqua.Count > 0)
                {
                    HienThiDanhSachLoaiNuoc(dgvLoaiDoUong, ketqua);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy loại nước!", "Thông báo", MessageBoxButtons.OK);
                }
            }
            txtMaLoai.Clear();
            txtTenLoai.Clear();
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            DialogResult ketqua = MessageBox.Show("Bạn muốn thoát !", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ketqua == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dgvLoaiDoUong_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            txtMaLoai.Text = dgvLoaiDoUong.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtTenLoai.Text = dgvLoaiDoUong.Rows[e.RowIndex].Cells[1].Value.ToString();
        }

        private void FormLN_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(str);
            connection.Open();
            loaddata();
        }
    }
}
