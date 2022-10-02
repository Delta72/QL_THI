using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QL_THI_2.Models;

namespace QL_THI_2.Controllers
{
    public class HocPhanController : Controller
    {
        QL_THIContext db = new QL_THIContext();
        public IActionResult DanhSachHocPhan()
        {
            List<modelHocPhan> L = new List<modelHocPhan>();
            foreach(var i in db.HOC_PHAN_THIs)
            {
                modelHocPhan m = new modelHocPhan();
                m.id = i.ID_HP;
                m.soNhom = (int)i.SONHOM_HP;
                m.hocKy = i.ID_HKNavigation.TEN_HK;
                m.namHoc = i.NAMHOCB_HP + " - " + i.NAMHOCK_HP;
                L.Add(m);
            }
            return View(L);
        }

        [HttpGet]
        public IActionResult ThemHocPhan()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ThemHocPhan")]
        public IActionResult ThemHocPhanMoi(modelHocPhan model)
        {
            return RedirectToAction("DanhSachHocPhan", "HocPhan");
        }

        [HttpGet]
        public IActionResult LayMaHocPhan()
        {
            List<modelMaHocPhan> L = new List<modelMaHocPhan>();
            foreach(var i in db.MA_HOC_PHANs.OrderBy(a => a.ID_MHP))
            {
                modelMaHocPhan m = new modelMaHocPhan()
                {
                    id = i.ID_MHP,
                    tenHocPhan = i.TEN_MHP,
                    soTinChi = (int)i.TINCHI_MHP,
                };
                L.Add(m);
            }
            return Json(L);
        }

        [HttpGet]
        public IActionResult LayMaHocKy()
        {
            List<modelHocKy> L = new List<modelHocKy>();
            foreach (var i in db.HOC_Kies.OrderBy(a => a.ID_HK))
            {
                modelHocKy m = new modelHocKy()
                {
                    id = i.ID_HK,
                    tenHocKy = i.TEN_HK,
                };
                L.Add(m);
            }
            return Json(L);
        }
    }
}
