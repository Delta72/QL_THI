using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QL_THI_2.Models;

namespace QL_THI_2.Controllers
{
    public class TaiKhoanController : Controller
    {
        QL_THIContext db = new QL_THIContext();

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

        public IActionResult LayAvatar(string id)
        {
            TAI_KHOAN T = db.TAI_KHOANs.Where(a => a.ID_TK == id).FirstOrDefault();
            string img = T.ANHDAIDIEN_TK;
            return Json(img);
        }
    }
}
