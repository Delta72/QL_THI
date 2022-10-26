using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QL_THI_2.Models
{
    public class modelSinhVien
    {
        public string id { get; set; }
        public List<string> diem { get; set; }
        public modelNhom nhom { get; set; }
    }

    public class DanhSachSinhVien
    {
        public List<modelSinhVien> sinhVien { get; set; }
        public int soTrang { get; set; }
        public int trangHienTai { get; set; }
    }
}
