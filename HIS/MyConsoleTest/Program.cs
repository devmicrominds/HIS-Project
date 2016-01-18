using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleTest
{
    class Program
    {
        private static IRepositoryFactory factory;
        static void Main(string[] args)
        {
            factory
            var repository = factory.GetRepository<Repository<Resource>>();
            var query = repository.GetQueryable();

        }
    }
}
