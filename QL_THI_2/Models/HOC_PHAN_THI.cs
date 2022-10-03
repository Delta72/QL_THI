using System;
using System.Collections.Generic;

#nullable disable

namespace QL_THI_2.Models
{
    public partial class HOC_PHAN_THI
    {
        public HOC_PHAN_THI()
        {
            NHOM_THIs = new HashSet<NHOM_THI>();
        }

        public string ID_HP { get; set; }
        public short ID_HK { get; set; }
        public string ID_TK { get; set; }
        public string ID_MHP { get; set; }
        public string NAMHOCB_HP { get; set; }
        public string NAMHOCK_HP { get; set; }
        public DateTime? HANNOP_HP { get; set; }
        public short? SONHOM_HP { get; set; }

        public virtual HOC_KY ID_HKNavigation { get; set; }
        public virtual MA_HOC_PHAN ID_MHPNavigation { get; set; }
        public virtual TAI_KHOAN ID_TKNavigation { get; set; }
        public virtual ICollection<NHOM_THI> NHOM_THIs { get; set; }
    }
}
