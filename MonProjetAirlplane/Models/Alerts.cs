using System;
using System.ComponentModel.DataAnnotations;

namespace MonProjetAirlplane.Models
{
    public class Alert
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        
        // Propriétés attendues par la vue Dashboard
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Severity { get; set; } = "Info"; // ex: "High", "Warning", "Info"
        public string FlightNumber { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsResolved { get; set; } = false;
    }
}