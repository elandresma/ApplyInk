using ApplyInk.Common.Enum;
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

        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
       ? $"https://applyink.azurewebsites.net/images/noimage.png"
       : $"https://applyink.blob.core.windows.net/users/{ImageId}";

        public UserType UserType { get; set; }

    }

}
