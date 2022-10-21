using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using QL_THI_2.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IO;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;

namespace QL_THI_2.Controllers
{
    public class NhomController : Controller
    {
        QL_THIContext db = new QL_THIContext();
        [Authorize(Roles = "admin")]
        public static void ThemNhomThi(dynamic D, string idHP)
        {
            using(var db = new QL_THIContext())
            {
                foreach (var i in D)
                {
                    NHOM_THI N = new NHOM_THI();
                    N.ID_N = TimIDNhom(Guid.NewGuid().ToString());
                    N.ID_HP = idHP;
                    N.ID_HT = 1;

                    string str = i.idGV;
                    int pFrom = str.IndexOf(">") + ">".Length;
                    int pTo = str.LastIndexOf("<");
                    N.ID_TK = str.Substring(pFrom, pTo - pFrom);

                    string stt = i.idN;
                    N.STT_N = short.Parse(stt);
                    N.DANOP_N = false;

                    db.NHOM_THIs.Add(N);
                    db.SaveChanges();

                    var p = @"wwwroot\user\" + N.ID_TK + @"\" + N.ID_N;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), p);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                }
            }
        }

        [NoDirectAccess]
        public static string TimIDNhom(string id)
        {
            using(var db = new QL_THIContext())
            {
                if (db.NHOM_THIs.Where(a => a.ID_HP == id).FirstOrDefault() == null)
                {
                    return id;
                }
                return TimIDNhom(Guid.NewGuid().ToString());
            }
        }

        [Authorize(Roles = "admin")]
        public IActionResult DanhSachNhomHocPhan(string id)
        {
            DanhSachNhom D = new DanhSachNhom();
            HOC_PHAN_THI H = db.HOC_PHAN_THIs.Where(a => a.ID_HP == id).FirstOrDefault();
            string hk = (H.HOCKY_HP == 1) ? "Học kỳ I" : (H.HOCKY_HP == 2) ? "Học kỳ II" : "Học kỳ hè";
            string nh = H.NAMHOCB_HP.ToString() + " - " + H.NAMHOCK_HP.ToString();
            string mon = H.ID_MHP + " - " + db.MA_HOC_PHANs.Where(a => a.ID_MHP == H.ID_MHP).Select(a => a.TEN_MHP).FirstOrDefault();
            D.hocPhan = hk + ", " + nh + " > " + mon;

            // Chi tiet hoc phan
            D.chiTietHP = new modelHocPhan();
            modelHocPhan m = HocPhanController.LayThongTinHP(H.ID_HP);
            D.chiTietHP = m;

            // Danh sach nhom trong hoc phan
            D.danhSachNhom = new List<modelNhom>();
            foreach(var i in db.NHOM_THIs.Where(a => a.ID_HP == id))
            {
                modelNhom n = LayThongTinNhom(i);
                D.danhSachNhom.Add(n);
            }
            return View(D);
        }

        [Authorize]
        public IActionResult ChiTietNhom(string id)
        {
            modelNhom m = new modelNhom();

            NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == id).FirstOrDefault();
            m = LayThongTinNhom(N);

            return View(m);
        }

