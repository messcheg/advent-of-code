using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using AocHelper;
using Microsoft.Win32.SafeHandles;

Run(@"..\..\..\example.txt", true);
//Run(@"..\..\..\example1.txt", true);
Run(@"E:\develop\advent-of-code-input\2023\day05.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 35;
    ulong supposedanswer2 = 46;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 35;
    ulong answer2 = 46;
    
    var seeds = S[0].Split(": ")[1].Split(" ").Select(s=>long.Parse(s)).ToList();
    var mappings = new Dictionary<string, (string destination, List <(long drange, long srange, long len)> map)>();

    string dest = "";
    string src = "";
    List<(long drange, long srange, long len)> map = new List<(long drange, long srange, long len)>();
    foreach (var s in S.Skip(2))
    {
        if (s.EndsWith(" map:"))
        {
            if (map != null)
            {
                mappings.Add(src,(dest,map));
            }
            map = new List<(long drange, long srange, long len)>();
            var s1 = s.Split(" ")[0].Split("-").ToList();
            dest = s1[2];
            src= s1[0];
        }
        else if (s != "")
        {
            var nums=s.Split(" ").Select(l=>long.Parse(l)).ToArray();
            map.Add((nums[0], nums[1], nums[2]));
        }
    }
    mappings.Add(src, (dest, map));

    var material = "seed";
    var prods = seeds.ToList();
    (string destination, List<(long drange, long srange, long len)> map) mapper;
    while (mappings.TryGetValue(material,out mapper))
    {
        material = mapper.destination;
        for(int i=0; i<prods.Count; i++)
        {
            var found = false;
            int j = 0;
            while(!found && j < mapper.map.Count)
            {
                var mp = mapper.map[j];
                if (prods[i]>=mp.srange && prods[i]<mp.srange+mp.len)
                {
                    prods[i] = prods[i] - mp.srange + mp.drange;
                    found = true;
                }
                j++;
            }
        }
    }
    answer1 = prods.Min();

    var prodcol = new List<(long pr, long rnge)>();
    for(int k =0; k<seeds.Count; k+=2)
    {
        prodcol.Add((seeds[k], seeds[k+1]));
    }
    material = "seed";
    while (mappings.TryGetValue(material, out mapper))
    {
        var newprodcol = new List<(long pr, long rnge)>();
        foreach (var p in prodcol)
        {
            int j = 0;
            var mapped = new List<(long p,long r)>();
            while (j < mapper.map.Count)
            {
                var mp = mapper.map[j];
                if (p.pr >= mp.srange && p.pr <= mp.srange + mp.len)
                {
                    var prnew = p.pr - mp.srange + mp.drange;
                    if (p.pr + p.rnge > mp.srange && p.pr + p.rnge <= mp.srange + mp.len)
                    {
                        newprodcol.Add((prnew, p.rnge));
                        mapped.Add((p.pr, p.rnge));
                    }
                    else
                    {
                        var newrng = mp.srange - p.pr + mp.len;
                        newprodcol.Add((prnew, newrng ));
                        mapped.Add((p.pr, newrng));
                    }
                }
                else if (mp.srange >= p.pr && mp.srange <= p.pr + p.rnge)
                {
                    var prnew = mp.drange;
                    if (mp.srange + mp.len > p.pr && mp.srange + mp.len <= p.pr + p.rnge)
                    {
                        newprodcol.Add((prnew, mp.len));
                        mapped.Add((mp.srange, mp.len));
                    }
                    else
                    {
                        var newrng = p.pr + p.rnge - mp.srange;
                        newprodcol.Add((prnew, newrng));
                        mapped.Add((mp.srange, newrng));
                    }
                }
                j++;
            }
            //add restproducts
            mapped = mapped.OrderBy(p => p.p).ToList();
            var rest = p;
            foreach(var mpd in mapped)
            {
                if (mpd.p > rest.pr)
                {
                    newprodcol.Add((rest.pr, mpd.p - rest.pr));
                }
                rest = (mpd.p + mpd.r, rest.rnge + rest.pr - mpd.p - mpd.r);
            }
            if (rest.rnge > 0) newprodcol.Add(rest);
        }
        prodcol = newprodcol;
        material = mapper.destination;
    }

    answer2 = (ulong)prodcol.Select(p => p.pr).Min();

    stopwatch.Stop();
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);

}
