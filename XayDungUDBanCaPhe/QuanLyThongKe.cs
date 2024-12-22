using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanCaPhe
{
    internal class QuanLyThongKe
    {

        private DateTime ngayTK;
        private int soluongHĐ;
        private double tongTienTK;
        private string maNV;
        // private List<QuanLyHĐ> dsHD;
        public QuanLyThongKe()
        {
            this.ngayTK = DateTime.Now;
            this.soluongHĐ = 0;
            this.tongTienTK = 0;
            this.maNV = null;
            //  this.dsHD = null;
        }
        public QuanLyThongKe(DateTime ngayTK, int soluongHĐ, double tongTienTK, string maNV)//, List<QuanLyHĐ> dsHD)
        {
            this.ngayTK = ngayTK;
            this.soluongHĐ = soluongHĐ;
            this.maNV = maNV;
            this.tongTienTK = tongTienTK;
            // this.dsHD = dsHD;
        }
        public DateTime NgayTK
        {
            get { return this.ngayTK; }
            set { this.ngayTK = value; }
        }
        public int SoLuongHĐ
        {
            get { return this.soluongHĐ; }
            set { this.soluongHĐ = value; }
        }

        public double TongTienTK
        {
            get { return this.tongTienTK; }
            set { this.tongTienTK = value; }
        }
        public string MaNV
        {
            get { return this.maNV; }
            set { this.maNV = value; }
        }
        //public List<QuanLyHĐ> DSHD
        //{
        //    get { return this.dsHD; }
        //    set { this.dsHD = value; }
        //}
    }
}

