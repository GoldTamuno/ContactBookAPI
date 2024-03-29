using ContactBook.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactBook.Infrastructure;

namespace ContactBook.Core
{
    public interface IAppUserRepository
    {
        Task<List<AppUserDTO>> Search(string term);
        Task<List<AppUserDTO>> GetAllUsers(PaginParameter userParameter);
        Task<AppUserDTO> GetUserById(string id);
        Task<string> AddUser(AppUserDTO appUserdto);
        Task<bool> UploadImage(string id, PhotoDTO photo);
    }
}
