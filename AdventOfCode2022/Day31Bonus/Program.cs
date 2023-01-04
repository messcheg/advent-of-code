using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Security;
using GeoCoordinatePortable;

internal class Program
{
    private static void Main(string[] args)
    {
        //Run(@"..\..\..\example_input.txt", false);
        //Run(@"..\..\..\real_input.txt", false);
        Run(@"..\..\..\DistanceGraph.txt", true);


        void Run(string inputfile, bool isMatrix)
        {
            Console.WriteLine($"--------------[{inputfile}]--------------------");
            Stopwatch stopwatch = Stopwatch.StartNew();
            var postcodefile = @"..\..\..\czech_cities.txt";

            var S = File.ReadAllLines(inputfile).ToList();
            var postcodes = File.ReadAllLines(postcodefile).Skip(1).Select(s => s.Split(',')).ToList();
            List<(string name, string postal, GeoCoordinate location)> cities;
            double[,] distances;
            if(!isMatrix)
                LoadFromCityfile(S, postcodes, out cities, out distances);
            else
                LoadFromHencofile(S, postcodes, out cities, out distances);

            List<(double distance, double hash, int[] path)>? best = GetBestpath(cities, distances);

            for (int j = 0; j < best[0].path.Length; j++)
            {
                if (j > 0)
                {
                    Console.WriteLine("         |  ");
                    Console.WriteLine($" drive: {distances[best[0].path[j - 1], best[0].path[j]]} {(isMatrix ? "km" : "m")}");
                    Console.WriteLine("         |  ");
                }
                var bc = cities[best[0].path[j]];
                Console.WriteLine($"{bc.name} postcode:{bc.postal} ({bc.location.ToString()})");

             }
            Console.WriteLine($"Total distance: {best[0].distance} {(isMatrix ? "km": "m")}");

            stopwatch.Stop();
            Console.WriteLine($"Used time to caclulate (ms): {stopwatch.ElapsedMilliseconds}");
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine();
        }
    }

    private static void LoadFromHencofile(List<string> S, List<string[]> postcodes, out List<(string name, string postal, GeoCoordinate location)> cities, out double[,] distances)
    {
        cities = new List<(string name, string postal, GeoCoordinate location)>();
        // calculate all distances
        distances = new double[S.Count, S.Count];
        int i = 0;
        while (i+3 < S.Count)
        {
            var s = S[i+3].Split('\t');
            if (i == 0) cities.Add((s[0], s[2], new GeoCoordinate(52.018223, 5.156038)));
            else
            {
                var detail = postcodes.Where(a => a[1] == s[2][0..3] + " " + s[2][3..5]).ToList();
                Debug.Assert(detail.Count > 0);
                double latitude = double.Parse(detail[0][9], CultureInfo.InvariantCulture);
                double longitude = double.Parse(detail[0][10], CultureInfo.InvariantCulture);

                cities.Add((s[0], detail[0][1], new GeoCoordinate(latitude, longitude)));
            }
            for (int j = 0; j < cities.Count; j++)
            {
                if (j != i)
                {
                    var distance = double.Parse(s[j + 3]);
                    distances[i, j] = distances[j, i] = distance;
                }
            }
            i++;
        }
    }

    private static void LoadFromCityfile(List<string> S, List<string[]> postcodes, out List<(string name, string postal, GeoCoordinate location)> cities, out double[,] distances)
    {
        cities = new List<(string name, string postal, GeoCoordinate location)>();
        // calculate all distances
        distances = new double[S.Count, S.Count];
        int i = 0;
        while (i < S.Count)
        {
            var s = S[i];
            var detail = postcodes.Where(a => a[2].StartsWith(s)).ToList();
            Debug.Assert(detail.Count > 0);
            double latitude = double.Parse(detail[0][9], CultureInfo.InvariantCulture);
            double longitude = double.Parse(detail[0][10], CultureInfo.InvariantCulture);

            cities.Add((s, detail[0][1], new GeoCoordinate(latitude, longitude)));

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
    }

    private static List<(double distance, double hash, int[] path)> GetBestpath(List<(string name, string postal, GeoCoordinate location)>? cities, double[,] distances)
    {
        double TotalDistance(int[] x)
        {
            double result = 0;
            for (int p = 0; p < x.Length - 1; p++)
            {
                result += distances[x[p], x[p + 1]];
            }
            return result;
        }
        double GetHash(int[] x)
        {
            double result = x[1];
            for (int p = 2; p < x.Length - 1; p++)
            {
                result += result * x.Length + x[p];
            }
            return result;
        }

        // start searching
        var best = new List<(double distance, double hash, int[] path)>();

        best.Add((0, 0, new int[cities.Count + 1]));
        for (int j = 0; j < cities.Count; j++)
        {
            best[0].path[j] = j;
        }
        best[0] = (TotalDistance(best[0].path), GetHash(best[0].path), best[0].path);
        var rnd = new Random();
        for (int k = 0; k < 100; k++)
        {
            var neighbours = new List<(double distance, double hash, int[] path)>();

            foreach (var b in best)
            {
                neighbours.Add(b);
                for (int m = 0; m < 10; m++)
                {
                    var nb = b.path.ToArray();
                    int cityFrom = rnd.Next(1, cities.Count - 1);
                    int cityTo = rnd.Next(1, cities.Count - 1);
                    if (cityFrom != cityTo)
                    {
                        (nb[cityFrom], nb[cityTo]) = (nb[cityTo], nb[cityFrom]);
                        neighbours.Add((TotalDistance(nb), GetHash(nb), nb));
                    }
                }
            }

            var b1 = neighbours.DistinctBy(x => (x.distance, x.hash));
            best = b1.OrderBy(x => x.distance).Take(10).ToList();
        }

        return best;
    }
}