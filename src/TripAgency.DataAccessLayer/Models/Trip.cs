using System;
using System.ComponentModel.DataAnnotations;

namespace TripAgency.DataAccessLayer.Models
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime DepartureDate { get; set; }

        public int CountNights { get; set; }

        public int CountKids { get; set; }

        public string Country { get; set; }

        public string TypeTrip { get; set; }

        public string Nutrition { get; set; }
    }
}
