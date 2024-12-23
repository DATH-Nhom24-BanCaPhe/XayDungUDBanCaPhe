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
using COMExcel = Microsoft.Office.Interop.Excel;

namespace XayDungUDBanCaPhe
{
    public partial class FormLapHD : Form
    {
        SqlConnection connection;
        SqlCommand command;
        string str = "Data Source = TRUCLY; Initial Catalog = csdl; Integrated Security = True;";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        DataTable dt = new DataTable();
        private List<QuanLyHĐ> qlhd = new List<QuanLyHĐ>();
        private List<QuanLyCTHĐ> qlct = new List<QuanLyCTHĐ>();

        void LoadCBMaNV()
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = " select maNV from nhanVien where viTriLamViec=N'Thu ngân' or viTriLamViec=N'Quản Lý'";
            SqlDataAdapter comboBoxAdapter = new SqlDataAdapter(command);
            DataTable comboBoxTable = new DataTable();
            comboBoxAdapter.Fill(comboBoxTable);
            cbMaNV.DataSource = comboBoxTable;
            cbMaNV.DisplayMember = "maNV";
            cbMaNV.ValueMember = "maNV";
        }
        void loadCBTenNuoc()
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = " select maNuoc,tenNuoc from doUong";
            SqlDataAdapter comboBoxAdapter = new SqlDataAdapter(command);
            DataTable comboBoxTable = new DataTable();
            comboBoxAdapter.Fill(comboBoxTable);
            cbTenNuoc.DataSource = comboBoxTable;
            cbTenNuoc.DisplayMember = "tenNuoc";
            cbTenNuoc.ValueMember = "maNuoc";
        }

        public FormLapHD()
        {
            InitializeComponent();
            btn_NewHĐ.Click += new EventHandler(btn_NewHĐ_Click);
            cbTenNuoc.SelectedIndexChanged += new EventHandler(cbTenNuoc_SelectedIndexChanged);
        }
        QuanLyHĐ hd = new QuanLyHĐ();
        private double LayGiatheoMaNuoc(string maNuoc)
        {
            double gia = 0;
            string query = "SELECT gia FROM doUong WHERE maNuoc = @maNuoc";

            using (SqlConnection connection = new SqlConnection(str))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maNuoc", maNuoc);
                    object result = command.ExecuteScalar();
                    connection.Close();
                    gia = result != null ? Convert.ToDouble(result) : 0;
                }
            }

            return gia;
        }
        private string LayMaNuocheoTenNuoc(string tenNuoc)
        {
            string maNuoc = String.Empty;
            string query = "SELECT maNuoc FROM doUong WHERE tenNuoc = @tenNuoc";

            using (SqlConnection connection = new SqlConnection(str))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tenNuoc", tenNuoc);
                    object result = command.ExecuteScalar();
                    connection.Close(); // Đóng kết nối sau khi xong

                    maNuoc = result != null ? result.ToString() : string.Empty;
                }

            }
            return maNuoc;
        }

        private void LuuHoaDonVaChiTiet()
        {
            try
            {
                string maHD = txtMaHD.Text;
                DateTime ngayLapHD = dtNgayLapHD.Value;
                double tongTien = dgvCTHĐ.Rows.Cast<DataGridViewRow>()
                    .Where(row => !row.IsNewRow)
                    .Sum(row => Convert.ToDouble(row.Cells["ThanhTienHD"].Value));
                string maNV = cbMaNV.Text;

                // Lưu vào bảng hoaDon(chỉ một dòng cho mỗi mã hóa đơn)
                string queryHD = "INSERT INTO hoaDon(maHD, ngayLapHD, maNV, tongTien) VALUES(@maHD, @ngayLapHD, @maNV, @tongTien)";
                using (SqlCommand commandHD = new SqlCommand(queryHD, connection))
                {
                    commandHD.Parameters.AddWithValue("@maHD", maHD);
                    commandHD.Parameters.AddWithValue("@ngayLapHD", ngayLapHD);
                    commandHD.Parameters.AddWithValue("@maNV", maNV);
                    commandHD.Parameters.AddWithValue("@tongTien", tongTien);
                    commandHD.ExecuteNonQuery();
                }
                //  Lưu vào bảng CTHĐ(lặp qua tất cả các dòng trong dgvCTHĐ)
                foreach (DataGridViewRow row in dgvCTHĐ.Rows)
                {
                    if (row.IsNewRow) continue; // Bỏ qua dòng trống

                    string maNuoc = row.Cells["MaNuoc"].Value?.ToString();
                    string tenNuoc = row.Cells["tenNuoc"].Value?.ToString();
                    int soLuong = Convert.ToInt32(row.Cells["SoLuong"].Value);
                    double gia = Convert.ToDouble(row.Cells["Gia"].Value);
                    double thanhTien = soLuong * gia;

                    string queryCTHD = "INSERT INTO CTHĐ(maHD, maNuoc, tenNuoc, soLuong, gia, thanhTien) VALUES(@maHD, @maNuoc, @tenNuoc, @soLuong, @gia, @thanhTien)";
                    using (SqlCommand commandCTHD = new SqlCommand(queryCTHD, connection))
                    {
                        commandCTHD.Parameters.AddWithValue("@maHD", txtMaHD.Text);
                        commandCTHD.Parameters.AddWithValue("@maNuoc", maNuoc);
                        commandCTHD.Parameters.AddWithValue("@tenNuoc", tenNuoc);
                        commandCTHD.Parameters.AddWithValue("@soLuong", soLuong);
                        commandCTHD.Parameters.AddWithValue("@gia", gia);
                        commandCTHD.Parameters.AddWithValue("@thanhTien", thanhTien);
                        commandCTHD.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Lưu hóa đơn và chi tiết hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maHD = txtMaHD.Text;
                string maNuoc = txtMaNuoc.Text;
                foreach (DataGridViewRow row in dgvCTHĐ.Rows)
                {
                    if (row.Cells["MaNuoc"].Value.ToString() == maNuoc)
                    {
                        dgvCTHĐ.Rows.Remove(row);
                        break;
                    }
                }
                double tongTien = 0;
                foreach (DataGridViewRow row in dgvCTHĐ.Rows)
                {
                    if (row.Cells["ThanhTienHD"].Value != null &&
                        double.TryParse(row.Cells["ThanhTienHD"].Value.ToString(), out double thanhTienRow))
                    {
                        tongTien += thanhTienRow;
                    }
                }
                string updateTongTienQuery = "UPDATE hoaDon SET tongTien = @tongTien WHERE maHD = @maHD";
                using (SqlCommand updateCommand = new SqlCommand(updateTongTienQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@tongTien", tongTien);
                    updateCommand.Parameters.AddWithValue("@maHD", maHD);
                    updateCommand.ExecuteNonQuery();
                }
                txtTongTien.Text = tongTien.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                if (dgvCTHĐ.Rows.Count == 0)
                {
                    txtMaHD.Clear();
                    txtMaNuoc.Clear();
                    cbMaNV.SelectedIndex = -1;
                    cbTenNuoc.SelectedIndex = -1;
                    nbSoLuong.Value = 0;
                    txtTongTien.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi xóa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txtMaHD.Text = hd.maTuTangMaHD();
            txtMaHD.Enabled = false;
            dtNgayLapHD.Value = DateTime.Now;
            txtMaNuoc.Clear();
            cbTenNuoc.SelectedIndex = -1;
            nbSoLuong.Value = 0;
            txtTongTien.Clear();
        }
        private bool isMaNVSelected = false;
        private void btn_Them_Click(object sender, EventArgs e)
        {
            try
            {
                string maHD = txtMaHD.Text;
                DateTime ngayLapHD = dtNgayLapHD.Value;
                if (dtNgayLapHD.Value.Date < DateTime.Today)
                {
                    MessageBox.Show("Ngày lập hóa đơn không được nhỏ hơn ngày hiện tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string maNV = cbMaNV.Text;
                if (string.IsNullOrEmpty(cbMaNV.Text))
                {
                    MessageBox.Show("Vui lòng thu ngân tạo hóa đơn nhập mã nhân viên của mình!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!isMaNVSelected)
                {
                    isMaNVSelected = true;
                    cbMaNV.Enabled = false;
                }
                string tenNuoc = cbTenNuoc.Text;
                if (string.IsNullOrEmpty(tenNuoc))
                {
                    MessageBox.Show("Vui lòng chọn tên nước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string maNuoc = LayMaNuocheoTenNuoc(tenNuoc);
                int soLuong = Convert.ToInt32(nbSoLuong.Value);
                if (soLuong <= 0)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                double gia = LayGiatheoMaNuoc(maNuoc);
                double thanhTien = gia * soLuong;
                dgvCTHĐ.Rows.Add(maNuoc, tenNuoc, soLuong, gia, thanhTien);
                double tongTien = 0;
                foreach (DataGridViewRow row in dgvCTHĐ.Rows)
                {
                    if (row.Cells["ThanhTienHD"].Value != null &&
                        double.TryParse(row.Cells["ThanhTienHD"].Value.ToString(), out double thanhTienRow))
                    {
                        tongTien += thanhTienRow;
                    }
                }
                txtTongTien.Text = tongTien.ToString();
                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txtMaNuoc.Clear();
            cbTenNuoc.SelectedIndex = -1;
            nbSoLuong.Value = 0;
        }

        private void btn_InHD_Click(object sender, EventArgs e)
        {
            // Khởi động chương trình Excel
            COMExcel.Application exApp = new COMExcel.Application();
            COMExcel.Workbook exBook; //Trong 1 chương trình Excel có nhiều Workbook
            COMExcel.Worksheet exSheet; //Trong 1 Workbook có nhiều Worksheet
            COMExcel.Range exRange;
            exBook = exApp.Workbooks.Add(COMExcel.XlWBATemplate.xlWBATWorksheet);
            exSheet = exBook.Worksheets[1];
            // Định dạng chung
            exRange = exSheet.Cells[1, 1];
            exRange.Range["A1:Z300"].Font.Name = "Times new roman"; //Font chữ
            exRange.Range["C2:E2"].Font.Size = 16;
            exRange.Range["C2:E2"].Font.Bold = true;
            exRange.Range["C2:E2"].Font.ColorIndex = 3; //Màu đỏ
            exRange.Range["C2:E2"].MergeCells = true;
            exRange.Range["C2:E2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C2:E2"].Value = "HÓA ĐƠN THANH TOÁN";
            // Biểu diễn thông tin chung của hóa đơn bán
            exRange.Range["B6:C9"].Font.Size = 12;
            exRange.Range["B6:B6"].Value = "Mã hóa đơn:";
            exRange.Range["C6:E6"].MergeCells = true;
            exRange.Range["C6:E6"].Value = txtMaHD.Text;
            exRange.Range["B7:B7"].Value = "Ngày lập hóa đơn:";
            exRange.Range["C7:E7"].MergeCells = true;
            exRange.Range["C7:E7"].Value = dtNgayLapHD.Value.ToString("dd/MM/yyyy");
            exRange.Range["B8:B8"].Value = "Mã nhân viên:";
            exRange.Range["C8:E8"].MergeCells = true;
            exRange.Range["C8:E8"].Value = cbMaNV.Text;
            //Lấy thông tin các mặt hàng
            //Tạo dòng tiêu đề bảng
            exRange.Range["A11:F11"].Font.Bold = true;
            exRange.Range["A11:F11"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C11:F11"].ColumnWidth = 12;
            exRange.Range["A11:A11"].Value = "STT";
            exRange.Range["B11:B11"].Value = "Tên sản phẩm";
            exRange.Range["C11:C11"].Value = "Số lượng";
            exRange.Range["D11:D11"].Value = "Đơn giá";
            exRange.Range["E11:E11"].Value = "Thành Tiền";

            int hang = 0;
            for (hang = 0; hang < dgvCTHĐ.Rows.Count; hang++)
            {
                //Điền số thứ tự vào cột 1 từ dòng 12
                exSheet.Cells[1][hang + 12] = hang + 1;

            }
            int hang1 = 0;
            foreach (DataGridViewRow dgvRow in dgvCTHĐ.Rows)
            {

                //Điền thông tin hàng từ cột thứ 2, dòng 12

                exSheet.Cells[2][hang1 + 12] = dgvRow.Cells["tenNuoc"].Value;
                exSheet.Cells[3][hang1 + 12] = dgvRow.Cells["SoLuong"].Value;
                exSheet.Cells[4][hang1 + 12] = dgvRow.Cells["Gia"].Value;
                exSheet.Cells[5][hang1 + 12] = dgvRow.Cells["ThanhTienHD"].Value;
                hang1++;
            }
            exRange = exSheet.Cells[4][hang + 14];
            exRange.Font.Bold = true;
            exRange.Value2 = "Tổng tiền:";
            exRange = exSheet.Cells[4 + 1][hang + 14];
            exRange.Font.Bold = true;

            //Điền thông tin hàng từ cột thứ 2, dòng 12
            exRange.Value2 = txtTongTien.Text;


            exSheet.Name = "Hóa đơn thanh toán";
            exApp.Visible = true;
        }

        private void btn_NewHĐ_Click(object sender, EventArgs e)
        {
            if (dgvCTHĐ.Rows.Count > 1)
            {
                LuuHoaDonVaChiTiet();
            }
            // string tiento = "HD";

            txtMaHD.Text = hd.maTuTangMaHD();
            txtMaHD.Enabled = false;
            dgvCTHĐ.Rows.Clear();
            dtNgayLapHD.Value = DateTime.Now;
            cbMaNV.Enabled = true;
            cbMaNV.SelectedIndex = -1;
            cbTenNuoc.SelectedIndex = -1;
            nbSoLuong.Value = 0;
            txtTongTien.Clear();
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            DialogResult ketqua = MessageBox.Show("Bạn muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ketqua == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dgvCTHĐ_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaNuoc.Text = dgvCTHĐ.Rows[e.RowIndex].Cells[0].Value.ToString();
            cbTenNuoc.Text = dgvCTHĐ.Rows[e.RowIndex].Cells[1].Value.ToString();
            nbSoLuong.Value = Convert.ToInt32(dgvCTHĐ.Rows[e.RowIndex].Cells[2].Value);
        }

        private void FormLapHD_Load(object sender, EventArgs e)
        {
            txtMaHD.Text = hd.maTuTangMaHD();
            connection = new SqlConnection(str);
            connection.Open();
            LoadCBMaNV();
            loadCBTenNuoc();
            txtMaHD.Enabled = false;
            dtNgayLapHD.Value = DateTime.Now;
            cbMaNV.SelectedIndex = -1;
            txtMaNuoc.Clear();
            cbTenNuoc.SelectedIndex = -1;
            nbSoLuong.Value = 0;
            txtTongTien.Clear();
            dgvCTHĐ.Rows.Clear();
            txtTongTien.Clear();
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCTHĐ.CurrentRow == null || dgvCTHĐ.CurrentRow.IsNewRow)
                {
                    MessageBox.Show("Vui lòng chọn dòng cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string maHD = txtMaHD.Text;
                DateTime ngayLapHD = dtNgayLapHD.Value;
                string maNV = cbMaNV.Text;
                string tenNuoc = cbTenNuoc.Text;
                string maNuoc = LayMaNuocheoTenNuoc(tenNuoc);
                int soLuong = Convert.ToInt32(nbSoLuong.Value);
                if (soLuong <= 0)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                double gia = LayGiatheoMaNuoc(maNuoc);
                double thanhTien = gia * soLuong;
                string query = "UPDATE CTHĐ SET soLuong = @soLuong, thanhTien = @thanhTien WHERE maHD = @maHD AND maNuoc = @maNuoc";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maHD", maHD);
                    command.Parameters.AddWithValue("@maNuoc", maNuoc);
                    command.Parameters.AddWithValue("@soLuong", soLuong);
                    command.Parameters.AddWithValue("@thanhTien", thanhTien);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật chi tiết hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK);
                }
                DataGridViewRow selectedRow = dgvCTHĐ.CurrentRow;
                selectedRow.Cells["SoLuong"].Value = soLuong;
                selectedRow.Cells["Gia"].Value = gia;
                selectedRow.Cells["ThanhTienHD"].Value = thanhTien;
                double tongTien = 0;
                foreach (DataGridViewRow row in dgvCTHĐ.Rows)
                {
                    if (row.Cells["ThanhTienHD"].Value != null &&
                        double.TryParse(row.Cells["ThanhTienHD"].Value.ToString(), out double thanhTienRow))
                    {
                        tongTien += thanhTienRow;
                    }
                }
                string updateTongTienQuery = "UPDATE hoaDon SET tongTien = @tongTien WHERE maHD = @maHD";
                using (SqlCommand updateCommand = new SqlCommand(updateTongTienQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@tongTien", tongTien);
                    updateCommand.Parameters.AddWithValue("@maHD", maHD);
                    updateCommand.ExecuteNonQuery();
                }
                txtTongTien.Text = tongTien.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            { MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void cbTenNuoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem ComboBox có dữ liệu và giá trị được chọn
                if (cbTenNuoc.SelectedItem != null && cbTenNuoc.SelectedItem is DataRowView drv)
                {
                    string maNuoc = drv["maNuoc"].ToString();
                    txtMaNuoc.Text = maNuoc; // Hiển thị mã nước vào TextBox
                }
                else
                {
                    txtMaNuoc.Clear(); // Xóa mã nước nếu không chọn
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi chọn nước: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
