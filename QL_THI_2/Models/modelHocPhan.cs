using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QL_THI_2.Models
{
    public class modelHocPhan
    {
        public string id { get; set; }
        public string ngayThem { get; set; }
        public string hocKy { get; set; }
        public string namHocB { get; set; }
        public string namHocK { get; set; }
        public int soNhom { get; set; }
        public string hanNop { get; set; }
        public int daNop { get; set; }
        public modelMaHocPhan maHocPhan { get; set; }
    }

    public class DanhSachHocPhan
    {
        public string hocKy_namHoc { get; set; }
        public List<modelHocPhan> hocPhan { get; set; }
    }
}
