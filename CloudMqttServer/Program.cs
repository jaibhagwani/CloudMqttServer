using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace CloudMqttServer
{
    class Program
    {
        private static MqttClient client;
        private static string deviceId = "Device_011";
        private static string assetKey = "Line_07";
        static void Main(string[] args)
        {
            string mqttURI = ConfigurationManager.AppSettings["MqttURI"].ToString();
            int mqttPort = Convert.ToInt32(ConfigurationManager.AppSettings["MqttPort"]);
            bool mqttSecure = Convert.ToBoolean(ConfigurationManager.AppSettings["MqttSecure"]);
            string mqttUser = ConfigurationManager.AppSettings["MqttUser"].ToString();
            string mqttPassword = ConfigurationManager.AppSettings["MqttPassword"].ToString();

            CloudMqttClientConnect(mqttURI, mqttPort, mqttSecure, mqttUser, mqttPassword);

            string message = "JSON payload";

            if(client.IsConnected)
            {
                client.Publish(deviceId + "/" + assetKey, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
            }
        }

        private static void CloudMqttClientConnect(string mqttURI, int mqttPort, bool mqttSecure, string username, string password)
        {
            try
            {
                string clientId = Guid.NewGuid().ToString();
                client = new MqttClient(mqttURI, mqttPort, mqttSecure, null, null, MqttSslProtocols.TLSv1_2);
                client.Connect(clientId, username, password);
            }
            catch (Exception e)
            {
                Console.WriteLine("MQTT Connection Failed" + e.Message);
            }
        }
    }
}
