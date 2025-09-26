using System.ComponentModel.DataAnnotations;

namespace FinancasApp.Web.Forms
{
    /// <summary>
    /// Formulário para cadastro de movimentação
    /// </summary>
    public class CadastroForm
    {
        [Required(ErrorMessage = "Por favor, informe o nome da movimentação.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data da movimentação.")]
        public string? Data { get; set; }

        [Required(ErrorMessage = "Por favor, informe o valor da movimentação.")]
        public string? Valor { get; set; }

        [Required(ErrorMessage = "Por favor, selecione o tipo da movimentação.")]
        public string? Tipo { get; set; }
    }
}