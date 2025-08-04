using System.Security.Cryptography;

namespace XTACore.XTAUtils;

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

        const string CHAR_POOL = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        
        int len = RandomNumberGenerator.GetInt32(in_min, in_max + 1);
        
        Span<char> buffer = stackalloc char[len];
        
        for (int i = 0; i < len; i++)
        {
            int index = RandomNumberGenerator.GetInt32(CHAR_POOL.Length);
            buffer[i] = CHAR_POOL[index];
        }
        
        return new string(buffer);
    }
    
    
}