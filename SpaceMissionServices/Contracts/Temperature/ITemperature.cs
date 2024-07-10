using SpaceMissionShared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionServices.Contracts.Temperature
{
    public  interface ITemperature
    {
        Task<bool> AddTemperature(TemperatureDTO temperature);
        Task<TemperatureDTO> LastTemperature();
        Task<List<TemperatureDTO>> GetAllTemperature(int Id = 0);
    }
}
