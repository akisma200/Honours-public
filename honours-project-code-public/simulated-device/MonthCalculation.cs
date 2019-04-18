using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace simulated_device
{
    class MonthCalculation
    {


        public static void MonthCalc()
        {
            var tariffCostWh = 0.00014343;
            var powerCostSubMeter1 = 0.000;
            var powerCostSubMeter2 = 0.000;
            var powerCostSubMeter3 = 0.000;
            var powerCostGlobal = 0.000;
            var powerCostGlobalReactive = 0.000;
            var costForEverything = 0.000;

            using (StreamReader r = new StreamReader("C:\\Users\\AKISMA200\\Documents\\AAAAAAAAUni\\Honours Project\\simulated-device\\month.json"))
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

                    var add5 = (rec.Global_reactive_power * tariffCostWh);
                    powerCostGlobalReactive += add5;

                    costForEverything += add1 + add2 + add3 + add4;

                    Console.WriteLine("==============================================");
                    Console.WriteLine(costForEverything);

                    Task.Delay(1);                    
                }
                costForEverything = costForEverything * 12;
                Console.WriteLine(costForEverything);
            }
        }
    }
}
  


    
