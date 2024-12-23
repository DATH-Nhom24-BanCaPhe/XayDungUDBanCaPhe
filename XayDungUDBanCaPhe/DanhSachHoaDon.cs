using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XayDungUDBanCaPhe
{
    internal class DanhSachHoaDon
    {
        private List<QuanLyCTHĐ> dsCTHĐ;
        private List<QuanLyHĐ> dsHD;
        private SqlConnection conn;
        public DanhSachHoaDon(SqlConnection conn)
        {
            this.dsCTHĐ = new List<QuanLyCTHĐ>();
            this.dsHD = new List<QuanLyHĐ>();
            this.conn = conn;
        }
        public DanhSachHoaDon(List<QuanLyHĐ> dsHD)
        {
            this.dsHD = dsHD;
        }
        public DanhSachHoaDon(List<QuanLyCTHĐ> dsCTHĐ)
        {
            this.dsCTHĐ = dsCTHĐ;
        }
        public List<QuanLyHĐ> DSHD
        {
            get { return this.dsHD; }
            set { this.dsHD = value; }
        }
        public bool Them(QuanLyHĐ hd)
        {
            string them = "insert into hoaDon(maHD,ngayLapHD,tongTien,maNV) values(@maHD,@ngayLapHD,@tongTien,@maNV)";
            SqlCommand command = new SqlCommand(them, conn);
            command.Parameters.AddWithValue("@maHD", hd.MaHD);
            command.Parameters.AddWithValue("@ngayLapHD", hd.NgayLapHoaDon);
            command.Parameters.AddWithValue("@tongTien", hd.TongTien);
            command.Parameters.AddWithValue("@maNV", hd.MaNV);
            command.ExecuteNonQuery();
            this.dsHD.Add(hd);
            return true;
        }
        public List<QuanLyHĐ> XemHĐ(DateTime ngayBD, DateTime ngayKT)
        {
            List<QuanLyHĐ> kq = new List<QuanLyHĐ>();
            SqlConnection conn = new SqlConnection("Data Source = TRUCLY; Initial Catalog = csdl; Integrated Security = True;");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM hoaDon WHERE ngayLapHD between @ngayBD and @ngayKT ", conn);
            command.Parameters.AddWithValue("@ngayBD", ngayBD.Date);
            command.Parameters.AddWithValue("@ngayKT", ngayKT.Date);

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                QuanLyHĐ hd = new QuanLyHĐ
                {
                    MaHD = reader["maHD"].ToString(),
                    NgayLapHoaDon = Convert.ToDateTime(reader["ngayLapHD"]),
                    TongTien = Convert.ToDouble(reader["tongTien"]),
                    MaNV = reader["maNV"].ToString()
                };
                kq.Add(hd);
            }
            return kq;
        }
    }
}


