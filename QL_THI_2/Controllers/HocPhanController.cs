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
                m.tenHocPhan = i.TEN_HP;

            }
            return View();
        }
    }
}
