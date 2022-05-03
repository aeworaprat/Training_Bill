using bill.Context;
using Microsoft.AspNetCore.Mvc;

namespace bill.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReceiptController
    {
        readonly BillDbContext dbContext;

        public ReceiptController(BillDbContext dbContext)
        {
            this.dbContext = dbContext;
        }








    }
}
