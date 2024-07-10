using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionPublisher.CommonClass
{
    public static class GlobalModule
    {
        /// <summary>
        /// FahrenheitToCelsius
        /// </summary>
        /// <param name="fahrenheit"></param>
        /// <returns></returns>
        public static double FahrenheitToCelsius(double fahrenheit)
        {
            return (fahrenheit - 32) * 5.0 / 9.0;
        }
        /// <summary>
        /// GetTemperature
        /// </summary>
        /// <returns></returns>
        public static double GetTemperature()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            return random.Next(-30, 130); // Simulating a range of temperatures in Fahrenheit
        }
    }
}
