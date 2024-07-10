using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using SpaceMissionSubscribe.CommonClass;
using System.Text;

namespace SpaceMissionSubscribe
{

    public class Worker : BackgroundService
    {
        private IMqttClient _mqttClient;
        private double _latestTemperture;
        private readonly HttpClient _httpClient;
        public Worker(HttpClient httpClient)
        {
            var mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();

            _httpClient = httpClient;

            _latestTemperture = GetLatestTemperature().GetAwaiter().GetResult();
        }

        /// <summary>
        /// StartAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<Task> StartAsync(CancellationToken cancellationToken)
        {

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(CommonVariable.MQTT_SERVER_ADDRESS, CommonVariable.MQTT_SERVER_PORT)
                //.WithTcpServer("host.docker.internal", 1883)  
                .Build();

            //*******************************************************************
            //  UseDisconnectedHandler
            //*******************************************************************
            _mqttClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("MQTT Subscriber connected to broker.");
                await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(CommonVariable.MESSAGE_TOPIC).Build());
                Console.WriteLine("Subscribed to topic 'test/topic'.");
            });

            //*******************************************************************
            //  UseApplicationMessageReceivedHandler
            //*******************************************************************
            _mqttClient.UseApplicationMessageReceivedHandler(async e =>
            {
                Console.WriteLine($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");

                await ProcessMessageAsync(e.ApplicationMessage);

            });

            //*******************************************************************
            //  UseDisconnectedHandler
            //*******************************************************************
            _mqttClient.UseDisconnectedHandler(async e =>
            {
                try
                {
                    _mqttClient.ConnectAsync(options).GetAwaiter().GetResult();
                    Log.Information($"Worker -> UseDisconnectedHandler : Connect to Broker {DateTime.Now}");
                }
                catch
                {
                    Log.Error($"Worker -> UseDisconnectedHandler : Could not Connect to Broker {DateTime.Now}");
                }
            });
            try
            {
                await _mqttClient.ConnectAsync(options, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Log.Error($"Worker -> UseDisconnectedHandler : Could not Connect to Broker {DateTime.Now}");
            }

            return base.StartAsync(cancellationToken);
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
                await Task.Delay(CommonVariable.INTERVAL, stoppingToken);
            }
        }

        /// <summary>
        /// ProcessMessageAsync
        /// </summary>
        /// <param name="message"></param>ApiService
        /// <returns></returns>
        async Task ProcessMessageAsync(MqttApplicationMessage message)
        {
            JObject jsonObject = JObject.Parse(Encoding.UTF8.GetString(message.Payload));
            try
            {
                if (jsonObject["temperature"] != null)
                {
                    var curTemperature = jsonObject["temperature"];

                    if (curTemperature != null && curTemperature.Type == JTokenType.Float)
                    {
                        Temperature temperature = new Temperature
                        {
                            Id = 0,
                            TemperatureCelsius = Convert.ToDouble(curTemperature),
                            DiffTemperature = Convert.ToDouble(curTemperature) - _latestTemperture,
                            Timestamp = DateTime.Now
                        };

                        var json = JsonConvert.SerializeObject(temperature);
                        var data = new StringContent(json, Encoding.UTF8, "application/json");
                        var response = _httpClient.PostAsync("/api/SpaceMission/AddTemperature", data).GetAwaiter().GetResult();

                        _latestTemperture = Convert.ToDouble(curTemperature);
                        Console.WriteLine($"Saved Last Temperature {_latestTemperture }");
                        //string result = response.Content.ReadAsStringAsync().Result;

                    }
                }

            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error($"ProcessMessageAsync {ex.ToString()}");
            }
            //Console.WriteLine($"Processing message: Topic={message.Topic}, Payload={Encoding.UTF8.GetString(message.Payload)}  temperature={jsonObject["temperature"]} timestamp={jsonObject["timestamp"]} ");
        }
        /// <summary>
        /// GetLatestTemperature
        /// </summary>
        /// <returns></returns>
        async Task<double> GetLatestTemperature()
        {
            _latestTemperture = 0;
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("/api/SpaceMission/GetLatestTemperature");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Temperature>(content);
                    _latestTemperture = result != null ? result.TemperatureCelsius : 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"GetLatestTemperature {ex.ToString()}");
            }

            return _latestTemperture;
        }

    }
}
