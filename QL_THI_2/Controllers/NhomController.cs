using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using QL_THI_2.Models;
using System.Threading.Tasks;

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
            string hk = db.HOC_Kies.Where(a => a.ID_HK == H.ID_HK).Select(a => a.TEN_HK).FirstOrDefault();
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
            m.id = N.ID_N;
            m.stt = N.STT_N.ToString().PadLeft(2, '0');
            m.hinhThuc = new modelHinhThuc();
            m.hinhThuc.tenHinhThuc = db.HINH_THUC_THIs.Where(a => a.ID_HT == N.ID_HT).Select(a => a.TEN_HT).FirstOrDefault();
            m.taiKhoan = new modelTaiKhoan();
            m.taiKhoan.id = N.ID_TK;
            m.taiKhoan.hoTen = db.TAI_KHOANs.Where(a => a.ID_TK == N.ID_TK).Select(a => a.HOTEN_TK).FirstOrDefault();
            m.taiKhoan.avatar = db.TAI_KHOANs.Where(a => a.ID_TK == N.ID_TK).Select(a => a.ANHDAIDIEN_TK).FirstOrDefault();
            m.daNop = (bool)N.DANOP_N;
            m.ngayThi = (N.NGAYTHI_N != null) ? ((DateTime)N.NGAYTHI_N).ToString() : "---";
            m.siSo = (N.TONGSO_N != null) ? (N.TONGSO_N.ToString().PadLeft(2, '0')) : "---";
            m.thamDu = (N.SOLUONGTHI_N != null) ? (N.SOLUONGTHI_N.ToString().PadLeft(2, '0')) : "---";
            m.zip = (N.LINKZIP_N != null) ? (N.LINKZIP_N) : null;
            m.excel = (N.LINKEXCEL_N != null) ? (N.LINKEXCEL_N) : null;
            m.soDe = (N.SODE_N != null) ? (N.SODE_N.ToString().PadLeft(2, '0')) : null;
            m.soDapAn = (N.SODAPAN_N != null) ? (N.SODAPAN_N.ToString().PadLeft(2, '0')) : null;

            return View(m);
        }
    }
}
