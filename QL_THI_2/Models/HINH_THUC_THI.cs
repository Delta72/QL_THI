using System;
using System.Collections.Generic;

#nullable disable

namespace QL_THI_2.Models
{
    public partial class HINH_THUC_THI
    {
        public HINH_THUC_THI()
        {
            NHOM_THIs = new HashSet<NHOM_THI>();
        }

        public short ID_HT { get; set; }
        public string TEN_HT { get; set; }

        public virtual ICollection<NHOM_THI> NHOM_THIs { get; set; }
    }
}
