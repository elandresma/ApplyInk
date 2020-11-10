using System;
using System.Collections.Generic;
using System.Text;

namespace ApplyInk.Common.Responses
{
    public class ShopResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int IdCity { get; set; }

        public CityResponse City { get; set; }

        public ICollection<UserResponse> Users { get; set; }

        public int UsersNumber => Users == null ? 0 : Users.Count;
    }
}
