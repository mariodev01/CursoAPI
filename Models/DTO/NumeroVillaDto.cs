using System.ComponentModel.DataAnnotations;

namespace CursoWebAPI.Models.DTO
{
    public class NumeroVillaDto
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }

        public string DetalleEspecial { get; set; }
    }
}
