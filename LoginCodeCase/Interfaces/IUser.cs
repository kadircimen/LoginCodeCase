using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginCodeCase.Model;
namespace LoginCodeCase.Interfaces
{
    public interface IUser
    {
        //IUser soyut nesnesinden türetilen servisin interface'i
        User Authenticate(string email, string password);
        IEnumerable<User> GetAll();
        User GetByID(int id);
        User GetByEmail(string mail);
        User Create(User user, string password);
        void Update(int UserID, string password = null);
        void Delete(int id);
    }
}
