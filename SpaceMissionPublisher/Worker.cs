using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Protocol;
using Serilog;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using MQTTnet.Client.Publishing;
using System.Collections.Concurrent;
using SpaceMissionPublisher.CommonClass;
using SpaceMissionPublisher.classes;



namespace SpaceMissionPublisher
{
    public class Worker : BackgroundService
    {
        private IMqttClient _mqttClient;
        private LinkedList<MqttApplicationMessage> unsuccessfulPayLoad = new LinkedList<MqttApplicationMessage>();
        public Worker()
        {
            var mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();

        }
        /// <summary>
        /// StartAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<Task> StartAsync(CancellationToken cancellationToken)
        {
            ReadConfig.LoadConfig();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(CommonVariable.MQTT_SERVER_ADDRESS, CommonVariable.MQTT_SERVER_PORT)
                //.WithTcpServer("host.docker.internal", 1883) 
                .Build();

            

            //*******************************************************************
            //  UseDisconnectedHandler
            //*******************************************************************
            _mqttClient.UseDisconnectedHandler(async e =>
            {
                try
                {
                    _mqttClient.ConnectAsync(options).GetAwaiter().GetResult(); 
                    Log.Information($"Worker -> UseDisconnectedHandler : Connect to Broker {DateTime.Now}");

                    SendUnsuccessfulPayLoad();

                    while (unsuccessfulPayLoad.Count > 0)
                    {
                        // Get the first element
                        MqttApplicationMessage curMessage = unsuccessfulPayLoad.First.Value;

                        var result = _mqttClient.PublishAsync(curMessage, CancellationToken.None).GetAwaiter().GetResult();

                        Log.Information($"Worker -> ExecuteAsync : Message Published {curMessage} {result.ReasonCode} {result.ReasonString}");

                        // Remove the first element
                        unsuccessfulPayLoad.RemoveFirst();
                    }

                }
                catch
                {
                    Log.Error($"Worker -> UseDisconnectedHandler : Could not Connect to Broker {DateTime.Now}");
                }
            });

            try
            {
                await _mqttClient.ConnectAsync(options, CancellationToken.None);
            }catch(Exception ex)
            {
                Log.Error($"Worker -> UseDisconnectedHandler : Could not Connect to Broker {DateTime.Now}");
            }

            return base.StartAsync(cancellationToken);
        }
        /// <summary>
        /// SendUnsuccessfulPayLoad
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void SendUnsuccessfulPayLoad()
        {
            MqttApplicationMessage curMessage;
            try
            {

                while (unsuccessfulPayLoad?.Count > 0)
                {

                    curMessage = unsuccessfulPayLoad?.First?.Value;
                    if (curMessage != null)
                    {
                        var result = _mqttClient.PublishAsync(curMessage, CancellationToken.None).GetAwaiter().GetResult();

                        Log.Information($"Worker -> ExecuteAsync : Message Published {curMessage} {result.ReasonCode} {result.ReasonString}");


                        unsuccessfulPayLoad?.RemoveFirst();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Worker -> SendUnsuccessfulPayLoad : Failed to publish message:  Exception {ex.Message} ");
            }

        }


        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            string payLoad = "";
            MqttApplicationMessage message = null;
            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    var temperatureFahrenheit = GlobalModule.GetTemperature();
                    var temperatureCelsius = GlobalModule.FahrenheitToCelsius(temperatureFahrenheit);

                    var data = new
                    {
                        temperature = temperatureCelsius,
                        timestamp = DateTime.UtcNow.ToString("o")
                    };

                    var json = JsonSerializer.Serialize(data);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    Console.WriteLine(json);

                    message = new MqttApplicationMessageBuilder()
                        //.WithTopic("spacemission/topic")  
                        .WithTopic(CommonVariable.MESSAGE_TOPIC)
                        .WithPayload(json)
                        .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                        .WithRetainFlag()
                        .Build();

                    var result = _mqttClient.PublishAsync(message, CancellationToken.None).GetAwaiter().GetResult();

                    if (result.ReasonCode != MqttClientPublishReasonCode.Success)
                    {
                        unsuccessfulPayLoad.AddLast(message);
                    }

                    Log.Information($"Worker -> ExecuteAsync : Message Published {payLoad} {result.ReasonCode} {result.ReasonString}");
                }
                catch (Exception ex)
                {
                    if (message != null)
                    {
                        Console.WriteLine($"Add to array {message}");
                        unsuccessfulPayLoad.AddLast(message);

                    }
                    Log.Error($"Worker -> ExecuteAsync : Failed to publish message:  {payLoad} Exception {ex.Message} ");
                }

                await Task.Delay(CommonVariable.INTERVAL, stoppingToken);
            }
        }


    }
}
