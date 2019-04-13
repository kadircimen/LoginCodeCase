using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoginCodeCase.Info
{
    //Password hashleme bulunduğundan Context'teki user ile kayıt edilen kullanıcı formu eşleşmemektedir.
    //AutoMapper ile bu kısımda belirtilen UserDto sınıfı User ile map edilmekte, kayıt işlemleri bu şekilde gerçekleşmektedir.
    public class UserDto
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDay { get; set; }
        public string Password { get; set; }
        public int CityID { get; set; }
        public string DistrictID { get; set; }
    }
    public class ChangePass
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
