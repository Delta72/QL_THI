using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QL_THI_2.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace QL_THI_2.Controllers
{
    public class TaiKhoanController : Controller
    {
        QL_THIContext db = new QL_THIContext();

        // Dang nhap
        [AllowAnonymous]
        public IActionResult DangNhap()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult TaskDangNhap(string taiKhoan, string matKhau)
        {
            TAI_KHOAN T = db.TAI_KHOANs.Where(a => a.ID_TK == taiKhoan && a.MK_TK == matKhau).FirstOrDefault();
            if(T == null)
            {
                return Json("error");
            }
            else
            {
                string role = "";
                if (T.LAADMIN_TK == true) role = "admin";
                if (T.LAADMIN_TK == false) role = "user";
                var claims = new[] {
                    new Claim(ClaimTypes.Name, T.HOTEN_TK),
                    new Claim(ClaimTypes.NameIdentifier, T.ID_TK),
                    new Claim(ClaimTypes.UserData, T.ANHDAIDIEN_TK),
                    new Claim(ClaimTypes.Role, role)};
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));

                db.Entry(T).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                T.LANHDCUOI_TK = DateTime.Now;
                db.Entry(T).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                return Json(role);
            }
        }

        [Authorize]
        public async Task<IActionResult> TaskDangXuat()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("DangNhap", "TaiKhoan");
        }

        [Authorize(Roles = "admin")]
        [NoDirectAccess]
        public IActionResult TimKiemTaiKhoan(string str)
        {
            List<modelTaiKhoan> L = new List<modelTaiKhoan>();
            str = str.ToLower();
            foreach(var i in db.TAI_KHOANs)
            {
                if(i.ID_TK.ToLower().Contains(str) || i.HOTEN_TK.ToLower().Contains(str))
                {
                    modelTaiKhoan m = new modelTaiKhoan();
                    m.id = i.ID_TK;
                    m.hoTen = i.HOTEN_TK;
                    m.avatar = i.ANHDAIDIEN_TK;
                    L.Add(m);
                }
            }
            return Json(L);
        }

        [AllowAnonymous]
        public IActionResult LayAvatar(string id)
        {
            TAI_KHOAN T = db.TAI_KHOANs.Where(a => a.ID_TK == id).FirstOrDefault();
            string img = T.ANHDAIDIEN_TK;
            return Json(img);
        }

        [Authorize(Roles = "admin")]
        public IActionResult DanhSachTaiKhoan()
        {
            List<modelTaiKhoan> L = new List<modelTaiKhoan>();
            foreach(var i in db.TAI_KHOANs.OrderBy(a => a.ID_TK))
            {
                modelTaiKhoan m = new modelTaiKhoan();
                m.id = i.ID_TK;
                m.hoTen = i.HOTEN_TK;
                m.email = i.EMAIL_TK;
                m.lanHDCuoi = ((DateTime)i.LANHDCUOI_TK).ToString("dd/MM/yyyy");
                m.avatar = i.ANHDAIDIEN_TK;
                m.isAdmin = (bool)i.LAADMIN_TK;
                m.hoatDong = (bool)i.HOATDONG_TK;
                L.Add(m);
            }
            return View(L);
        }

        [Authorize(Roles = "admin")]
        public IActionResult KhoaTaiKhoan(string id)
        {
            try
            {
                TAI_KHOAN T = db.TAI_KHOANs.Where(a => a.ID_TK == id).FirstOrDefault();
                db.Entry(T).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                if (T.HOATDONG_TK == true)
                {
                    T.HOATDONG_TK = false;
                }
                else
                {
                    T.HOATDONG_TK = true;
                }
                db.Entry(T).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                var data = new
                {
                    id = T.ID_TK,
                    hd = T.HOATDONG_TK
                };
                return Json(data);
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        [Authorize(Roles = "admin")]
        public IActionResult ThemTaiKhoan()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult KiemTraID(string id)
        {
            bool data = false;
            if(db.TAI_KHOANs.Where(a => a.ID_TK == id).FirstOrDefault() != null)
            {
                data = true;
            }
            return Json(data);
        }

        [Authorize(Roles = "admin")]
        public IActionResult TaskThemTaiKhoan(string id, string mk, string hoTen, string email)
        {
            TAI_KHOAN TK = new TAI_KHOAN();

            TK.ID_TK = id;
            TK.MK_TK = Rijindael.Encrypt(mk, id);
            TK.HOTEN_TK = hoTen;
            TK.EMAIL_TK = email;
            TK.NGAYTAO_TK = DateTime.Now;
            TK.LANHDCUOI_TK = DateTime.Now;
            TK.ANHDAIDIEN_TK = "/template/assets/img/avatars/ano.png";
            TK.HOATDONG_TK = true;
            TK.LAADMIN_TK = false;

            db.TAI_KHOANs.Add(TK);
            db.SaveChanges();

            modelTaiKhoan m = new modelTaiKhoan()
            {
                id = id,
                hoTen = hoTen,
                email = email,
                lanHDCuoi = ((DateTime)TK.LANHDCUOI_TK).ToString("dd/MM/yyyy"),
                avatar = TK.ANHDAIDIEN_TK,
                hoatDong = (bool)TK.HOATDONG_TK,
                isAdmin = (bool)TK.LAADMIN_TK
            };

            return Json(m);
        }

        [Authorize(Roles = "admin")]
        [NoDirectAccess]
        public IActionResult TimKiemTaiKhoanDS(string str)
        {
            List<modelTaiKhoan> L = new List<modelTaiKhoan>();
            if (str == null)
            {
                foreach (var i in db.TAI_KHOANs)
                {
                    modelTaiKhoan m = new modelTaiKhoan();
                    m.id = i.ID_TK;
                    m.hoTen = i.HOTEN_TK;
                    m.email = i.EMAIL_TK;
                    m.lanHDCuoi = ((DateTime)i.LANHDCUOI_TK).ToString("dd/MM/yyyy");
                    m.avatar = i.ANHDAIDIEN_TK;
                    m.isAdmin = (bool)i.LAADMIN_TK;
                    m.hoatDong = (bool)i.HOATDONG_TK;
                    L.Add(m);
                }
            }
            else
            {
                str = str.ToLower();
                foreach (var i in db.TAI_KHOANs)
                {
                    if (i.ID_TK.ToLower().Contains(str) || i.HOTEN_TK.ToLower().Contains(str) || i.EMAIL_TK.ToLower().Contains(str))
                    {
                        modelTaiKhoan m = new modelTaiKhoan();
                        m.id = i.ID_TK;
                        m.hoTen = i.HOTEN_TK;
                        m.email = i.EMAIL_TK;
                        m.lanHDCuoi = ((DateTime)i.LANHDCUOI_TK).ToString("dd/MM/yyyy");
                        m.avatar = i.ANHDAIDIEN_TK;
                        m.isAdmin = (bool)i.LAADMIN_TK;
                        m.hoatDong = (bool)i.HOATDONG_TK;
                        L.Add(m);
                    }
                }

            }
            return View(L);
        }
    }
}
