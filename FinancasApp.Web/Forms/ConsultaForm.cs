using System.ComponentModel.DataAnnotations;

namespace FinancasApp.Web.Forms
{
    /// <summary>
    /// Formulário para consulta de movimentações
    /// </summary>
    public class ConsultaForm
    {
        [Required(ErrorMessage = "Por favor, informe a data de início.")]
        public string? DataMin { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data de fim.")]
        public string? DataMax { get; set; }
    }
}