        [NoDirectAccess]
        public modelNhom LayThongTinNhom(NHOM_THI N)
        {
            modelNhom m = new modelNhom();
            m.id = N.ID_N;
            m.stt = N.STT_N.ToString().PadLeft(2, '0');
            m.hinhThuc = new modelHinhThuc();
            m.hinhThuc.tenHinhThuc = db.HINH_THUC_THIs.Where(a => a.ID_HT == N.ID_HT).Select(a => a.TEN_HT).FirstOrDefault();
            m.taiKhoan = new modelTaiKhoan();
            m.taiKhoan.id = N.ID_TK;
            m.taiKhoan.dn = db.TAI_KHOANs.Where(a => a.ID_TK == N.ID_TK).Select(a => a.DN_TK).FirstOrDefault();
            m.taiKhoan.hoTen = db.TAI_KHOANs.Where(a => a.ID_TK == N.ID_TK).Select(a => a.HOTEN_TK).FirstOrDefault();
            m.taiKhoan.avatar = db.TAI_KHOANs.Where(a => a.ID_TK == N.ID_TK).Select(a => a.ANHDAIDIEN_TK).FirstOrDefault();
            m.daNop = (bool)N.DANOP_N;
            m.ngayThi = (N.NGAYTHI_N != null) ? ((DateTime)N.NGAYTHI_N).ToString("dd/MM/yyyy") : "---";
            m.siSo = (N.SISO_N != null) ? (N.SISO_N.ToString().PadLeft(2, '0')) : "---";
            m.thamDu = (N.SOLUONGTHI_N != null) ? (N.SOLUONGTHI_N.ToString().PadLeft(2, '0')) : "---";
            m.zipBaiThi = (N.LINKZIPBAI_N != null) ? (N.LINKZIPBAI_N) : null;
            m.excelDiem = (N.LINKEXCELDIEM_N != null) ? (N.LINKEXCELDIEM_N) : null;
            m.pdfDe = (N.LINKPDFDE_N != null) ? (N.LINKPDFDE_N) : null;
            m.pdfDiem = (N.LINKPDFDIEM_N != null) ? (N.LINKPDFDIEM_N) : null;
            m.elearning = (N.LINKELEARNING_N != null) ? (N.LINKELEARNING_N) : null;
            m.soDe = (N.SODE_N != null) ? (N.SODE_N.ToString().PadLeft(2, '0')) : "---";
            m.soDapAn = (N.SODAPAN_N != null) ? (N.SODAPAN_N.ToString().PadLeft(2, '0')) : "---";
            HOC_PHAN_THI H = db.HOC_PHAN_THIs.Where(a => a.ID_HP == N.ID_HP).FirstOrDefault();
            string hk = (H.HOCKY_HP == 1) ? "Học kỳ I" : (H.HOCKY_HP == 2) ? "Học kỳ II" : "Học kỳ hè";
            string nh = H.NAMHOCB_HP.ToString() + " - " + H.NAMHOCK_HP.ToString();
            string mon = H.ID_MHP + " - " + db.MA_HOC_PHANs.Where(a => a.ID_MHP == H.ID_MHP).Select(a => a.TEN_MHP).FirstOrDefault();
            m.duongDan = hk + ", " + nh + " > " + mon + " > " + "Nhóm " + m.stt;
            string[] thanhPhan = H.DIEMTHANHPHAN_HP.Split(" |");
            thanhPhan = thanhPhan.Where(a => a != "").ToArray();
            m.thanhPhan = new List<string>();
            foreach(var i in thanhPhan) { m.thanhPhan.Add(i); }
            return m;
        }

        [NoDirectAccess]
        public List<modelDiem> LayThongTinDiem(string idN)
        {
            List<modelDiem> L = new List<modelDiem>();
            foreach(var i in db.CHI_TIET_DIEMs.Where(a => a.ID_N == idN).OrderBy(a => a.MSSV_CTBT))
            {
                modelDiem m = new modelDiem();
                m.mssv = i.MSSV_CTBT;
                m.diem = new List<double>();
                string[] str = i.DIEM_CTBT.Split(" ");
                foreach(var s in str)
                {
                    double d = 0;
                    if (double.TryParse(s, out d)) m.diem.Add(d);
                }
                L.Add(m);
            }
            return L;
        }

        [NoDirectAccess]
        public DoThi VeDoThi(List<modelDiem> m)
        {
            DoThi D = new DoThi();
            D.soLuong = new List<int>();
            D.chiTietDiem = new List<double>();

            List<double> d = new List<double>();
            foreach(var i in m)
            {
                double a = 0;
                foreach(var x in i.diem) { a = x; }
                d.Add(a);
            }

            foreach(var i in d.OrderBy(a => a).Distinct())
            {
                int c = 0;
                foreach(var x in d)
                {
                    if(x == i)
                    {
                        c++;
                    }
                }
                D.soLuong.Add(c);
                D.chiTietDiem.Add(i);
            }
            return D;
        }

        [Authorize]
        public IActionResult DanhSachNhomThiCaNhan()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<DanhSachNhom> DS = new List<DanhSachNhom>();

