using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripAgency.DataAccessLayer.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string Phone { get; set; }

        public ICollection<Sale> Sales { get; set; }

        public Employee()
        {
            Sales = new HashSet<Sale>();
        }
    }
}
