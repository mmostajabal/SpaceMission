﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionPublisher.CommonClass
{
    public static class CommonVariable
    {
        public static int INTERVAL = 5000;

        public static string MESSAGE_TOPIC = "spacemission/topic";

        public static string MQTT_SERVER_ADDRESS = "host.docker.internal";

        public static int MQTT_SERVER_PORT = 1883;

        public static string MQTT_SERVER_USER = "";

        public static string MQTT_SERVER_PASSWORD = "";
    }
}
