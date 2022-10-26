using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QL_THI_2.Models;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QL_THI_2.Controllers
{
    public class ChiTietDiemController : Controller
    {
        QL_THIContext db = new QL_THIContext();

        [NoDirectAccess]
        public IActionResult LuuChiTietDiem(string str)
        {
            var result = "success";
            dynamic j = JsonConvert.DeserializeObject(str);

            try
            {
                string id = "";
                
                foreach(var i in j) { id = i.id; }
                using (var temp = new QL_THIContext())
                {
                    foreach (var i in db.CHI_TIET_DIEMs.Where(a => a.ID_N == id))
                    {
                        temp.CHI_TIET_DIEMs.Remove(i);
                    }
                    temp.SaveChanges();
                }

                using (var temp = new QL_THIContext())
                {
                    foreach (var i in j)
                    {
                        CHI_TIET_DIEM C = new CHI_TIET_DIEM();
                        C.ID_N = i.id;
                        C.MSSV_CTBT = i.mssv;
                        C.DIEM_CTBT = i.diem;
                        temp.CHI_TIET_DIEMs.Add(C);
                    }
                    temp.SaveChanges();
                }
                LuuChiTietExcel(j);
            }
            catch (Exception)
            {
                result = "error";
            }

            return Json(result);
        }

        [NoDirectAccess]
        public void LuuChiTietExcel(dynamic d)
        {
            string idN = "";
            foreach (var i in d) { idN = i.id; }

            using(ExcelEngine ex = new ExcelEngine())
            {
                IApplication app = ex.Excel;
                app.DefaultVersion = ExcelVersion.Excel2016;

                IWorkbook wb = app.Workbooks.Create(1);
                IWorksheet ws = wb.Worksheets[0];

                var row = 0;
                foreach (var i in d)
                {
                    row++;
                    string mssv = i.mssv;
                    ws.Range[row, 1].Value = mssv;
                    string diem = i.diem;
                    string[] arrDiem = diem.Split(" ");
                    var cell = 1;
                    foreach (var x in arrDiem)
                    {
                        cell++;
                        ws.Range[row, cell].Value = x;
                    }
                }

                var link = "/user/" + User.FindFirstValue(ClaimTypes.NameIdentifier)
                    + "/" + idN + "/" + Guid.NewGuid().ToString() + ".xlsx";
                NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == idN).First();
                UploadController.DeleteFile(N.LINKEXCELDIEM_N, Directory.GetCurrentDirectory());
                db.Entry(N).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                N.LINKEXCELDIEM_N = link;
                if (N.NGAYTHI_N == null) N.NGAYTHI_N = DateTime.Now;
                db.Entry(N).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                var path = Directory.GetCurrentDirectory()
                    + "/wwwroot" + link;

                FileStream stream = new FileStream(path, FileMode.Create);
                wb.SaveAs(stream);
                stream.Close();

                wb.Close();
                ex.Dispose();
            }
        }
    }
}
