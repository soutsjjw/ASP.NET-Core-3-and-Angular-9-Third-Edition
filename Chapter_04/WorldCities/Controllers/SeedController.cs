using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using WorldCities.Data;

namespace WorldCities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SeedController(
            ApplicationDbContext context,
            IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult> Import()
        {
            var path = Path.Combine(_env.ContentRootPath,
                "Data/Source/worldcities.xlsx");
            
            using(var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using(var ep = new ExcelPackage(stream))
                {
                    // get the first worksheet
                    var ws = ep.Workbook.Worksheets[0];

                    // initialize the record counters;
                    var nCountries = 0;
                    var nCities = 0;

                    #region Import all Countries
                    // create a list containing all the countries
                    // already existing into the Database (it
                    // will be empty on first run).
                    var lstCountries = _context.Countries.ToList();

                    // iterates through all rows, skipping the first one
                    for(int nRow = 2; nRow <= ws.Dimension.End.Row; nRow++)
                    {

                    }
                    #endregion
                }
            }
        }
    }
}
