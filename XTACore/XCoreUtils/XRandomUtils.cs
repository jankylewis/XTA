using System.Security.Cryptography;

namespace XTACore.XCoreUtils;

public class XRandomUtils
{
    public XRandomUtils() {}

    public Guid GenGuid() => Guid.NewGuid();

    public string GenRandomString(int in_min, int in_max)
    {
        if (in_min < 0 || in_max < 0)
            throw new ArgumentOutOfRangeException("Min/ Max Length cannot be negative.      ");
        
        if (in_min > in_max)
            throw new ArgumentException("Min Length cannot be greater than Max Length.        ");

        const string CHAR_POOL =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz" +
            "0123456789" +
            @"!""#$%&'()*+,-./:;<=>?@[\]^_`{|}~";
        
        int len = RandomNumberGenerator.GetInt32(in_min, in_max + 1);
        
        Span<char> buffer = stackalloc char[len];
        
        for (int l_idx = 0; l_idx < len; l_idx++)
        {
            int index = RandomNumberGenerator.GetInt32(CHAR_POOL.Length);
            buffer[l_idx] = CHAR_POOL[index];
        }
        
        return new string(buffer);
    }
}