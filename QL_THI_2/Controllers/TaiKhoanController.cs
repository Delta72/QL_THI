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
    }
}
