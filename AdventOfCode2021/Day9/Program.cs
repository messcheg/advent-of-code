void w<T>(T val) { Console.WriteLine(val);}

//string inputfile = @"..\..\..\example_input.txt";
string inputfile = @"..\..\..\real_input.txt";

var S = File.ReadAllLines(inputfile).ToList();
long answer1 = 0;
long answer2 = 0;

var basindicator = new int[S.Count, S[0].Length];
int i = 0;
int indicator = 0;

foreach (var s in S)
{
    for (int j= 0; j < s.Length; j++)
    {
        if (j == 0 || s[j] < s[j - 1])
        { 
            if (j == s.Length-1 || s[j] < s[j+1])
            {
                if (i == 0 || s[j] < S[i-1][j])
                {
                    if (i == S.Count -1 || s[j] < S[i+1][j])
                    {
                        string t = "" + s[j];
                        answer1 += 1 + int.Parse(t);
                    }
                }
            }
        }
    }

    for (int j=0;j<s.Length;j++)
    {
        if ( s[j] != '9')
        { 
            if ( j== 0)
            {
                basindicator[i,j] = indicator;
                indicator++;
            }
            else if (s[j - 1] < '9')
            {
                basindicator[i, j] = basindicator[i, j - 1];
            }
            else 
            {
                basindicator[i, j] = indicator;
                indicator++;
            }

        }
        else
        {
            basindicator[i, j] = -1;
        }
    }
    
    i++;
}
bool changed = true;
while (changed)
{
    changed = false;
    for (int k = 0; k < S.Count; k++)
        for (int j = 0; j < S[0].Length; j++)
        {
            if (j > 0 && basindicator[k, j-1] != -1 && basindicator[k, j - 1] < basindicator[k, j])
            {
                basindicator[k, j] = basindicator[k, j - 1];
                changed = true;
            }
            if (k > 0 && basindicator[k-1, j] != -1 && basindicator[k-1, j] < basindicator[k, j])
            {
                basindicator[k, j] = basindicator[k-1, j];
                changed = true;
            }
            if (j < S[0].Length -1 && basindicator[k, j + 1] != -1 && basindicator[k, j + 1] < basindicator[k, j])
            {
                basindicator[k, j] = basindicator[k, j + 1];
                changed = true;
            }
            if (k < S.Count-1 && basindicator[k + 1, j] != -1 && basindicator[k + 1, j] < basindicator[k, j])
            {
                basindicator[k, j] = basindicator[k + 1, j];
                changed = true;
            }
        }
}
var basinssize = new int[indicator + 1];

for (int k = 0; k < S.Count; k++)
    for (int j = 0; j < S[0].Length; j++)
        if (basindicator[k, j] != -1)
            basinssize[basindicator[k, j]]++;

var X = new List<int>(basinssize);
X.Sort();

answer2 = X[^1] * X[^2] * X[^3];

w(answer1);
w(answer2);