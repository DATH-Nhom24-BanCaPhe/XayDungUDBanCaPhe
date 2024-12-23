using XayDungUDBanCaPhe;
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
    public partial class FormTK : Form
    {
        SqlConnection connection;
        SqlCommand command;
        string str = "Data Source = TRUCLY; Initial Catalog = csdl; Integrated Security = True;";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        private List<QuanLyThongKe> qltk = new List<QuanLyThongKe>();
        private List<QuanLyHĐ> qlhd = new List<QuanLyHĐ>();

        void loadTK()
        {
            SqlCommand command = new SqlCommand("select * from thongKe", connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvThongKe.DataSource = dt;
        }
        void loaddataHD()
        {
            SqlCommand command = new SqlCommand("select * from hoaDon", connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            da.Fill(table);
            dgvThongKe.DataSource = table;
        }
        public FormTK()
        {
            InitializeComponent();
        }
        private void LoadMaNV()
        {
            // Tạo kết nối đến cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(str))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT maNV from nhanVien where viTriLamViec=N'Nhân viên thống kê'";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Thực thi truy vấn và lấy dữ liệu
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        txtMaNV.Text = result.ToString(); // Gán mã nhân viên vào TextBox
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy mã nhân viên.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private int demSLHĐTong(DateTime ngayBD, DateTime ngayKT)
        {
            int soLuong = 0;
            SqlConnection connection = new SqlConnection(str);
            connection.Open();
            string query = "select count(maHD)  from hoaDon where  ngayLapHD between @ngayBD and @ngayKT";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ngayBD", ngayBD.Date);
            command.Parameters.AddWithValue("@ngayKT", ngayKT.Date);
            object result = command.ExecuteScalar();
            if (result != null)
            {
                soLuong = Convert.ToInt32(result);
            }
            return soLuong;
        }
        private int DemSLHĐ(DateTime ngayTK)
        {
            int soluongHĐ = 0;
            using (SqlConnection connection = new SqlConnection(str))
            {
                try
                {
                    connection.Open();
                    // Sử dụng câu lệnh SELECT đúng để đếm số lượng hóa đơn theo ngày thống kê
                    string query = @"SELECT COUNT(maHD) 
                             FROM hoaDon 
                             WHERE CAST(ngayLapHD AS DATE) = @ngayTK"; // Lọc hóa đơn theo ngày thống kê
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        // Thêm tham số @ngayTK vào câu lệnh SQL
                        command.Parameters.AddWithValue("@ngayTK", ngayTK.Date);
                        object result = command.ExecuteScalar(); // Thực thi câu lệnh và lấy kết quả
                        if (result != DBNull.Value && result != null)
                        {
                            soluongHĐ = Convert.ToInt32(result); // Gán số lượng hóa đơn vào biến soluongHĐ
                        }
                        else
                        {
                            MessageBox.Show("Không có dữ liệu cho ngày này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        txtSLHĐ.Text = soluongHĐ.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            return soluongHĐ;
        }
        private double TinhtongTienTK(DateTime ngayTK)
        {
            double tongTienTK = 0;
            using (SqlConnection connection = new SqlConnection(str))
            {
                try
                {
                    connection.Open();
                    // Câu lệnh SQL để tính tổng tiền cho ngày thống kê
                    string query = @"SELECT SUM(tongTien) 
                             FROM hoaDon 
                             WHERE CAST(ngayLapHD AS DATE) = @ngayTK"; // Lọc theo ngày thống kê
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ngayTK", ngayTK.Date); // Thêm tham số ngày thống kê vào câu lệnh SQL
                        object result = command.ExecuteScalar(); // Thực thi câu lệnh và lấy kết quả
                        if (result != DBNull.Value && result != null)
                        {
                            tongTienTK = Convert.ToDouble(result); // Gán tổng tiền vào biến tongTienTKTK
                        }
                        else
                        {
                            MessageBox.Show("Không có dữ liệu cho ngày này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        txtTongTienTK.Text = tongTienTK.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return tongTienTK; // Trả về tổng tiền tính được
        }
        private double tongTienTKTong(DateTime ngayBD, DateTime ngayKT)
        {
            double tong = 0; ;
            SqlConnection connection = new SqlConnection(str);
            connection.Open();
            string query = "select count(maHD) from hoaDon where  ngayLapHD between @ngayBD and @ngayKT";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ngayBD", ngayBD.Date);
            command.Parameters.AddWithValue("@ngayKT", ngayKT.Date);
            object result = command.ExecuteScalar();
            if (result != DBNull.Value && result != null)
            {
                tong = Convert.ToDouble(result);
            }
            return tong;
        }
        private void btn_Them_Click(object sender, EventArgs e)
        {
            DateTime ngayTK = dtNgayTK.Value;
            int soluongHĐ = DemSLHĐ(ngayTK);
            double tongTienTK = TinhtongTienTK(ngayTK);
            string maNV = txtMaNV.Text;
            DanhSachThongKe ds = new DanhSachThongKe(connection);
            QuanLyThongKe tk = new QuanLyThongKe(ngayTK, soluongHĐ, tongTienTK, maNV);
            ds.Them(tk);
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            DateTime ngayTK = dtNgayTK.Value;
            DanhSachThongKe ds = new DanhSachThongKe(connection);
            ds.Xoa(ngayTK);
            //loadTK();
            dtNgayTK.Value = DateTime.Now;
            txtSLHĐ.Clear();
            txtTongTienTK.Clear();
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
            exRange.Range["C2:E2"].Value = "THỐNG KÊ";
            // Biểu diễn thông tin chung của hóa đơn bán
            exRange.Range["B6:C9"].Font.Size = 12;
            exRange.Range["B6:B6"].Value = "Thời gian thống kê:";
            // exRange.Range["C6:C6"].NumberFormat.Name = "Date";
            exRange.Range["C6:C6"].Value = dtNgayTK.Value.ToString("dd/MM/yyyy");
            exRange.Range["B7:B7"].Value = "Thời gian bắt đầu:";
            exRange.Range["C7:C7"].Value = dtNgayBD.Value.ToString("dd/MM/yyyy");
            exRange.Range["B8:B8"].Value = "Thời gian kết thúc:";
            exRange.Range["C8:C8"].Value = dtNgayKT.Value.ToString("dd/MM/yyyy");
            exRange.Range["B9:B9"].Value = "Mã nhân viên lập thống kê:";
            exRange.Range["C9:C9"].Value = txtMaNV.Text;
            //Tạo dòng tiêu đề bảng
            exRange.Range["A11:F11"].Font.Bold = true;
            exRange.Range["A11:F11"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C11:F11"].ColumnWidth = 12;
            exRange.Range["A11:A11"].Value = "STT";
            exRange.Range["B11:B11"].Value = "Mã hóa đơn";
            exRange.Range["C11:C11"].Value = "Ngày lập hóa đơn";
            exRange.Range["D11:D11"].Value = "Tổng tiền hóa đơn";
            exRange.Range["E11:E11"].Value = "Mã nhân viên lập hóa đơn";
            int hang = 0;
            for (hang = 0; hang < dgvThongKe.Rows.Count; hang++)
            {
                //Điền số thứ tự vào cột 1 từ dòng 12
                exSheet.Cells[1][hang + 12] = hang + 1;

            }
            int hang1 = 0;
            foreach (DataGridViewRow dgvRow in dgvThongKe.Rows)
            {

                //Điền thông tin hàng từ cột thứ 2, dòng 12

                exSheet.Cells[2][hang1 + 12] = dgvRow.Cells["MaHD"].Value;
                exSheet.Cells[3][hang1 + 12] = dgvRow.Cells["NgayLapHoaDon"].Value;
                exSheet.Cells[3][hang1 + 12].NumberFormat = "dd/mm/yyyy";
                exSheet.Cells[4][hang1 + 12] = dgvRow.Cells["TongTienHD"].Value;
                exSheet.Cells[5][hang1 + 12] = dgvRow.Cells["MaNVLapHD"].Value;
                hang1++;
            }
            exRange = exSheet.Cells[4][hang + 14];
            exRange.Font.Bold = true;
            exRange.Value2 = "Số lượng hóa đơn:";
            exRange = exSheet.Cells[4 + 1][hang + 14];
            exRange.Font.Bold = true;
            // foreach (DataGridViewRow dgvRow in dgvThongKe.Rows)
            // {
            int soLuongHD = demSLHĐTong(dtNgayBD.Value.Date, dtNgayKT.Value.Date);

            exRange.Value2 = soLuongHD;

            // }
            exRange = exSheet.Cells[4][hang + 15];
            exRange.Font.Bold = true;
            exRange.Value2 = "Tổng tiền thống kê:";
            exRange = exSheet.Cells[4 + 1][hang + 15];
            exRange.Font.Bold = true;
            // foreach (DataGridViewRow dgvRow in dgvThongKe.Rows)
            // {
            double tong = tongTienTKTong(dtNgayBD.Value.Date, dtNgayKT.Value.Date);
            exRange.Value2 = tong;

            // }
            exSheet.Name = "Thống kê";
            exApp.Visible = true;
        }
        
        private void HienThiDanhSachHoaDon(DataGridView dgv, List<QuanLyHĐ> ds)
        {
            dgvThongKe.DataSource = ds.ToList();
        }
        private void btn_Tim_Click(object sender, EventArgs e)
        {
            DateTime ngayBD = dtNgayBD.Value;
            DateTime ngayKT = dtNgayKT.Value;
            DanhSachHoaDon ds = new DanhSachHoaDon(connection);
            List<QuanLyHĐ> kq = ds.XemHĐ(ngayBD, ngayKT);
            if (kq.Count > 0)
            {
                HienThiDanhSachHoaDon(dgvThongKe, kq);
            }
            else
            {
                MessageBox.Show("Không có hóa đơn nào trong khoảng thời gian này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            DialogResult ketqua = MessageBox.Show("Bạn muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ketqua == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void FormTK_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(str);
            connection.Open();
            LoadMaNV();
            //loadTK();
            dtNgayTK.Value = DateTime.Now;
            txtSLHĐ.Clear();
            txtTongTienTK.Clear();
        }
    }
}
