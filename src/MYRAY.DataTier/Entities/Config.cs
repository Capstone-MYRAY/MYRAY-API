using System;
using System.Collections.Generic;

namespace MYRAY.DataTier.Entities
{
    public partial class Config
    {
        public int Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}
