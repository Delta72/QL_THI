using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QL_THI_2.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace QL_THI_2.Controllers
{
    public class ThongBaoController : Controller
    {
        QL_THIContext db = new QL_THIContext();

        [Authorize]
        public IActionResult DanhSachThongBao(string p)
        {
            int page = (p == null) ? 1 : (int.Parse(p));
            int soTB = 10;
            int skip = (page-1)*soTB;
            DanhSachThongBao D = new DanhSachThongBao();
            D.DS = new List<modelThongBao>();

            List<modelThongBao> L = new List<modelThongBao>();
            foreach(var i in db.THONG_BAOs.OrderByDescending(a => a.THOIGIAN_TB).Skip(skip).Take(soTB))
            {
                modelThongBao m = new modelThongBao();
                m.id = i.ID_TB;
                m.taiKhoan = new modelTaiKhoan();
                m.taiKhoan.id = i.ID_TK;
                m.taiKhoan.hoTen = db.TAI_KHOANs.Where(a => a.ID_TK == i.ID_TK).Select(a => a.HOTEN_TK).FirstOrDefault();
                m.taiKhoan.avatar = db.TAI_KHOANs.Where(a => a.ID_TK == i.ID_TK).Select(a => a.ANHDAIDIEN_TK).FirstOrDefault();
                m.tuaDe = i.TUADE_TB;
                m.noiDung = i.NOIDUNG_TB;
                m.thoiGian = ((DateTime)i.THOIGIAN_TB).ToString("dd/MM/yyyy");
                L.Add(m);
            }
            D.DS = L;

            int trang = db.THONG_BAOs.Count();
            D.soTrang = (trang % soTB == 0) ? (trang / soTB) : ((trang / soTB) + 1);
            D.trangHienTai = page;

            return View(D);
        }

        [Authorize(Roles = "admin")]
        public IActionResult ThemThongBao()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult TaskThemThongBao(string tuaDe, string noiDung)
        {
            THONG_BAO T = new THONG_BAO();
            T.ID_TB = TimIDTB(Guid.NewGuid().ToString());
            T.TUADE_TB = tuaDe;
            T.NOIDUNG_TB = noiDung;
            T.THOIGIAN_TB = DateTime.Now;
            T.ID_TK = User.FindFirstValue(ClaimTypes.NameIdentifier);

            db.THONG_BAOs.Add(T);
            db.SaveChanges();

            return Json(T.ID_TB);
        }

        public static string TimIDTB(string id)
        {
            using (var db = new QL_THIContext())
            {
                if (db.THONG_BAOs.Where(a => a.ID_TB == id).FirstOrDefault() == null)
                {
                    return id;
                }
                return TimIDTB(Guid.NewGuid().ToString());
            }
        }

        [Authorize]
        public IActionResult ChiTietThongBao(string id)
        {
            THONG_BAO T = db.THONG_BAOs.Where(a => a.ID_TB == id).FirstOrDefault();
            modelThongBao m = new modelThongBao();
            m.id = T.ID_TB;
            m.tuaDe = T.TUADE_TB;
            m.noiDung = T.NOIDUNG_TB;
            m.taiKhoan = new modelTaiKhoan();
            m.taiKhoan.id = T.ID_TK;
            m.taiKhoan.hoTen = db.TAI_KHOANs.Where(a => a.ID_TK == T.ID_TK).Select(a => a.HOTEN_TK).FirstOrDefault();
            m.taiKhoan.avatar = db.TAI_KHOANs.Where(a => a.ID_TK == T.ID_TK).Select(a => a.ANHDAIDIEN_TK).FirstOrDefault();
            m.thoiGian = ((DateTime)T.THOIGIAN_TB).ToString("dd/MM/yyyy");
            return View(m);
        }

        [Authorize(Roles = "admin")]
        public IActionResult XoaThongBao(string id)
        {
            THONG_BAO T = db.THONG_BAOs.Where(a => a.ID_TB == id).FirstOrDefault();
            db.Remove(T);
            db.SaveChanges();
            return Json(true);
        }
    }
}
