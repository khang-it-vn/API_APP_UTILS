using System;
using System.Collections.Generic;

namespace AppUtils.Data
{
    public class Repairer : Person
    {
        public Guid ID { get; set; }
        public bool TrangThaiHoatDong { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
        
    }
}
