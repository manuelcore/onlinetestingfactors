using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicatieWeb.Models
{
    public class Intrebari
    {
        public int Id { get; set; }
        public string Intrebare { get; set; }
        public string VarianteRaspuns { get; set; }
        public string RaspunsCorect { get; set; }
        public int IdTest { get; set; }
        public int Dificultate { get; set; }
        public int Lungime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
