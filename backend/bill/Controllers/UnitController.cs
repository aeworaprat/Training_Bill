using bill.Context;
using bill.Models;
using bill.Repositories;
using bill.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace bill.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UnitController : ControllerBase
    {
        readonly UnitRepository unitRepository;

        public UnitController(UnitRepository unitRepository)
        {
            this.unitRepository = unitRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<UnitViewModel> units = unitRepository.GetAll();

            return Ok(new Result { status_code = 1, message = "success", data = units });
        }

        [HttpPost]
        public IActionResult InsertUnit([FromBody] UnitViewModel param)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Result { status_code = -1, message = "invalit" });
            }
            else
            {
                bool alreadyUsed = unitRepository.CheckNameAlreadyInUse(param);
                if (alreadyUsed)
                {
                    return Ok(new Result { status_code = -1, message = "fail" });
                }
                else
                {
                    Result result = unitRepository.InsertUnit(param);
                    return Ok(result);
                }
            }
        }

        [HttpPost]
        public IActionResult DeleteUnit([FromBody] UnitViewModel param)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Result { status_code = -1, message = "invalit" });
            }
            else
            {
                bool alreadyUsed = unitRepository.CheckUnitInUse(param);
                if (alreadyUsed)
                {
                    return Ok(new Result { status_code = -1, message = "fail" });
                }
                else
                {
                    Result result = unitRepository.DeleteUnit(param);
                    return Ok(result);
                }
            }
        }

        [HttpPost]
        public IActionResult UpdateUnit([FromBody] UnitViewModel param)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Result { status_code = -1, message = "invalit" });
            }
            else
            {
                bool alreadyUsed = unitRepository.CheckNameAlreadyInUse(param, true);
                if (alreadyUsed)
                {
                    return Ok(new Result { status_code = -1, message = "fail" });
                }
                else
                {
                    Result result = unitRepository.UpdateUnit(param);
                    return Ok(result);
                }
            }
        }
    }
}
