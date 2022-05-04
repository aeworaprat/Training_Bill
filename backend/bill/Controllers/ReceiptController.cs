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

            int max_id = dbContext.receipts.Max(x => x.receipt_id);

            for (int i = 0; i < param.list.list_item_id.Length; i++)
            {
                list list = new list();
                list.list_quantity = param.list.list_quantity[i];
                list.list_discount = param.list.list_discount[i];
                list.list_item_id = param.list.list_item_id[i];
                list.list_bill_id = max_id;
                dbContext.lists.Add(list);
                dbContext.SaveChanges();
            }
            return Ok(true);
        }

        public IActionResult GetAll()
        {
            var receipt = (from a in dbContext.receipts select new
                            {
                                receipt_id = a.receipt_id,
                                receipt_code = a.receipt_code,
                                receipt_date = a.receipt_date.ToString(),
                                receipt_total_price = a.receipt_total_price
                            }).ToArray();

            return Ok(receipt);
        }

        public IActionResult GetReceiptById(int id)
        {
            var result = (from a in dbContext.receipts
                          where a.receipt_id == id
                          select new
                          {
                              receipt_code = a.receipt_code,
                              receipt_date = a.receipt_date.ToString(),
                              receipt_product_price = a.receipt_product_price,
                              receipt_product_discount = a.receipt_product_discount,
                              receipt_discount = a.receipt_discount,
                              receipt_total_price = a.receipt_total_price
                          }).ToArray();

            return Ok(result[0]);
        }

        public IActionResult GetListById(int id)
        {
            var result = (from a in dbContext.receipts
                          join b in dbContext.lists on a.receipt_id equals b.list_bill_id
                          join c in dbContext.items on b.list_item_id equals c.item_id
                          join d in dbContext.units on c.item_unit_id equals d.unit_id
                          where a.receipt_id == id
                          select new
                          {
                              list_quantity = b.list_quantity,
                              list_discount = b.list_discount,
                              item_code = c.item_code,
                              item_name = c.item_name,
                              item_price = c.item_price,
                              unit_name = d.unit_name
                          }).ToArray();
            return Ok(result);
        }
    }
}
