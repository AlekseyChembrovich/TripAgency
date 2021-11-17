using System.Collections.Generic;
using TripAgency.DataAccessLayer.Models;

namespace TripAgency.DataAccessLayer.Repository.EntityFramework.Interfaces
{
    public interface IRepositorySale : IRepository<Sale>
    {
        IEnumerable<Sale> GetAllIncludeForeignKey();
    }
}
