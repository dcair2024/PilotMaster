using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotMaster.Domain.Entities
{
    public class Navio
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Bandeira { get; set; } = string.Empty;

        // Novo campo para regra de idade > 18 anos
        public DateTime DataConstrucao { get; set; }

        // Propriedade de Navegação
        public ICollection<Manobra> Manobras { get; set; } = new List<Manobra>();
    }
}