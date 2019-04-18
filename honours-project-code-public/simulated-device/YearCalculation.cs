using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace simulated_device
{
    class YearCalculation
    {
        public static void YearCalc()
        {
            
            var tariffCostWh = 0.00014343;
            var tariffCostKWh = 0.14343;
            var powerCostSubMeter1 = 0.000;
            var powerCostSubMeter2 = 0.000;
            var powerCostSubMeter3 = 0.000;
            var powerCostGlobal = 0.000;
            var powerCostGlobalReactive = 0.000;
            var costForEverything = 0.000;

            using (StreamReader r = new StreamReader("C:\\Users\\AKISMA200\\Documents\\AAAAAAAAUni\\Honours Project\\simulated-device\\year.json"))
            {
                string json = r.ReadToEnd();

                List<Record> items = JsonConvert.DeserializeObject<List<Record>>(json);
                
                foreach (var rec in items)
                {
                    //============================================================================================================================
                    //working out cost by global wattage for one hour if the energy consumption in one minute was to remain the same for the hour
                    
                    //watts = voltage * amperes
                    var watts = (rec.Voltage * rec.Global_intensity); //= global wattage

                    //get wattage as KWh = (watt * number of hours)/1000 
                    var KWh = (watts * 1)/1000;

                    //Get cost for 1 KWh = KWh * tariffCost
                    var totalPowerCostKWh = KWh * tariffCostKWh;
                    //==============================================================================================================================

                    //Working out the total cost for each sub meter over the year then also working out the cost for everything else over the year then
                    //adding them all together to ge the full spend on energy for that year on the propesed tariff cost

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

                    //get cost for all wasted energy over all time
                    var add5 = (rec.Global_reactive_power * tariffCostWh);
                    powerCostGlobalReactive += add5;

                    //add everything together to get the final cost for the year according to energy consumed for the year
                    costForEverything += add1 + add2 + add3 + add4 ;

                    Console.WriteLine("==============================================");
                    Console.WriteLine(costForEverything);
                    Console.WriteLine(totalPowerCostKWh);
                                        
                    Task.Delay(1);
                }
                Console.WriteLine(costForEverything);
            }
        }
    }
}

    
