﻿using bill.Context;
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
                item items = new item();
                items.item_name = param.item_name;
                items.item_price = param.item_price;
                items.item_unit_id = param.item_unit_id;
                dbContext.items.Add(items);
                dbContext.SaveChanges();

                item items2 = dbContext.items.FirstOrDefault(s => s.item_id == items.item_id);
                items2.item_code = "ITEM-"+items.item_id.ToString("D4");
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
                item items = dbContext.items.FirstOrDefault(s => s.item_id == param.item_id);
                dbContext.items.Remove(items);
                dbContext.SaveChanges();
            }
            return Ok(new Result { status_code = 1, message = "success" });
        }
    }
}
