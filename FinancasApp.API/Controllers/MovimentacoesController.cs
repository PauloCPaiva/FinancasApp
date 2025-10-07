using FinancasApp.Domain.Dtos;
using FinancasApp.Domain.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancasApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacoesController(IMovimentacaoService service) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(MovimentacaoResponse), 201)]
        public IActionResult Post([FromBody] MovimentacaoRequest request)
        {
            try
            {
                var response = service.Criar(request);

                return StatusCode(201, new { message = "Movimentação cadastrada com sucesso", response });
            }
            catch (ValidationException e)
            {
                return StatusCode(400, e.Errors.Select(e => e.ErrorMessage));
            }
            catch (ApplicationException e)
            {
                return StatusCode(422, new { e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { e.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MovimentacaoResponse), 200)]
        public IActionResult Put(Guid id, [FromBody] MovimentacaoRequest request)
        {
            try
            {
                var response = service.Alterar(id, request);

                return StatusCode(201, new { message = "Movimentação atualizada com sucesso", response });
            }
            catch (ValidationException e)
            {
                return StatusCode(400, e.Errors.Select(e => e.ErrorMessage));
            }
            catch (ApplicationException e)
            {
                return StatusCode(422, new { e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { e.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MovimentacaoResponse), 200)]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var response = service.Excluir(id);

                return StatusCode(201, new { message = "Movimentação excluída com sucesso", response });
            }
            catch (ApplicationException e)
            {
                return StatusCode(422, new { e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { e.Message });
            }
        }

        [HttpGet("{dataMin}/{dataMax}")]
        [ProducesResponseType(typeof(List<MovimentacaoResponse>), 200)]
        public IActionResult Get(DateTime dataMin, DateTime dataMax)
        {
            try
            {
                var dtMin = dataMin.ToString("yyyy-MM-dd");
                var dtMax = dataMax.ToString("yyyy-MM-dd");

                var response = service.ConsultarPorDatas(dtMin, dtMax);

                return StatusCode(200, response);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { e.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MovimentacaoResponse), 200)]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var response = service.ObterPorId(id);

                return StatusCode(200, response);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { e.Message });
            }
        }
    }
}