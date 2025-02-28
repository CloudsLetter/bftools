﻿namespace BF1ServerTools.Configs;

public class AuthConfig
{
    public bool IsUseMode1 { get; set; }
    public int SelectedIndex { get; set; }
    public bool ReverseOrder { get; set; }
    public List<AuthInfo> AuthInfos { get; set; }
    public List<string> BF1SpartaGatewayProxyAddr { get; set; }
    public string CurrentBF1SpartaGatewayProxyAddr { get; set; }
    public bool AutoApplyRuleOffline { get; set; }
    public bool AutoApplyRuleOnline { get; set; }

    public class AuthInfo
    {
        public string Avatar2 { get; set; }
        public string DisplayName2 { get; set; }
        public long PersonaId2 { get; set; }
        public string Remid { get; set; }
        public string Sid { get; set; }
        public string AccessToken { get; set; }
        public string SessionId2 { get; set; }
    }
}
