using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;




namespace PilotMaster.Domain.Entities
{
    public class Manobra
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }

        // Informações essenciais para o cálculo do custo base:
        public decimal GRT { get; set; }
        public string Area { get; set; } = string.Empty; // Ex: "Area I", "Ilhéus"
        [Precision(10, 2)]
        public decimal Calado { get; set; }

        // Fatores de risco/condição (para o multiplicador maior):
        public bool SemMaquinaLeme { get; set; }
        public bool Emergencia { get; set; }
        // Você pode adicionar mais campos conforme necessário (ex: AtracacaoContrabordo)

        public int NavioId { get; set; }
        public Navio? Navio { get; set; }
    }
}