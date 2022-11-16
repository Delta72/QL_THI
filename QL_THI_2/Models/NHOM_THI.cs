using System;
using System.Collections.Generic;

#nullable disable

namespace QL_THI_2.Models
{
    public partial class NHOM_THI
    {
        public NHOM_THI()
        {
            CHINH_SUA_DIEMs = new HashSet<CHINH_SUA_DIEM>();
            CHI_TIET_DIEMs = new HashSet<CHI_TIET_DIEM>();
        }

        public string ID_N { get; set; }
        public string ID_HP { get; set; }
        public short ID_HT { get; set; }
        public string ID_TK { get; set; }
        public short? STT_N { get; set; }
        public DateTime? NGAYTHI_N { get; set; }
        public int? SISO_N { get; set; }
        public int? SOLUONGTHI_N { get; set; }
        public short? SODE_N { get; set; }
        public short? SODAPAN_N { get; set; }
        public bool? DANOP_N { get; set; }
        public string LINKZIPBAI_N { get; set; }
        public string LINKEXCELDIEM_N { get; set; }
        public string LINKPDFDE_N { get; set; }
        public string LINKPDFDIEM_N { get; set; }
        public string LINKELEARNING_N { get; set; }

        public virtual HOC_PHAN_THI ID_HPNavigation { get; set; }
        public virtual HINH_THUC_THI ID_HTNavigation { get; set; }
        public virtual TAI_KHOAN ID_TKNavigation { get; set; }
        public virtual ICollection<CHINH_SUA_DIEM> CHINH_SUA_DIEMs { get; set; }
        public virtual ICollection<CHI_TIET_DIEM> CHI_TIET_DIEMs { get; set; }
    }
}
