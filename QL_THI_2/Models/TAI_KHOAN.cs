using System;
using System.Collections.Generic;

#nullable disable

namespace QL_THI_2.Models
{
    public partial class TAI_KHOAN
    {
        public TAI_KHOAN()
        {
            HOC_PHAN_THIs = new HashSet<HOC_PHAN_THI>();
            NHOM_THIs = new HashSet<NHOM_THI>();
            THONG_BAOs = new HashSet<THONG_BAO>();
        }

        public string ID_TK { get; set; }
        public string MK_TK { get; set; }
        public string HOTEN_TK { get; set; }
        public DateTime? NGAYTAO_TK { get; set; }
        public string EMAIL_TK { get; set; }
        public DateTime? LANHDCUOI_TK { get; set; }
        public bool? LAADMIN_TK { get; set; }

        public virtual ICollection<HOC_PHAN_THI> HOC_PHAN_THIs { get; set; }
        public virtual ICollection<NHOM_THI> NHOM_THIs { get; set; }
        public virtual ICollection<THONG_BAO> THONG_BAOs { get; set; }
    }
}
