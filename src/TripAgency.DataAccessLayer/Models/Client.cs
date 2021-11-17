using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripAgency.DataAccessLayer.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string Phone { get; set; }

        public string Passport { get; set; }

        public ICollection<Sale> Sales { get; set; }

        public Client()
        {
            Sales = new HashSet<Sale>();
        }
    }
}
