using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoginCodeCase.Model
{
    //Kullanıcı Tablosu
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDay { get; set; }
        public byte[] passHash { get; set; }
        public byte[] passSalt { get; set; }
        public int CityID { get; set; }
        public string DistrictID { get; set; }
    }
    public class UserLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
