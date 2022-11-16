using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QL_THI_2.Models
{
    public class modelDiem
    {
        public string idNhom { get; set; }
        public string mssv { get; set; }
        public List<double> diem { get; set; }
    }

    public class DanhSachDiem
    {
        public List<modelDiem> ds { get; set; }
        public int lanChinhSua { get; set; }
        public string lyDo { get; set; }
        public string ngaySua { get; set; }
    }

    public class DoThi
    {
        public List<int> soLuong { get; set; }
        public List<double> chiTietDiem { get; set; }
    }
}
