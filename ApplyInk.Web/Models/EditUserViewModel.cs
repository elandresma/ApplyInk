using ApplyInk.Common.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyInk.Web.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }


        [Display(Name = "First Name")]
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string SocialNetworkURL { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
               ? $"https://applyinkweb.azurewebsites.net/images/noimage.png"
               : $"https://applylnk.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }


        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }


        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }


        [Display(Name = "Shop")]
        public int ShopId { get; set; }

        public IEnumerable<SelectListItem> Shops { get; set; }


        [Display(Name = "Categories")]
        public string CategoriesID { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        public string userType { get; set; }

    }
}
