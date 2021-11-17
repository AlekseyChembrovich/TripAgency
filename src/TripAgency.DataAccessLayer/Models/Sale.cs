using System;
using System.ComponentModel.DataAnnotations;

namespace TripAgency.DataAccessLayer.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        public DateTime Data { get; set; }

        public int Count { get; set; }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        public int TripId { get; set; }

        public virtual Trip Trip { get; set; }

        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
