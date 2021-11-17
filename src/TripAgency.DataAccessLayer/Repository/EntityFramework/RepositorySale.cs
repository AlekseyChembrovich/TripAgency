using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TripAgency.DataAccessLayer.Models;
using TripAgency.DataAccessLayer.Repository.EntityFramework.Interfaces;

namespace TripAgency.DataAccessLayer.Repository.EntityFramework
{
    public class RepositorySale : BaseRepository<Sale>, IRepositorySale
    {
        public RepositorySale(DatabaseContext context) : base(context)
        {
        }

        public IEnumerable<Sale> GetAllIncludeForeignKey()
        {
            return Context.Sale
                .Include(x => x.Client)
                .Include(x => x.Employee)
                .Include(x => x.Trip);
        }
    }
}
