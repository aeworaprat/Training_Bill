using bill.Context;
using bill.Models;
using Microsoft.AspNetCore.Mvc;

namespace bill.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        readonly BillDbContext dbContext;

        public WeatherForecastController(BillDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult get()
        {
            List<unit> units = dbContext.units.ToList();  
            return Ok();
        }

        [HttpGet]
        public IActionResult insert()
        {
            unit unit = new unit();
            unit.unit_name = "กล่อง";
            dbContext.units.Add(unit);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult SelectOne()
        {
            unit unit = dbContext.units.FirstOrDefault(s=>s.unit_id == 5);
         
            return Ok();
        }

        [HttpGet]
        public IActionResult SelectOneLinq()
        {
            unit unit = (from a in dbContext.units
                        where a.unit_id == 6
                        select a).FirstOrDefault();

            return Ok();
        }

        [HttpGet]
        public IActionResult SelectMany()
        {
            List<unit> units = (from a in dbContext.units
                         where a.unit_id < 6
                         select a).ToList();

            return Ok();
        }

        [HttpGet]
        public IActionResult SelectMany2()
        {
            List<unit> units = dbContext.units.Where(s => s.unit_id < 6).ToList();

            return Ok();
        }

        [HttpGet]
        public IActionResult Delete()
        {
            unit unit = dbContext.units.FirstOrDefault(s => s.unit_id == 4);
            dbContext.units.Remove(unit);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult Update()
        {
            unit unit = dbContext.units.FirstOrDefault(s => s.unit_id == 6);
            //unit unit = new unit();
            //unit.unit_id = 6;
            unit.unit_name = "กล่อง1";
            dbContext.SaveChanges();
            return Ok();
        }





    }
}