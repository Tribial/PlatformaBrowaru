using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformaBrowaru.Data.Data;
using PlatformaBrowaru.Data.Repository.Interfaces;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Data.Repository.Repositories
{
    public class KindRepository : IKindRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public KindRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Kind Get(Func<Kind, bool> function)
        {
            return _dbContext.Kinds.FirstOrDefault(function);
        }
    }
}
