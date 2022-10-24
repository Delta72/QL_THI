using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QL_THI_2.Models;
using Microsoft.AspNetCore.Authorization;

namespace QL_THI_2.Controllers
{
    public class TimKiemController : Controller
    {
        QL_THIContext db = new QL_THIContext();

        [Authorize]
        public IActionResult TimKiemNhom()
        {
            List<modelNhom> L = new List<modelNhom>();
            foreach(var n in db.NHOM_THIs)
            {
                modelNhom m = new modelNhom();
                m = NhomController.LayThongTinNhom(n);
                L.Add(m);
            }
            return View(L.OrderBy(a => a.duongDan));
        }

        [NoDirectAccess]
        public IActionResult HienBoLocNhom()
        {
            List<modelMaHocPhan> MHP = new List<modelMaHocPhan>();
            List<modelHinhThuc> HT = new List<modelHinhThuc>();
            List<string> NH = new List<string>();

            foreach(var i in db.MA_HOC_PHANs.OrderBy(a => a.ID_MHP))
            {
                modelMaHocPhan m = new modelMaHocPhan()
                {
                    id = i.ID_MHP,
                    tenHocPhan = i.TEN_MHP,
                };
                MHP.Add(m);
            }

            foreach(var i in db.HINH_THUC_THIs)
            {
                modelHinhThuc m = new modelHinhThuc()
                {
                    id = i.ID_HT,
                    tenHinhThuc = i.TEN_HT,
                };
                HT.Add(m);
            }

            foreach(var i in db.HOC_PHAN_THIs.OrderBy(a => a.NAMHOCB_HP).Select(a => a.NAMHOCB_HP).Distinct())
            {
                string str = i;
                int c = int.Parse(i) + 1;
                NH.Add(i + " - " + c);
            }

            var data = new
            {
                MHP = MHP,
                HT = HT,
                NH = NH,
            };
            return Json(data);
        }
    }
}
