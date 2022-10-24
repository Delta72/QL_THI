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
    }
}
