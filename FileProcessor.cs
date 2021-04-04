using System;
using System.IO;
using System.Collections.Generic;

namespace Console_Application_C_ {
    public static class FileProcessor {

        public static void process(string file) {
            List<Entity> csvData;
            try {
                csvData = getCSVData(file);
            } catch(Exception e) {
                Console.WriteLine($"File not found: {file}");
                throw e;
            }
            
            Dictionary<string, List<Entity>> contryGroup = groupByCountry(csvData);
            List<CountryEntity> countryData = new List<CountryEntity>();
            foreach(var item in contryGroup) {
                countryData.Add(reduce(item.Value));
            }
            
            try {
                writeCSVData("ReportByCountry.csv", countryData);
            } catch (Exception e) {
                Console.WriteLine($"Cuoldn't write file: ReportByCountry.csv");
                throw e;
            }
        }
        private static List<Entity> getCSVData(string file) {
            List<Entity> data = new List<Entity>();

            using (StreamReader sr = new StreamReader(file)) {

                // Read the first line. That's the header.
                // If we have different files and different tables
                // this will be used to decide what conversion or class to be used later.
                // Currently is not needed.
                string[] line = sr.ReadLine().Split(";");
                while(!sr.EndOfStream) {
                    line = sr.ReadLine().Split(";");
                    
                    try {
                        Entity ent = new Entity(
                            line[0], 
                            line[1], 
                            line[2], 
                            line[3], 
                            Int32.Parse(line[4])
                        );

                        data.Add(ent);
                    } catch (Exception e) {
                        // If one of the columns contains ambigious data
                        // or doesn't have such, just continue with the 
                        // next entry.
                        Console.WriteLine("A line contains ambigious data! Skipping...");
                        continue;
                    }
                }
            }

            return data;
        }

        private static Dictionary<string, List<Entity>> groupByCountry(List<Entity> data) {
            Dictionary<string, List<Entity>> countryGroup = new Dictionary<string, List<Entity>>();

            foreach(Entity e in data) {
                if(!countryGroup.ContainsKey(e.country)) {
                    List<Entity> l = new List<Entity>();
                    l.Add(e);
                    countryGroup.Add(e.country, l);
                } else {
                    countryGroup[e.country].Add(e);
                }
            }

            return countryGroup;
        }
        private static CountryEntity reduce(List<Entity> entries) {
            string country = "";
            int medainIdx = entries.Count % 2 == 0 ? entries.Count / 2 : (entries.Count+1) / 2;
            int median = 0;
            int count = 0;
            int total = 0;
            int max = 0;
            int min = -1;
            string minP = "";
            string maxP = "";
            foreach(var e in entries) {
                // The first go initilize our statistic with meanigful information 
                if(min == -1) {
                    min = e.score;
                    median = e.score;
                    maxP = e.fName + " " + e.lName;
                    minP = e.fName + " " + e.lName;
                    country = e.country;
                }

                if(max < e.score) {
                    max = e.score;
                    maxP = e.fName + " " + e.lName;
                }
                if(min > e.score) {
                    min = e.score;
                    minP = e.fName + " " + e.lName;
                }

                total += e.score;
                count++;

                if(count == medainIdx) median = e.score;
            }

            float avg = total/count;

            return new CountryEntity(country, avg, median, max, maxP, min, minP, count);
        }
        private static void writeCSVData(string name, List<CountryEntity> data) {
            string header = "Country;Average score;Median score;Max score;Max score person;Min score;Min score person;Record count";

            // Sort the data by avg score in descending order.
            data.Sort(delegate(CountryEntity a, CountryEntity b) {
                return a.avg >= b.avg ? -1 : 1;
            });

            using (StreamWriter file = new(name)) {
                file.WriteLineAsync(header);
                foreach(CountryEntity entry in data) {
                    // For some reason the await doesn't work with the asynch method?!
                    //await file.WriteLineAsync(entry.toString());
                    file.WriteLine(entry.toString());
                }
            }
        }
    }
}