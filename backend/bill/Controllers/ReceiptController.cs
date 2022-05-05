using bill.Context;
using bill.Models;
using bill.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace bill.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReceiptController : ControllerBase
    {
        readonly BillDbContext dbContext;

        public ReceiptController(BillDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult InsertReceipt([FromBody] ReceiptViewModel param)
        {
            receipt receipt = new receipt();
            string code = "BILL";
            code += DateTime.Now.ToString("ddMMyyyyhhmmss");
            receipt.receipt_code = code;
            receipt.receipt_date = DateOnly.FromDateTime(DateTime.Now);
            receipt.receipt_product_price = param.receipt_product_price;
            receipt.receipt_product_discount = param.receipt_product_discount;
            receipt.receipt_discount = param.receipt_discount;
            receipt.receipt_total_price = param.receipt_total_price;
            dbContext.receipts.Add(receipt);

            dbContext.SaveChanges();
            receipt receipt2 = dbContext.receipts.FirstOrDefault(s => s.receipt_id == receipt.receipt_id);
            receipt2.receipt_code = "BILL-" + receipt.receipt_id.ToString("D4");
            dbContext.SaveChanges();

            for (int i = 0; i < param.receipt_list.Length; i++)
            {
                list list = new list();
                list.list_quantity = param.receipt_list[i].list_quantity;
                list.list_price = param.receipt_list[i].list_price;
                list.list_discount = param.receipt_list[i].list_discount;
                list.list_discount_bath = param.receipt_list[i].list_discount_bath;
                list.list_total_price = param.receipt_list[i].list_total_price;
                list.list_item_id = param.receipt_list[i].list_item_id;
                list.list_bill_id = receipt.receipt_id;
                dbContext.lists.Add(list);
                dbContext.SaveChanges();
            }
            return Ok(new Result { status_code = 1, message = "success"});
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var receipt = (from a in dbContext.receipts select new ReceiptViewModel
                            {
                                receipt_id = a.receipt_id,
                                receipt_code = a.receipt_code,
                                receipt_date = a.receipt_date.ToString(),
                                receipt_total_price = a.receipt_total_price
                            }).ToArray();

            return Ok(new Result { status_code = 1, message = "success", data = receipt });
        }

        [HttpGet]
        public IActionResult GetReceiptById(int id)
        {
            var result = (from x in dbContext.receipts
                          where x.receipt_id == id
                          select new ReceiptViewModel
                          {
                              receipt_code = x.receipt_code,
                              receipt_date = x.receipt_date.ToString(),
                              receipt_product_price = x.receipt_product_price,
                              receipt_product_discount = x.receipt_product_discount,
                              receipt_discount = x.receipt_discount,
                              receipt_total_price = x.receipt_total_price,
                              receipt_list = (from a in dbContext.lists
                                              join b in dbContext.items on a.list_item_id equals b.item_id
                                              join c in dbContext.units on b.item_unit_id equals c.unit_id
                                              where a.list_bill_id == id
                                              select new ListViewModel()
                                              {
                                                  list_quantity = a.list_quantity,
                                                  list_price = a.list_price,
                                                  list_discount = a.list_discount,
                                                  list_discount_bath = a.list_discount_bath,
                                                  list_total_price = a.list_total_price,
                                                  list_item = new ItemViewModel()
                                                  {
                                                      item_code = b.item_code,
                                                      item_name = b.item_name,
                                                      item_unit = new UnitViewModel()
                                                      {
                                                          unit_name = c.unit_name
                                                      }
                                                  }
                                              }).ToArray()
                          }).FirstOrDefault();

            if (result == null)
            {
                return Ok(new Result { status_code = -1, message = "not found"});
            }
            else
            {
                return Ok(new Result { status_code = 1, message = "success", data = result });
            }
        }

        //[HttpGet]
        //public IActionResult GetListById(int id)
        //{
        //    var result = (from a in dbContext.receipts
        //                  join b in dbContext.lists on a.receipt_id equals b.list_bill_id
        //                  join c in dbContext.items on b.list_item_id equals c.item_id
        //                  join d in dbContext.units on c.item_unit_id equals d.unit_id
        //                  where a.receipt_id == id
        //                  select new
        //                  {
        //                      list_quantity = b.list_quantity,
        //                      list_discount = b.list_discount,
        //                      item_code = c.item_code,
        //                      item_name = c.item_name,
        //                      item_price = c.item_price,
        //                      unit_name = d.unit_name
        //                  }).ToArray();
        //    return Ok(result);
        //}
    }
}
