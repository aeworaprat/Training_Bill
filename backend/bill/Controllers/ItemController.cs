using bill.Context;
using bill.Models;
using bill.Repositories;
using bill.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace bill.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ItemController : ControllerBase
    {
        readonly ItemRepository itemRepository;

        public ItemController(ItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ItemViewModel> items = itemRepository.GetAll();
            

            return Ok(new Result { status_code = 1, message = "success", data = items});
        }

        [HttpPost]
        public IActionResult InsertItem([FromBody] ItemViewModel param)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Result { status_code = -1, message = "invalit" });
            }
            else
            {
                if (itemRepository.CheckNameAlreadyInUse(param))
                {
                    return Ok(new Result { status_code = -1, message = "fail" });
                }
                else
                {
                    Result result = itemRepository.InsertItem(param);
                    return Ok(result);
                }
            }
                
        }

        [HttpPost]
        public IActionResult UpdateItem([FromBody] ItemViewModel param)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Result { status_code = -1, message = "invalit" });
            }
            else
            {
                if (itemRepository.CheckNameAlreadyInUse(param, true))
                {
                    return Ok(new Result { status_code = -1, message = "fail" });
                }
                else
                {
                    Result result = itemRepository.UpdateItem(param);
                    return Ok(result);
                }
            }
        }

        [HttpPost]
        public IActionResult DeleteItem([FromBody] ItemViewModel param)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Result { status_code = -1, message = "invalit" });
            }
            else
            {
                if (itemRepository.CheckNameInUseDelete(param))
                {
                    return Ok(new Result { status_code = -1, message = "fail" });
                }
                else
                {
                    Result result = itemRepository.DeleteItem(param);
                    return Ok(result);
                }
            }
        }
    }
}
