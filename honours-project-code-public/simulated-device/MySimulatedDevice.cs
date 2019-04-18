using System;
using System.Collections.Generic;
using System.Text;
using System;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace simulated_device
{
    class MySimulatedDevice
    {
        private static DeviceClient s_deviceClient;

        // The device connection string to authenticate the device with your IoT hub.
        // Using the Azure CLI:
        // az iot hub device-identity show-connection-string --hub-name {YourIoTHubName} --device-id MyDotnetDevice --output table
        private readonly static string s_connectionString = "";

        // Async method to send simulated telemetry
        private static async void SendDeviceToCloudMessagesAsync()
        {
            var tariffCostWh = 0.00014343;
            var powerCostSubMeter1 = 0.000;
            var powerCostSubMeter2 = 0.000;
            var powerCostSubMeter3 = 0.000;
            var powerCostGlobal = 0.000;
            var powerCostGlobalReactive = 0.000;
            var costForEverything = 0.000;
            var low = 0;
            var high = 5;

            using (StreamReader r = new StreamReader("household_power_consumption_copy.json"))
            {
                string json = r.ReadToEnd();

                List<Record> items = JsonConvert.DeserializeObject<List<Record>>(json);

                foreach (var rec in items)
                {
                    
                    //gets cost for everything under sub meter 1 over all time 
                    var add1 = (rec.Sub_metering_1 * tariffCostWh);
                    powerCostSubMeter1 += add1;

                    //gets cost for everything under sub meter 2 over all time 
                    var add2 = (rec.Sub_metering_2 * tariffCostWh);
                    powerCostSubMeter2 += add2;

                    //gets cost for everything under sub meter 3 over all time 
                    var add3 = (rec.Sub_metering_3 * tariffCostWh);
                    powerCostSubMeter3 += add3;

                    //gets cost for everything not under the sub meters over all time 
                    var add4 = (rec.Global_active_power * tariffCostWh);
                    powerCostGlobal += add4;

                    costForEverything += add1 + add2 + add3 + add4;                  

                    var telemetryDataPoint = new
                    {
                        date = rec.Date,
                        time = rec.Time,
                        global_reactive_power = rec.Global_reactive_power,
                        global_active_power = rec.Global_active_power,
                        voltage = rec.Voltage,
                        global_intensity = rec.Global_intensity,
                        sub_metering_1 = rec.Sub_metering_1,
                        sub_metering_2 = rec.Sub_metering_2,
                        sub_metering_3 = rec.Sub_metering_3,
                        powerCostGlobal,
                        powerCostSubMeter1,
                        powerCostSubMeter2,
                        powerCostSubMeter3,
                        costForEverything,
                        low,
                        high
                    };

                    var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                    var message = new Message(Encoding.ASCII.GetBytes(messageString));

                    // Send the telemetry message
                    await s_deviceClient.SendEventAsync(message);
                    Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                    await Task.Delay(1000);
                }
            }
        }

        private static void Main(string[] arg)
        {
            Console.WriteLine("=========================");
            Console.WriteLine("CALCULATION FOR MONTH");
            //MonthCalculation.MonthCalc();
            Console.WriteLine("=========================");
            Console.WriteLine("press enter to continue");
            //Console.ReadLine();

            Console.WriteLine("=========================");
            Console.WriteLine("CALCULATION FOR YEAR");
            //YearCalculation.YearCalc();
            Console.WriteLine("=========================");
            Console.WriteLine("press enter to continue");
            //Console.ReadLine();

            Console.WriteLine("=========================");
            Console.WriteLine("IOT-SIM-START");
            Console.WriteLine("IoT Hub Simulated Smart Meter Device. Ctrl-C to exit. \n");

            // Connect to the IoT hub using the MQTT protocol
            s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString, TransportType.Mqtt);
            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }
    }
}