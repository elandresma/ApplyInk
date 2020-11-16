using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplyInk.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplyInk.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetProfessions()
        {
            return Ok(_context.Categories.OrderBy(p => p.Name));
        }
    }
}
