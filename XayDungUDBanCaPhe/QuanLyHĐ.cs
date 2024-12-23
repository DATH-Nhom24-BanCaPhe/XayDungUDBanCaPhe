using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XayDungUDBanCaPhe
{
    internal class QuanLyHĐ
    {
        string str = "Data Source = TRUCLY; Initial Catalog = csdl; Integrated Security = True;";
        private string maHD;
        private DateTime ngayLapHD;
        private double tongTien;
        private string maNV;
        public QuanLyHĐ()
        {
            this.maHD = null;
            this.ngayLapHD = DateTime.Now;
            this.maNV = null;
            this.tongTien = 0;
        }
        public QuanLyHĐ(string maHD, DateTime ngayLapHD, double tongTien, string maNV)
        {
            this.maHD = maHD;
            this.ngayLapHD = ngayLapHD;
            this.tongTien = tongTien;
            this.maNV = maNV;
        }
        public string MaHD
        {
            get { return this.maHD; }
            set { this.maHD = value; }
        }
        public DateTime NgayLapHoaDon
        {
            get { return this.ngayLapHD; }
            set { this.ngayLapHD = value; }
        }
        public double TongTien
        {
            get { return this.tongTien; }
            set { this.tongTien = value; }
        }
        public string MaNV
        {
            get { return this.maNV; }
            set { this.maNV = value; }
        }
        public string maTuTangMaHD()
        {

            //string key = tiento;

            //// Lấy ngày hiện tại
            //string[] partsDay = DateTime.Now.ToShortDateString().Split('/');
            //// Ví dụ 07/08/2009
            //string d = String.Format("{0}{1}{2}", partsDay[0], partsDay[1], partsDay[2]);
            //key = key + d;

            //// Trả về khóa mà không có thời gian
            //return key;


            string sql = @"select *from hoaDon";
            SqlConnection con = new SqlConnection();
            con.ConnectionString = str;
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string maHD = "";
            if (dt.Rows.Count <= 0)
            {
                maHD = "HD001";
            }
            else
            {
                int k;
                maHD = "HD";
                k = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1][0].ToString().Substring(2, 3));
                k = k + 1;
                if (k < 10)
                {
                    maHD = maHD + "00";
                }
                else if (k < 100)
                {
                    maHD = maHD + "0";
                }
                maHD = maHD + k.ToString();
            }
            return maHD;
        }
    }
}

