using ApplyInk.Common.Enum;
using ApplyInk.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplyInk.Common.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string SocialNetworkURL { get; set; }

        public Guid ImageId { get; set; }

        public string ImageFacebook { get; set; }

        public LoginType LoginType { get; set; }

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

        public UserType UserType { get; set; }

        public ICollection<CategoryResponse> Categories { get; set; }

        public ShopResponse Shop { get; set; }

        public string FullName => $"{FirstName} {LastName}";

    }

}
