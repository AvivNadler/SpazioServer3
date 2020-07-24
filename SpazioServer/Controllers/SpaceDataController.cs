using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;
using SpazioServer.Models;
using System.Diagnostics;

namespace SpazioServer.Controllers
{
    public class SpaceDataController : ApiController
    {
        //public List<string> comments = new List<string>();

        // GET api/<controller>
        /*  public IEnumerable<string> Get()
          {
              return new string[] { "value1", "value2" };
          }*/

        [HttpGet]
        public List<SpaceData> Get(string field, string city, string street, string number, string date)
        {
            List<SpaceData> list = new List<SpaceData>();
            DBServices dbs = new DBServices();
            foreach (Space item in dbs.readSpacesBySearch(field, city, street, number))
            {
                if (dbs.readAllAvailbilities(item.Id, date).Count > 0)
                {
                    SpaceData s = new SpaceData();
                    s.Space = item;
                    //s.Realavailability = dbs.readAllAvailbilities(s.Space.Id, date);
                    s.Facility = dbs.readFacilities(s.Space.Id);
                    s.Equipment = dbs.readEquipments(s.Space.Id).ToArray();
                    //s.Orders = dbs.readOrdersOfSpace(s.Space.Id);
                    s.WeekAvailabilities = dbs.readWeekAvailbilitiesById(s.Space.Id);

                    List<string> tempAvail = new List<string>();
                    foreach (string item2 in dbs.readAllAvailbilities(s.Space.Id, date))
                    {
                        tempAvail.Add(item2.Split('-')[0].Split(':')[0] + ":" + item2.Split('-')[0].Split(':')[1] + "-" + item2.Split('-')[1].Split(':')[0] + ":" + item2.Split('-')[1].Split(':')[1]);
                    }
                    s.Realavailability = tempAvail;
                    double grade = GradeCalc3("", s);
                    s.Space.Grade = grade;
                    list.Add(s);
                }

            }
            List<SpaceData> SortedList = list.OrderBy(spacedata => spacedata.Space.Grade).ToList();

            return SortedList;
        }

