using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using Serilog;
using SpaceMissionExcel.CommonClass;
using System.Linq.Expressions;
using System.Net.Http;

namespace SpaceMissionExcel
{
    public class Worker : BackgroundService
    {

        private readonly HttpClient _httpClient;
        private int _id;
        public Worker(HttpClient httpClient)
        {
            _httpClient = httpClient;

            //Console.WriteLine("Enter");
            _id = LastIdOfTemperatures(CommonVariable.EXCEL_FILE);
            //Console.WriteLine($"Id {_id}");
        }
        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //Console.WriteLine($"CurId {_id}");
                var temperatures = GetAllTemperatures().GetAwaiter().GetResult();
                //Console.WriteLine($"temperatures {temperatures.Count}");
                if (temperatures?.Count > 0)
                {
                    //Console.WriteLine("Append 2 Excel");
                    AppendDataToExcel(temperatures, CommonVariable.EXCEL_FILE);
                    //Console.WriteLine("Append 3 Excel");
                }

                //AppendDataToExcel(temperatures, CommonVariable.EXCEL_FILE);

                await Task.Delay(CommonVariable.INTERVAL, stoppingToken);
            }
        }
        /// <summary>
        /// GetAllTemperatures
        /// </summary>
        /// <returns></returns>
        public async Task<List<Temperature>> GetAllTemperatures()
        {
            List<Temperature> temperatures = new List<Temperature>();

            try
            {
                //Console.WriteLine("API");
                HttpResponseMessage response = _httpClient.GetAsync("/api/SpaceMission/GetAll?Id=" + _id.ToString()).GetAwaiter().GetResult();
                //Console.WriteLine("API2");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    temperatures = JsonConvert.DeserializeObject<List<Temperature>>(content);
                    //Console.WriteLine($"API3 {temperatures.Count}");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"GetLatestTemperature {ex.ToString()}");
            }

            return temperatures;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="temperatures"></param>
        /// <param name="filePath"></param>
        public void AppendDataToExcel(List<Temperature> temperature, string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (var workbook = new XLWorkbook(filePath))
                    {
                        //Console.WriteLine("Exist file");
                        var worksheet = workbook.Worksheet("TemperatureData");

                        // Find the last row with data
                        //Console.WriteLine("Add 2 Excel");
                        int lastRow = worksheet.LastRowUsed().RowNumber();
                        //Console.WriteLine($"Add 2 Excel lastrow {lastRow}");

                        for (int i = 0; i < temperature.Count; i++)
                        {
                            lastRow++;
                            // Add headers
                            worksheet.Cell(lastRow, 1).Value = temperature[i].Id;
                            worksheet.Cell(lastRow, 2).Value = temperature[i].Timestamp;
                            worksheet.Cell(lastRow, 3).Value = temperature[i].TemperatureCelsius;
                            worksheet.Cell(lastRow, 4).Value = temperature[i].DiffTemperature;
                            
                            ////Console.WriteLine(i);
                        }

                        _id = temperature[temperature.Count - 1].Id;
                        // Save the changes

                        workbook.SaveAs(filePath);

                        //Console.WriteLine($"Add to excel file number rows {lastRow}");
                    }
                }
                else
                {
                    using (var workbook = File.Exists(filePath) ? new XLWorkbook(filePath) : new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("TemperatureData");

                        // Find the last row with data
                        int lastRow = 2;

                        if (worksheet.RowsUsed().Count() == 0)
                        {
                            // Add headers if the worksheet is empty
                            worksheet.Cell(1, 1).Value = "Id";
                            worksheet.Cell(1, 2).Value = "TemperatureCelsius";
                            worksheet.Cell(1, 3).Value = "DiffTemperature";
                            worksheet.Cell(1, 4).Value = "Timestamp";
                        }

                        // Add headers
                        for (int i = 0; i < temperature.Count; i++)
                        {
                            // Add headers
                            worksheet.Cell(lastRow + 1, 1).Value = temperature[i].Id;
                            worksheet.Cell(lastRow + 1, 2).Value = temperature[i].Timestamp;
                            worksheet.Cell(lastRow + 1, 3).Value = temperature[i].TemperatureCelsius;
                            worksheet.Cell(lastRow + 1, 4).Value = temperature[i].DiffTemperature;
                        }

                        lastRow = temperature.Count + 1;
                        // Save the changes
                        workbook.SaveAs(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Could not add date to Excel {ex.ToString()}");
                Log.Error($"Could not add date to Excel {filePath}");
            }
        }
        /// <summary>
        /// LastIdOfTemperatures
        /// </summary>
        /// <returns></returns>
        private int LastIdOfTemperatures(string filePath)
        {
            int lastId = 0;
            try
            {
                if (File.Exists(filePath))
                {
                    using (var workbook = new XLWorkbook(filePath))
                    {
                        var worksheet = workbook.Worksheet("TemperatureData");

                        // Find the last row with data
                        
                        int lastRow = worksheet.LastRowUsed().RowNumber();

                        //Console.WriteLine($"Last Row {lastRow}");
                        lastId = lastRow;
                        lastId = Convert.ToInt32(worksheet.Cell(lastRow, 1).Value);

                        //Console.WriteLine($"LastId {lastId}");
                    }
                }
            }catch(Exception ex) {
                Log.Error("LastIdOfTemperatures Could not find the last Id ");
            }

            return lastId;

        }

    }
}
