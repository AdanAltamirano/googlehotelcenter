using APIServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices
{
    public class AgencyService: IDisposable
    {
        private readonly OzHotelesEntities Context = new OzHotelesEntities();

        public void Dispose()
        {
            Context.Dispose();
        }

        public IQueryable<vAgencies> Get()
        {
            return Context.vAgencies.AsQueryable();
        }
    }
}
