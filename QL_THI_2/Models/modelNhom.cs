using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QL_THI_2.Models
{
    public class modelNhom
    {
        public string id { get; set; }
        public string stt { get; set; }
        public modelHinhThuc hinhThuc { get; set; }
        public modelTaiKhoan taiKhoan { get; set; }
        public DateTime ngayThi { get; set; }
        public int siSo { get; set; }
        public int thamDu { get; set; }
        public string zip { get; set; }
        public string excel { get; set; }
        public short soDe { get; set; }
        public short soDapAn { get; set; }
        public bool daNop { get; set; }
    }

    public class DanhSachNhom
    {
        public string hocPhan { get; set; }
        public List<modelNhom> danhSachNhom { get; set; }
    }
}
