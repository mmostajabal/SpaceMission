using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpaceMissionModels;
using SpaceMissionServices.Contracts.Temperature;
using SpaceMissionServices.Data;
using SpaceMissionShared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionServices.Services.TemperatureSrv
{
    public class TemperatureService : ITemperature
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public TemperatureService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// AddTemperature
        /// </summary>
        /// <param name="temperatureDTO"></param>
        /// <returns></returns>
        public async Task<bool> AddTemperature(TemperatureDTO temperatureDTO)
        {
            _dbContext.Add(_mapper.Map<Temperature>(temperatureDTO));
            _dbContext.SaveChangesAsync().GetAwaiter().GetResult();

            return true;
        }

        public async Task<List<TemperatureDTO>> GetAllTemperature(int Id = 0)
        {
            if(Id == 0)
                return _dbContext.Temperatures.Select(s=>_mapper.Map<TemperatureDTO>(s)).ToList();
            else
                return _dbContext.Temperatures.Where(c => c.Id > Id).Select(s => _mapper.Map<TemperatureDTO>(s)).ToList();
        }
        /// <summary>
        /// LastTemperature
        /// </summary>
        /// <returns></returns>
        public async Task<TemperatureDTO> LastTemperature()
        {
            int maxId = _dbContext.Temperatures.MaxAsync(t => t.Id).GetAwaiter().GetResult(); // Get the maximum Id

            return _mapper.Map<TemperatureDTO>(_dbContext.Temperatures.FindAsync(maxId).GetAwaiter().GetResult());
        }
    }
}
