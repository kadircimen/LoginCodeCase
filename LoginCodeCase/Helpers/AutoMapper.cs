using LoginCodeCase.Info;
using LoginCodeCase.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LoginCodeCase.Helpers
{
    //AutoMapper ile map edilen sınıfların create edildiği sınıf
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
