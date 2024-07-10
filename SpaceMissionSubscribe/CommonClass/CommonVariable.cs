using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionSubscribe.CommonClass
{
    public static class CommonVariable
    {
        public static int INTERVAL = 5000;

        public static string MESSAGE_TOPIC = "spacemission12/topic";

        public static string MQTT_SERVER_ADDRESS = "host.docker.internal";

        public static int MQTT_SERVER_PORT = 1883;

        public static string MQTT_SERVER_USER = "";

        public static string MQTT_SERVER_PASSWORD = "";

        public static string BASEURL_API = "";

        public static string EXCEL_FILE = "temperature_data";
    }
}
