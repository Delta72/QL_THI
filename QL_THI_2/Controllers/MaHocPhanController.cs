using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QL_THI_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QL_THI_2.Controllers
{
    public class MaHocPhanController : Controller
    {
        QL_THIContext db = new QL_THIContext();

        [Authorize(Roles = "admin")]
        public IActionResult MaHocPhan()
        {
            List<modelMaHocPhan> L = new List<modelMaHocPhan>();
            foreach (var i in db.MA_HOC_PHANs)
            {
                modelMaHocPhan m = new modelMaHocPhan()
                {
                    id = i.ID_MHP,
                    ma = i.MA_MHP,
                    tenHocPhan = i.TEN_MHP,
                    soTinChi = (short)i.TINCHI_MHP,
                    
                };
                m.chinhSua = (db.HOC_PHAN_THIs.Where(a => a.ID_MHP == m.id).Count() > 0) ? 0 : 1;
                L.Add(m);
            }
            return View(L);
        }

        [Authorize(Roles ="admin")]
        [NoDirectAccess]
        public IActionResult ThemMaHocPhan(string ma, string ten, string tc)
        {
            try
            {
                MA_HOC_PHAN M = new MA_HOC_PHAN()
                {
                    MA_MHP = ma,
                    TEN_MHP = ten,
                    TINCHI_MHP = short.Parse(tc)
                };
                db.MA_HOC_PHANs.Add(M);
                db.SaveChanges();
                return Json(true);
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        [NoDirectAccess]
        [Authorize(Roles = "admin")]
        public IActionResult ChinhSuaMHP(string id, string ma, string ten, string tc)
        {
            try
            {
                MA_HOC_PHAN M = db.MA_HOC_PHANs.Where(a => a.ID_MHP == int.Parse(id)).FirstOrDefault();
                if (db.HOC_PHAN_THIs.Where(a => a.ID_MHP == M.ID_MHP).Count() > 0)
                {
                    db.Entry(M).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    M.TEN_MHP = ten;
                    M.TINCHI_MHP = short.Parse(tc);
                    db.Entry(M).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    return Json("exists");
                }
                else
                {
                    db.Entry(M).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    M.MA_MHP = ma;
                    M.TEN_MHP = ten;
                    M.TINCHI_MHP = short.Parse(tc);
                    db.Entry(M).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    return Json("success");
                }
            }
            catch (Exception)
            {
                return Json("error");
            }
        }
    }
}
