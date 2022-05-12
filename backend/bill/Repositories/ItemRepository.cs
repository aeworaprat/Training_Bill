using bill.Context;
using bill.Models;
using bill.ViewModels;

namespace bill.Repositories
{
    public class ItemRepository
    {
        readonly BillDbContext dbContext;

        public ItemRepository(BillDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<ItemViewModel> GetAll()
        {
            List<ItemViewModel> items = (from a in dbContext.items
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

            return items;
        }

        public bool CheckNameAlreadyInUse(ItemViewModel param, bool excludeSelf = false)
        {
            if (excludeSelf)
            {
                return (from a in dbContext.items
                        where a.item_id != param.item_id && a.item_name == param.item_name
                        select a).Any();
            }
            else
            {
                return dbContext.items.Any(x => x.item_name == param.item_name);
            }
        }

        public Result InsertItem(ItemViewModel param)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    int count = (from a in dbContext.items select a).Count();
                    string code = "ITEM-";
                    if (count > 0)
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

                    transaction.Commit();
                    return new Result { status_code = -1, message = "success" };

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Result { status_code = -1, message = "catch" };
                }
            }
        }

        public Result UpdateItem(ItemViewModel param)
        {
            item items = dbContext.items.SingleOrDefault(s => s.item_id == param.item_id);
            if (items == null)
            {
                return new Result { status_code = -1, message = "not fonnd" };
            }
            else
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        items.item_name = param.item_name;
                        items.item_price = param.item_price;
                        items.item_unit_id = param.item_unit_id;
                        dbContext.SaveChanges();

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
        }

        public Result DeleteItem(ItemViewModel param)
        {
            item items = dbContext.items.SingleOrDefault(s => s.item_id == param.item_id);
            if (items == null)
            {
                return new Result { status_code = -1, message = "not fonnd" };
            }
            else
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        dbContext.items.Remove(items);
                        dbContext.SaveChanges();

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
        }


        public bool CheckNameInUseDelete(ItemViewModel param)
        {
            return (from a in dbContext.items
                    where a.item_id != param.item_id && a.item_name == param.item_name
                    select a).Any();
        }

    }
}
