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
                short count = 1;
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
                    N.STT_N = count;
                    count++;
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
            string mon = db.MA_HOC_PHANs.Where(a => a.ID_MHP == H.ID_MHP).Select(a => a.MA_MHP).FirstOrDefault() + " - " + db.MA_HOC_PHANs.Where(a => a.ID_MHP == H.ID_MHP).Select(a => a.TEN_MHP).FirstOrDefault();
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
            D.danhSachNhom = D.danhSachNhom.OrderBy(a => a.stt).ToList();
            return View(D);
        }

        [Authorize]
        public IActionResult ChiTietNhom(string id)
        {
            modelNhom m = new modelNhom();
            NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == id).FirstOrDefault();
            m = LayThongTinNhom(N);
            m.diem = new List<DanhSachDiem>();
            m.diem = LayThongTinDiem(N.ID_N);
            m.doThi = VeDoThi(m.diem);
            return View(m);
        }

        [NoDirectAccess]
        public static modelNhom LayThongTinNhom(NHOM_THI N)
        {
            using(var db = new QL_THIContext())
            {
                modelNhom m = new modelNhom();
                m.id = N.ID_N;
                m.stt = N.STT_N.ToString().PadLeft(2, '0');
                m.hinhThuc = new modelHinhThuc();
                m.hinhThuc.id = N.ID_HT;
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
                m.hanNop = ((DateTime)H.HANNOP_HP).ToString("dd/MM/yyyy");
                string hk = (H.HOCKY_HP == 1) ? "Học kỳ I" : (H.HOCKY_HP == 2) ? "Học kỳ II" : "Học kỳ hè";
                string nh = H.NAMHOCB_HP.ToString() + " - " + H.NAMHOCK_HP.ToString();
                string mon = H.ID_MHP + " - " + db.MA_HOC_PHANs.Where(a => a.ID_MHP == H.ID_MHP).Select(a => a.TEN_MHP).FirstOrDefault();
                m.duongDan = hk + ", " + nh + " > " + mon + " > " + "Nhóm " + m.stt;
                string[] thanhPhan = H.DIEMTHANHPHAN_HP.Split(" |");
                thanhPhan = thanhPhan.Where(a => a != "").ToArray();
                m.thanhPhan = new List<string>();
                foreach (var i in thanhPhan) { m.thanhPhan.Add(i); }
                m.hocPhan = new modelHocPhan();
                m.hocPhan = HocPhanController.LayThongTinHP(H.ID_HP);
                return m;
            }
        }

        [NoDirectAccess]
        public List<DanhSachDiem> LayThongTinDiem(string idN)
        {
            List<DanhSachDiem> L = new List<DanhSachDiem>();
            CHINH_SUA_DIEM csd = db.CHINH_SUA_DIEMs.Where(a => a.ID_N == idN).OrderBy(a => a.LANCHINHSUA_V).LastOrDefault();
            if (csd != null)
            {
                int max = (csd == null) ? 0 : (int)csd.LANCHINHSUA_V;
                for (int i = 0; i <= max; i++)
                {
                    List<CHINH_SUA_DIEM> Lcsd = db.CHINH_SUA_DIEMs.Where(a => a.ID_N == idN && a.LANCHINHSUA_V == i).ToList();
                    foreach (var item in Lcsd)
                    {
                        List<CHI_TIET_DIEM> Lctd = db.CHI_TIET_DIEMs.Where(a => a.ID_N == idN & a.SOCHINHSUA_CTBT == item.LANCHINHSUA_V).ToList();
                        DanhSachDiem D = new DanhSachDiem();
                        D.ds = new List<modelDiem>();
                        if(item.LYDO_V != "Xóa file excel")
                        {
                            foreach (var val in Lctd)
                            {
                                modelDiem m = new modelDiem();
                                m.mssv = val.MSSV_CTBT;
                                m.diem = new List<double>();
                                string[] str = val.DIEM_CTBT.Split(" ");
                                foreach (var s in str)
                                {
                                    double d = 0;
                                    if (double.TryParse(s, out d)) m.diem.Add(d);
                                }
                                D.ds.Add(m);
                            }
                        }
                        D.lanChinhSua = (int)item.LANCHINHSUA_V;
                        D.lyDo = item.LYDO_V;
                        D.ngaySua = ((DateTime)item.THOIGIAN_V).ToString("dd/MM/yyyy");
                        L.Add(D);
                    }
                }
            }
            return L;
        }

        [NoDirectAccess]
        public List<DoThi> VeDoThi(List<DanhSachDiem> m)
        {
            List<DoThi> DT = new List<DoThi>();
            foreach(var val in m)
            {
                DoThi D = new DoThi();
                D.soLuong = new List<int>();
                D.chiTietDiem = new List<double>();

                List<double> d = new List<double>();
                foreach (var i in val.ds)
                {
                    double a = 0;
                    foreach (var x in i.diem) { a = x; }
                    d.Add(a);
                }

                foreach (var i in d.OrderBy(a => a).Distinct())
                {
                    int c = 0;
                    foreach (var x in d)
                    {
                        if (x == i)
                        {
                            c++;
                        }
                    }
                    D.soLuong.Add(c);
                    D.chiTietDiem.Add(i);
                }
                DT.Add(D);
            }
            
            return DT;
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
                    d.chiTietHP = new modelHocPhan();
                    d.chiTietHP = HocPhanController.LayThongTinHP(i.ID_HP);

                    foreach (var x in db.NHOM_THIs.Where(a => a.ID_HP == i.ID_HP))
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
            m.diem = new List<DanhSachDiem>();
            m.diem = LayThongTinDiem(N.ID_N);
            m.doThi = VeDoThi(m.diem);
            ViewData["Diem"] = (m.diem.Count > 0) ? m.diem.Last().lanChinhSua : 0;
            return View(m);
        }

        [Authorize]
        [NoDirectAccess]
        public IActionResult ChiTietNhomThiCaNhanJson(string id)
        {
            NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == id).FirstOrDefault();
            modelNhom m = LayThongTinNhom(N);
            m.diem = new List<DanhSachDiem>();
            m.diem = LayThongTinDiem(N.ID_N);
            m.doThi = VeDoThi(m.diem);
            List<string> h = new List<string>();
            foreach (var i in db.HINH_THUC_THIs.Select(a => a.TEN_HT))
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
            else
            {
                N.NGAYTHI_N = DateTime.Now;
            }
            N.SISO_N = (m.siSo == null) ? 0 : int.Parse(m.siSo);
            N.SOLUONGTHI_N = (m.thamDu == null) ? 0 : int.Parse(m.thamDu);
            N.SODE_N = (m.soDe == null) ? (short)0 : short.Parse(m.soDe);
            N.SODAPAN_N = (m.soDapAn == null) ? (short)0 : short.Parse(m.soDapAn);
            if (m.fileZip != null)
            {
                UploadController.DeleteFile(N.LINKZIPBAI_N, currentDir);
                N.LINKZIPBAI_N = UploadController.UploadFile(m.fileZip, m.id, idTK, currentDir);
            }
            if (m.filePDFDe != null)
            {
                UploadController.DeleteFile(N.LINKPDFDE_N, currentDir);
                N.LINKPDFDE_N = UploadController.UploadFile(m.filePDFDe, m.id, idTK, currentDir);
            }
            if (m.filePDFDiem != null)
            {
                UploadController.DeleteFile(N.LINKPDFDIEM_N, currentDir);
                N.LINKPDFDIEM_N = UploadController.UploadFile(m.filePDFDiem, m.id, idTK, currentDir);
            }
            if (m.fileExcel != null)
            {
                string tp = db.HOC_PHAN_THIs.Where(a => a.ID_HP == N.ID_HP).Select(a => a.DIEMTHANHPHAN_HP).FirstOrDefault();
                string[] thanhPhan = tp.Split(" |");
                thanhPhan = thanhPhan.Where(a => a != "").ToArray();
                if (KiemTraExcel(m, idTK, currentDir, thanhPhan.Length))
                {
                    try
                    {
                        List<modelChiTietDiem> L = LayDiem(m, idTK, currentDir, thanhPhan.Length);
                        var m2 = L.First(); var idN = m2.idNhom;
                        // Luu thong tin moi
                        using (var dbtemp = new QL_THIContext())
                        {
                            CHINH_SUA_DIEM CS = db.CHINH_SUA_DIEMs.Where(a => a.ID_N == idN).OrderBy(a => a.LANCHINHSUA_V).LastOrDefault();
                            int scs = 0;
                            if(CS == null)
                            {
                                CS = new CHINH_SUA_DIEM();
                                CS.ID_N = idN;
                                CS.LANCHINHSUA_V = 0;
                                CS.LYDO_V = "Thêm file excel";
                                CS.THOIGIAN_V = DateTime.Now;
                                dbtemp.CHINH_SUA_DIEMs.Add(CS);
                            }
                            else
                            {
                                CHINH_SUA_DIEM CS2 = new CHINH_SUA_DIEM()
                                {
                                    ID_N = CS.ID_N,
                                    LANCHINHSUA_V = CS.LANCHINHSUA_V + 1,
                                    THOIGIAN_V = DateTime.Now,
                                    LYDO_V = "Cập nhật file excel",
                                };
                                dbtemp.CHINH_SUA_DIEMs.Add(CS2);
                                scs = CS2.LANCHINHSUA_V;
                            }
                            foreach (var i in L)
                            {
                                CHI_TIET_DIEM C = new CHI_TIET_DIEM()
                                {
                                    ID_N = i.idNhom,
                                    MSSV_CTBT = i.mssv,
                                    DIEM_CTBT = i.diem,
                                    SOCHINHSUA_CTBT = scs,
                                };
                                dbtemp.CHI_TIET_DIEMs.Add(C);
                            }
                            dbtemp.SaveChanges();
                        }
                        if(N.LINKEXCELDIEM_N != null)
                        {
                            UploadController.DeleteFile(N.LINKEXCELDIEM_N, currentDir);
                        }
                        N.LINKEXCELDIEM_N = UploadController.UploadFile(m.fileExcel, m.id, idTK, currentDir);
                    }
                    catch (Exception)
                    {
                        result = "excel";
                    }
                }
                else
                {
                    result = "excel";
                }
            }
            if (m.elearning != null)
            {
                N.LINKELEARNING_N = m.elearning;
            }

            bool daNop = (N.LINKZIPBAI_N == null) ? false
                : (N.LINKPDFDE_N == null) ? false
                : (N.LINKPDFDIEM_N == null) ? false
                : (N.LINKEXCELDIEM_N == null) ? false
                : true;
            if (N.LINKELEARNING_N != null) daNop = true;
            N.DANOP_N = daNop;

            db.Entry(N).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return Json(result);
        }

        [NoDirectAccess]
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
                        List<string> L = new List<string>();
                        while (reader.Read())
                        {
                            for (int i = 0; i <= tp; i++)
                            {
                                if(i == 0)
                                {
                                    string str = reader.GetValue(i).ToString();
                                    foreach (var x in L)
                                    {
                                        if(x == str) hopLe = false;
                                    }
                                    L.Add(str);
                                }
                                else
                                {
                                    double d = 0;
                                    string str = reader.GetValue(i).ToString();
                                    if (!double.TryParse(str, out d)) hopLe = false;
                                }
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

        [NoDirectAccess]
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
                            double d = Math.Round(double.Parse(str), 1);
                            c.diem += d.ToString() + " ";
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

        [NoDirectAccess]
        public static void ChinhSuaDanhSachNhom(dynamic d, string idHP)
        {
            List<string> L = new List<string>();
            List<string> Lid = new List<string>();
            using (var db = new QL_THIContext())
            {
                // them/chinh sua nhom
                foreach(var i in d)
                {
                    string idN = i.id;
                    short stt = i.stt;
                    NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == idN).FirstOrDefault();
                    if (N != null)
                    {
                        // chinh sua stt nhom da co
                        db.Entry(N).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                        N.STT_N = stt;
                        db.Entry(N).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        Lid.Add(N.ID_N);
                    }
                    else
                    {
                        // them nhom chua co
                        N = new NHOM_THI();
                        N.ID_N = TimIDNhom(Guid.NewGuid().ToString());
                        N.ID_TK = idN;
                        N.ID_HP = idHP;
                        N.ID_HT = 1;
                        N.STT_N = stt;
                        N.DANOP_N = false;
                        db.NHOM_THIs.Add(N);

                        var p = @"wwwroot\user\" + N.ID_TK + @"\" + N.ID_N;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), p);
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        Lid.Add(N.ID_N);
                    }
                }
                db.SaveChanges();
            }
            using(var db = new QL_THIContext())
            {
                // xoa nhom ko co
                foreach (var i in db.NHOM_THIs.Where(a => a.ID_HP == idHP))
                {
                    L.Add(i.ID_N);
                }
                foreach(var i in L)
                {
                    if(Lid.Where(a => a == i).FirstOrDefault() == null)
                    {
                        NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == i).FirstOrDefault();
                        var p = new DirectoryInfo(Directory.GetCurrentDirectory() +  @"\user\" + N.ID_TK + @"\" + N.ID_N);
                        if(p != null) p.Delete();
                        db.NHOM_THIs.Remove(N);
                    }
                }
                db.SaveChanges();
            }
        }

        [NoDirectAccess]
        public IActionResult KiemTraXoaNhom(string id)
        {
            var report = "";
            try
            {
                NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == id).FirstOrDefault();
                if(N.LINKELEARNING_N != null || N.LINKEXCELDIEM_N != null || N.LINKPDFDE_N != null || N.LINKZIPBAI_N != null || N.LINKPDFDIEM_N != null)
                {
                    report = "submitted";
                }
            }
            catch (Exception)
            {
                report = "error";
            }
            return Json(report);
        }

        [NoDirectAccess]
        public IActionResult XoaFile(string id, string type)
        {
            try
            {
                NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == id).First();
                db.Entry(N).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                if(type == "zip")
                {
                    UploadController.DeleteFile(N.LINKZIPBAI_N, Directory.GetCurrentDirectory());
                    N.LINKZIPBAI_N = null;
                }
                if(type == "pdfDe")
                {
                    UploadController.DeleteFile(N.LINKPDFDE_N, Directory.GetCurrentDirectory());
                    N.LINKPDFDE_N = null;
                }
                if (type == "excel")
                {
                    UploadController.DeleteFile(N.LINKEXCELDIEM_N, Directory.GetCurrentDirectory());
                    using (var dtb = new QL_THIContext())
                    {
                        CHINH_SUA_DIEM C = dtb.CHINH_SUA_DIEMs.Where(a => a.ID_N == N.ID_N).OrderBy(a => a.LANCHINHSUA_V).Last();
                        CHINH_SUA_DIEM C2 = new CHINH_SUA_DIEM()
                        {
                            ID_N = C.ID_N,
                            LANCHINHSUA_V = C.LANCHINHSUA_V + 1,
                            THOIGIAN_V = DateTime.Now,
                            LYDO_V = "Xóa file excel"
                        };
                        dtb.CHINH_SUA_DIEMs.Add(C2);
                        dtb.SaveChanges();
                    }
                    N.LINKEXCELDIEM_N = null;
                }
                if (type == "pdfDiem")
                {
                    UploadController.DeleteFile(N.LINKPDFDIEM_N, Directory.GetCurrentDirectory());
                    N.LINKPDFDIEM_N = null;
                }
                db.Entry(N).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
                return Json(true);
            }
            catch (Exception)
            {
                return Json("error");
            }
        }
    }
}
