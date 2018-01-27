using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OAuthLab.DAL.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }

        public string SecretHash
        {
            get
            {
                byte[] md5 = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(FirstName + LastName));
                return Encoding.ASCII.GetString(md5);
            }
        }

        public ICollection<Order> Order { get; set; }
    }
}
