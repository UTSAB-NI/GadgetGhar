using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Ecommerce.DataAccess.Repository
{
    public class AppicationUserRepository :Repository<ApplicationUser>, IApplicationUserRepository
    {

        private ApplicationDbContext _db;
        public AppicationUserRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        

       
    }
}
