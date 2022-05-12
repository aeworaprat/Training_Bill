using bill.Context;
using bill.Models;
using bill.ViewModels;

namespace bill.Repositories
{
    public class ReceiptRepository
    {
        readonly BillDbContext dbContext;

        public ReceiptRepository(BillDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Result InsertReceipt(ReceiptViewModel param)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    int count = (from a in dbContext.receipts orderby a.receipt_id ascending select a).Count();
                    string code = "BILL-";
                    if (count > 0)
                    {
                        string receipt_code = (from a in dbContext.receipts orderby a.receipt_code ascending select a.receipt_code).LastOrDefault();
                        string sub = receipt_code.Substring(5);
                        int last = int.Parse(sub);
                        ++last;

                        code += last.ToString("D4");
                    }
                    else
                    {
                        code += "0001";
                    }

                    receipt receipt = new receipt();
                    receipt.receipt_code = code;
                    receipt.receipt_date = DateOnly.FromDateTime(DateTime.Now);
                    receipt.receipt_product_price = param.receipt_product_price;
                    receipt.receipt_product_discount = param.receipt_product_discount;
                    receipt.receipt_discount = param.receipt_discount;
                    receipt.receipt_total_price = param.receipt_total_price;
                    dbContext.receipts.Add(receipt);
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
                    transaction.Commit();
                    return new Result { status_code = 1, message = "success" };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Result { status_code = -1, message = "catch" };
                }
            }
        }

        public List<ReceiptViewModel> GetAll()
        {
            List<ReceiptViewModel> receipt = (from a in dbContext.receipts
                                               select new ReceiptViewModel
                                               {
                                                   receipt_id = a.receipt_id,
                                                   receipt_code = a.receipt_code,
                                                   receipt_date = a.receipt_date.ToString(),
                                                   receipt_total_price = a.receipt_total_price
                                               }).ToList();

            return receipt;
        }

        public ReceiptViewModel GetReceiptById(int id)
        {
            ReceiptViewModel? result = (from x in dbContext.receipts
                                       where x.receipt_id == id
                                       select new ReceiptViewModel
                                       {
                                           receipt_code = x.receipt_code,
                                           receipt_date = x.receipt_date.ToString(),
                                           receipt_product_price = x.receipt_product_price,
                                           receipt_product_discount = x.receipt_product_discount, 
                                           receipt_discount = x.receipt_discount,
                                           receipt_total_price = x.receipt_total_price
                                       }).FirstOrDefault();

            if (result == null)
            {
                return null;
            }
            else
            {

                var test = (from a in dbContext.lists
                            join b in dbContext.items on a.list_item_id equals b.item_id
                            join c in dbContext.units on b.item_unit_id equals c.unit_id
                            where a.list_bill_id == id
                            select new
                            {
                                list_quantity = a.list_quantity,
                                list_price = a.list_price,
                                list_discount = a.list_discount,
                                list_discount_bath = a.list_discount_bath,
                                list_total_price = a.list_total_price,
                                item_code = b.item_code,
                                item_name = b.item_name,
                                unit_name = c.unit_name
                            }).ToArray();

                result.receipt_list = new ListViewModel[test.Length];
                for (int i = 0; i < test.Length; i++)
                {
                    result.receipt_list[i] = new ListViewModel
                    {
                        list_quantity = test[i].list_quantity,
                        list_price = test[i].list_price,
                        list_discount = test[i].list_discount,
                        list_discount_bath = test[i].list_discount_bath,
                        list_total_price = test[i].list_total_price,
                        list_item = new ItemViewModel
                        {
                            item_code = test[i].item_code,
                            item_name = test[i].item_name,
                            item_unit = new UnitViewModel
                            {
                                unit_name = test[i].unit_name
                            }
                        }
                    };
                }
                return result;
            }
        }
    }
}
