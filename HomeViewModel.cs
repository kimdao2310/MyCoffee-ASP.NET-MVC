using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLMYCF_65130390.Models
{
    public class HomeViewModel
    {
        public List<QUANG_CAO> Banners { get; set; }
        public List<SAN_PHAM> SanPhamMoi { get; set; }
        public List<TIN_TUC> TinTucMoi { get; set; }
    }
}