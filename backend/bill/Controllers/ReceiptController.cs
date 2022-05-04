﻿using bill.Context;
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








    }
}
