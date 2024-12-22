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
    public partial class FormSP : Form
    {
        SqlConnection connection;
        SqlCommand command;
        string str = "Data Source = TRUCLY; Initial Catalog = csdl; Integrated Security = True;";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        private List<QuanLySanPham> qldu = new List<QuanLySanPham>();
        void loaddata()
        {
            command = connection.CreateCommand();
            command.CommandText = "select * from doUong";
            adapter.SelectCommand = command;
            table.Clear();
            adapter.Fill(table);
            dgvDoUong.DataSource = table;

        }
        void LoadComboBox()
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT maLoai FROM LoaiNuoc";
                SqlDataAdapter comboBoxAdapter = new SqlDataAdapter(command);
                DataTable comboBoxTable = new DataTable();
                comboBoxAdapter.Fill(comboBoxTable);

                cbMaLoai.DataSource = comboBoxTable;
                cbMaLoai.DisplayMember = "maLoai";
                cbMaLoai.ValueMember = "maLoai";
            }
        }
        public FormSP()
        {
            InitializeComponent();
        }

        private void btn_Them_Click(object sender, EventArgs e)
        {

            DanhSachSanPham ds = new DanhSachSanPham(connection);
            QuanLySanPham sp = new QuanLySanPham(cbMaLoai.SelectedValue.ToString(), txtMaNuoc.Text, txtTenNuoc.Text, double.Parse(txtGia.Text));
            ds.Them(sp);
            LoadComboBox();
            loaddata();

            cbMaLoai.SelectedIndex = -1;
            txtMaNuoc.Clear();
            txtTenNuoc.Clear();
            txtGia.Clear();
        }

        private void btn_oa_Click(object sender, EventArgs e)
        {
            DanhSachSanPham ds = new DanhSachSanPham(connection);
            string maNuoc = txtMaNuoc.Text;
            ds.Xoa(maNuoc);
            LoadComboBox();
            loaddata();

            cbMaLoai.SelectedIndex = -1;
            txtMaNuoc.Clear();
            txtTenNuoc.Clear();
            txtGia.Clear();
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {

            DanhSachSanPham ds = new DanhSachSanPham(connection);
            QuanLySanPham sp = new QuanLySanPham(cbMaLoai.SelectedValue.ToString(), txtMaNuoc.Text, txtTenNuoc.Text, double.Parse(txtGia.Text));
            ds.Sua(sp);
            LoadComboBox();
            loaddata();

            cbMaLoai.SelectedIndex = -1;
            txtMaNuoc.Clear();
            txtTenNuoc.Clear();
            txtGia.Clear();
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            DialogResult ketqua = MessageBox.Show("Bạn muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ketqua == DialogResult.Yes)
            {
                this.Close();
            }
        }
        private void HienThiDanhSachSanPham(DataGridView dgv, List<QuanLySanPham> ds)
        {
            dgv.DataSource = ds.ToList();
        }
        private void btn_Tim_Click(object sender, EventArgs e)
        {

            if (radTimMa.Checked)
            {
                DanhSachSanPham ds = new DanhSachSanPham(connection);
                List<QuanLySanPham> ketqua = ds.TimTheoMa(txtTimMa.Text);

                if (ketqua.Count > 0)
                {
                    HienThiDanhSachSanPham(dgvDoUong, ketqua);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy!", "Thông báo", MessageBoxButtons.OK);
                }
            }
            if (radTimTen.Checked)
            {
                DanhSachSanPham ds = new DanhSachSanPham(connection);
                List<QuanLySanPham> ketqua = ds.TimTheoTen(txtTimTen.Text);
                if (ketqua.Count > 0)
                {
                    HienThiDanhSachSanPham(dgvDoUong, ketqua);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy ", "Thông báo", MessageBoxButtons.OK);
                }
            }
            cbMaLoai.SelectedIndex = -1;
            txtMaNuoc.Clear();
            txtTenNuoc.Clear();
            txtGia.Clear();
        }

        private void FormSP_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(str);
            connection.Open();
            LoadComboBox();
            loaddata();
        }

        private void dgvDoUong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dgvDoUong.CurrentCell.RowIndex;
            cbMaLoai.Text = dgvDoUong.Rows[i].Cells[0].Value.ToString();
            txtMaNuoc.Text = dgvDoUong.Rows[i].Cells[1].Value.ToString();
            txtTenNuoc.Text = dgvDoUong.Rows[i].Cells[2].Value.ToString();
            txtGia.Text = dgvDoUong.Rows[i].Cells[3].Value.ToString();
        }
    }
}
