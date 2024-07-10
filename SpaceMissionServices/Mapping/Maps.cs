using AutoMapper;
using SpaceMissionModels;
using SpaceMissionShared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionServices.Mapping
{
    public class Maps : Profile
    {

        public Maps()
        {
            CreateMap<Temperature, TemperatureDTO>().ReverseMap();
            CreateMap<TemperatureDTO, Temperature>().ReverseMap();

        }
    }
}
