using QuanLyBanCaPhe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XayDungUDBanCaPhe
{
    public partial class FormNV : Form
    {
        SqlConnection connection;
        SqlCommand command;
        string str = "Data Source = TRUCLY; Initial Catalog = csdl; Integrated Security = True;";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();

        private List<QuanLyNhanVien> dsNhanVien = new List<QuanLyNhanVien>();


        public FormNV()
        {
            InitializeComponent();
            cbChucVu.Items.Add("Quản lý");
            cbChucVu.Items.Add("Nhân viên vệ sinh");
            cbChucVu.Items.Add("Thu ngân");
            cbChucVu.Items.Add("Nhân viên pha chế");
            cbChucVu.Items.Add("Nhân viên phục vụ");
            cbChucVu.Items.Add("Bảo vệ");
            cbChucVu.Items.Add("Nhân viên thống kê");
        }
        QuanLyNhanVien nv = new QuanLyNhanVien();
        private bool IsValidPhoneNumber(string sĐT)
        {
            string sđt = @"^(0[1-9]{1}[0-9]{8})$";
            return Regex.IsMatch(sĐT, sđt);
        }
        private void btn_Them_Click(object sender, EventArgs e)
        {
            DanhSachNhanVien ds = new DanhSachNhanVien(connection);
            txtMaNV.Enabled = false;
            if (ds.tinhTuoi(dtNgaySinh.Value) < 18)
            {
                MessageBox.Show("Không đủ tuổi .Vui lòng nhập lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!IsValidPhoneNumber(txtSĐT.Text))
            {
                MessageBox.Show("Số điện thoại không đúng định dạng.Vui lòng nhập lại!", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            string viTriLamViec = cbChucVu.SelectedItem.ToString();
            string gioiTinh = null;
            if (radNam.Checked == true)
                gioiTinh = "Nam";
            else if (radNu.Checked == true)
                gioiTinh = "Nữ";
            else
            {
                MessageBox.Show("Vui lòng chọn giới tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //return;
            }
            QuanLyNhanVien nv = new QuanLyNhanVien(txtMaNV.Text, txtHoTen.Text, dtNgaySinh.Value, txtSĐT.Text, dtNgayVaoLam.Value, viTriLamViec, gioiTinh);
            ds.Them(nv);
            loadnv();

            txtMaNV.Text = nv.maTuTangMaNV();
            txtHoTen.Clear();
            dtNgaySinh.Value = DateTime.Now;
            txtSĐT.Clear();
            dtNgaySinh.Value = DateTime.Now;
            cbChucVu.SelectedIndex = -1;
            radNam.Checked = false;
            radNu.Checked = false;
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            string iD = txtMaNV.Text;
            DanhSachNhanVien ds = new DanhSachNhanVien(connection);
            ds.Xoa(iD);
            loadnv();

            txtHoTen.Clear();
            dtNgaySinh.Value = DateTime.Now;
            txtSĐT.Clear();
            dtNgayVaoLam.Value = DateTime.Now;
            cbChucVu.SelectedIndex = -1;
            radNam.Checked = false;
            radNu.Checked = false;
        }
        public void loadnv()
        {
            SqlCommand command = new SqlCommand("select * from nhanVien", connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvNhanVien.DataSource = dt;
        }
        private void HienThiDanhSachNhanVien(DataGridView dgv, List<QuanLyNhanVien> ds)
        {
            dgv.DataSource = ds;

        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {

            DanhSachNhanVien ds = new DanhSachNhanVien(connection);
            txtMaNV.Enabled = false;
            if (ds.tinhTuoi(dtNgaySinh.Value) < 18)
            {
                MessageBox.Show("Không đủ tuổi .Vui lòng nhập lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!IsValidPhoneNumber(txtSĐT.Text))
            {
                MessageBox.Show("Số điện thoại không đúng định dạng.Vui lòng nhập lại!", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            string gioiTinh = null;
            if (radNam.Checked == true)
                gioiTinh = "Nam";
            else if (radNu.Checked == true)
                gioiTinh = "Nữ";
            else
            {
                MessageBox.Show("Vui lòng chọn giới tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            QuanLyNhanVien nv = new QuanLyNhanVien(txtMaNV.Text, txtHoTen.Text, dtNgaySinh.Value, txtSĐT.Text, dtNgayVaoLam.Value, cbChucVu.SelectedItem.ToString(), gioiTinh);
            ds.Sua(nv);
            loadnv();
            txtMaNV.Text = nv.maTuTangMaNV();
            txtHoTen.Clear();
            dtNgaySinh.Value = DateTime.Now;
            txtSĐT.Clear();
            dtNgayVaoLam.Value = DateTime.Now;
            cbChucVu.SelectedIndex = -1;
            radNam.Checked = false;
            radNu.Checked = false;
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            DialogResult ketQua = MessageBox.Show("Bạn muốn thoát!", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ketQua == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btn_Tim_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimMa.Text) && string.IsNullOrWhiteSpace(txtTimTen.Text))
            {
                MessageBox.Show("Vui lòng nhập mã hoặc tên nhân viên để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (radTimMa.Checked)
            {
                DanhSachNhanVien ds = new DanhSachNhanVien(connection);
                List<QuanLyNhanVien> ketqua = ds.TimTheoMa(txtTimMa.Text.Trim());
                if (ketqua.Count > 0)
                {
                    HienThiDanhSachNhanVien(dgvNhanVien, ketqua);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên với mã này!", "Thông báo", MessageBoxButtons.OK);
                }
            }
            else if (radTimTen.Checked)
            {
                DanhSachNhanVien ds = new DanhSachNhanVien(connection);
                List<QuanLyNhanVien> ketqua = ds.TimTheoTen(txtTimTen.Text);
                if (ketqua.Count > 0)
                {
                    HienThiDanhSachNhanVien(dgvNhanVien, ketqua);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên với tên này!", "Thông báo", MessageBoxButtons.OK);
                }
            }
            txtMaNV.Text = nv.maTuTangMaNV();
            txtHoTen.Clear();
            dtNgaySinh.Value = DateTime.Now;
            txtSĐT.Clear();
            dtNgayVaoLam.Value = DateTime.Now;
            cbChucVu.SelectedIndex = -1;
            radNam.Checked = false;
            radNu.Checked = false;
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dgvNhanVien.CurrentRow.Index;
            txtMaNV.Text = dgvNhanVien.Rows[i].Cells[0].Value.ToString();
            txtHoTen.Text = dgvNhanVien.Rows[i].Cells[1].Value.ToString();
            dtNgaySinh.Text = Convert.ToString(dgvNhanVien.Rows[i].Cells[2].Value);
            txtSĐT.Text = dgvNhanVien.Rows[i].Cells[3].Value.ToString();
            dtNgayVaoLam.Text = (Convert.ToDateTime(dgvNhanVien.Rows[i].Cells[4].Value)).ToString();
            cbChucVu.Text = dgvNhanVien.Rows[i].Cells[5].Value.ToString();
            string gioiTinh = dgvNhanVien.Rows[i].Cells[6].Value.ToString();
            gbGioiTinh.Text = gioiTinh;
            if (gioiTinh == "Nam")
            {
                radNam.Checked = true;
            }
            else
                radNu.Checked = true;
        }

        private void FormNV_Load(object sender, EventArgs e)
        {
            txtMaNV.Text = nv.maTuTangMaNV();
            txtMaNV.Enabled = false;
            connection = new SqlConnection(str);
            connection.Open();
            loadnv();
        }
    }
}
