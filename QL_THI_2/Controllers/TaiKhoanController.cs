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
using System.IO;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;

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
            TAI_KHOAN T = db.TAI_KHOANs.Where(a => a.DN_TK == taiKhoan).FirstOrDefault();
            if(T == null)
            {
                return Json("error");
            }
            else if(Rijindael.Decrypt(T.MK_TK, T.ID_TK.Replace("-", "")) != matKhau)
            {
                return Json("error");
            }
            else if(T.HOATDONG_TK == false)
            {
                return Json("blocked");
            }
            else
            {
                string role = "";
                if (T.LAADMIN_TK == true) role = "admin";
                if (T.LAADMIN_TK == false) role = "user";
                var claims = new[] {
                    new Claim(ClaimTypes.Name, T.HOTEN_TK),
                    new Claim(ClaimTypes.Authentication, T.DN_TK),
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
                if(i.DN_TK.ToLower().Contains(str) || i.HOTEN_TK.ToLower().Contains(str) || i.EMAIL_TK.ToLower().Contains(str))
                {
                    modelTaiKhoan m = LayThongTinTaiKhoan(i);
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
            foreach(var i in db.TAI_KHOANs.OrderBy(a => a.DN_TK))
            {
                modelTaiKhoan m = new modelTaiKhoan();
                m.id = i.ID_TK;
                m.dn = i.DN_TK;
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
            if(db.TAI_KHOANs.Where(a => a.DN_TK == id).FirstOrDefault() != null)
            {
                data = true;
            }
            return Json(data);
        }

        [NoDirectAccess]
        public string TimIDTK(string id)
        {
            if(db.TAI_KHOANs.Where(a => a.ID_TK == id).FirstOrDefault() == null)
            {
                return id;
            }
            return TimIDTK(Guid.NewGuid().ToString());
        }

        [Authorize(Roles = "admin")]
        public IActionResult TaskThemTaiKhoan(string id, string mk, string hoTen, string email)
        {
            TAI_KHOAN TK = new TAI_KHOAN();

            TK.DN_TK = id;
            TK.ID_TK = TimIDTK(Guid.NewGuid().ToString());
            TK.MK_TK = Rijindael.Encrypt(mk, TK.ID_TK.Replace("-",""));
            TK.HOTEN_TK = hoTen;
            TK.EMAIL_TK = email;
            TK.NGAYTAO_TK = DateTime.Now;
            TK.LANHDCUOI_TK = DateTime.Now;
            TK.ANHDAIDIEN_TK = "/template/assets/img/avatars/ano.png";
            TK.HOATDONG_TK = true;
            TK.LAADMIN_TK = false;

            db.TAI_KHOANs.Add(TK);
            db.SaveChanges();

            TaoThuMuc(TK.ID_TK);

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

        [NoDirectAccess]
        public void TaoThuMuc(string id)
        {
            var p = @"wwwroot\user\" + id;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), p);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
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
                    m.dn = i.DN_TK;
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
                    if (i.DN_TK.ToLower().Contains(str) || i.HOTEN_TK.ToLower().Contains(str) || i.EMAIL_TK.ToLower().Contains(str))
                    {
                        modelTaiKhoan m = new modelTaiKhoan();
                        m.id = i.ID_TK;
                        m.dn = i.DN_TK;
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

        [NoDirectAccess]
        public static modelTaiKhoan LayThongTinTaiKhoan(TAI_KHOAN T)
        {
            modelTaiKhoan m = new modelTaiKhoan()
            {
                id = T.ID_TK,
                dn = T.DN_TK,
                hoTen = T.HOTEN_TK,
                avatar = T.ANHDAIDIEN_TK,
                email = T.EMAIL_TK,
                lanHDCuoi = ((DateTime)T.LANHDCUOI_TK).ToString("dd/MM/yyyy"),
                isAdmin = (bool)T.LAADMIN_TK,
            };
            return m;
        }

        [NoDirectAccess]
        public IActionResult LayThongTinTaiKhoanJson(string id)
        {
            TAI_KHOAN T = db.TAI_KHOANs.Where(a => a.ID_TK == id).First();
            modelTaiKhoan m = LayThongTinTaiKhoan(T);
            return Json(m);
        }

        [Authorize]
        public IActionResult ChiTietTaiKhoan()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            TAI_KHOAN T = db.TAI_KHOANs.Where(a => a.ID_TK == id).FirstOrDefault();
            modelTaiKhoan m = LayThongTinTaiKhoan(T);
            return View(m);
        }

        [Authorize]
        [NoDirectAccess]
        public IActionResult SuaThongTin(modelTaiKhoan m)
        {
            var result = "success";
            try
            {
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                TAI_KHOAN T = db.TAI_KHOANs.Where(a => a.ID_TK == id).First();
                TAI_KHOAN T2 = db.TAI_KHOANs.Where(a => a.DN_TK == m.dn).FirstOrDefault();
                if (T2 != null && T2.ID_TK != id)
                {
                    result = "dn";
                }
                else
                {
                    db.Entry(T).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    T.DN_TK = m.dn;
                    T.HOTEN_TK = m.hoTen;
                    T.EMAIL_TK = m.email;

                    if(m.img != null)
                    {
                        UploadController.DeleteFile(T.ANHDAIDIEN_TK, Directory.GetCurrentDirectory());
                        T.ANHDAIDIEN_TK = UploadImage(m.img);
                    }

                    db.Entry(T).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    result = "success";
                }
            }
            catch (Exception)
            {
                result = "error";
            }
            return Json(result);
        }

        [NoDirectAccess]
        public string UploadImage(IFormFile img)
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var filename = id + System.IO.Path.GetExtension(img.FileName);
            var filepath = Directory.GetCurrentDirectory() + "\\wwwroot\\user\\" + id + "\\" + filename;
            using var image = SixLabors.ImageSharp.Image.Load(img.OpenReadStream());
            image.Mutate(x => x.Resize(200, 200));
            image.Save(filepath);
            var report = "\\user\\" + id + "\\" + filename;
            return report;
        }

        [Authorize]
        [NoDirectAccess]
        public IActionResult DoiMatKhau(string p1, string p2)
        {
            var result = "success";
            try
            {
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                TAI_KHOAN T = db.TAI_KHOANs.Where(a => a.ID_TK == id).First();
                var pass = Rijindael.Decrypt(T.MK_TK, T.ID_TK.Replace("-", ""));
                if (pass != p1)
                {
                    result = "mk";
                }
                else
                {
                    db.Entry(T).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    T.MK_TK = Rijindael.Encrypt(p2, T.ID_TK.Replace("-", ""));
                    db.Entry(T).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    result = "success";
                }
            }
            catch (Exception)
            {
                result = "error";
            }
            return Json(result);
        }
    }
}
