/*
var initialFishes = new int[]
{
    3,4,3,1,2
};
*/

var initialFishes = new int[]
{
    1,4,3,3,1,3,1,1,1,2,1,1,1,4,4,1,5,5,3,1,3,5,2,1,5,2,4,1,4,5,4,1,5,1,5,5,1,1,1,4,1,5,1,1,1,1,1,4,1,2,5,1,4,1,2,1,1,5,1,1,1,1,4,1,5,1,1,2,1,4,5,1,2,1,2,2,1,1,1,1,1,5,5,3,1,1,1,1,1,4,2,4,1,2,1,4,2,3,1,4,5,3,3,2,1,1,5,4,1,1,1,2,1,1,5,4,5,1,3,1,1,1,1,1,1,2,1,3,1,2,1,1,1,1,1,1,1,2,1,1,1,1,2,1,1,1,1,1,1,4,5,1,3,1,4,4,2,3,4,1,1,1,5,1,1,1,4,1,5,4,3,1,5,1,1,1,1,1,5,4,1,1,1,4,3,1,3,3,1,3,2,1,1,3,1,1,4,5,1,1,1,1,1,3,1,4,1,3,1,5,4,5,1,1,5,1,1,4,1,1,1,3,1,1,4,2,3,1,1,1,1,2,4,1,1,1,1,1,2,3,1,5,5,1,4,1,1,1,1,3,3,1,4,1,2,1,3,1,1,1,3,2,2,1,5,1,1,3,2,1,1,5,1,1,1,1,1,1,1,1,1,1,2,5,1,1,1,1,3,1,1,1,1,1,1,1,1,5,5,1
};

var lantarnFishVertility = new List<int>(10000);

for (int i = 0; i < initialFishes.Length; i++) lantarnFishVertility.Add(initialFishes[i]);



for (int days=0; days < 80; days++)
{
    var count = lantarnFishVertility.Count;
    for (int i = 0; i < count; i++)
    {
        if (lantarnFishVertility[i] == 0)
        {
            lantarnFishVertility[i] = 6;
            lantarnFishVertility.Add(8);
        }
        else lantarnFishVertility[i] = lantarnFishVertility[i] - 1;
    }
}

Console.WriteLine("Number of fishes 80: " + lantarnFishVertility.Count);

var lantarnfishbreed = new ulong[9];
for (int i = 0; i < initialFishes.Length; i++) lantarnfishbreed[initialFishes[i]]++;

for (int days = 0; days < 256; days++)
{
    var newbreed = new ulong[9];
    newbreed[6] = newbreed[8] = lantarnfishbreed[0];
    for (int j = 1; j<9;j++)
    {
        newbreed[j - 1] += lantarnfishbreed[j];
    }
    lantarnfishbreed = newbreed;
}
ulong totalfishes = 0;
foreach (var breedgroup in lantarnfishbreed) totalfishes += breedgroup;

Console.WriteLine("Number of fishes 256: " + totalfishes);
