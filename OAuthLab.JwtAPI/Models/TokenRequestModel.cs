using OAuthLab.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthLab.JwtAPI.Models
{
    public class TokenRequestModel
    {
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string SecretHash { get; set; }
        public Customer Customer { get; set; }

        public TokenRequestModel()
        { }
    }
}
