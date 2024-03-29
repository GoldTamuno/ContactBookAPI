using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ContactBook.Core.DTO
{
    public class PhotoDTO
    {
       public IFormFile ImageUrl { get; set; }
    }
}
