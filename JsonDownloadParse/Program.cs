using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GeoJSON.Net;

namespace JsonDownloadParse
{
    public class RailData {
        public List<Features> Features {
            get;
            set;
        }
    }
    
    public class Features
    {
        public string Type
        {
            get;
            set;
        }

        public Properties Properties
        {
            get;
            set;
        }

        public Geometory Geometry {
            get;
            set;
        }
    }

    public class Properties {
        public string Classnumber {
            get;
            set;
        }

        public string Operatordist {
            get;
            set;
        }

        public string Linename {
            get;
            set;
        }

        public string Company {
            get;
            set;
        }

        public string Station {
            get;
            set;
        }
    }

    public class Geometory {
        public string Type {
            get;
            set;
        }

        public IList<JArray> Coordinates{
            get;
            set;
        }
    }
    
    class Program
    {
        public static void saveToCsv(RailData deseriarize_data)
        {
            // デシリアライズしたデータをCSVに書き換え
            using (var stream_writer = new System.IO.StreamWriter(@"station.csv"))
            {
                foreach (var json_parse_data in deseriarize_data.Features)
                {
                    var coordinates_data = json_parse_data.Geometry.Coordinates;
                    var company_name = json_parse_data.Properties.Company;
                    var line_name = json_parse_data.Properties.Linename;
                    var station_name = json_parse_data.Properties.Station;

                    foreach (var coord_data in coordinates_data)
                    {
                        var longitude = (double)coord_data[0];
                        var latitude = (double)coord_data[1];
                        Console.WriteLine("{0},{1},{2},{3},{4}", company_name, line_name, station_name, latitude, longitude);
                        stream_writer.WriteLine("{0},{1},{2},{3},{4}", company_name, line_name, station_name, latitude, longitude);
                    }
                }
            }
        }

        public static string json_data_reader(string filename) {
            // jsonデータを読み込んでデシリアライズしている
            StreamReader sr = new StreamReader(filename);
            return sr.ReadToEnd();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("アプリ実行中");

            var json_data = json_data_reader("stationdata.geojson");
            // デシリアライズで用いるLailDataプロパティ他は上の方に書いてあります
            var deseriarize = JsonConvert.DeserializeObject<RailData>(json_data);

            //var count = 1;

            // デシリアライズしたデータの中から路線名と駅名と緯度を抜き出して表示
            /*foreach (var json_parse_data in deseriarize.Features) {
                var CoordinatesData = json_parse_data.Geometry.Coordinates;

                foreach (var coord_data in CoordinatesData) {
                    Console.WriteLine("{0}回目",count);
                    Console.WriteLine("{0},{1}駅",json_parse_data.Properties.Linename,json_parse_data.Properties.Station);
                    var coordinatesDataTest = coord_data[1];

                    Console.WriteLine((double)coordinatesDataTest);
                    count += 1;
                }   
            }*/

            saveToCsv(deseriarize);
        }
    }
}
