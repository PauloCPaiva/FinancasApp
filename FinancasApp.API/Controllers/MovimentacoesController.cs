using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancasApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacoesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post()
        {
            return Ok(new { message = "Movimentação cadastrada com sucesso!" });
        }

        [HttpPut]
        public IActionResult Put()
        {
            return Ok(new { message = "Movimentação atualizada com sucesso!" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(new { message = "Movimentação excluída com sucesso!" });
        }

        [HttpGet("{dataMin}/{dataMax}")]
        public IActionResult Get(DateTime dataMin, DateTime dataMax)
        {
            return Ok(new { message = "Movimentações obtidas com sucesso!" });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(new { message = "Movimentação obtida com sucesso!" });
        }
    }
}