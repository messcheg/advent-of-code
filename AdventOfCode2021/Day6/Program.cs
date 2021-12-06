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


var lantarnfishbreed = new ulong[9];
for (int i = 0; i < initialFishes.Length; i++) lantarnfishbreed[initialFishes[i]]++;

for (int days = 0; days < 256; days++)
{
    if (days == 80) Console.WriteLine("Number of fishes 80: "  +SumFish(lantarnfishbreed)); 

    var newbreed = new ulong[9];
    newbreed[6] = newbreed[8] = lantarnfishbreed[0];
    for (int j = 1; j<9;j++) newbreed[j - 1] += lantarnfishbreed[j];
    lantarnfishbreed = newbreed;
}
Console.WriteLine("Number of fishes 256: " + SumFish(lantarnfishbreed));

ulong SumFish(ulong[] lantarnfishbreed)
{
    ulong totalfishes = 0;
    foreach (var breedgroup in lantarnfishbreed) totalfishes += breedgroup;
    return totalfishes;
}