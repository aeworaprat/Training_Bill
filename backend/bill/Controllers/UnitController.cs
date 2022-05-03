using bill.Context;
using bill.Models;
using bill.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace bill.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UnitController : ControllerBase
    {
        readonly BillDbContext dbContext;

        public UnitController(BillDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<unit> units = dbContext.units.OrderBy(x=>x.unit_id).ToList();

            return Ok(units);
        }

        [HttpPost]
        public IActionResult InsertUnit([FromBody] UnitViewModel param)
        {
            if (dbContext.units.Any(x => x.unit_name == param.unit_name))
            {
                return Ok(false);
            }
            else
            {
                unit unit = new unit();
                unit.unit_name = param.unit_name;
                dbContext.units.Add(unit);
                dbContext.SaveChanges();
            }
            return Ok(true);
        }

        [HttpPost]
        public IActionResult DeleteUnit([FromBody] UnitViewModel param)
        {
            int count = (from a in dbContext.units
                         join b in dbContext.items on a.unit_id equals b.item_unit_id
                         where b.item_unit_id == param.unit_id select a).Count();
            if (count > 0)
            {
                return Ok(false);
            }
            else
            {
                unit unit = dbContext.units.FirstOrDefault(s => s.unit_id == param.unit_id);
                dbContext.units.Remove(unit);
                dbContext.SaveChanges();
                return Ok(true);
            }
        }

        [HttpPost]
        public IActionResult UpdateUnit([FromBody] UnitViewModel param)
        {
            int count = (from a in dbContext.units
                         where a.unit_id != param.unit_id && a.unit_name == param.unit_name
                         select a).Count();

            if (count > 0)
            {
                return Ok(false);
            }
            else
            {
                unit unit = dbContext.units.FirstOrDefault(s => s.unit_id == param.unit_id);
                unit.unit_name = param.unit_name;
                dbContext.SaveChanges();
            }
            return Ok(true);
        }
    }
}
