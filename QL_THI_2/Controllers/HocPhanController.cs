using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QL_THI_2.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace QL_THI_2.Controllers
{
    public class HocPhanController : Controller
    {
        QL_THIContext db = new QL_THIContext();

        public int SoNhomDaNop(string id)
        {
            HOC_PHAN_THI HP = db.HOC_PHAN_THIs.Where(a => a.ID_HP == id).FirstOrDefault();
            int danop = db.NHOM_THIs.Where(a => a.ID_HP == id && a.DANOP_N == true).Count();
            return danop;
        }

        [Authorize(Roles = "admin")]
        public IActionResult ThemHocPhan()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ThemHocPhan")]
        [Authorize(Roles = "admin")]
        public IActionResult ThemHocPhanMoi(modelHocPhan model)
        {
            return RedirectToAction("DanhSachHocPhan", "HocPhan");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
        public IActionResult TaskThemHocPhan(string jsonHP, string jsonNhom, string soNhom)
        {
            // Them hoc phan
            var str = jsonHP.Trim('[', ']');
            dynamic j = JsonConvert.DeserializeObject(str);
            string maHocPhan = j.maHocPhan;
            short maHocKy = (short)j.maHocKy;
            string namHocB = j.namHocB;
            string namHocK = j.namHocK;
            string hanNop = j.hanNop;
            short so_nhom = 0; short.TryParse(soNhom, out so_nhom);

            // Luu hoc phan vao csdl
            HOC_PHAN_THI H = new HOC_PHAN_THI();
            string id = Guid.NewGuid().ToString();
            H.ID_HP = TimIDHocPhan(id);
            H.ID_HK = maHocKy;
            H.ID_TK = User.FindFirstValue(ClaimTypes.NameIdentifier);
            H.ID_MHP = maHocPhan;
            H.NAMHOCB_HP = namHocB;
            H.NAMHOCK_HP = namHocK;
            H.HANNOP_HP = DateTime.Parse(hanNop);
            H.SONHOM_HP = so_nhom;
            db.HOC_PHAN_THIs.Add(H);
            db.SaveChanges();

            // Them nhom thi
            dynamic j2 = JsonConvert.DeserializeObject(jsonNhom);
            NhomController.ThemNhomThi(j2, H.ID_HP);

            return Json(true);
        }

        public string TimIDHocPhan(string id)
        {
            if(db.HOC_PHAN_THIs.Where(a => a.ID_HP == id).FirstOrDefault() == null)
            {
                return id;
            }
            return TimIDHocPhan(Guid.NewGuid().ToString());
        }

        public IActionResult LayNamHoc()
        {
            List<string> namHoc = new List<string>();
            var temp = db.HOC_PHAN_THIs
                .Select(a => a.NAMHOCB_HP)
                .Distinct()
                .ToList();
            foreach(var i in temp)
            {
                namHoc.Add(i);
            }
            return Json(namHoc);
        }

        [Authorize(Roles = "admin")]
        public IActionResult DanhSachHocPhan()
        {
            List<DanhSachHocPhan> D = new List<DanhSachHocPhan>();
            var temp = db.HOC_PHAN_THIs
                .Select(a => new { a.ID_HK, a.NAMHOCB_HP, a.NAMHOCK_HP })
                .Distinct()
                .ToList();
            foreach (var item in temp)
            {
                DanhSachHocPhan d = new DanhSachHocPhan();
                string hk = db.HOC_Kies.Where(a => a.ID_HK == item.ID_HK).Select(a => a.TEN_HK).FirstOrDefault();
                string nh = item.NAMHOCB_HP.ToString() + " - " + item.NAMHOCK_HP.ToString();
                d.hocKy_namHoc = hk + ", " + nh;

                List<modelHocPhan> L = new List<modelHocPhan>();
                foreach (var i in db.HOC_PHAN_THIs.Where(a => a.ID_HK == item.ID_HK && a.NAMHOCB_HP == item.NAMHOCB_HP && a.NAMHOCK_HP == item.NAMHOCK_HP).OrderBy(a => a.ID_MHP))
                {
                    modelHocPhan m = new modelHocPhan();
                    m.id = i.ID_HP;
                    m.soNhom = (int)i.SONHOM_HP;
                    m.hocKy = db.HOC_Kies.Where(a => a.ID_HK == i.ID_HK).Select(a => a.TEN_HK).First();
                    m.maHocPhan = new modelMaHocPhan()
                    {
                        id = i.ID_MHP,
                        tenHocPhan = db.MA_HOC_PHANs.Where(a => a.ID_MHP == i.ID_MHP).Select(a => a.TEN_MHP).FirstOrDefault()
                    };
                    m.namHocB = i.NAMHOCB_HP;
                    m.namHocK = i.NAMHOCK_HP;
                    m.hanNop = ((DateTime)i.HANNOP_HP).ToString("dd/MM/yyyy");

                    m.daNop = SoNhomDaNop(i.ID_HP);

                    L.Add(m);
                }
                d.hocPhan = L;
                D.Add(d);
            }
            return View(D);
        }

        public IActionResult TimKiemHocPhan(string hocKy, string namHoc)
        {
            int hk; int.TryParse(hocKy, out hk);
            DanhSachHocPhan D = new DanhSachHocPhan();
            string shk = db.HOC_Kies.Where(a => a.ID_HK == hk).Select(a => a.TEN_HK).FirstOrDefault();
            D.hocKy_namHoc = shk + ", " + namHoc + " - " + (int.Parse(namHoc) + 1);

            List<modelHocPhan> L = new List<modelHocPhan>();
            foreach (var i in db.HOC_PHAN_THIs.Where(a => a.ID_HK == hk && a.NAMHOCB_HP == namHoc))
            {
                modelHocPhan m = new modelHocPhan();
                m.id = i.ID_HP;
                m.soNhom = (int)i.SONHOM_HP;
                m.hocKy = db.HOC_Kies.Where(a => a.ID_HK == i.ID_HK).Select(a => a.TEN_HK).First();
                m.maHocPhan = new modelMaHocPhan()
                {
                    id = i.ID_MHP,
                    tenHocPhan = db.MA_HOC_PHANs.Where(a => a.ID_MHP == i.ID_MHP).Select(a => a.TEN_MHP).FirstOrDefault()
                };
                m.namHocB = i.NAMHOCB_HP;
                m.namHocK = i.NAMHOCK_HP;
                m.hanNop = ((DateTime)i.HANNOP_HP).ToString("dd/MM/yyyy");

                m.daNop = SoNhomDaNop(i.ID_HP);

                L.Add(m);
            }
            D.hocPhan = L;
            return View(D);
        }
    }
}
