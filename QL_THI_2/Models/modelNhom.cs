using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QL_THI_2.Models
{
    public class modelNhom
    {
        public string duongDan { get; set; }
        public string id { get; set; }
        public string stt { get; set; }
        public modelHinhThuc hinhThuc { get; set; }
        public modelTaiKhoan taiKhoan { get; set; }
        public string ngayThi { get; set; }
        public string hanNop { get; set; }
        public string siSo { get; set; }
        public string thamDu { get; set; }
        public string zipBaiThi { get; set; }
        public string excelDiem { get; set; }
        public string pdfDe { get; set; }
        public string pdfDiem { get; set; }
        public string elearning { get; set; }
        public string soDe { get; set; }
        public string soDapAn { get; set; }
        public bool daNop { get; set; }
        public int slNop { get; set; }
        public IFormFile fileZip { get; set; }
        public IFormFile filePDFDe { get; set; }
        public IFormFile filePDFDiem { get; set; }
        public IFormFile fileExcel { get; set; }
        public List<string> thanhPhan { get; set; }
        public List<modelDiem> diem { get; set; }
        public DoThi doThi { get; set; }
    }

    public class DanhSachNhom
    {
        public string hocPhan { get; set; }
        public modelHocPhan chiTietHP { get; set; }
        public List<modelNhom> danhSachNhom { get; set; }
    }
}
