// Project: BF1ServerTools
// TranslateKeyUtils.cs
// 
// Create Date: 2024-03-29 15:15


using NStandard;

namespace BF1ServerTools.Utils; 

public class TranslateKeyUtils {
    // eg: test -> 002F405F
    public static string calculateHash(string id) {
        uint hash = 0xFFFFFFFF;
        for (int i = 0, len = id.Length; i < len; i++) {
            hash = hashChar(hash, id.CharAt(i));
        }
        return hash.ToString("X8");
    }

    private static uint hashChar(uint baseHash, char id) {
        return id + 33 * baseHash;
    }
}