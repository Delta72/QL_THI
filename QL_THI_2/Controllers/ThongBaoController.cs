using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QL_THI_2.Models;
using Microsoft.AspNetCore.Authorization;

namespace QL_THI_2.Controllers
{
    public class ThongBaoController : Controller
    {
        QL_THIContext db = new QL_THIContext();

        [Authorize]
        public IActionResult DanhSachThongBao(int page)
        {
            int skip = (page-1)*10;
            List<modelThongBao> L = new List<modelThongBao>();
            foreach(var i in db.THONG_BAOs.OrderBy(a => a.THOIGIAN_TB).Skip(skip).Take(10))
            {
                modelThongBao m = new modelThongBao();
                m.id = i.ID_TB;
                m.taiKhoan.id = i.ID_TK;
                m.taiKhoan.hoTen = db.TAI_KHOANs.Where(a => a.ID_TK == i.ID_TK).Select(a => a.HOTEN_TK).FirstOrDefault();
                m.taiKhoan.avatar = db.TAI_KHOANs.Where(a => a.ID_TK == i.ID_TK).Select(a => a.ANHDAIDIEN_TK).FirstOrDefault();
                m.tuaDe = i.TUADE_TB;
                m.noiDung = Rijindael.Decrypt(i.NOIDUNG_TB, i.ID_TB);
                m.thoiGian = ((DateTime)i.THOIGIAN_TB).ToString("dd/MM/yyyy");
                L.Add(m);
            }
            return View(L);
        }
    }
}
