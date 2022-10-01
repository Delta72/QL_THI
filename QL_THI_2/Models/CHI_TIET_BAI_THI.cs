using System;
using System.Collections.Generic;

#nullable disable

namespace QL_THI_2.Models
{
    public partial class CHI_TIET_BAI_THI
    {
        public string ID_CTBT { get; set; }
        public string ID_N { get; set; }
        public string MSSV_CTBT { get; set; }
        public string DIEM_CTBT { get; set; }

        public virtual NHOM_THI ID_NNavigation { get; set; }
    }
}
