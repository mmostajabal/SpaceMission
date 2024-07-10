using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionSubscribe
{
    public static class ReadConfig
    {
        /// <summary>
        /// LoadConfig
        /// </summary>
        /// <returns></returns>
        public static bool LoadConfig()
        {
            String curPath = Environment.CurrentDirectory + "/config.txt";
            //String curPath = "config.txt";

            String line;
            string[] arguments;

            using (StreamReader sr = new StreamReader(curPath))
            {
                line = sr.ReadLine();

                //Continue to read until you reach end of file
                while (line != null)
                {
                    try
                    {
                        arguments = line.Split('=');

                        if (arguments.Length == 2)
                        {
                            arguments[1] = arguments[1].Trim().Replace("\\n", "").Replace("\\r", "").Replace(" ", "");
                            switch (arguments[0].ToUpper().Trim())
                            {
                                case "MESSAGE_TOPIC":
                                    CommonVariable.MESSAGE_TOPIC = arguments[1];
                                    break;

                                case "MQTT_SERVER_USER":
                                    CommonVariable.MQTT_SERVER_USER = arguments[1];
                                    break;

                                case "MQTT_SERVER_PASSWORD":
                                    CommonVariable.MQTT_SERVER_PASSWORD = arguments[1];
                                    break;

                                case "MQTT_SERVER_ADDRESS":
                                    CommonVariable.MQTT_SERVER_ADDRESS = arguments[1];
                                    break;

                                case "MQTT_SERVER_PORT":
                                    if (arguments[1] is int) {
                                        CommonVariable.MQTT_SERVER_PORT = Convert.ToInt32(arguments[1]);
                                    }
                                    break;
                                case "BASEURL_API":
                                    CommonVariable.BASEURL_API = arguments[1];
                                    break;
                            }
                        }

                        line = sr.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"ReadConfig {ex.ToString()}");
                    }
                }
            }

            return true;
        }

    }
}
