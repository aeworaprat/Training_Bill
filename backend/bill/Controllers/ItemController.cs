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
            var result = (from a in dbContext.units
                          select new U
                          {
                              id = a.unit_id,
                              name = a.unit_name,
                             
                          }).ToArray();
            var items = (from b in dbContext.items
                         select new 
                         {
                             id = b.item_id,
                             name = b.item_name,
                             unit_id = b.item_unit_id
                         }).ToList();
            foreach(var a in result)
            {
                a.items = items.Where(x => x.unit_id == a.id).Select(x => new I { id = x.id, name = x.name}).ToList();
            }
        }

        class U
        {
            public int id { get; set; }
            public string name { get; set; }
            public List<I> items { get; set; }
        }

        class I
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var items = (from a in dbContext.items
                         join b in dbContext.units on a.item_unit_id equals b.unit_id
                         orderby a.item_id ascending
                         select new ItemViewModel
                         {
                             item_id = a.item_id,
                             item_code = a.item_code,
                             item_name = a.item_name,
                             item_price = a.item_price,
                             item_unit_id = a.item_unit_id,
                             item_unit = new UnitViewModel
                             {
                               unit_id = b.unit_id,
                               unit_name = b.unit_name,
                             }
                         }).ToList();

            return Ok(new Result { status_code = 1, message = "success", data = items});
        }

        [HttpPost]
        public IActionResult InsertItem([FromBody] ItemViewModel param)
        {
            if (dbContext.items.Any(x => x.item_name == param.item_name))
            {
                return Ok(new Result { status_code = -1, message = "fail"});
            }
            else
            {
                int count = (from a in dbContext.items select a).Count();
                string code = "ITEM-";
                if(count > 0)
                {
                    string item_code = (from a in dbContext.items orderby a.item_id ascending select a.item_code).LastOrDefault();
                    string sub = item_code.Substring(5);
                    int last = int.Parse(sub);
                    ++last;

                    code += last.ToString("D4");
                }
                else
                {
                    code += "0001";
                }

                item items = new item();
                items.item_code = code;
                items.item_name = param.item_name;
                items.item_price = param.item_price;
                items.item_unit_id = param.item_unit_id;
                dbContext.items.Add(items);
                dbContext.SaveChanges();
            }
            return Ok(new Result { status_code = 1, message = "success" });
        }

        [HttpPost]
        public IActionResult UpdateItem([FromBody] ItemViewModel param)
        {
            int count = (from a in dbContext.items
                         where a.item_id != param.item_id && a.item_name == param.item_name
                         select a).Count();
            if(count > 0)
            {
                return Ok(new Result { status_code = -1, message = "fail" });
            }
            else
            {
                item items = dbContext.items.FirstOrDefault(s => s.item_id == param.item_id);
                items.item_name = param.item_name;
                items.item_price = param.item_price;
                items.item_unit_id = param.item_unit_id;
                dbContext.SaveChanges();
            }
            return Ok(new Result { status_code = 1, message = "success" });
        }

        [HttpPost]
        public IActionResult DeleteItem([FromBody] ItemViewModel param)
        {
            int count = (from a in dbContext.lists
                         where a.list_item_id == param.item_id
                         select a).Count();

            if(count > 0)
            {
                return Ok(new Result { status_code = -1, message = "fail" });
            }
            else
            {
                item items = dbContext.items.SingleOrDefault(s => s.item_id == param.item_id);
                if (items == null)
                {
                    return Ok(new Result { status_code = -1, message = "not fonnd" });
                }
                else
                {
                    dbContext.items.Remove(items);
                    dbContext.SaveChanges();
                }            }
            return Ok(new Result { status_code = 1, message = "success" });
        }
    }
}
