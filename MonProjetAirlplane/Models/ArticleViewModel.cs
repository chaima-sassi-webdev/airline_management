using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonProjetAirlplane.Models
{
    [Table("Articles")]
    public class ArticleViewModel
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom de l'article est requis.")]
        [MaxLength(100)]
        [Display(Name = "Nom de l'article")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La catégorie est requise.")]
        [MaxLength(50)]
        [Display(Name = "Catégorie")]
        public string Category { get; set; } = string.Empty; // Repas, Boisson, Duty Free, Confort

        [Required(ErrorMessage = "Le prix est requis.")]
        [Range(0.01, 10000.00, ErrorMessage = "Le prix doit être supérieur à 0.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Prix (€)")]
        public decimal Price { get; set; }

        [MaxLength(250)]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Disponible en stock")]
        public bool IsAvailable { get; set; } = true;
    }
}