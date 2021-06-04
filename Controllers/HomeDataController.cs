using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AplicatieWeb.Areas.Identity.Data;
using AplicatieWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AplicatieWeb.Controllers
{
    [Route("api/[controller]")]
    public class HomeDataController : ControllerBase
    {
        private readonly DataContext _db;
        private readonly UserManager<AplicatieUtilizator> _userManager;

        public HomeDataController(DataContext db, UserManager<AplicatieUtilizator> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        private async Task<AplicatieUtilizator> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }


        internal class IntrebariClass
        {
            public int id { get; set; }
            public string text { get; set; }
            public string responses { get; set; }
            public string raspunsCorect { get; set; }
            public int lungimeTest { get; set; }
            public int idTest { get; set; }
            public int lungimeIntrebari { get; set; }
            public int alternareComplexitate { get; set; }
            public int durata { get; set; }
        }

        internal class RezultateClass
        {
            public string factor { get; set; }
            public int valoare { get; set; }
            public int rezultat { get; set; }
        }

        internal class RezultatObtinutClass
        {
            public bool TestulAFostDat { get; set; }
            public int LungimeTest { get; set; }
            public int NotaObtinuta { get; set; }

        }

        // GET api/values
        [HttpGet]
        [Authorize(Roles="Student,Administrator")]
        [Route("/Teste")]
        public IActionResult GetTests()
        {
            try
            {
                var teste = _db.Teste.ToList();

                return Ok(teste);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Eroare: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Student,Administrator")]
        [Route("/GetTestul1")]
        public IActionResult GetTestul1()
        {
            try
            {
                //generam aleator urmatoarele variabile:
                //lungime test - interval [0,1]. 0 - 10 intrebari; 2 - 20 intrebari
                //complexitateIntrebari - 0 (usor, greu), 1 (greu,usor)
                //lungimeIntrebari - 0 (scurte), 1 (lungi)

                Random rnd = new Random();
                int lungimeTest = rnd.Next(2);
                int complexitateIntrebari = rnd.Next(2);
                int lungimeIntrebari = rnd.Next(2);

                var nrIntrebari = lungimeTest == 0 ? 5:10;

                var test = new List<IntrebariClass>();


                        var intrebariUsoare = (from intrebare in _db.Intrebari where intrebare.IsDeleted!=true && intrebare.IdTest==1 && intrebare.Dificultate == 0 && intrebare.Lungime== lungimeIntrebari select intrebare).Take(nrIntrebari);
                        var intrebariGrele = (from intrebare in _db.Intrebari where intrebare.IsDeleted != true && intrebare.IdTest == 1 && intrebare.Dificultate == 1 && intrebare.Lungime == lungimeIntrebari select intrebare).Take(nrIntrebari);
                        if (complexitateIntrebari == 0)
                        {
                    test = intrebariUsoare.Union(intrebariGrele).Select(x => new IntrebariClass
                    {
                        text = x.Intrebare,
                        responses = x.VarianteRaspuns,
                        raspunsCorect = x.RaspunsCorect,
                        lungimeTest = lungimeTest,
                        lungimeIntrebari = lungimeIntrebari,
                        alternareComplexitate = complexitateIntrebari
                    }).ToList();
                        }
                        else
                        {
                             test = intrebariGrele.Union(intrebariUsoare).Select(x => new IntrebariClass
                             {
                                text = x.Intrebare,
                                 responses = x.VarianteRaspuns,
                                raspunsCorect = x.RaspunsCorect,
                                 lungimeTest = lungimeTest,
                                 lungimeIntrebari = lungimeIntrebari,
                                 alternareComplexitate = complexitateIntrebari
                             }).ToList();
                        }

                return Ok(test);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Eroare: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Student,Administrator")]
        [Route("/GetTestul2")]
        public IActionResult GetTestul2()
        {
            try
            {
                //generam aleator urmatoarele variabile:
                //lungime test - interval [0,2]. 0 - 10 intrebari; 1 - 15 intrebari; 2 - 20 intrebari
                //complexitateIntrebari - 0 (usor, greu), 1 (greu,usor)
                //lungimeIntrebari - 0 (scurte), 1 (lungi)

                Random rnd = new Random();
                int lungimeTest = rnd.Next(2);
                int complexitateIntrebari = rnd.Next(2);
                int lungimeIntrebari = rnd.Next(2);

                var nrIntrebari = lungimeTest == 0 ? 5:10;

                var test = new List<IntrebariClass>();


                var intrebariUsoare = (from intrebare in _db.Intrebari where intrebare.IsDeleted != true && intrebare.IdTest == 2 && intrebare.Dificultate == 0 && intrebare.Lungime == lungimeIntrebari select intrebare).Take(nrIntrebari);
                var intrebariGrele = (from intrebare in _db.Intrebari where intrebare.IsDeleted != true && intrebare.IdTest ==2 && intrebare.Dificultate == 1 && intrebare.Lungime == lungimeIntrebari select intrebare).Take(nrIntrebari);
                if (complexitateIntrebari == 0)
                {
                    test = intrebariUsoare.Union(intrebariGrele).Select(x => new IntrebariClass
                    {
                        text = x.Intrebare,
                        responses = x.VarianteRaspuns,
                        raspunsCorect = x.RaspunsCorect,
                        lungimeTest = lungimeTest,
                        lungimeIntrebari = lungimeIntrebari,
                        alternareComplexitate = complexitateIntrebari
                    }).ToList();
                }
                else
                {
                    test = intrebariGrele.Union(intrebariUsoare).Select(x => new IntrebariClass
                    {
                        text = x.Intrebare,
                        responses = x.VarianteRaspuns,
                        raspunsCorect = x.RaspunsCorect,
                        lungimeTest = lungimeTest,
                        lungimeIntrebari = lungimeIntrebari,
                        alternareComplexitate = complexitateIntrebari
                    }).ToList();
                }

                return Ok(test);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Eroare: {ex.Message}");
            }
        }
        [HttpGet]
        [Authorize(Roles = "Student,Administrator")]
        [Route("/GetTestul3")]
        public IActionResult GetTestul3()
        {
            try
            {
                //generam aleator urmatoarele variabile:
                //durata - 0 (15 sec), 1 (30 sec)
                //daca durata = 15 sec => complexitateIntrebari = 0; altfel, complexitateIntrebari = 1

                Random rnd = new Random();
                int durata = rnd.Next(2);
                int durataIntrebare = durata == 0 ? 15 : 30;
                int lungimeIntrebari;
                if (durata == 0)
                    lungimeIntrebari = 0;
                else
                    lungimeIntrebari = 1;

                var test = new List<IntrebariClass>();

                if (lungimeIntrebari == 0)
                {
                    test = _db.Intrebari.Where(i=> i.IsDeleted != true && i.IdTest==3 && i.Lungime==0 ).Select(x => new IntrebariClass
                    {
                        text = x.Intrebare,
                        responses = x.VarianteRaspuns,
                        raspunsCorect = x.RaspunsCorect,
                        durata= durataIntrebare,
                        lungimeIntrebari = lungimeIntrebari,
                        lungimeTest=0
                    }).Take(10).ToList();
                }
                else
                {
                    test = _db.Intrebari.Where(i => i.IsDeleted != true && i.IdTest == 3 && i.Lungime == 1).Select(x => new IntrebariClass
                    {
                        text = x.Intrebare,
                        responses = x.VarianteRaspuns,
                        raspunsCorect = x.RaspunsCorect,
                        durata = durataIntrebare,
                        lungimeIntrebari = lungimeIntrebari,
                        lungimeTest=0
                    }).Take(10).ToList();
                }

                return Ok(test);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Eroare: {ex.Message}");
            }
        }
        [HttpGet]
        [Authorize(Roles = "Student,Administrator")]
        [Route("/GetTestul4")]
        public IActionResult GetTestul4()
        {
            try
            {
                //generam aleator urmatoarele variabile:
                //durata - 0 (15 sec), 1 (30 sec)
                //daca durata = 15 sec => complexitateIntrebari = 0; altfel, complexitateIntrebari = 1

                Random rnd = new Random();
                int durata = rnd.Next(2);
                int durataIntrebare = durata == 0 ? 15 : 30;
                int lungimeIntrebari;
                if (durata == 0)
                    lungimeIntrebari = 0;
                else
                    lungimeIntrebari = 1;

                var test = new List<IntrebariClass>();

                if (lungimeIntrebari == 0)
                {
                    test = _db.Intrebari.Where(i => i.IsDeleted != true && i.IdTest == 4 && i.Lungime == 0).Select(x => new IntrebariClass
                    {
                        text = x.Intrebare,
                        responses = x.VarianteRaspuns,
                        raspunsCorect = x.RaspunsCorect,
                        durata = durataIntrebare,
                        lungimeIntrebari = lungimeIntrebari,
                        lungimeTest=0
                    }).Take(10).ToList();
                }
                else
                {
                    test = _db.Intrebari.Where(i => i.IsDeleted != true && i.IdTest == 4 && i.Lungime == 1).Select(x => new IntrebariClass
                    {
                        text = x.Intrebare,
                        responses = x.VarianteRaspuns,
                        raspunsCorect = x.RaspunsCorect,
                        durata = durataIntrebare,
                        lungimeIntrebari = lungimeIntrebari,
                        lungimeTest=0
                    }).Take(10).ToList();
                }

                return Ok(test);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Eroare: {ex.Message}");
            }
        }
        [HttpGet]
        [Authorize(Roles = "Student,Administrator")]
        [Route("/GetEsteTerminat")]
        public IActionResult GetEsteTerminat(int idTest)
        {
            try
            {

                var rezultat = _db.Rezultate.Where(r => r.Utilizator == User.Identity.Name && r.IdTest == idTest).Select(r=>new
                {
                    LungimeTest=r.LungimeTest,
                    Nota=r.Nota
                }).OrderByDescending(r=>r.Nota).FirstOrDefault();

                var rezultatFinal  = new RezultatObtinutClass
                    {
                        TestulAFostDat = rezultat!=null?true:false,
                        LungimeTest = rezultat!=null?rezultat.LungimeTest:0,
                        NotaObtinuta = rezultat!=null?rezultat.Nota:0
                    };

                return Ok(rezultatFinal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Eroare: {ex.Message}");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Student,Administrator")]
        [Route("/SalvareRezultat")]
        public IActionResult SalvareRezultat(int test, int scor,int NrIntrebari, int LungimeIntrebari, int LungimeTest = -1, int AlternareComplexitate = -1, int Durata=-1)
        {

                var utilizator = User.Identity.Name;

                try
                {
                    Rezultate rez = new Rezultate();
                    rez.Utilizator = utilizator;
                    rez.IdTest = test;
                    rez.Nota = scor*10/NrIntrebari;
                    rez.LungimeTest = LungimeTest;
                    rez.LungimeIntrebari = LungimeIntrebari;
                    rez.AlternareDificultate = AlternareComplexitate;
                    rez.Durata = Durata;
                rez.Data = DateTime.Now;
                    _db.Add(rez);
                    _db.SaveChanges();

                    return Ok(rez);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Eroare: {ex.Message}");
                }
        }

        //rapoarte
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Route("/GetRapoarte")]
        public IActionResult GetRapoarte(int test, string tipRaport)
        {
            try
            {
                var rezultate = new List<RezultateClass>();
                if (tipRaport.IndexOf("lt")!=-1)
                    rezultate = _db.Rezultate.Where(r => r.IdTest == test).Select(r => new RezultateClass
                    {
                        factor = "Lungime test",
                        valoare = r.LungimeTest,
                        rezultat = r.Nota
                    }).Distinct().ToList();
                else if(tipRaport.IndexOf("li") != -1)
                {
                    rezultate = _db.Rezultate.Where(r => r.IdTest == test).Select(r => new RezultateClass
                    {
                        factor = "Lungime intrebari",
                        valoare = r.LungimeIntrebari,
                        rezultat = r.Nota
                    }).Distinct().ToList();
                }
                else if (tipRaport.IndexOf("ac") != -1)
                {
                    rezultate = _db.Rezultate.Where(r => r.IdTest == test).Select(r => new RezultateClass
                    {
                        factor = "Alternare complexitate",
                        valoare = r.AlternareDificultate,
                        rezultat = r.Nota
                    }).Distinct().ToList();
                }

                return Ok(rezultate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Eroare: {ex.Message}");
            }
        }

        //intrebari
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Route("/GetIntrebari")]
        public IActionResult GetIntrebari()
        {
            try
            {
                var intrebari =  _db.Intrebari.Where(i=>i.IsDeleted!=true)
                    .Select(r => new 
                    {
                        id=r.Id,
                        text=r.Intrebare,
                        responses=r.VarianteRaspuns,
                        raspunsCorect=r.RaspunsCorect,
                        idTest=r.IdTest,
                        lungimeIntrebari=r.Lungime,
                        alternareComplexitate=r.Dificultate
                    }).ToList();

                return Ok(intrebari);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Eroare: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [Route("/SalvareIntrebare")]
        public IActionResult SalvareIntrebare(int adaugareEditare, int id, string intrebare, string variante, string raspuns, 
            string idtest, string lungime, string dificultate)
        {

            try
            {
                if (adaugareEditare == 0)
                {
                    Intrebari intrb = new Intrebari();
                    intrb.Intrebare = intrebare;
                    intrb.VarianteRaspuns = variante;
                    intrb.RaspunsCorect = raspuns;
                    intrb.IdTest = Convert.ToInt32(idtest);
                    intrb.Lungime = Convert.ToInt32(lungime);
                    intrb.Dificultate = Convert.ToInt32(dificultate);

                    _db.Add(intrb);
                    _db.SaveChanges();
                    return Ok(intrb);
                }
                else
                {
                    Intrebari intrb = _db.Intrebari.Where(i => i.Id == id).FirstOrDefault();
                    intrb.Intrebare = intrebare;
                    intrb.VarianteRaspuns = variante;
                    intrb.RaspunsCorect = raspuns;
                    intrb.IdTest = Convert.ToInt32(idtest);
                    intrb.Lungime = Convert.ToInt32(lungime);
                    intrb.Dificultate = Convert.ToInt32(dificultate);

                    _db.SaveChanges();
                    return Ok(intrb);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Eroare: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [Route("/StergereIntrebare")]
        public IActionResult StergereIntrebare(int id)
        {

            try
            {
                Intrebari intrebare = _db.Intrebari.Where(i => i.Id == id).FirstOrDefault();

                intrebare.IsDeleted = true;
                _db.SaveChanges();

                return Ok(intrebare);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Eroare: {ex.Message}");
            }
        }

        //note
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Route("/GetNote")]
        public IActionResult GetNote()
        {
            try
            {
                var note = (from rez in _db.Rezultate
                            join stud in _db.AspNetUsers on rez.Utilizator equals stud.UserName
                            select new { Student = stud.Nume+' '+stud.Prenume, IdTest=rez.IdTest, Nota=rez.Nota }).ToList();

                return Ok(note);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Eroare: {ex.Message}");
            }
        }

    }
}