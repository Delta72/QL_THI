using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QL_THI_2.Models
{
    public class modelChiTietDiem
    {
        public string idNhom { get; set; }
        public string mssv { get; set; }
        public List<double> diem { get; set; }
    }
}
