using System;

namespace Conversor.Models
{
    public class Conversao
    {
        public int Id { get; set; }
        public string MoedaOrigem { get; set; } = "";
        public string MoedaDestino { get; set; } = "";
        public double Valor { get; set; }
        public double Resultado { get; set; }
        public DateTime Criadoem { get; set; } = DateTime.UtcNow;
    }
}
