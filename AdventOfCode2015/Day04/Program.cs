using System;
using System.Security.Cryptography;
using System.Text;

 static string CalculateMD5Hash(string key, long number)
{
    var input = key + number.ToString();
    using (MD5 md5 = MD5.Create())
    {
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("x2"));
        }
        return sb.ToString();
    }
}

//Console.WriteLine(CalculateMD5Hash("abcdef",609043));

var key = "yzbqklnj";
long i = 0;
while (!CalculateMD5Hash(key, i).StartsWith("00000")) i++;
Console.WriteLine("Answer1: " + i.ToString());

i = 0;
while (!CalculateMD5Hash(key, i).StartsWith("000000"))
{
   // Console.WriteLine(CalculateMD5Hash(key, i));
    i++;
}
Console.WriteLine("Answer2: " + i.ToString());