            var temp = db.HOC_PHAN_THIs
                .Select(a => new { a.HOCKY_HP, a.NAMHOCB_HP, a.NAMHOCK_HP })
                .Distinct()
                .ToList();
            foreach(var item in temp.OrderByDescending(a => a.NAMHOCK_HP))
            {
                DanhSachNhom d = new DanhSachNhom();
                string hk = (item.HOCKY_HP == 1) ? "Học kỳ I" : (item.HOCKY_HP == 2) ? "Học kỳ II" : "Học kỳ hè";
                string nh = item.NAMHOCB_HP.ToString() + " - " + item.NAMHOCK_HP.ToString();
                d.hocPhan = hk + ", " + nh;
                d.danhSachNhom = new List<modelNhom>();

                foreach (var i in db.HOC_PHAN_THIs.Where(a => a.HOCKY_HP == item.HOCKY_HP && a.NAMHOCB_HP == item.NAMHOCB_HP && a.NAMHOCK_HP == item.NAMHOCK_HP).OrderBy(a => a.ID_MHP))
                {
                    foreach(var x in db.NHOM_THIs.Where(a => a.ID_HP == i.ID_HP))
                    {
                        if(x.ID_TK == id)
                        {
                            modelNhom n = new modelNhom();
                            n.id = x.ID_N;
                            n.stt = i.ID_MHP + "." + x.STT_N.ToString().PadLeft(2, '0');
                            n.hanNop = ((DateTime)i.HANNOP_HP).ToString("dd/MM/yyyy");

                            n.slNop = 0;
                            if (x.LINKEXCELDIEM_N != null && x.LINKEXCELDIEM_N != "") n.slNop += 1;
                            if (x.LINKPDFDE_N != null && x.LINKPDFDE_N != "") n.slNop += 1;
                            if (x.LINKPDFDIEM_N != null && x.LINKPDFDIEM_N != "") n.slNop += 1;
                            if (x.LINKZIPBAI_N != null && x.LINKZIPBAI_N != "") n.slNop += 1;
                            if (x.LINKELEARNING_N != null && x.LINKELEARNING_N != "") n.slNop += 1;

                            n.daNop = (bool)x.DANOP_N;
                            d.danhSachNhom.Add(n);
                        }
                    }
                }

                if(d.danhSachNhom.Count > 0)
                {
                    DS.Add(d);
                }
            }

            return View(DS);
        }

