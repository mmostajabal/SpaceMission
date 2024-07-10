using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionShared.DTO
{
    public class TemperatureDTO
    {
        public int Id { get; set; }
        public double TemperatureCelsius { get; set; }
        public double DiffTemperature { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
