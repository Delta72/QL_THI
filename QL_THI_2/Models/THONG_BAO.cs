using System;
using System.Collections.Generic;

#nullable disable

namespace QL_THI_2.Models
{
    public partial class THONG_BAO
    {
        public string ID_TB { get; set; }
        public string ID_TK { get; set; }
        public string TUADE_TB { get; set; }
        public string NOIDUNG_TB { get; set; }
        public DateTime? THOIGIAN_TB { get; set; }

        public virtual TAI_KHOAN ID_TKNavigation { get; set; }
    }
}
