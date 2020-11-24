using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApplyInk.Common.Enum;
using ApplyInk.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using ApplyInk.Web.Helpers;
using ApplyInk.Common.Services;
using ApplyInk.Web.Models;
using System.IO;

namespace ApplyInk.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IApiService _apiService;
        private readonly IBlobHelper _blobHepler;
        private readonly Random _random = new Random();

        public SeedDb(DataContext context, IUserHelper userHelper, IApiService apiService, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _apiService = apiService;
            _blobHepler = blobHelper;

        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
            await CheckRolesAsync();
            await CheckUsersAsync();
           
        }
        //Ramdom
        private async Task CheckUsersAsync()
        {
            if (!_context.Users.Any())
            {
                await CheckAdminsAsync();
                await CheckCustomersAsync();
                await CheckTatooerAsync();
            }
            
        }

        private async Task CheckCustomersAsync()
        {
            for (int i = 1; i <= 10; i++)
            {
                await CheckUserAsync($"100{i}", $"Customer{i}@yopmail.com", UserType.User);
            }
        }

        private async Task CheckAdminsAsync()
        {
            await CheckUserAsync("1001", "Admin1@yopmail.com", UserType.Admin);
            await CheckUserAsync("1002", "Admin2@yopmail.com", UserType.Admin);
        }

        private async Task CheckTatooerAsync()
        {
            for (int i = 1; i <= 10; i++)
            {
                await CheckUserAsync($"100{i}", $"Tattooer{i}@yopmail.com", UserType.Tattooer);
            }
        }

        private async Task<User> CheckUserAsync( string document, string email, UserType userType)
        {
            RandomUsers randomUsers;

            do
            {
                randomUsers = await _apiService.GetRandomUser("https://randomuser.me", "api");
            } while (randomUsers == null);

            Guid imageId = Guid.Empty;
            RandomUser randomUser = randomUsers.Results.FirstOrDefault();
            string imageUrl = randomUser.Picture.Large.ToString().Substring(22);
            Stream stream = await _apiService.GetPictureAsync("https://randomuser.me", imageUrl);
            if (stream != null)
            {
                imageId = await _blobHepler.UploadBlobAsync(stream, "users");
            }

            int ShopId = _random.Next(1, _context.Shops.Count());
          


            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = randomUser.Name.First,
                    LastName = randomUser.Name.Last,
                    Email = email,
                    UserName = email,
                    PhoneNumber = randomUser.Cell,
                    Address = $"{randomUser.Location.Street.Number}, {randomUser.Location.Street.Name}",                   
                    UserType = userType,
                    ImageId = imageId,
                 
                };


                if (userType.ToString().Equals(UserType.Tattooer.ToString()))
                {
                    List<Category> Categories = new List<Category>();
                    int CategoryId = _random.Next(1, _context.Categories.Count());

                    Categories.Add(await _context.Categories.FindAsync(CategoryId));
                    user.Categories = Categories;
                    user.SocialNetworkURL = "https://www.instagram.com/yulderq/";
                    user.Shop = await _context.Shops.FindAsync(ShopId);
                }     
                
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }

        //End Ramdom
      

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
            await _userHelper.CheckRoleAsync(UserType.Tattooer.ToString());
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Realist" });
                _context.Categories.Add(new Category { Name = "Trash Polka" });
                _context.Categories.Add(new Category { Name = "Tribal " });
                _context.Categories.Add(new Category { Name = "Oriental" });
                _context.Categories.Add(new Category { Name = "Old School " });
                _context.Categories.Add(new Category { Name = "New School" });
                _context.Categories.Add(new Category { Name = "Neotraditional" });
                _context.Categories.Add(new Category { Name = "BlackWork" });
            }

            await _context.SaveChangesAsync();
        }



        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    Departments = new List<Department>
                {
                    new Department
                    {
                        Name = "Antioquia",
                        Cities = new List<City>
                        {
                            new City {
                                Name = "Medellín",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Medellin", Address = "Cr 28 # 107-486 ", Latitude = 6.297059486595525, Logitude = -75.53852334335151 }
                                }
                               
                            },
                                new City { Name = "Envigado",
                                Shops = new List<Shop>
                                {
                                     new Shop { Name = "Applylnk Envigado", Address = "Cl. 37 Sur ##35-06, Envigado, Antioquia" , Latitude =6.171052080275017, Logitude = -75.58787056146134 },
                                     new Shop { Name = "Applylnk Envigado two", Address = "Cra. 27g ## 36 Sur - 51, Envigado, Antioquia", Latitude =6.166797326169355 , Logitude = -75.57700469947343 }

                                }
                            },
                            new City { Name = "Itagüí",
                            Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Itagüí", Address = "Cra. 50A ## 38 - 38, Itagüi, Antioquia", Latitude = 6.172613978619887, Logitude = -75.6100090017207}
                                }
                            }
                        }
                    },
                    new Department
                    {
                        Name = "Pasto",
                        Cities = new List<City>
                        {
                            new City { Name = "Albania",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Albania", Address = "Carrera 30, Tumaco, San Andres de Tumaco, Nariño", Latitude = 1.7872548724952042 , Logitude = -78.783922663065}
                                }
                            },
                             new City { Name = "Distracción",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Distracción", Address = "Cl. 12 #No. 22A-27, Pasto, Nariño", Latitude = 1.2089128009690464,  Logitude = -77.28207835341424}
                                }
                             },
                              new City { Name = "Maicao",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Maicao", Address = "Cra. 25 ## 15 62, Pasto, Nariño", Latitude = 1.2140574944949598, Logitude = -77.28101688302809}
                                }
                              }
                        }
                    },
                    new Department
                    {
                        Name = "Valle del Cauca",
                        Cities = new List<City>
                        {
                            new City { Name = "Calí",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Calí", Address = "Cl. 15 #21-02, Cali, Valle del Cauca", Latitude = 3.4381074240809304, Logitude = -76.52617185735528}
                                }
                            },
                            new City { Name = "Buenaventura",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Buenaventura", Address = "Cra. 44 #4a-84 #4a-2 a, Buenaventura, Valle del Cauca" , Latitude =3.88325407253073 , Logitude = -77.02270863831103}
                                }
                            },
                            new City { Name = "Palmira",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Palmira", Address = "Cra. 24 Nte. ## 34-62, Palmira, Valle del Cauca" , Latitude = 3.5232093568996268, Logitude = -76.29735353784812}
                                }
                            }
                        }
                    }
                }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Ecuador",
                    Departments = new List<Department>
                {
                    new Department
                    {
                        Name = "Azuay",
                        Cities = new List<City>
                        {
                            new City { Name = "Chordeleg",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Chordeleg", Address = "15 de Octubre, Chordeleg, Ecuador", Latitude =-2.928479512375827 , Logitude =  -78.78015873632873}
                                }
                            },
                            new City { Name = "Cuenca",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Cuenca", Address = "Calle Gran Colombia, Cuenca, Ecuador" , Latitude = -2.8953236240920925 , Logitude = -79.00992855130343}
                                }
                            },
                            new City { Name = "Nabón",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Nabón", Address = "Nabon, Ecuador", Latitude =-3.335201269021675 , Logitude = -79.06567462187886}
                                }
                            }
                        }
                    },
                    new Department
                    {
                        Name = "Tungurahua",
                        Cities = new List<City>
                        {
                            new City { Name = "Ambato",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Ambato", Address = "Av. Doce de Noviembre, Ambato 180201, Ecuador", Latitude = -1.2399173409953868, Logitude = -78.62587882251391}
                                }
                            },
                            new City { Name = "Cevallos",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Cevallos", Address = "Cevallos, Ecuador", Latitude = -1.3470335818655912, Logitude = -78.62069255898653}
                                }
                            }
                        }
                    }
                }
                }); _context.Countries.Add(new Country
                {
                    Name = "Argentina",
                    Departments = new List<Department>
                {
                    new Department
                    {
                        Name = "Buenos Aires",
                        Cities = new List<City>
                        {
                            new City { Name = "Quilmes",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Quilmes", Address = "Gaboto 649, Quilmes, Provincia de Buenos Aires, Argentina", Latitude = -34.724307909247024,  Logitude = -58.26077077631599}
                                }
                            },
                            new City { Name = "Mar del Plata",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Mar del Plata", Address = "Arenales 2735, Primera Junta, B7600EGM Mar del Plata, Provincia de Buenos Aires, Argentina" , Latitude =-38.011047895945396 , Logitude = -57.54681654722352}
                                }
                            },
                            new City { Name = "Bahía Blanca",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Bahía Blanca", Address = "Sarmiento 2153, B8000 Bahía Blanca, Provincia de Buenos Aires, Argentina" , Latitude = -38.699936704316414, Logitude =  -62.24156744832685}
                                }
                            }
                        }
                    },
                    new Department
                    {
                        Name = "Córdoba",
                        Cities = new List<City>
                        {
                            new City { Name = "General Roca",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk General Roca", Address = "Av. Gral. Julio Argentino Roca 1343, Gral. Roca, Río Negro, Argentina" , Latitude =-39.03035036573331 , Logitude = -67.57615409383367}
                                }
                            },
                            new City { Name = "Ischilín",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Ischilín", Address = "Ischilín, Córdoba, Argentina" , Latitude =-30.4705312378108, Logitude = -64.5809755087245}
                                }
                            },
                            new City { Name = "Cruz del Eje",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Cruz del Eje", Address = "X5280 Cruz del Eje, Córdoba, Argentina", Latitude = -30.720397739740605, Logitude = -64.80630593702605 }
                                }
                            }
                        }
                    },
                    new Department
                    {
                        Name = "La Pampa",
                        Cities = new List<City>
                        {
                            new City { Name = "Catriló",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Catriló", Address = "Estrada, San Martín y, Catriló, La Pampa, Argentina", Latitude =-36.40543212551708  , Logitude = -63.424586547296585}
                                }
                            },
                            new City { Name = "Hucal",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Hucal", Address = "Hucal, La Pampa, Argentina", Latitude = -37.7902126033856, Logitude =-64.02369145951667 }
                                }
                            },
                            new City { Name = "Maracó",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Maracó", Address = "Calle 13 552, L6360 Gral. Pico, La Pampa, Argentina", Latitude = -35.66197529020277, Logitude = -63.75875179168671 }
                                }
                            }
                        }
                    }
                }
                });
                await _context.SaveChangesAsync();
            }
        }
    }


}

