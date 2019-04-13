using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginCodeCase.Info
{
    //Adres bilgilerinin class ı
    public class Cities
    {
        public List<CityValues> DropDown { get; set; }
    }
    public class CityValues
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class Districts
    {
        public List<DistrictValues> DistrictsDrop { get; set; }
    }
    public class DistrictValues
    {
        public int ID { get; set; }
        public int CityID { get; set; }
        public string Name { get; set; }
    }
}
