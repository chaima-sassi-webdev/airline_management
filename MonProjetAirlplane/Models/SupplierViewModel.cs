using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonProjetAirlplane.Models
{
    [Table("Suppliers")]
    public class SupplierViewModel
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom du fournisseur est requis.")]
        [MaxLength(150)]
        [Display(Name = "Nom du Fournisseur")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le type de service est requis.")]
        [MaxLength(100)]
        [Display(Name = "Type de Service (Carburant, Catering, Maintenance...)")]
        public string ServiceType { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email de contact est requis.")]
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        [Display(Name = "Email")]
        public string ContactEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le numéro de téléphone est requis.")]
        [Display(Name = "Téléphone")]
        public string Phone { get; set; } = string.Empty;
    }
}