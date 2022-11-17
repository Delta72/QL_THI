using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QL_THI_2.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

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
                    ma = i.MA_MHP,
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

        [NoDirectAccess]
        public IActionResult LocNhom(string txtSearch, string mhp, string ht, string hk, string nh, string date, string daThi, string daNop, string gv)
        {
            txtSearch = txtSearch == null ? "" : txtSearch;
            short hinhThuc = short.Parse(ht);
            short hocKy = short.Parse(hk);
            short d = short.Parse(date);
            dynamic GV = JsonConvert.DeserializeObject(gv);

            List<NHOM_THI> Na = new List<NHOM_THI>();
            List<NHOM_THI> Nb = new List<NHOM_THI>();

            // tim theo txtSearch
            if(txtSearch != null)
            {
                foreach(var i in db.NHOM_THIs)
                {
                    HOC_PHAN_THI H = db.HOC_PHAN_THIs.Where(a => a.ID_HP == i.ID_HP).First();
                    string hp = H.ID_MHP + " " + db.MA_HOC_PHANs.Where(a => a.ID_MHP == H.ID_MHP).Select(a => a.TEN_MHP).First();
                    TAI_KHOAN T = db.TAI_KHOANs.Where(a => a.ID_TK == i.ID_TK).FirstOrDefault();
                    string tk = T.DN_TK + " " + T.HOTEN_TK + " " + T.EMAIL_TK;
                    hp = hp.ToLower(); tk = tk.ToLower();
                    if(hp.Contains(txtSearch.ToLower()) || tk.Contains(txtSearch.ToLower()))
                    {
                        Na.Add(i);
                    }
                }
            }
            else
            {
                Na = db.NHOM_THIs.ToList();
            }

            // tim theo mhp
            if(mhp != "0")
            {
                foreach (var i in Na.ToList())
                {
                    HOC_PHAN_THI H = db.HOC_PHAN_THIs.Where(a => a.ID_HP == i.ID_HP).First();
                    if (int.Parse(mhp) != H.ID_MHP)
                    {
                        Na.Remove(i);
                    }
                }
            }

            // tim theo hinhthuc
            if(hinhThuc != 0)
            {
                foreach (var i in Na.ToList())
                {
                    if (i.ID_HT != hinhThuc)
                    {
                        Na.Remove(i);
                    }
                }
            }

            // tim theo hoc ky
            if(hocKy != 0)
            {
                foreach (var i in Na.ToList())
                {
                    HOC_PHAN_THI H = db.HOC_PHAN_THIs.Where(a => a.ID_HP == i.ID_HP).First();
                    if (H.HOCKY_HP != hocKy)
                    {
                        Na.Remove(i);
                    }
                }
            }

            // tim theo nam hoc
            if(nh != "0")
            {
                foreach (var i in Na.ToList())
                {
                    HOC_PHAN_THI H = db.HOC_PHAN_THIs.Where(a => a.ID_HP == i.ID_HP).First();
                    if (H.NAMHOCB_HP != nh)
                    {
                        Na.Remove(i);
                    }
                }
            }

            // tim theo ngay
            if(date != "0")
            {
                foreach(var i in Na.ToList())
                {
                    if (i.NGAYTHI_N == null) Na.Remove(i);
                }
                DateTime D = DateTime.Now;
                if (date == "1")
                {
                    foreach (var i in Na.ToList())
                    {
                        DateTime iD = (DateTime)i.NGAYTHI_N;
                        if((D - iD).TotalDays > 7 || (iD - D).TotalDays > 7)
                        {
                            double c = (D - iD).TotalDays;
                            Na.Remove(i);
                        }
                    }
                }
                if (date == "2")
                {
                    foreach (var i in Na.ToList())
                    {
                        DateTime iD = (DateTime)i.NGAYTHI_N;
                        if ((D - iD).TotalDays > 30 || (iD - D).TotalDays > 30)
                        {
                            Na.Remove(i);
                        }
                    }
                }
                if (date == "3")
                {
                    foreach (var i in Na.ToList())
                    {
                        DateTime iD = (DateTime)i.NGAYTHI_N;
                        if ((D - iD).TotalDays > 90 || (iD - D).TotalDays > 90)
                        {
                            Na.Remove(i);
                        }
                    }
                }
                if (date == "4")
                {
                    foreach (var i in Na.ToList())
                    {
                        DateTime iD = (DateTime)i.NGAYTHI_N;
                        if ((D - iD).TotalDays > 180 || (iD - D).TotalDays > 180)
                        {
                            Na.Remove(i);
                        }
                    }
                }
                if (date == "5")
                {
                    foreach (var i in Na.ToList())
                    {
                        DateTime iD = (DateTime)i.NGAYTHI_N;
                        if ((D - iD).TotalDays > 365 || (iD - D).TotalDays > 365)
                        {
                            Na.Remove(i);
                        }
                    }
                }
            }

            // theo da thi
            if(daThi != "0")
            {
                DateTime D = DateTime.Now;
                if(daThi == "1")
                {
                    foreach (var i in Na.ToList())
                    {
                        if (i.NGAYTHI_N == null)
                        {
                            Na.Remove(i);
                        }
                        else if (i.NGAYTHI_N != null && (DateTime)i.NGAYTHI_N > D)
                        {
                                Na.Remove(i);
                        }
                    }
                }
                else
                {
                    foreach (var i in Na.ToList())
                    {
                        if (i.NGAYTHI_N != null)
                        {
                            if((DateTime)i.NGAYTHI_N < D)
                            {
                                Na.Remove(i);
                            }
                        }
                    }
                }
            }

            // da nop
            if (daNop != "0")
            {
                if (daNop == "1")
                {
                    foreach (var i in Na.ToList())
                    {
                        if (i.DANOP_N == false)
                        {
                            Na.Remove(i);
                        }
                    }
                }
                else
                {
                    foreach (var i in Na.ToList())
                    {
                        if (i.DANOP_N == true)
                        {
                            Na.Remove(i);
                        }
                    }
                }
            }

            // theo giao vien
            List<string> MGV = new List<string>();
            foreach (var i in GV)
            {
                if (i != null)
                {
                    string str = i;
                    MGV.Add(str.Trim('{', '}'));
                }
            }
            if(MGV.Count() > 0)
            {
                foreach (var i in MGV)
                {
                    foreach(var x in Na.ToList())
                    {
                        if(x.ID_TK == i)
                        {
                            Nb.Add(x);
                        }
                    }
                }
                Na = Nb.OrderBy(a => a.ID_N).Distinct().ToList();
            }

            List<modelNhom> L = new List<modelNhom>();
            foreach(var i in Na)
            {
                L.Add(NhomController.LayThongTinNhom(i));
            }

            return View(L.OrderBy(a => a.duongDan));
        }

        [Authorize]
        public IActionResult TimKiemSinhVien(string page)
        {
            int p = page != null ? int.Parse(page) : 1;
            int soSV = 100;
            int skip = (p - 1)*soSV;
            DanhSachSinhVien D = new DanhSachSinhVien();
            List<modelSinhVien> L = new List<modelSinhVien>();

            foreach(var i in db.CHI_TIET_DIEMs.OrderBy(a => a.MSSV_CTBT).Skip(skip).Take(soSV))
            {
                int max = (db.CHINH_SUA_DIEMs.Where(a => a.ID_N == i.ID_N).OrderBy(a => a.LANCHINHSUA_V).LastOrDefault()).LANCHINHSUA_V;
                if(i.SOCHINHSUA_CTBT == max)
                {
                    modelSinhVien m = new modelSinhVien();
                    m.id = i.MSSV_CTBT;
                    m.diem = new List<string>();
                    foreach (var s in i.DIEM_CTBT.Split(" ").Where(a => a != ""))
                    {
                        m.diem.Add(s);
                    }
                    m.nhom = new modelNhom();
                    m.nhom = NhomController.LayThongTinNhom(db.NHOM_THIs.Where(a => a.ID_N == i.ID_N).First());
                    L.Add(m);
                }
            }
            D.sinhVien = new List<modelSinhVien>();
            D.sinhVien = L;
            D.soTrang = (db.CHI_TIET_DIEMs.Count() % soSV == 0) 
                ? (db.CHI_TIET_DIEMs.Count() / soSV) 
                : ((db.CHI_TIET_DIEMs.Count() / soSV) + 1);
            D.trangHienTai = p;
            return View(D);
        }

        [NoDirectAccess]
        public IActionResult LocSV(string txtSearch, string mhp, string ht, string hk, string nh, string date, string gv, string page)
        {
            txtSearch = txtSearch == null ? "" : txtSearch.ToLower();
            int p = page == null ? 1 : int.Parse(page);
            int soSV = 100;
            int skip = (p - 1) * soSV;
            short hinhThuc = short.Parse(ht);
            short hocKy = short.Parse(hk);
            short d = short.Parse(date);
            dynamic GV = JsonConvert.DeserializeObject(gv);

            List<CHI_TIET_DIEM> Na = new List<CHI_TIET_DIEM>();
            List<CHI_TIET_DIEM> Nb = new List<CHI_TIET_DIEM>();

            // tim theo txtSearch
            if (txtSearch != null)
            {
                foreach (var i in db.CHI_TIET_DIEMs.OrderBy(a => a.MSSV_CTBT).ToList())
                {
                    var mssv = i.MSSV_CTBT;
                    NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == i.ID_N).First();
                    HOC_PHAN_THI H = db.HOC_PHAN_THIs.Where(a => a.ID_HP == N.ID_HP).First();
                    string hp = H.ID_MHP + " " + db.MA_HOC_PHANs.Where(a => a.ID_MHP == H.ID_MHP).Select(a => a.TEN_MHP).First();
                    TAI_KHOAN T = db.TAI_KHOANs.Where(a => a.ID_TK == N.ID_TK).FirstOrDefault();
                    string tk = T.DN_TK + " " + T.HOTEN_TK + " " + T.EMAIL_TK;
                    hp = hp.ToLower(); tk = tk.ToLower();
                    if (mssv.ToLower().Contains(txtSearch) || hp.Contains(txtSearch) || tk.Contains(txtSearch))
                    {
                        Na.Add(i);
                    }
                }
            }
            else
            {
                Na = db.CHI_TIET_DIEMs.OrderBy(a => a.MSSV_CTBT).ToList();
            }

            // mhp
            if (mhp != "0")
            {
                foreach (var i in Na.ToList())
                {
                    NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == i.ID_N).First();
                    HOC_PHAN_THI H = db.HOC_PHAN_THIs.Where(a => a.ID_HP == N.ID_HP).First();
                    if (int.Parse(mhp) != H.ID_MHP)
                    {
                        Na.Remove(i);
                    }
                }
            }

            // tim theo hinhthuc
            if (hinhThuc != 0)
            {
                foreach (var i in Na.ToList())
                {
                    NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == i.ID_N).FirstOrDefault();
                    if (N.ID_HT != hinhThuc)
                    {
                        Na.Remove(i);
                    }
                }
            }

            // tim theo hoc ky
            if (hocKy != 0)
            {
                foreach (var i in Na.ToList())
                {
                    NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == i.ID_N).FirstOrDefault();
                    HOC_PHAN_THI H = db.HOC_PHAN_THIs.Where(a => a.ID_HP == N.ID_HP).First();
                    if (H.HOCKY_HP != hocKy)
                    {
                        Na.Remove(i);
                    }
                }
            }

            // tim theo nam hoc
            if (nh != "0")
            {
                foreach (var i in Na.ToList())
                {
                    NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == i.ID_N).FirstOrDefault();
                    HOC_PHAN_THI H = db.HOC_PHAN_THIs.Where(a => a.ID_HP == N.ID_HP).First();
                    if (H.NAMHOCB_HP != nh)
                    {
                        Na.Remove(i);
                    }
                }
            }

            // tim theo ngay
            if (date != "0")
            {
                DateTime D = DateTime.Now;
                foreach(var i in Na.ToList())
                {
                    NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == i.ID_N).FirstOrDefault();
                    DateTime iD = (DateTime)N.NGAYTHI_N;
                    if(date == "1")
                    {
                        if ((D - iD).TotalDays > 7 || (iD - D).TotalDays > 7)
                        {
                            Na.Remove(i);
                        }
                    }
                    if(date == "2")
                    {
                        if ((D - iD).TotalDays > 30 || (iD - D).TotalDays > 30)
                        {
                            Na.Remove(i);
                        }
                    }
                    if(date == "3")
                    {
                        if ((D - iD).TotalDays > 90 || (iD - D).TotalDays > 90)
                        {
                            Na.Remove(i);
                        }
                    }
                    if(date == "4")
                    {
                        if ((D - iD).TotalDays > 180 || (iD - D).TotalDays > 180)
                        {
                            Na.Remove(i);
                        }
                    }
                    if(date == "5")
                    {
                        if ((D - iD).TotalDays > 365 || (iD - D).TotalDays > 365)
                        {
                            Na.Remove(i);
                        }
                    }
                }
            }

            // theo giao vien
            List<string> MGV = new List<string>();
            foreach (var i in GV)
            {
                if (i != null)
                {
                    string str = i;
                    MGV.Add(str.Trim('{', '}'));
                }
            }
            if (MGV.Count() > 0)
            {
                foreach (var i in MGV)
                {
                    foreach (var x in Na.ToList())
                    {
                        NHOM_THI N = db.NHOM_THIs.Where(a => a.ID_N == x.ID_N).FirstOrDefault();
                        if (N.ID_TK == i)
                        {
                            Nb.Add(x);
                        }
                    }
                }
                Na = Nb.GroupBy(a => new { a.ID_N, a.MSSV_CTBT }).Select(g => g.FirstOrDefault()).ToList();
            }

            List<modelSinhVien> L = new List<modelSinhVien>();
            foreach(var i in Na)
            {
                modelSinhVien m = new modelSinhVien();
                m.id = i.MSSV_CTBT;
                m.diem = new List<string>();
                foreach (var s in i.DIEM_CTBT.Split(" ").Where(a => a != ""))
                {
                    m.diem.Add(s);
                }
                m.nhom = new modelNhom();
                m.nhom = NhomController.LayThongTinNhom(db.NHOM_THIs.Where(a => a.ID_N == i.ID_N).First());
                L.Add(m);
            }
            DanhSachSinhVien Ds = new DanhSachSinhVien();
            Ds.sinhVien = new List<modelSinhVien>();
            Ds.sinhVien = L.Skip(skip).Take(soSV).ToList();
            Ds.soTrang = (Na.Count() % soSV == 0)
                ? (Na.Count() / soSV)
                : ((Na.Count() / soSV) + 1);
            Ds.trangHienTai = p;
            return View(Ds);
        }
    }
}
