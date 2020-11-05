using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApplyInk.Common.Enum;
using ApplyInk.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using ApplyInk.Web.Helpers;

namespace ApplyInk.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
            await CheckRolesAsync();
           
            await CheckUserAsync("1010", "Admin", "Admin2", "admin1@yopmail.com", "322 311 4620", "Avenida siempre viva", UserType.Admin);

        }

        private async Task<User> CheckUserAsync(
          string document,
          string firstName,
          string lastName,
          string email,
          string phone,
          string address,
          UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    Shop = _context.Shops.FirstOrDefault(),
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }


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
                _context.Categories.Add(new Category { Name = "Realista ", });
                _context.Categories.Add(new Category { Name = "Trash Polka", });
                _context.Categories.Add(new Category { Name = "tribal ", });
                _context.Categories.Add(new Category { Name = "japoneses", });
                _context.Categories.Add(new Category { Name = "Old School ", });
                _context.Categories.Add(new Category { Name = "New School", });
                _context.Categories.Add(new Category { Name = "neotradicionales ", });
                _context.Categories.Add(new Category { Name = "Acuarela", });
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
                                 new Shop { Name = "Applylnk Medellin"}
                                }

                            },
                                new City { Name = "Envigado",
                                Shops = new List<Shop>
                                {
                                     new Shop { Name = "Applylnk Envigado"},
                                     new Shop { Name = "Applylnk Envigado two"}
                                }
                            },
                            new City { Name = "Itagüí",
                            Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Itagüí"}
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
                                 new Shop { Name = "Applylnk Albania"}
                                }
                            },
                             new City { Name = "Distracción",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Distracción"}
                                }
                             },
                              new City { Name = "Maicao",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Maicao"}
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
                                 new Shop { Name = "Applylnk Calí"}
                                }
                            },
                            new City { Name = "Buenaventura",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Buenaventura"}
                                }
                            },
                            new City { Name = "Palmira",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Palmira"}
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
                                 new Shop { Name = "Applylnk Chordeleg"}
                                }
                            },
                            new City { Name = "Cuenca",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Cuenca"}
                                }
                            },
                            new City { Name = "Nabón",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Nabón"}
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
                                 new Shop { Name = "Applylnk Ambato"}
                                }
                            },
                            new City { Name = "Cevallos",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Cevallos"}
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
                                 new Shop { Name = "Applylnk Quilmes"}
                                }
                            },
                            new City { Name = "Mar del Plata",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Mar del Plata"}
                                }
                            },
                            new City { Name = "Bahía Blanca",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Bahía Blanca"}
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
                                 new Shop { Name = "Applylnk General Roca"}
                                }
                            },
                            new City { Name = "Ischilín",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Ischilín"}
                                }
                            },
                            new City { Name = "Cruz del Eje",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Cruz del Eje"}
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
                                 new Shop { Name = "Applylnk Catriló"}
                                }
                            },
                            new City { Name = "Hucal",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Hucal"}
                                }
                            },
                            new City { Name = "Maracó",
                                Shops = new List<Shop>
                                {
                                 new Shop { Name = "Applylnk Maracó"}
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

