using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EmolumentController : BaseApiController
    {
        private readonly IEmolumentService _emolumentService;
        public EmolumentController(IEmolumentService emolumentService)
        {
            _emolumentService = emolumentService;
        }

        [HttpPost("emoluments")]
        public async Task<ActionResult<IReadOnlyList<Emolument>>> AddEmoluments([FromBody] List<Emolument> emoluments)
        {
            var emols = await _emolumentService.AddEmoluments(emoluments);
            return Ok(emols);
        }

        [HttpPut("emoluments")]
        public async Task<int> UpdateEmoluments([FromBody] List<Emolument> emoluments)
        {
            var emols = await _emolumentService.UpdateEmoluments(emoluments);
            return emols;
        }

        [HttpPut("emolument")]
        public async Task<ActionResult<Emolument>> UpdateEmolument([FromBody] Emolument emolument)
        {
            var emol = await _emolumentService.UpdateEmolument(emolument);
            return emol;
        }

        [HttpDelete("emolument")]
        public async Task<int> DeleteEmolument([FromBody] Emolument emolument)
        {
            var emol = await _emolumentService.DeleteEmolument(emolument);
            return emol;
        }



    }
}