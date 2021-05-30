using DotNetCoreCqrs.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCqrs.Data.DbContexts
{
    public static class SeedDb
    {
        public static void seedDb(StoreContext db)
        {
            db.Owners.RemoveRange(db.Owners);
            db.Cars.RemoveRange(db.Cars);
            db.SaveChanges();

            Owner owner = new Owner();
            owner.FirstName = "Mohammad";
            owner.LastName = "Tofic";

            Car car1 = new Car();
            car1.Color = "BLACK";
            car1.Model = CARMODEL.BMW;

            Car car2 = new Car();
            car2.Color = "BLUE";
            car2.Model = CARMODEL.VOLVO;

            owner.cars.Add(car1);
            owner.cars.Add(car2);

            db.Owners.Add(owner);

            db.SaveChanges();

        }
    }
}
