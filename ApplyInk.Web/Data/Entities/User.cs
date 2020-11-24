using ApplyInk.Common.Enum;
using ApplyInk.Common.Enums;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Display(Name = "Login Type")]
        public LoginType LoginType { get; set; }

        public string ImageFacebook { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath
        {
            get
            {
                if (LoginType == LoginType.Facebook && string.IsNullOrEmpty(ImageFacebook) ||
                    LoginType == LoginType.Applylnk && ImageId == Guid.Empty)
                {
                    return $"https://applyinkweb.azurewebsites.net/images/noimage.png";
                }

                if (LoginType == LoginType.Facebook)
                {
                    return ImageFacebook;
                }

                return $"https://applylnk.blob.core.windows.net/users/{ImageId}";
            }
        }
        
        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        public ICollection<Category> Categories { get; set; }

        public Shop Shop { get; set; }

        [Display(Name = "User")]
        public string FullName => $"{FirstName} {LastName}";

        [JsonIgnore]
        [NotMapped]
        public ICollection<MasterDetailMeeting> masterDetailMeeting { get; set; }
    }
}
