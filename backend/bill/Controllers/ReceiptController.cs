using bill.Context;
using bill.Models;
using bill.Repositories;
using bill.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace bill.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReceiptController : ControllerBase
    {
        readonly ReceiptRepository receiptRepository;

        public ReceiptController(ReceiptRepository receiptRepository)
        {
            this.receiptRepository = receiptRepository;
           
        }

        [HttpPost]
        public IActionResult InsertReceipt([FromBody] ReceiptViewModel param)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Result { status_code = -1, message = "invalit" });
            }
            else 
            {
                if(param.receipt_list.Length == 0)
                {
                    return Ok(new Result { status_code = -1, message = "no value" });
                }
                else
                {
                    Result result = receiptRepository.InsertReceipt(param);
                    return Ok(result);
                }
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ReceiptViewModel> list = receiptRepository.GetAll();

            return Ok(new Result { status_code = 1, message = "success", data = list });
        }

        [HttpGet]
        public IActionResult GetReceiptById(int id)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Result { status_code = -1, message = "invalit" });
            }
            else
            {
                var result = receiptRepository.GetReceiptById(id);
                if (result == null)
                {
                    return Ok(new Result { status_code = -1, message = "not found" });
                }
                else
                {
                    return Ok(new Result { status_code = 1, message = "success", data = result });
                }
            }
        }
    }
}
