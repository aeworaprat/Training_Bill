using bill.Context;
using bill.Models;
using bill.ViewModels;

namespace bill.Repositories
{
    public class UnitRepository
    {
        readonly BillDbContext dbContext;

        public UnitRepository(BillDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<UnitViewModel> GetAll()
        {
            List<UnitViewModel> units = (from a in dbContext.units
                                         orderby a.unit_id ascending
                                         select new UnitViewModel
                                         {
                                             unit_id = a.unit_id,
                                             unit_name = a.unit_name,
                                         }).ToList();

            return units;
        }

        public bool CheckNameAlreadyInUse(UnitViewModel param, bool excludeSelf = false)
        {
            if (excludeSelf)
            {
                return dbContext.units.Any(x => x.unit_id != param.unit_id && x.unit_name == param.unit_name);
            }
            else
            {
                return dbContext.units.Any(x => x.unit_name == param.unit_name);
            }
            
        }

        public Result InsertUnit(UnitViewModel param)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    unit unit = new unit();
                    unit.unit_name = param.unit_name;
                    dbContext.units.Add(unit);
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

        public Result DeleteUnit(UnitViewModel param)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    unit unit = dbContext.units.SingleOrDefault(s => s.unit_id == param.unit_id);
                    if (unit == null)
                    {
                        return new Result { status_code = -1, message = "not fonnd" };
                    }
                    else
                    {
                        dbContext.units.Remove(unit);
                        dbContext.SaveChanges();

                        transaction.Commit();
                        return new Result { status_code = 1, message = "success" };
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Result { status_code = -1, message = "catch" };
                }
            }
        }

        public Result UpdateUnit(UnitViewModel param)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    unit unit = dbContext.units.SingleOrDefault(s => s.unit_id == param.unit_id);
                    if (unit == null)
                    {
                        return new Result { status_code = -1, message = "not fonnd" };
                    }
                    else
                    {
                        unit.unit_name = param.unit_name;
                        dbContext.SaveChanges();

                        transaction.Commit();
                        return new Result { status_code = 1, message = "success" };
                    }
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return new Result { status_code = -1, message = "catch" };
                }
             }
        }

        public bool CheckUnitInUse(UnitViewModel param)
        {
            return (from a in dbContext.units
                    join b in dbContext.items on a.unit_id equals b.item_unit_id
                    where b.item_unit_id == param.unit_id
                    select a).Any();
        }
    }
}
