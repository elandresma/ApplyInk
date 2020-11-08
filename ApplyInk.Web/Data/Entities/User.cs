using ApplyInk.Common.Enum;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyInk.Web.Data.Entities
{
    public class User : IdentityUser
    {
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


        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

       [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
               ? $"https://applyink.azurewebsites.net/images/noimage.png"
               : $"https://applyink.blob.core.windows.net/users/{ImageId}";
        
        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        public ICollection<Category> Categories { get; set; }

        public Shop Shop { get; set; }

        [Display(Name = "User")]
        public string FullName => $"{FirstName} {LastName}";

        public ICollection<MasterDetailMeeting> masterDetailMeeting { get; set; }
    }
}
