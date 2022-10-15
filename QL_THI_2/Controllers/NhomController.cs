using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using QL_THI_2.Models;
using System.Threading.Tasks;
using System.Security.Claims;

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
                    N.ID_TK = i.idGV;
                    string stt = i.idN;
                    N.STT_N = short.Parse(stt);
                    N.DANOP_N = false;

                    db.NHOM_THIs.Add(N);
                    db.SaveChanges();
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

            D.danhSachNhom = new List<modelNhom>();
            foreach(var i in db.NHOM_THIs.Where(a => a.ID_HP == id))
            {
                modelNhom n = new modelNhom();
                n.id = i.ID_N;
                n.stt = i.STT_N.ToString().PadLeft(2, '0');
                n.hinhThuc = new modelHinhThuc();
                n.hinhThuc.tenHinhThuc = db.HINH_THUC_THIs.Where(a => a.ID_HT == i.ID_HT).Select(a => a.TEN_HT).FirstOrDefault();
                n.taiKhoan = new modelTaiKhoan();
                n.taiKhoan.id = i.ID_TK;
                n.taiKhoan.hoTen = db.TAI_KHOANs.Where(a => a.ID_TK == i.ID_TK).Select(a => a.HOTEN_TK).FirstOrDefault();
                n.taiKhoan.avatar = db.TAI_KHOANs.Where(a => a.ID_TK == i.ID_TK).Select(a => a.ANHDAIDIEN_TK).FirstOrDefault();
                n.daNop = (bool)i.DANOP_N;
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
            return m;
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
            return View(m);
        }

        [Authorize]
        [NoDirectAccess]
        public IActionResult ChiTietNhomThiCaNhanJson(string id)
        {
            NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == id).FirstOrDefault();
            modelNhom m = LayThongTinNhom(N);

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
    }
}
