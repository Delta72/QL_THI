using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QL_THI_2.Models
{
    public class modelThongBao
    {
        public string id { get; set; }
        public modelTaiKhoan taiKhoan { get; set; }
        public string tuaDe { get; set; }
        public string noiDung { get; set; }
        public string thoiGian { get; set; }
    }

    public class DanhSachThongBao
    {
        public List<modelThongBao> DS { get; set; }
        public int soTrang { get; set; }
        public int trangHienTai { get; set; }
        public string str { get; set; }
    }
}
