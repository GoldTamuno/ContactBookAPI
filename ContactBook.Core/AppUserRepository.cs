using ContactBook.Core.DTO;
using ContactBook.Model.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactBook.Data;
using ContactBook.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ContactBook.Infrastructure.Interface;

namespace ContactBook.Core
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly ContactDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;

        public AppUserRepository(ContactDbContext context, IPhotoService photoService, UserManager<AppUser> userManager)
        {
            _context = context;
            _photoService = photoService;
            _userManager = userManager;
        }
        public async Task<List<AppUserDTO>> Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return new List<AppUserDTO>();
            }
            var users = await _userManager.Users
                .Where(p => p.Email.Contains(term)
                            || p.FirstName.Contains(term)
                            || p.LastName.Contains(term)
                            || p.City.Contains(term)
                            || p.State.Contains(term)
                            || p.Country.Contains(term)
                ).ToListAsync();
            var AppUserDTO = users.Select(item => new AppUserDTO
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                ImageUrl = item.ImageUrl,
                City = item.City,
                State = item.State,
                Country = item.Country,
                FacebookUrl = item.FacebookUrl,
                TwitterUrl = item.TwitterUrl
            }).ToList();
            return AppUserDTO;
        }

        public async Task<List<AppUserDTO>> GetAllUsers(PaginParameter userParameter)
        {
            var contacts = _context.AppUsers
                .OrderBy(c => c.FirstName)
                .Skip((userParameter.PageNumber - 1) * userParameter.PageSize)
                .Take(userParameter.PageSize)
                .ToList();

            var data = new List<AppUserDTO>();
            foreach (var userData in _context.AppUsers.ToList())
            {
                data.Add(new AppUserDTO
                {
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Email = userData.Email,
                    PhoneNumber = userData.PhoneNumber,
                    City = userData.City,
                    Country = userData.Country,
                    ImageUrl = userData.ImageUrl,
                    State = userData.State,
                    FacebookUrl = userData.FacebookUrl,
                    TwitterUrl = userData.TwitterUrl
                });
            }

            return data;
        }

        public async Task<AppUserDTO> GetUserById(string id)
        {
            var userData = await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == id);
            var user = new AppUserDTO
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                Email = userData.Email,
                PhoneNumber = userData.PhoneNumber,
                City = userData.City,
                Country = userData.Country,
                ImageUrl = userData.ImageUrl,
                State = userData.State,
                FacebookUrl = userData.FacebookUrl,
                TwitterUrl = userData.TwitterUrl
            };

            return user;
        }

        public async Task<string> AddUser(AppUserDTO appUserDto)
        {
            var existingUser = await _context.AppUsers.FirstOrDefaultAsync(e => e.Email == appUserDto.Email);
            if (existingUser != null)
            {
                return "User already exist";
            }

            var newAppUser = new AppUser
            {
                FirstName = appUserDto.FirstName,
                LastName = appUserDto.LastName,
                Email = appUserDto.Email,
                PhoneNumber = appUserDto.PhoneNumber,
                City = appUserDto.City,
                Country = appUserDto.Country

            };
            _context.AppUsers.Add(newAppUser);
            var saveChanges = await _context.SaveChangesAsync();
            if (saveChanges > 0)
            {
                return "User added Successfully";
            }

            return "User could not be added";
        }

        public async Task<bool> UploadImage(string id, PhotoDTO photo)
        {

            var newPhoto = await _photoService.AddPhotoAsync(photo.ImageUrl);
            var userData = await _userManager.FindByIdAsync(id);
                
            userData.ImageUrl = newPhoto.Url.AbsolutePath;

            await _userManager.UpdateAsync(userData);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
