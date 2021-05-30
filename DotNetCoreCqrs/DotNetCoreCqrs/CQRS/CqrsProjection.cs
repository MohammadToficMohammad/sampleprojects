using DotNetCoreCqrs.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//queries handler
namespace DotNetCoreCqrs.CQRS
{
    public class CqrsProjection : ICqrsProjection
    {

        private StoreContext db;

        public CqrsProjection(StoreContext _db) 
        {
            db = _db;
        }

        public async Task<T> HandleAsync<T>(CqrsQuery query) where T : class
        {

            switch (query.type)
            {
                case CQRSQUERYTYPE.GETCAR:
                    int carId = (int)query.eventData.data[0];
                    return ( await db.Cars.Where(x=>x.CarId== carId).AsNoTracking().FirstOrDefaultAsync()) as T ;
                    //break;
                case CQRSQUERYTYPE.GETALLCARS:
                    return (await db.Cars.AsNoTracking().ToListAsync()) as T;
                    //break;
            }


            return null;
        }

    }

    public enum CQRSQUERYTYPE
    {
        GETCAR,
        GETALLCARS
    }
}


