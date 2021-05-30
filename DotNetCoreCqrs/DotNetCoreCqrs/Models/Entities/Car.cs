using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotNetCoreCqrs.Models.Entities
{
    public class Car
    {
        public int CarId { get; set; }

        public string Color { get; set; }


        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CARMODEL Model { get; set; }

        public Owner owner { get; set; }

    }

    public enum CARMODEL 
    {
    BMW,
    VOLVO
    }
}
