using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicatieWeb.Models
{
    public class Rezultate
    {
        public int Id { get; set; }
        public string Utilizator { get; set; }
        public int Nota { get; set; }
        public int IdTest { get; set; }
        public int LungimeTest { get; set; }
        public int LungimeIntrebari { get; set; }
        public int AlternareDificultate { get; set; }
        public int Durata { get; set; }
        public DateTime Data { get; set; }
    }
}
