using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QL_THI_2.Models
{
    public class modelTaiKhoan
    {
        public string id { get; set; }
        public string matKhau { get; set; }
        public string hoTen { get; set; }
        public string ngayTao { get; set; }
        public string email { get; set; }
        public string lanHDCuoi { get; set; }
        public string avatar { get; set; }
        public bool isAdmin { get; set; }
        public bool hoatDong { get; set; }
    }
}