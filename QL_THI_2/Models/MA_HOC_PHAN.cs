using System;
using System.Collections.Generic;

#nullable disable

namespace QL_THI_2.Models
{
    public partial class MA_HOC_PHAN
    {
        public MA_HOC_PHAN()
        {
            HOC_PHAN_THIs = new HashSet<HOC_PHAN_THI>();
        }

        public int ID_MHP { get; set; }
        public string MA_MHP { get; set; }
        public string TEN_MHP { get; set; }
        public short? TINCHI_MHP { get; set; }

        public virtual ICollection<HOC_PHAN_THI> HOC_PHAN_THIs { get; set; }
    }
}
