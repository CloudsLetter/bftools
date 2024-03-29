// Project: BF1ServerTools.RES
// TranslateKeyData.cs
// 
// Create Date: 2024-03-29 14:46

using System.Windows.Input;

namespace BF1ServerTools.RES.Data; 

public class TranslateKeyData {
    public enum TranslateKeyFlag {
        NOTHING, TOO_LONG, OFFENSIVE, WHITESPACE, MULTILINE, FAKE_ENGLISH
    }
    public class TranslateKey {
        public string Hash;
        public string DisplayText;
        public TranslateKeyFlag Flag;

        public string ToRule() {
            return Hash + " " + DisplayText;
        }
    }

    public static List<TranslateKey> TranslateKeys = new () {
        // TODO unimplemented, fill data here
    };
}