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
        public string namHoc { get; set; }
        public int soNhom { get; set; }
        public modelMaHocPhan maHocPhan { get; set; }
    }
}
