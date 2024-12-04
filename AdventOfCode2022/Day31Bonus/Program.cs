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
        //Run(@"..\..\..\example_input.txt", false, true);
        //Run(@"..\..\..\real_input.txt", false);
        Run(@"..\..\..\DistanceGraph.txt", true, false, true);
        Run(@"..\..\..\DistanceGraph.txt", true);
        

        void Run(string inputfile, bool isMatrix, bool bruteforce = false, bool hyperspaceOnMars = false)
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

            if(hyperspaceOnMars)
            {
                MakeHyperspaceTable(cities, distances);
            }

            (double distance,  int[] path) best;
            if (bruteforce)
                best = GetBestpathBruteForce(cities, distances);
            else
                best = GetBestpath(cities, distances);

            for (int j = 0; j < best.path.Length; j++)
            {
                if (j > 0)
                {
                    Console.WriteLine("         |  ");
                    Console.WriteLine($" {(hyperspaceOnMars? "Fly" : "Drive")}: {distances[best.path[j - 1], best.path[j]]} {(hyperspaceOnMars? "HyperHubs" : (isMatrix ? "km" : "m"))}");
                    Console.WriteLine("         |  ");
                }
                var bc = cities[best.path[j]];
                Console.WriteLine($"{bc.name} postcode:{bc.postal} ({bc.location.ToString()})");

             }
            Console.WriteLine($"Total distance: {best.distance} {(hyperspaceOnMars ? "HyperHubs" : (isMatrix ? "km" : "m"))}");

            stopwatch.Stop();
            Console.WriteLine($"Used time to caclulate (ms): {stopwatch.ElapsedMilliseconds}");
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine();
        }
    }

    private static void MakeHyperspaceTable(List<(string name, string postal, GeoCoordinate location)> cities, double[,] distances)
    {
        int[] GetHypers(string zip)
        {
            int num(char n) { return n - '0'; }
            if (zip.Length == 6)
               return new int[] { num(zip[0]), num(zip[1]), num(zip[2]), num(zip[4]), num(zip[5]) };
            if (zip.Length == 5)
                return new int[] { num(zip[0]), num(zip[1]), num(zip[2]), num(zip[3]), num(zip[4]) };
            if (zip.Length == 4)
                return new int[] { 0, num(zip[0]), num(zip[1]), num(zip[2]), num(zip[3])};
            return new int[] {0,0,0,0,0 };
        }
        for (int i=0;i < cities.Count-1;i++)
        {
            var pci = GetHypers(cities[i].postal);
            for (int j=i+1;j<cities.Count;j++)
            {
                var pcj = GetHypers(cities[j].postal);
                double hyperDist = Math.Sqrt(pci.Zip(pcj).Select(a => a.First * a.Second).Sum());
                distances[i, j] = distances[j, i] = hyperDist;
            }
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

    private static (double distance, int[] path) GetBestpath(List<(string name, string postal, GeoCoordinate location)>? cities, double[,] distances)
    {
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
        int[] path = new int[cities.Count + 1];

        for (int j = 0; j < cities.Count; j++)
        {
            path[j] = j;
        }
        (double distance, int[] path) best = (TotalDistance(path, distances), path.ToArray());

        best = (TotalDistance(path, distances), path);
        var rnd = new Random();

        var neighbours = new List<(double distance, double hash, int[] path)>();

        for (int k = 0; k < 50; k++)
        {
            HashSet<double> added = new HashSet<double>();
            neighbours.Add((best.distance, GetHash(best.path), best.path));
            double mdist = best.distance;
            int cnt = neighbours.Count;
            for (int i=0; i< cnt; i++)
            {
                var b = neighbours[i];
                added.Add(b.hash);
                int m = 0;
                while( m < 10)
                {
                    var nb = b.path.ToArray();
                    int cityFrom = rnd.Next(1, cities.Count);
                    int cityTo = rnd.Next(1, cities.Count);
                    if (cityFrom != cityTo )
                    {
                        (nb[cityFrom], nb[cityTo]) = (nb[cityTo], nb[cityFrom]);
                        var hash = GetHash(nb);
                        if (!added.Contains(hash))
                        {
                            var distance = TotalDistance(nb, distances);
                            if (distance < best.distance) best = (distance, nb.ToArray());
                            else if (distance > mdist) mdist = distance;
                            neighbours.Add((distance, hash, nb));
                            m++;
                        }
                    }
                }
            }
            var stddev = mdist / (mdist - best.distance) * .3;
            neighbours = neighbours.OrderBy(b => MullerBoxRnd(rnd, b.distance,stddev)).Take(50).ToList();

        }

        return best;
    }

    private static (double distance, int[] path) GetBestpathBruteForce(List<(string name, string postal, GeoCoordinate location)>? cities, double[,] distances)
    {
        // start searching
        int[] path = new int[cities.Count + 1];
        var usedAbove = new HashSet<int>();
        for (int j = 0; j < cities.Count; j++)
        {
            path[j] = j;
            usedAbove.Add(j);
        }
        (double distance, int[] path) best = (TotalDistance(path, distances), path.ToArray());
        
        int level = cities.Count -1;
        bool down = true;
        while (level > 0) 
        { 
            if (level == cities.Count - 1)
            {
                double distance = TotalDistance(path, distances);
                if (distance < best.distance) best = (distance, path.ToArray());
                down = false;
            }
            int i = path[level];
            if (!down)
            {
                usedAbove.Remove(i);
                i++;
                while (i < cities.Count && usedAbove.Contains(i)) { i++; }
            }
            if (i == cities.Count) level--;
            else
            {
                down = true;
                usedAbove.Add(i);
                path[level] = i;
                level++;
                i = 0;
                while (i < cities.Count && usedAbove.Contains(i)) { i++; }
                path[level] = i;
            }
        }
        return best;
    }


    private static double TotalDistance(int[] x, double[,] distances)
    {
        double result = 0;
        for (int p = 0; p < x.Length - 1; p++)
        {
            result += distances[x[p], x[p + 1]];
        }
        return result;
    }

    private static double MullerBoxRnd(Random rand, double mean, double stdDev)
    {
        double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                     Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
        double randNormal =
                     mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
        return randNormal;
    }
}