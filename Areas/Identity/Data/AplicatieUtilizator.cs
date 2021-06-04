using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AplicatieWeb.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the UserAplicatie class
    public class AplicatieUtilizator : IdentityUser
    {
        [PersonalData]
        [Column(TypeName="nvarchar(100)")]
        public string Nume { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Prenume { get; set; }
    }
}
