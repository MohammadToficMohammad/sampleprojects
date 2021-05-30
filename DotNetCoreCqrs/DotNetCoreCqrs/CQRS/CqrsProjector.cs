using DotNetCoreCqrs.Data.DbContexts;
using DotNetCoreCqrs.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCqrs.CQRS
{
    public class CqrsProjector
    {
        private ICqrsAggregate cqrsCommandsHandler;
        private StoreContext db;



        public CqrsProjector(ICqrsAggregate _cqrsCommandsHandler, StoreContext _db) 
        {
            cqrsCommandsHandler = _cqrsCommandsHandler;
            db = _db;
            cqrsCommandsHandler.carAdded += CqrsAddCarProjection;
            cqrsCommandsHandler.carDeleted += CqrsDeleteCarProjection;
        }

        public async Task<object> CqrsAddCarProjection(object sender,EventData edata)
        {
            Car car = (Car)edata.data[0];
            db.Cars.Add(car);
            await db.SaveChangesAsync();

            return null;
        }

        public async Task<object> CqrsDeleteCarProjection(object sender, EventData edata)
        {
            int carId = (int)edata.data[0];
            db.Cars.Remove(await db.Cars.Where(x=>x.CarId== carId).FirstOrDefaultAsync());
            await db.SaveChangesAsync();

            return null;
        }
    }
}
