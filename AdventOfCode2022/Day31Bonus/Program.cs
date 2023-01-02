using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Security;
using GeoCoordinatePortable;

internal class Program
{
    private static void Main(string[] args)
    {
        Run(@"..\..\..\example_input.txt", true);
        // Run(@"..\..\..\real_input.txt", false);


        void Run(string inputfile, bool isTest)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var postcodefile = @"..\..\..\czech_cities.txt";

            var S = File.ReadAllLines(inputfile).ToList();
            var postcodes = File.ReadAllLines(postcodefile).Skip(1).Select(s => s.Split(',')).ToList();
            long answer1 = 0;
            long supposedanswer1 = 0;

            var cities = new List<(string name, string postal, GeoCoordinate location)>();
            // calculate all distances
            var distances = new double[S.Count, S.Count];

            int i = 0;
            while (i < S.Count)
            {
                var s = S[i];
                var detail = postcodes.Where(a => a[2].StartsWith(s)).ToList();
                Debug.Assert(detail.Count > 0);
                double latitude = double.Parse(detail[0][9],CultureInfo.InvariantCulture);
                double longitude = double.Parse(detail[0][10], CultureInfo.InvariantCulture);

                cities.Add((s, detail[0][1], new GeoCoordinate(latitude , longitude )));

                for (int j = 0; j < cities.Count; j++) 
                {
                    if (j != i)
                    {
                        var distance = cities[j].location.GetDistanceTo(cities[i].location);
                        distances[i, j] = distances[j, i] = distance;
                    }
                }
                i++;
            }
            double TotalDistance(int[] x)
            {
                double result = 0;
                for (int p = 0; p < x.Length - 1; p++)
                {
                    result += distances[x[p], x[p + 1]];
                }
                return result;
            }
            // start searching
            var best = new List<int[]>();
            best.Add(new int[cities.Count + 1]);
            for (int j= 0; j < cities.Count; j++)
            {
                best[0][j] = j;
            }
            var rnd = new Random();
            for (int k = 0; k < 100; k++)
            {
                var neighbours = new List<int[]>();
                
                foreach (var b in best)
                {
                    neighbours.Add(b);
                    for (int m = 0; m< 10; m++)
                    {
                        var nb = b.ToArray();
                        int cityFrom = rnd.Next(1,cities.Count-1);
                        int cityTo = rnd.Next(1, cities.Count - 1);
                        if (cityFrom != cityTo)
                        {
                            (nb[cityFrom], nb[cityTo]) = (nb[cityTo], nb[cityFrom]);
                            neighbours.Add(nb);
                        }
                    }
                }
                
                best = neighbours.Distinct().OrderBy(x => TotalDistance(x)).Take(10).ToList();
            }

            for (int j=0; j< best[0].Length; j++) 
            {
                var bc = cities[best[0][j]];
                Console.WriteLine($"{bc.name} , {bc.location.ToString()}");
            }
            Console.WriteLine($"Total distance: {TotalDistance(best[0])}, (745282,1772587214)");

            stopwatch.Stop();
            Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
            Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
            w(1, answer1, supposedanswer1, isTest);
        }


        static void w<T>(int number, T val, T supposedval, bool isTest)
        {
            string? v = val == null ? "(null)" : val.ToString();
            string? sv = supposedval == null ? "(null)" : supposedval.ToString();

            var previouscolour = Console.ForegroundColor;
            Console.Write("Answer Part " + number + ": ");
            Console.ForegroundColor = v == sv ? ConsoleColor.Green : ConsoleColor.White;
            Console.Write(v);
            Console.ForegroundColor = previouscolour;
            if (isTest)
            {
                Console.Write(" ... supposed (example) answer: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(sv);
                Console.ForegroundColor = previouscolour;
            }
            else
                Console.WriteLine();
        }
    }

}