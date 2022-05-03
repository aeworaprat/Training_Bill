using bill.Context;
using bill.Models;
using bill.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace bill.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ItemController : ControllerBase
    {
        readonly BillDbContext dbContext;

        public ItemController(BillDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var items = (from a in dbContext.items
                         join b in dbContext.units on a.item_unit_id equals b.unit_id
                         select new
                         {
                             item_id = a.item_id,
                             item_code = a.item_code,
                             item_name = a.item_name,
                             item_price = a.item_price,
                             item_unit_id = a.item_unit_id,
                             unit_id = b.unit_id,
                             unit_name = b.unit_name
                         } into item orderby item.item_id select item).ToList();

            return Ok(items);
        }

        [HttpPost]
        public IActionResult InsertItem([FromBody] ItemViewModel param)
        {
            if (dbContext.items.Any(x => x.item_name == param.item_name))
            {
                return Ok(false);
            }
            else
            {
                item item = new item();
                string code = "ITEM";
                code += DateTime.Now.ToString("ddMMyyyyhhmmss");
                item.item_code = code;
                item.item_name = param.item_name;
                item.item_price = param.item_price;
                item.item_unit_id = param.item_unit_id;
                dbContext.items.Add(item);
                dbContext.SaveChanges();
            }
            return Ok(true);
        }

        public IActionResult UpdateItem([FromBody] ItemViewModel param)
        {
            int count = (from a in dbContext.items
                         where a.item_id != param.item_id && a.item_name == param.item_name
                         select a).Count();
            if(count > 0)
            {
                return Ok(false);
            }
            else
            {
                item items = dbContext.items.FirstOrDefault(s => s.item_id == param.item_id);
                items.item_name = param.item_name;
                items.item_price = param.item_price;
                items.item_unit_id = param.item_unit_id;
                dbContext.SaveChanges();
            }
            return Ok(true);
        }

        public IActionResult DeleteItem([FromBody] ItemViewModel param)
        {
            item items = dbContext.items.FirstOrDefault(s => s.item_id == param.item_id);
            dbContext.items.Remove(items);
            dbContext.SaveChanges();
            return Ok(true);
        }

       

    }
}