        public double GradeCalc(string temp, SpaceData s)
        {
            //PREMIUM
            DBServices dbs = new DBServices();
            double Grade = 0;
            Dictionary<string, int> Grades = dbs.getGrades();
            Dictionary<string, int> Data = new Dictionary<string, int>();
            if (s.Space.Field == "Art")
            {
                Data = dbs.readArtFiltersData();
            }
            else
            if (s.Space.Field == "Sport")
            {
                Data = dbs.readSportFiltersData();
            }
            else
            {
                Data = dbs.readBeautyFiltersData();
            }

            int ordersAmount = dbs.getOrdersAmountById(s.Space.Id);
            int premium = 0;
            bool isPremium = dbs.isUserPremium(s.Space.UserEmail);
            int visits = dbs.readSpaceVisits(s.Space.Id);
            double conversionRate = 0;

            Dictionary<string, double> SpaceGrades = new Dictionary<string, double>()
                                            {
                                                {"PriceGrade",0},
                                                {"RatingGrade",0},
                                                {"CapacityGrade",0},
                                                {"OrdersGrade",0},
                                                {"PremiumGrade",0},
                                                {"ConversionGrade",0},
                                                {"ToiletGrade",0},
                                                {"ParkingGrade",0},
                                                {"KitchenGrade",0},
                                                {"IntercomGrade",0},
                                                {"WifiGrade",0},
                                                {"AccessibleGrade",0},
                                                {"AirConditionGrade",0},
                                                {"TRXGrade",0}
                                            };

            //foreach (KeyValuePair<string, double> item in SpaceGrades)
            //{
            //System.Diagnostics.Trace.WriteLine("message");
            //Trace.WriteLine("Key: {0}, Value: {1} item.Key");

            ////debug.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
            //Debug.WriteLine(SpaceGrades,"spacegrades");
            //this.comments.Add("testincomments");
            //}


            if (visits != 0)
            {
                conversionRate = (ordersAmount / visits);
            }

            if (isPremium)
            {
                premium = 1;
            }


            //foreach (KeyValuePair<string, int> item in Data)
            //{
            //System.Diagnostics.Trace.WriteLine("message");
            //Trace.WriteLine("Key: {0}, Value: {1} item.Key");

            //Debug.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
            //Debug.WriteLine(SpaceGrades,"spacegrades");
            //this.comments.Add("testincomments");
            //}

            double avgrating = 0;
            avgrating = dbs.readFiltersDataRating(s.Space.Field); // relate to average or not? #TODO
            if (s.Space.Rank != 3.499)
            {
                SpaceGrades["RatingGrade"] = (avgrating - s.Space.Rank) * Grades["Rating"] * (-1);
                Grade += (avgrating - s.Space.Rank) * Grades["Rating"] * (-1);

            }
            SpaceGrades["OrdersGrade"] = ordersAmount * Grades["Order"];
            Grade += ordersAmount * Grades["Order"];

            SpaceGrades["PremiumGrade"] = premium * Grades["Premium"];
            Grade += premium * Grades["Premium"];

            SpaceGrades["ConversionGrade"] = conversionRate * Grades["Conversion"];
            Grade += conversionRate * Grades["Conversion"];

            if (Data["Counter"] != 0)
            {
                double priceAVG = (Data["minPriceAvg"] + Data["maxPriceAvg"] / 2.0);
                SpaceGrades["PriceGrade"] = ((s.Space.Price - (priceAVG)) / 10) * (Grades["Price"]) * (-1);
                Grade += ((s.Space.Price - (priceAVG)) / 10) * (Grades["Price"]) * (-1);

                double capacityAVG = (Data["minCapacityAvg"] + Data["maxCapacityAvg"] / 2.0);
                SpaceGrades["CapacityGrade"] = ((Convert.ToDouble(s.Space.Capabillity) - (capacityAVG)) / 10) * (Grades["Capacity"]);
                Grade += ((Convert.ToDouble(s.Space.Capabillity) - (capacityAVG)) / 10) * (Grades["Capacity"]);




                if (s.Facility.Toilet)
                {
                    SpaceGrades["ToiletGrade"] = Grades["Facility"] * Data["ToiletCounter"] / Data["Counter"];
                    Grade += Grades["Facility"] * Data["ToiletCounter"] / Data["Counter"];
                }
                if (s.Facility.Parking)
                {
                    SpaceGrades["ParkingGrade"] = Grades["Facility"] * Data["ToiletCounter"] / Data["Counter"];
                    Grade += Grades["Facility"] * Data["ParkingCounter"] / Data["Counter"];
                }
                if (s.Facility.Kitchen)
                {
                    SpaceGrades["KitchenGrade"] = Grades["Facility"] * Data["KitchenCounter"] / Data["Counter"];
                    Grade += Grades["Facility"] * Data["KitchenCounter"] / Data["Counter"];
                }
                if (s.Facility.Intercom)
                {
                    SpaceGrades["IntercomGrade"] = Grades["Facility"] * Data["KitchenCounter"] / Data["Counter"];
                    Grade += Grades["Facility"] * Data["IntercomCounter"] / Data["Counter"];
                }
                if (s.Facility.Accessible)
                {
                    Grade += Grades["Facility"] * Data["AccessibleCounter"] / Data["Counter"];
                    SpaceGrades["AccessibleGrade"] = Grades["Facility"] * Data["AccessibleCounter"] / Data["Counter"];

                }
                if (s.Facility.Wifi)
                {
                    Grade += Grades["Facility"] * Data["WiFiCounter"] / Data["Counter"];
                    SpaceGrades["WifiGrade"] = Grades["Facility"] * Convert.ToDouble(Data["WiFiCounter"]) / Data["Counter"];

                }
                if (s.Facility.AirCondition)
                {
                    Grade += Grades["Facility"] * Data["AirConditionCounter"] / Data["Counter"];
                    SpaceGrades["AirConditionGrade"] = Grades["Facility"] * Data["AirConditionCounter"] / Data["Counter"];

                }

                for (int i = 0; i < s.Equipment.Length; i++)
                {
                    if (s.Equipment[i].Name == "TRX")
                    {
                        SpaceGrades["TRXGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["TRXCounter"] / Data["Counter"];
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["TRXCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Treadmill")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["TreadmillCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Stationary bicycle")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["StationaryBicycleCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Bench")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["BenchCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Dumbells")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["DumbellsCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Barbell")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["BarbellCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Dryers")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["DryersCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Nail Polish Racks")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["NailPolishRacksCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Reception Area Seating and Decor")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["ReceptionAreaSeatingandDecorCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Laser Hair Removal")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["LaserHairRemovalCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Barbell")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["BarbellCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "PedicureManicure")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["PedicureManicureCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Hair Coloring Kit")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["HairColoringKitCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Canvas")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["CanvasCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Green Screen")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["GreenScreenCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Potter's Wheel")
                    {
                        Grade += Grades["Equipment"] * Data["PottersWheelCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Guitar")
                    {
                        Grade += Grades["Equipment"] * Data["GuitarCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Drum")
                    {
                        Grade += Grades["Equipment"] * Data["DrumCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Speaker")
                    {
                        Grade += Grades["Equipment"] * Data["SpeakerCounter"] / Data["Counter"];
                    }
                    else { break; }


                }
            }


            double totalGrade = 0;
            foreach (KeyValuePair<string, double> item in SpaceGrades)
            {
                //System.Diagnostics.Trace.WriteLine("message");
                //Trace.WriteLine("Key: {0}, Value: {1} item.Key");

                //debug.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
                totalGrade += item.Value;
                //Debug.WriteLine(SpaceGrades,"spacegrades");
                //this.comments.Add("testincomments");
            }
            //debug.WriteLine("Space: " + s.Space.Id + " Total Grade: " + totalGrade );

            //Premium
            return Grade;
        }

        public Dictionary<string, double> GradeCalc2(string temp, SpaceData s)
        {
            //PREMIUM
            DBServices dbs = new DBServices();
            double Grade = 0;
            Dictionary<string, int> Grades = dbs.getGrades();
            Dictionary<string, int> Data = new Dictionary<string, int>();

            Dictionary<string, double> SpaceGrades = new Dictionary<string, double>()
                                            {
                                                {"PriceGrade",0},
                                                {"RatingGrade",0},
                                                {"CapacityGrade",0},
                                                {"OrdersGrade",0},
                                                {"PremiumGrade",0},
                                                {"ConversionGrade",0},
                                                {"ToiletGrade",0},
                                                {"ParkingGrade",0},
                                                {"KitchenGrade",0},
                                                {"IntercomGrade",0},
                                                {"WifiGrade",0},
                                                {"AccessibleGrade",0},
                                                {"AirConditionGrade",0},

                                            };
            if (s.Space.Field == "Art")
            {
                Data = dbs.readArtFiltersData();
                SpaceGrades.Add("CanvasGrade", 0);
                SpaceGrades.Add("Green ScreenGrade", 0);
                SpaceGrades.Add("Potter's WheelGrade", 0);
                SpaceGrades.Add("GuitarGrade", 0);
                SpaceGrades.Add("DrumGrade", 0);
                SpaceGrades.Add("SpeakerGrade", 0);
            }
            else
            if (s.Space.Field == "Sport")
            {
                Data = dbs.readSportFiltersData();
                SpaceGrades.Add("TRXGrade", 0);
                SpaceGrades.Add("TreadmillGrade", 0);
                SpaceGrades.Add("StationaryBicycleGrade", 0);
                SpaceGrades.Add("BenchGrade", 0);
                SpaceGrades.Add("DumbellsGrade", 0);
                SpaceGrades.Add("BarbellGrade", 0);
            }
            else
            {
                Data = dbs.readBeautyFiltersData();
                SpaceGrades.Add("DryersGrade", 0);
                SpaceGrades.Add("Nail Polish RacksGrade", 0);
                SpaceGrades.Add("Reception Area Seating and DecorGrade", 0);
                SpaceGrades.Add("Laser Hair RemovalGrade", 0);
                SpaceGrades.Add("PedicureManicureGrade", 0);
                SpaceGrades.Add("Hair Coloring KitGrade", 0);
            }

            int ordersAmount = dbs.getOrdersAmountById(s.Space.Id);
            int premium = 0;
            bool isPremium = dbs.isUserPremium(s.Space.UserEmail);
            int visits = dbs.readSpaceVisits(s.Space.Id);
            double conversionRate = 0;



            //foreach (KeyValuePair<string, double> item in SpaceGrades)
            //{
            //System.Diagnostics.Trace.WriteLine("message");
            //Trace.WriteLine("Key: {0}, Value: {1} item.Key");

            //Debug.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
            //Debug.WriteLine(SpaceGrades,"spacegrades");
            //this.comments.Add("testincomments");
            //}


            if (visits != 0)
            {
                conversionRate = (ordersAmount / visits);
            }

            if (isPremium)
            {
                premium = 1;
            }


            //foreach (KeyValuePair<string, int> item in Data)
            //{
            //System.Diagnostics.Trace.WriteLine("message");
            //Trace.WriteLine("Key: {0}, Value: {1} item.Key");

            //Debug.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
            //Debug.WriteLine(SpaceGrades,"spacegrades");
            //this.comments.Add("testincomments");
            //}


            double priceAVG = (Data["minPriceAvg"] + Data["maxPriceAvg"]) / 2.0;
            //debug.WriteLine("priceAVG " + priceAVG.ToString() + " s.Space.Price " + s.Space.Price.ToString() + " minpriceavg " + Data["minPriceAvg"] + " maxpriceavg " + Data["maxPriceAvg"]);
            SpaceGrades["PriceGrade"] = ((s.Space.Price - (priceAVG)) / 10) * (Grades["Price"]) * (-1);
            Grade += ((s.Space.Price - (priceAVG)) / 10) * (Grades["Price"]) * (-1);

            double capacityAVG = (Data["minCapacityAvg"] + Data["maxCapacityAvg"]) / 2.0;
            SpaceGrades["CapacityGrade"] = ((Convert.ToDouble(s.Space.Capabillity) - (capacityAVG)) / 10) * (Grades["Capacity"]);
            Grade += ((Convert.ToDouble(s.Space.Capabillity) - (capacityAVG)) / 10) * (Grades["Capacity"]);

            double avgrating = 0;
            avgrating = dbs.readFiltersDataRating(s.Space.Field); // relate to average or not? #TODO
            if (s.Space.Rank != 3.499)
            {
                SpaceGrades["RatingGrade"] = (avgrating - s.Space.Rank) * Grades["Rating"] * (-1);
                Grade += (avgrating - s.Space.Rank) * Grades["Rating"] * (-1);

            }
            SpaceGrades["OrdersGrade"] = ordersAmount * Grades["Order"];
            Grade += ordersAmount * Grades["Order"];

            SpaceGrades["PremiumGrade"] = premium * Grades["Premium"];
            Grade += premium * Grades["Premium"];

            SpaceGrades["ConversionGrade"] = conversionRate * Grades["Conversion"];
            Grade += conversionRate * Grades["Conversion"];


            if (s.Facility.Toilet)
            {
                SpaceGrades["ToiletGrade"] = Grades["Facility"] * Convert.ToDouble(Data["ToiletCounter"]) / Data["Counter"];
                Grade += Grades["Facility"] * Convert.ToDouble(Data["ToiletCounter"]) / Data["Counter"];
            }
            if (s.Facility.Parking)
            {
                SpaceGrades["ParkingGrade"] = Grades["Facility"] * Convert.ToDouble(Data["ToiletCounter"]) / Data["Counter"];
                Grade += Grades["Facility"] * Convert.ToDouble(Data["ParkingCounter"]) / Data["Counter"];
            }
            if (s.Facility.Kitchen)
            {
                SpaceGrades["KitchenGrade"] = Grades["Facility"] * Convert.ToDouble(Data["KitchenCounter"]) / Data["Counter"];
                Grade += Grades["Facility"] * Convert.ToDouble(Data["KitchenCounter"]) / Data["Counter"];
            }
            if (s.Facility.Intercom)
            {
                SpaceGrades["IntercomGrade"] = Grades["Facility"] * Convert.ToDouble(Data["IntercomCounter"]) / Data["Counter"];
                Grade += Grades["Facility"] * Convert.ToDouble(Data["IntercomCounter"]) / Data["Counter"];
            }
            if (s.Facility.Accessible)
            {
                Grade += Grades["Facility"] * Convert.ToDouble(Data["AccessibleCounter"]) / Data["Counter"];
                SpaceGrades["AccessibleGrade"] = Grades["Facility"] * Convert.ToDouble(Data["AccessibleCounter"]) / Data["Counter"];

            }
            if (s.Facility.Wifi)
            {
                Grade += Grades["Facility"] * Convert.ToDouble(Data["WiFiCounter"]) / Data["Counter"];
                SpaceGrades["WifiGrade"] = Grades["Facility"] * Convert.ToDouble(Data["WiFiCounter"]) / Data["Counter"];

            }
            if (s.Facility.AirCondition)
            {
                Grade += Grades["Facility"] * Convert.ToDouble(Data["AirConditionCounter"]) / Data["Counter"];
                SpaceGrades["AirConditionGrade"] = Grades["Facility"] * Convert.ToDouble(Data["AirConditionCounter"]) / Data["Counter"];

            }

            for (int i = 0; i < s.Equipment.Length; i++)
            {
                if (s.Space.Field == "Sport")
                {
                    if (s.Equipment[i].Name == "TRX")
                    {
                        SpaceGrades["TRXGrade"] = Grades["Equipment"] * Convert.ToDouble(Data["TRXCounter"]) / Data["Counter"];
                        Grade += Grades["Equipment"] * Convert.ToDouble(Data["TRXCounter"]) / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Treadmill")
                    {
                        Grade += Grades["Equipment"] * Convert.ToDouble(Data["TreadmillCounter"]) / Data["Counter"];
                        SpaceGrades["TreadmillGrade"] = Grades["Equipment"] * Convert.ToDouble(Data["TreadmillCounter"]) / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "Stationary bicycle")
                    {
                        Grade += Grades["Equipment"] * Convert.ToDouble(Data["StationaryBicycleCounter"]) / Data["Counter"];
                        SpaceGrades["StationaryBicycleGrade"] = Grades["Equipment"] * Convert.ToDouble(Data["StationaryBicycleCounter"]) / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Bench")
                    {
                        Grade += Grades["Equipment"] * Convert.ToDouble(Data["BenchCounter"]) / Data["Counter"];
                        SpaceGrades["BenchGrade"] = Grades["Equipment"] * Convert.ToDouble(Data["BenchCounter"]) / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "Dumbells")
                    {
                        Grade += Grades["Equipment"] * Convert.ToDouble(Data["DumbellsCounter"]) / Data["Counter"];
                        SpaceGrades["DumbellsGrade"] = Grades["Equipment"] * Convert.ToDouble(Data["DumbellsCounter"]) / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "Barbell")
                    {
                        Grade += Grades["Equipment"] * Convert.ToDouble(Data["BarbellCounter"]) / Data["Counter"];
                        SpaceGrades["BarbellGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["BarbellCounter"] / Data["Counter"];

                    }
                    else { break; }
                }
                if (s.Space.Field == "Beauty")
                {
                    if (s.Equipment[i].Name == "Dryers")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["DryersCounter"] / Data["Counter"];
                        SpaceGrades["DryersGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["DryersCounter"] / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "Nail Polish Racks")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["NailPolishRacksCounter"] / Data["Counter"];
                        SpaceGrades["Nail Polish RacksGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["NailPolishRacksCounter"] / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "Reception Area Seating and Decor")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["ReceptionAreaSeatingandDecorCounter"] / Data["Counter"];
                        SpaceGrades["Reception Area Seating and DecorGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["ReceptionAreaSeatingandDecorCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Laser Hair Removal")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["LaserHairRemovalCounter"] / Data["Counter"];
                        SpaceGrades["Laser Hair RemovalGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["LaserHairRemovalCounter"] / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "PedicureManicure")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["PedicureManicureCounter"] / Data["Counter"];
                        SpaceGrades["PedicureManicureGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["PedicureManicureCounter"] / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "Hair Coloring Kit")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["HairColoringKitCounter"] / Data["Counter"];
                        SpaceGrades["Hair Coloring KitGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["HairColoringKitCounter"] / Data["Counter"];
                    }
                    else { break; }
                }
                else
                {
                    if (s.Equipment[i].Name == "Canvas")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["CanvasCounter"] / Data["Counter"];
                        SpaceGrades["CanvasGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["CanvasCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Green Screen")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["GreenScreenCounter"] / Data["Counter"];
                        SpaceGrades["Green ScreenGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["GreenScreenCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Potter's Wheel")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["PottersWheelCounter"] / Data["Counter"];
                        SpaceGrades["Potter's WheelGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["PottersWheelCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Guitar")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["GuitarCounter"] / Data["Counter"];
                        SpaceGrades["GuitarGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["GuitarCounter"] / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "Drum")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["DrumCounter"] / Data["Counter"];
                        SpaceGrades["DrumGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["DrumCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Speaker")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["SpeakerCounter"] / Data["Counter"];
                        SpaceGrades["SpeakerGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["SpeakerCounter"] / Data["Counter"];
                    }
                    else { break; }
                }

            }
            double totalGrade = 0;
            foreach (KeyValuePair<string, double> item in SpaceGrades)
            {
                //System.Diagnostics.Trace.WriteLine("message");
                //Trace.WriteLine("Key: {0}, Value: {1} item.Key");

                //debug.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
                totalGrade += item.Value;
                ////debug.WriteLine(SpaceGrades,"spacegrades");
                //this.comments.Add("testincomments");
            }
            //debug.WriteLine("Space: " + s.Space.Id + " Total Grade: " + totalGrade);
            totalGrade = Math.Round(totalGrade, 2);

            SpaceGrades.Add("TotalGrade", totalGrade);

            //Premium
            return SpaceGrades;
        }
        public double GradeCalc3(string temp, SpaceData s)
        {
            //PREMIUM
            DBServices dbs = new DBServices();
            double Grade = 0;
            Dictionary<string, int> Grades = dbs.getGrades();
            Dictionary<string, int> Data = new Dictionary<string, int>();

            Dictionary<string, double> SpaceGrades = new Dictionary<string, double>()
                                            {
                                                {"PriceGrade",0},
                                                {"RatingGrade",0},
                                                {"CapacityGrade",0},
                                                {"OrdersGrade",0},
                                                {"PremiumGrade",0},
                                                {"ConversionGrade",0},
                                                {"ToiletGrade",0},
                                                {"ParkingGrade",0},
                                                {"KitchenGrade",0},
                                                {"IntercomGrade",0},
                                                {"WifiGrade",0},
                                                {"AccessibleGrade",0},
                                                {"AirConditionGrade",0},

                                            };
            if (s.Space.Field == "Art")
            {
                Data = dbs.readArtFiltersData();
                SpaceGrades.Add("CanvasGrade", 0);
                SpaceGrades.Add("Green ScreenGrade", 0);
                SpaceGrades.Add("Potter's WheelGrade", 0);
                SpaceGrades.Add("GuitarGrade", 0);
                SpaceGrades.Add("DrumGrade", 0);
                SpaceGrades.Add("SpeakerGrade", 0);
            }
            else
            if (s.Space.Field == "Sport")
            {
                Data = dbs.readSportFiltersData();
                SpaceGrades.Add("TRXGrade", 0);
                SpaceGrades.Add("TreadmillGrade", 0);
                SpaceGrades.Add("StationaryBicycleGrade", 0);
                SpaceGrades.Add("BenchGrade", 0);
                SpaceGrades.Add("DumbellsGrade", 0);
                SpaceGrades.Add("BarbellGrade", 0);
            }
            else
            {
                Data = dbs.readBeautyFiltersData();
                SpaceGrades.Add("DryersGrade", 0);
                SpaceGrades.Add("Nail Polish RacksGrade", 0);
                SpaceGrades.Add("Reception Area Seating and DecorGrade", 0);
                SpaceGrades.Add("Laser Hair RemovalGrade", 0);
                SpaceGrades.Add("PedicureManicureGrade", 0);
                SpaceGrades.Add("Hair Coloring KitGrade", 0);
            }

            int ordersAmount = dbs.getOrdersAmountById(s.Space.Id);
            int premium = 0;
            bool isPremium = dbs.isUserPremium(s.Space.UserEmail);
            int visits = dbs.readSpaceVisits(s.Space.Id);
            double conversionRate = 0;



            //foreach (KeyValuePair<string, double> item in SpaceGrades)
            //{
            //System.Diagnostics.Trace.WriteLine("message");
            //Trace.WriteLine("Key: {0}, Value: {1} item.Key");

            //Debug.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
            //Debug.WriteLine(SpaceGrades,"spacegrades");
            //this.comments.Add("testincomments");
            //}


            if (visits != 0)
            {
                conversionRate = (ordersAmount / visits);
            }

            if (isPremium)
            {
                premium = 1;
            }


            //foreach (KeyValuePair<string, int> item in Data)
            //{
            //System.Diagnostics.Trace.WriteLine("message");
            //Trace.WriteLine("Key: {0}, Value: {1} item.Key");

            //Debug.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
            //Debug.WriteLine(SpaceGrades,"spacegrades");
            //this.comments.Add("testincomments");
            //}


            double priceAVG = (Data["minPriceAvg"] + Data["maxPriceAvg"]) / 2.0;
            //debug.WriteLine("priceAVG " + priceAVG.ToString() + " s.Space.Price " + s.Space.Price.ToString() + " minpriceavg " + Data["minPriceAvg"] + " maxpriceavg " + Data["maxPriceAvg"]);
            SpaceGrades["PriceGrade"] = ((s.Space.Price - (priceAVG)) / 10) * (Grades["Price"]) * (-1);
            Grade += ((s.Space.Price - (priceAVG)) / 10) * (Grades["Price"]) * (-1);

            double capacityAVG = (Data["minCapacityAvg"] + Data["maxCapacityAvg"]) / 2.0;
            SpaceGrades["CapacityGrade"] = ((Convert.ToDouble(s.Space.Capabillity) - (capacityAVG)) / 10) * (Grades["Capacity"]);
            Grade += ((Convert.ToDouble(s.Space.Capabillity) - (capacityAVG)) / 10) * (Grades["Capacity"]);

            double avgrating = 0;
            avgrating = dbs.readFiltersDataRating(s.Space.Field); // relate to average or not? #TODO
            if (s.Space.Rank != 3.499)
            {
                SpaceGrades["RatingGrade"] = (avgrating - s.Space.Rank) * Grades["Rating"] * (-1);
                Grade += (avgrating - s.Space.Rank) * Grades["Rating"] * (-1);

            }
            SpaceGrades["OrdersGrade"] = ordersAmount * Grades["Order"];
            Grade += ordersAmount * Grades["Order"];

            SpaceGrades["PremiumGrade"] = premium * Grades["Premium"];
            Grade += premium * Grades["Premium"];

            SpaceGrades["ConversionGrade"] = conversionRate * Grades["Conversion"];
            Grade += conversionRate * Grades["Conversion"];


            if (s.Facility.Toilet)
            {
                SpaceGrades["ToiletGrade"] = Grades["Facility"] * Convert.ToDouble(Data["ToiletCounter"]) / Data["Counter"];
                Grade += Grades["Facility"] * Convert.ToDouble(Data["ToiletCounter"]) / Data["Counter"];
            }
            if (s.Facility.Parking)
            {
                SpaceGrades["ParkingGrade"] = Grades["Facility"] * Convert.ToDouble(Data["ToiletCounter"]) / Data["Counter"];
                Grade += Grades["Facility"] * Convert.ToDouble(Data["ParkingCounter"]) / Data["Counter"];
            }
            if (s.Facility.Kitchen)
            {
                SpaceGrades["KitchenGrade"] = Grades["Facility"] * Convert.ToDouble(Data["KitchenCounter"]) / Data["Counter"];
                Grade += Grades["Facility"] * Convert.ToDouble(Data["KitchenCounter"]) / Data["Counter"];
            }
            if (s.Facility.Intercom)
            {
                SpaceGrades["IntercomGrade"] = Grades["Facility"] * Convert.ToDouble(Data["IntercomCounter"]) / Data["Counter"];
                Grade += Grades["Facility"] * Convert.ToDouble(Data["IntercomCounter"]) / Data["Counter"];
            }
            if (s.Facility.Accessible)
            {
                Grade += Grades["Facility"] * Convert.ToDouble(Data["AccessibleCounter"]) / Data["Counter"];
                SpaceGrades["AccessibleGrade"] = Grades["Facility"] * Convert.ToDouble(Data["AccessibleCounter"]) / Data["Counter"];

            }
            if (s.Facility.Wifi)
            {
                Grade += Grades["Facility"] * Convert.ToDouble(Data["WiFiCounter"]) / Data["Counter"];
                SpaceGrades["WifiGrade"] = Grades["Facility"] * Convert.ToDouble(Data["WiFiCounter"]) / Data["Counter"];

            }
            if (s.Facility.AirCondition)
            {
                Grade += Grades["Facility"] * Convert.ToDouble(Data["AirConditionCounter"]) / Data["Counter"];
                SpaceGrades["AirConditionGrade"] = Grades["Facility"] * Convert.ToDouble(Data["AirConditionCounter"]) / Data["Counter"];

            }

            for (int i = 0; i < s.Equipment.Length; i++)
            {
                if (s.Space.Field == "Sport")
                {
                    if (s.Equipment[i].Name == "TRX")
                    {
                        SpaceGrades["TRXGrade"] = Grades["Equipment"] * Convert.ToDouble(Data["TRXCounter"]) / Data["Counter"];
                        Grade += Grades["Equipment"] * Convert.ToDouble(Data["TRXCounter"]) / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Treadmill")
                    {
                        Grade += Grades["Equipment"] * Convert.ToDouble(Data["TreadmillCounter"]) / Data["Counter"];
                        SpaceGrades["TreadmillGrade"] = Grades["Equipment"] * Convert.ToDouble(Data["TreadmillCounter"]) / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "Stationary bicycle")
                    {
                        Grade += Grades["Equipment"] * Convert.ToDouble(Data["StationaryBicycleCounter"]) / Data["Counter"];
                        SpaceGrades["StationaryBicycleGrade"] = Grades["Equipment"] * Convert.ToDouble(Data["StationaryBicycleCounter"]) / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Bench")
                    {
                        Grade += Grades["Equipment"] * Convert.ToDouble(Data["BenchCounter"]) / Data["Counter"];
                        SpaceGrades["BenchGrade"] = Grades["Equipment"] * Convert.ToDouble(Data["BenchCounter"]) / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "Dumbells")
                    {
                        Grade += Grades["Equipment"] * Convert.ToDouble(Data["DumbellsCounter"]) / Data["Counter"];
                        SpaceGrades["DumbellsGrade"] = Grades["Equipment"] * Convert.ToDouble(Data["DumbellsCounter"]) / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "Barbell")
                    {
                        Grade += Grades["Equipment"] * Convert.ToDouble(Data["BarbellCounter"]) / Data["Counter"];
                        SpaceGrades["BarbellGrade"] = Grades["Equipment"] * Convert.ToDouble(Data["BarbellCounter"]) / Data["Counter"];

                    }
                    else
                    {
                        break;
                    }
                }
                if (s.Space.Field == "Beauty")
                {
                    if (s.Equipment[i].Name == "Dryers")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["DryersCounter"] / Data["Counter"];
                        SpaceGrades["DryersGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["DryersCounter"] / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "Nail Polish Racks")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["NailPolishRacksCounter"] / Data["Counter"];
                        SpaceGrades["Nail Polish RacksGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["NailPolishRacksCounter"] / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "Reception Area Seating and Decor")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["ReceptionAreaSeatingandDecorCounter"] / Data["Counter"];
                        SpaceGrades["Reception Area Seating and DecorGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["ReceptionAreaSeatingandDecorCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Laser Hair Removal")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["LaserHairRemovalCounter"] / Data["Counter"];
                        SpaceGrades["Laser Hair RemovalGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["LaserHairRemovalCounter"] / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "PedicureManicure")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["PedicureManicureCounter"] / Data["Counter"];
                        SpaceGrades["PedicureManicureGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["PedicureManicureCounter"] / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "Hair Coloring Kit")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["HairColoringKitCounter"] / Data["Counter"];
                        SpaceGrades["Hair Coloring KitGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["HairColoringKitCounter"] / Data["Counter"];

                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (s.Equipment[i].Name == "Canvas")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["CanvasCounter"] / Data["Counter"];
                        SpaceGrades["CanvasGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["CanvasCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Green Screen")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["GreenScreenCounter"] / Data["Counter"];
                        SpaceGrades["Green ScreenGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["GreenScreenCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Potter's Wheel")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["PottersWheelCounter"] / Data["Counter"];
                        SpaceGrades["Potter's WheelGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["PottersWheelCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Guitar")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["GuitarCounter"] / Data["Counter"];
                        SpaceGrades["GuitarGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["GuitarCounter"] / Data["Counter"];

                    }
                    if (s.Equipment[i].Name == "Drum")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["DrumCounter"] / Data["Counter"];
                        SpaceGrades["DrumGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["DrumCounter"] / Data["Counter"];
                    }
                    if (s.Equipment[i].Name == "Speaker")
                    {
                        Grade += Convert.ToDouble(Grades["Equipment"]) * Data["SpeakerCounter"] / Data["Counter"];
                        SpaceGrades["SpeakerGrade"] = Convert.ToDouble(Grades["Equipment"]) * Data["SpeakerCounter"] / Data["Counter"];

                    }
                    else
                    {
                        break;
                    }
                }

            }
            double totalGrade = 0;
            foreach (KeyValuePair<string, double> item in SpaceGrades)
            {
                //System.Diagnostics.Trace.WriteLine("message");
                //Trace.WriteLine("Key: {0}, Value: {1} item.Key");

                //debug.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
                totalGrade += item.Value;
                ////debug.WriteLine(SpaceGrades,"spacegrades");
                //this.comments.Add("testincomments");
            }
            //debug.WriteLine("Space: " + s.Space.Id + " Total Grade: " + totalGrade);
            totalGrade = Math.Round(totalGrade, 2);

            SpaceGrades.Add("TotalGrade", totalGrade);

            //Premium
            return totalGrade;
        }


        [HttpGet]
        [Route("api/SpaceData/grades")]
        public Dictionary<int, Dictionary<string, double>> GetGrades()
        {
            Dictionary<int, Dictionary<string, double>> spaceGrades = new Dictionary<int, Dictionary<string, double>>();
            List<SpaceData> list = new List<SpaceData>();
            DBServices dbs = new DBServices();
            string field = "";
            string city = "";
            string street = "";
            string number = "";
            string date = "11/06/2020";

            foreach (Space item in dbs.readSpacesBySearch(field, city, street, number))
            {
                if (dbs.readAllAvailbilities(item.Id, date).Count > 0)
                {
                    SpaceData s = new SpaceData();
                    s.Space = item;
                    //s.Realavailability = dbs.readAllAvailbilities(s.Space.Id, date);
                    s.Facility = dbs.readFacilities(s.Space.Id);
                    s.Equipment = dbs.readEquipments(s.Space.Id).ToArray();
                    //s.Orders = dbs.readOrdersOfSpace(s.Space.Id);
                    s.WeekAvailabilities = dbs.readWeekAvailbilitiesById(s.Space.Id);

                    List<string> tempAvail = new List<string>();
                    foreach (string item2 in dbs.readAllAvailbilities(s.Space.Id, date))
                    {
                        tempAvail.Add(item2.Split('-')[0].Split(':')[0] + ":" + item2.Split('-')[0].Split(':')[1] + "-" + item2.Split('-')[1].Split(':')[0] + ":" + item2.Split('-')[1].Split(':')[1]);
                    }
                    s.Realavailability = tempAvail;
                    double grade = GradeCalc("", s);
                    s.Space.Grade = grade;
                    list.Add(s);
                    spaceGrades.Add(s.Space.Id, GradeCalc2("", s));
                }

            }
            return spaceGrades;
        }

        //[HttpGet]
        //[Route("api/SpaceData/comments")]

        //public Dictionary<int, string> GetComments()
        //{
        //    Dictionary<int, string> commentsDict = new Dictionary<int, string>();
        //    for(int i = 0; i<this.comments.Count;i++)
        //    {
        //        commentsDict.Add(i,comments[i]);
        //    }
        //    return commentsDict;
        //}

        [HttpGet]
        [Route("api/SpaceData/AllSpaces")]
        public List<SpaceData> Get2()
        {
            List<SpaceData> list = new List<SpaceData>();
            DBServices dbs = new DBServices();
            foreach (Space item in dbs.readAllSpaces())
            {

                SpaceData s = new SpaceData();
                s.Space = item;
                //s.Realavailability = dbs.readAllAvailbilities(s.Space.Id, date);
                s.Facility = dbs.readFacilities(s.Space.Id);
                s.Equipment = dbs.readEquipments(s.Space.Id).ToArray();
                //s.Orders = dbs.readOrdersOfSpace(s.Space.Id);
                s.WeekAvailabilities = dbs.readWeekAvailbilitiesById(s.Space.Id);

                //List<string> tempAvail = new List<string>();
                //foreach (string item2 in dbs.readAllAvailbilities(s.Space.Id, date))
                //{
                //    tempAvail.Add(item2.Split('-')[0].Split(':')[0] + ":" + item2.Split('-')[0].Split(':')[1] + "-" + item2.Split('-')[1].Split(':')[0] + ":" + item2.Split('-')[1].Split(':')[1]);
                //}
                //s.Realavailability = tempAvail;

                list.Add(s);



            }
            return list;
        }

        [HttpGet]
        [Route("api/SpaceData/LastAddedSpaces")]
        public List<SpaceData> Get()
        {
            List<SpaceData> list = new List<SpaceData>();
            DBServices dbs = new DBServices();
            foreach (Space item in dbs.readLastAddedSpaces())
            {

                SpaceData s = new SpaceData();
                s.Space = item;
                //s.Realavailability = dbs.readAllAvailbilities(s.Space.Id, date);
                s.Facility = dbs.readFacilities(s.Space.Id);
                s.Equipment = dbs.readEquipments(s.Space.Id).ToArray();
                //s.Orders = dbs.readOrdersOfSpace(s.Space.Id);
                s.WeekAvailabilities = dbs.readWeekAvailbilitiesById(s.Space.Id);

                //List<string> tempAvail = new List<string>();
                //foreach (string item2 in dbs.readAllAvailbilities(s.Space.Id, date))
                //{
                //    tempAvail.Add(item2.Split('-')[0].Split(':')[0] + ":" + item2.Split('-')[0].Split(':')[1] + "-" + item2.Split('-')[1].Split(':')[0] + ":" + item2.Split('-')[1].Split(':')[1]);
                //}
                //s.Realavailability = tempAvail;

                list.Add(s);



            }
            return list;
        }


        public List<SpaceData> Get(int userId)
        {

            Favourite f = new Favourite();
            f.getFavourites(userId);
            string today = DateTime.Today.ToString("dd/MM/yyyy");
            List<SpaceData> list = new List<SpaceData>();
            DBServices dbs = new DBServices();

            foreach (int item in f.getFavourites(userId))
            {
                SpaceData s = new SpaceData();
                s.Space = dbs.readSpaceById(item);
                //s.Realavailability = dbs.readAllAvailbilities(s.Space.Id, date);
                s.Facility = dbs.readFacilities(s.Space.Id);
                s.Equipment = dbs.readEquipments(s.Space.Id).ToArray();
                //s.Orders = dbs.readOrdersOfSpace(s.Space.Id);
                s.WeekAvailabilities = dbs.readWeekAvailbilitiesById(s.Space.Id);

                List<string> tempAvail = new List<string>();
                foreach (string item2 in dbs.readAllAvailbilities(s.Space.Id, today))
                {
                    tempAvail.Add(item2.Split('-')[0].Split(':')[0] + ":" + item2.Split('-')[0].Split(':')[1] + "-" + item2.Split('-')[1].Split(':')[0] + ":" + item2.Split('-')[1].Split(':')[1]);
                }
                s.Realavailability = tempAvail;

                list.Add(s);

            }
            return list;
        }

        [HttpGet]
        public List<SpaceData> Get(string userEmail)
        {
            List<SpaceData> list = new List<SpaceData>();
            DBServices dbs = new DBServices();
            string today = DateTime.Today.ToString("dd/MM/yyyy");

            foreach (Space item in dbs.readMySpaces(userEmail))
            {

                SpaceData s = new SpaceData();
                s.Space = item;
                //s.Realavailability = dbs.readAllAvailbilities(s.Space.Id, date);
                s.Facility = dbs.readFacilities(s.Space.Id);
                s.Equipment = dbs.readEquipments(s.Space.Id).ToArray();
                //s.Orders = dbs.readOrdersOfSpace(s.Space.Id);
                s.WeekAvailabilities = dbs.readWeekAvailbilitiesById(s.Space.Id);
                s.Ordersdata = dbs.readOrdersDataBySpaceId(s.Space.Id);


                List<string> tempAvail = new List<string>();
                foreach (string item2 in dbs.readAllAvailbilities(s.Space.Id, today))
                {
                    tempAvail.Add(item2.Split('-')[0].Split(':')[0] + ":" + item2.Split('-')[0].Split(':')[1] + "-" + item2.Split('-')[1].Split(':')[0] + ":" + item2.Split('-')[1].Split(':')[1]);
                }
                s.Realavailability = tempAvail;

                list.Add(s);



            }
            return list;
        }

        // GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        public SpaceData Post([FromBody]SpaceData spaceData)
        {
            bool userType;
            Space s = spaceData.Space;
            Facility f = spaceData.Facility;
            Equipment[] e = spaceData.Equipment;
            WeekAvailability[] wa = spaceData.Availability;
            DBServices dbs = new DBServices();

            //Availability a = spaceData.Availability;



            int newSpaceId = dbs.insert(s);

            dbs.updateUserSpacePubish(s.UserEmail);


            f.SpaceId = newSpaceId;

            //  countertest variabe for test how many rows affected 
            int countertest = 0;
            countertest += dbs.insert(f);

            foreach (Equipment item in e)
            {
                item.SpaceId = newSpaceId;
                countertest += dbs.insert(item);
                //dbs.insert(item); test if it can replace the row above??
            }
            foreach (WeekAvailability item in wa)
            {
                item.FkSpaceId = newSpaceId;
                countertest += dbs.insert(item);

            }

            //test if the user is already a spaceOwner
            userType = dbs.userTypeCheckandUpdate(s.UserEmail);

            //e.SpaceId = newSpaceId;
            //a.SpaceId = newSpaceId;
            //countertest += dbs.insert(a);

            return spaceData;
        }







        // POST api/<controller>
        [HttpPut]
        [Route("api/SpaceData/Update/")]
        public SpaceData Put([FromBody]SpaceData spaceData)
        {
            Space s = spaceData.Space;
            Facility f = spaceData.Facility;
            Equipment[] e = spaceData.Equipment;
            WeekAvailability[] wa = spaceData.Availability;
            DBServices dbs = new DBServices();

            //Availability a = spaceData.Availability;



            int numaffected = dbs.updateSpace(s);

            f.SpaceId = s.Id;

            //  countertest variabe for test how many rows affected 
            int countertest = 0;
            countertest += dbs.update(f);




            //the variables are for debegging

            int equipmentsRowsDelete = dbs.deleteEquipment(s.Id);

            int WeekAvailabilitiesRowsDelete = dbs.deleteWeekAvailabilities(s.Id);

            foreach (Equipment item in e)
            {
                item.SpaceId = s.Id;
                countertest += dbs.insert(item);
                //dbs.insert(item); test if it can replace the row above??
            }
            foreach (WeekAvailability item in wa)
            {
                item.FkSpaceId = s.Id;
                countertest += dbs.insert(item);

            }

            //test if the user is already a spaceOwner
            //userType = dbs.userTypeCheckandUpdate(s.UserEmail);

            //e.SpaceId = newSpaceId;
            //a.SpaceId = newSpaceId;
            //countertest += dbs.insert(a);

            return spaceData;
        }



        [HttpGet]
        [Route("api/SpaceData/LastAddedByField/{userId}")]
        public List<SpaceData> GetLastAddedSpacesForUser(int userId)
        {


            List<SpaceData> list = new List<SpaceData>();
            DBServices dbs = new DBServices();

            if (userId == 369)
            {
                foreach (Space item in dbs.readLastAddedSpaces())
                {

                    SpaceData s = new SpaceData();
                    s.Space = item;
                    //s.Realavailability = dbs.readAllAvailbilities(s.Space.Id, date);
                    s.Facility = dbs.readFacilities(s.Space.Id);
                    s.Equipment = dbs.readEquipments(s.Space.Id).ToArray();
                    //s.Orders = dbs.readOrdersOfSpace(s.Space.Id);
                    s.WeekAvailabilities = dbs.readWeekAvailbilitiesById(s.Space.Id);

                    //List<string> tempAvail = new List<string>();
                    //foreach (string item2 in dbs.readAllAvailbilities(s.Space.Id, date))
                    //{
                    //    tempAvail.Add(item2.Split('-')[0].Split(':')[0] + ":" + item2.Split('-')[0].Split(':')[1] + "-" + item2.Split('-')[1].Split(':')[0] + ":" + item2.Split('-')[1].Split(':')[1]);
                    //}
                    //s.Realavailability = tempAvail;

                    list.Add(s);



                }
                return list;

            }
            else
            {
                Search s = new Search();

                if (s.getUserSearches(userId) == 0)
                {
                    foreach (Space item in dbs.readLastAddedSpaces())
                    {

                        SpaceData spaceData2 = new SpaceData();
                        spaceData2.Space = item;
                        //s.Realavailability = dbs.readAllAvailbilities(s.Space.Id, date);
                        spaceData2.Facility = dbs.readFacilities(spaceData2.Space.Id);
                        spaceData2.Equipment = dbs.readEquipments(spaceData2.Space.Id).ToArray();
                        //s.Orders = dbs.readOrdersOfSpace(s.Space.Id);
                        spaceData2.WeekAvailabilities = dbs.readWeekAvailbilitiesById(spaceData2.Space.Id);

                        //List<string> tempAvail = new List<string>();
                        //foreach (string item2 in dbs.readAllAvailbilities(s.Space.Id, date))
                        //{
                        //    tempAvail.Add(item2.Split('-')[0].Split(':')[0] + ":" + item2.Split('-')[0].Split(':')[1] + "-" + item2.Split('-')[1].Split(':')[0] + ":" + item2.Split('-')[1].Split(':')[1]);
                        //}
                        //s.Realavailability = tempAvail;

                        list.Add(spaceData2);



                    }
                    return list;

                }
                else
                {


                    Search userLastSearch = dbs.readLastSearchOfUser(userId);

                    string field = userLastSearch.Field;

                    foreach (Space item in dbs.readLastFiveSpacesByField(field))
                    {

                        SpaceData spaceData = new SpaceData();
                        spaceData.Space = item;
                        //s.Realavailability = dbs.readAllAvailbilities(s.Space.Id, date);
                        spaceData.Facility = dbs.readFacilities(spaceData.Space.Id);
                        spaceData.Equipment = dbs.readEquipments(spaceData.Space.Id).ToArray();
                        //s.Orders = dbs.readOrdersOfSpace(s.Space.Id);
                        spaceData.WeekAvailabilities = dbs.readWeekAvailbilitiesById(spaceData.Space.Id);

                        //List<string> tempAvail = new List<string>();
                        //foreach (string item2 in dbs.readAllAvailbilities(s.Space.Id, date))
                        //{
                        //    tempAvail.Add(item2.Split('-')[0].Split(':')[0] + ":" + item2.Split('-')[0].Split(':')[1] + "-" + item2.Split('-')[1].Split(':')[0] + ":" + item2.Split('-')[1].Split(':')[1]);
                        //}
                        //s.Realavailability = tempAvail;

                        list.Add(spaceData);
                    }
                }
                return list;
            }
        }


        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}