        [Authorize]
        public IActionResult ChiTietNhomThiCaNhan(string id)
        {
            NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == id).FirstOrDefault();
            modelNhom m = LayThongTinNhom(N);
            m.diem = new List<modelDiem>();
            m.diem = LayThongTinDiem(N.ID_N);
            m.doThi = VeDoThi(m.diem);
            return View(m);
        }

        [Authorize]
        [NoDirectAccess]
        public IActionResult ChiTietNhomThiCaNhanJson(string id)
        {
            NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == id).FirstOrDefault();
            modelNhom m = LayThongTinNhom(N);
            m.diem = new List<modelDiem>();
            m.diem = LayThongTinDiem(N.ID_N);
            m.doThi = VeDoThi(m.diem);
            List<string> h = new List<string>();
            foreach(var i in db.HINH_THUC_THIs.Select(a => a.TEN_HT))
            {
                h.Add(i);
            }

            var data = new
            {
                nhom = m,
                hinhThuc = h,
                TK = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };
            return Json(data);
        }

        [NoDirectAccess]
        public IActionResult LuuThongTinNhom(modelNhom m)
        {
            var result = "success";
            string idTK = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string currentDir = Directory.GetCurrentDirectory();
            NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == m.id).FirstOrDefault();
            db.Entry(N).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            if (m.ngayThi != null)
            {
                N.ID_HT = (short)m.hinhThuc.id;
                DateTime nThi = DateTime.ParseExact(m.ngayThi, "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);
                N.NGAYTHI_N = nThi;
            }
            N.SISO_N = (m.siSo == null) ? 0 : int.Parse(m.siSo);
            N.SOLUONGTHI_N = (m.thamDu == null) ? 0 : int.Parse(m.thamDu);
            N.SODE_N = (m.soDe == null) ? (short)0 : short.Parse(m.soDe);
            N.SODAPAN_N = (m.soDapAn == null) ? (short)0 : short.Parse(m.soDapAn);

            bool daNop = true;
            if (m.fileZip != null)
            {
                UploadController.DeleteFile(N.LINKZIPBAI_N, currentDir);
                N.LINKZIPBAI_N = UploadController.UploadFile(m.fileZip, m.id, idTK, currentDir);
            }
            else { daNop = false; }
            if (m.filePDFDe != null)
            {
                UploadController.DeleteFile(N.LINKPDFDE_N, currentDir);
                N.LINKPDFDE_N = UploadController.UploadFile(m.filePDFDe, m.id, idTK, currentDir);
            }
            else { daNop = false; }
            if (m.filePDFDiem != null)
            {
                UploadController.DeleteFile(N.LINKPDFDIEM_N, currentDir);
                N.LINKPDFDIEM_N = UploadController.UploadFile(m.filePDFDiem, m.id, idTK, currentDir);
            }
            else { daNop = false; }
            if (m.fileExcel != null)
            {
                string tp = db.HOC_PHAN_THIs.Where(a => a.ID_HP == N.ID_HP).Select(a => a.DIEMTHANHPHAN_HP).FirstOrDefault();
                string[] thanhPhan = tp.Split(" |");
                thanhPhan = thanhPhan.Where(a => a != "").ToArray();
                if (KiemTraExcel(m, idTK, currentDir, thanhPhan.Length))
                {
                    List<modelChiTietDiem> L = LayDiem(m, idTK, currentDir, thanhPhan.Length);
                    // Xoa thong tin cu
                    using(var dbtemp = new QL_THIContext())
                    {
                        foreach (var i in db.CHI_TIET_DIEMs.Where(a => a.ID_N == m.id))
                        {
                            dbtemp.CHI_TIET_DIEMs.Remove(i);
                        }
                        dbtemp.SaveChanges();
                    }
                    // Luu thong tin moi
                    using (var dbtemp = new QL_THIContext())
                    {
                        foreach (var i in L)
                        {
                            CHI_TIET_DIEM C = new CHI_TIET_DIEM()
                            {
                                ID_N = i.idNhom,
                                MSSV_CTBT = i.mssv,
                                DIEM_CTBT = i.diem,
                            };
                            dbtemp.CHI_TIET_DIEMs.Add(C);
                        }
                        dbtemp.SaveChanges();
                    }
                    UploadController.DeleteFile(N.LINKEXCELDIEM_N, currentDir);
                    N.LINKEXCELDIEM_N = UploadController.UploadFile(m.fileExcel, m.id, idTK, currentDir);
                }
                else
                {
                    daNop = false;
                    result = "excel";
                }
            }
            else { daNop = false; }
            if (m.elearning != null)
            {
                N.LINKELEARNING_N = m.elearning;
            }
            else { daNop = false; }

            N.DANOP_N = daNop;

            db.Entry(N).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return Json(result);
        }

        public Boolean KiemTraExcel(modelNhom m, string idTK, string currentDir, int tp)
        {
            bool hopLe = true;
            string link = UploadController.UploadFile(m.fileExcel, m.id, idTK, currentDir);
            var filepath = currentDir + "\\wwwroot" + link;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            try
            {
                using (var stream = System.IO.File.Open(filepath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
                    {
                        while (reader.Read())
                        {
                            for (int i = 1; i <= tp; i++)
                            {
                                double d = 0;
                                string str = reader.GetValue(i).ToString();
                                if (!double.TryParse(str, out d)) hopLe = false;
                            }
                        }
                        reader.Close();
                    }
                    stream.Close();
                }
            }
            catch (Exception)
            {
                hopLe = false;
            }
            UploadController.DeleteFile(link, currentDir);
            return hopLe;
        }

        public List<modelChiTietDiem> LayDiem(modelNhom m, string idTK, string currentDir, int tp)
        {
            string link = UploadController.UploadFile(m.fileExcel, m.id, idTK, currentDir);
            var filepath = currentDir + "\\wwwroot" + link;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            List<modelChiTietDiem> L = new List<modelChiTietDiem>();
            using (var stream = System.IO.File.Open(filepath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        modelChiTietDiem c = new modelChiTietDiem();
                        c.diem = "";
                        c.idNhom = m.id;
                        c.mssv = reader.GetValue(0).ToString();
                        for (int i = 1; i <= tp; i++)
                        {
                            string str = reader.GetValue(i).ToString();
                            c.diem += str + " ";
                        }
                        c.diem = c.diem[..^1];
                        L.Add(c);
                    }
                    reader.Close();
                }
                stream.Close();
            }
            UploadController.DeleteFile(link, currentDir);
            return L;
        }
    }
}
