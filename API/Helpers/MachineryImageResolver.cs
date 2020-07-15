using API.Dtos;
using AutoMapper;
using Core.Entities.Masters;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class MachineryImageResolver : IValueResolver<Category, CategoryToReturnDto, string>
    {
        private readonly IConfiguration _config;
        public MachineryImageResolver(IConfiguration config)
        {
            _config = config;
        }

    
        public string Resolve(Category source, CategoryToReturnDto destination, string destMember, ResolutionContext context)
        {
            /* 
            if (!string.IsNullOrEmpty(source.imageUrl))
            {
                return _config["ApiUrl"] + source.imageUrl;
            }
            */
            return null;
            
        }
    
    }
}