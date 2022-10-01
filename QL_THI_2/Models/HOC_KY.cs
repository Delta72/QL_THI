using System;
using System.Collections.Generic;

#nullable disable

namespace QL_THI_2.Models
{
    public partial class HOC_KY
    {
        public HOC_KY()
        {
            HOC_PHAN_THIs = new HashSet<HOC_PHAN_THI>();
        }

        public short ID_HK { get; set; }
        public string TEN_HK { get; set; }

        public virtual ICollection<HOC_PHAN_THI> HOC_PHAN_THIs { get; set; }
    }
}
