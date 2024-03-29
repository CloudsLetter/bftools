using BF1ServerTools.API;
using BF1ServerTools.RES;
using BF1ServerTools.RES.Data;
using BF1ServerTools.Data;
using BF1ServerTools.Utils;
using BF1ServerTools.Helper;
using BF1ServerTools.Configs;
using Newtonsoft.Json;
using NStandard;

namespace BF1ServerTools.Views;


public class Players
{
    public string PlayerName { get; set; }
}


/// <summary>
/// RuleView.xaml 的交互逻辑
/// </summary>
public partial class RuleView : UserControl
{
    /// <summary>
    /// Auth配置文件路径
    /// </summary>
    private readonly string F_Rule_Path = FileUtil.D_Config_Path + @"\RuleConfig.json";

    /// <summary>
    /// Rule配置文件，以json格式保存到本地
    /// </summary>
    private RuleConfig RuleConfig = new();

    /// <summary>
    /// 绑定UI 配置文件名称动态集合
    /// </summary>
    public ObservableCollection<string> ConfigNames { get; set; } = new();

    ////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 绑定UI 队伍1规则集
    /// </summary>
    public RuleTeamModel RuleTeam1Model { get; set; } = new();
    /// <summary>
    /// 绑定UI 队伍2规则集
    /// </summary>
    public RuleTeamModel RuleTeam2Model { get; set; } = new();

    ////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 绑定UI 武器数据
    /// </summary>
    public ObservableCollection<RuleWeaponModel> DataGrid_RuleWeaponModels { get; set; } = new();

    /// <summary>
    /// 绑定UI 规则日志
    /// </summary>
    public ObservableCollection<RuleLog> DataGrid_RuleLogs { get; set; } = new();

    public RuleView()
    {
        InitializeComponent();
        this.DataContext = this;
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

        // 添加武器数据列表
        foreach (var item in WeaponData.AllWeaponInfo)
        {
            DataGrid_RuleWeaponModels.Add(new()
            {
                Kind = item.Kind,
                Name = item.Chinese,
                English = item.English,
                Image = ClientHelper.GetWeaponImagePath(item.English),
                Team1 = false,
                Team2 = false
            });
        }
        // 初始化中文ID数据列表
        TranslateKeyRules_Initialize();

        // 如果配置文件不存在就创建
        if (!File.Exists(F_Rule_Path))
        {
            RuleConfig.SelectedIndex = 0;
            RuleConfig.RuleInfos = new();
            // 初始化10个配置文件槽
            for (int i = 0; i < 10; i++)
            {
                RuleConfig.RuleInfos.Add(new()
                {
                    RuleName = $"自定义规则 {i}",
                    WhiteLifeKD = true,
                    WhiteLifeKPM = true,
                    WhiteLifeWeaponStar = true,
                    WhiteLifeVehicleStar = true,
                    WhiteKill = true,
                    WhiteKD = true,
                    WhiteKPM = true,
                    WhiteRank = true,
                    WhiteWeapon = true,
                    WhiteLifeMaxAccuracyRatio = true,
                    WhiteLifeMaxHeadShotRatio = true,
                    WhiteMaxWR = true,
                    WhiteAllowToggleTeam = true,
                    WhiteScore = true,
                    Team1Rule = new(),
                    Team2Rule = new(),
                    Team1Weapon = new(),
                    Team2Weapon = new(),
                    BlackList = new(),
                    WhiteList = new(),
                    Allow2LowScoreTeam = false,
                }) ;
            }
            // 保存配置文件
            SaveConfig();
        }

        // 如果配置文件存在就读取
        if (File.Exists(F_Rule_Path))
        {
            using var streamReader = new StreamReader(F_Rule_Path);
            RuleConfig = JsonHelper.JsonDese<RuleConfig>(streamReader.ReadToEnd());
            // 读取配置文件名称
            foreach (var item in RuleConfig.RuleInfos)
                ConfigNames.Add(item.RuleName);
            // 读取选中配置文件索引
            ComboBox_ConfigNames.SelectedIndex = RuleConfig.SelectedIndex;
            Globals.previousSelectedIndex = RuleConfig.SelectedIndex;
        }
        new Thread(UpdateWhitelist)
        {
            Name = "UpdateWhiteListThread",
        }.Start();

    }

    private void SetRule2Zero()
    {
        this.Dispatcher.Invoke(
    new Action(
        delegate
        {
            Globals.AlreadyToggleTeamPlayer.Clear();
            ListBox_CustomWhites.Items.Clear();
            ListBox_CustomBlacks.Items.Clear();
            CheckBox_WhiteLifeKD.IsChecked = false;
            CheckBox_WhiteLifeKPM.IsChecked = false;
            CheckBox_WhiteLifeMaxAccuracyRatio.IsChecked = false;
            CheckBox_WhiteLifeMaxHeadShotRatio.IsChecked = false;
            CheckBox_WhiteLifeMaxWR.IsChecked = false;
            CheckBox_WhiteLifeWeaponStar.IsChecked = false;
            CheckBox_WhiteLifeVehicleStar.IsChecked = false;
            CheckBox_WhiteKill.IsChecked = false;
            CheckBox_WhiteKD.IsChecked = false;
            CheckBox_WhiteKPM.IsChecked = false;
            CheckBox_WhiteRank.IsChecked = false;
            CheckBox_WhiteWeapon.IsChecked = false;
            CheckBox_WhiteToggleTeamLimt.IsChecked = false;
            CheckBox_AllowToggle2LowScoreTeam.IsChecked = false;
            CheckBox_WhiteScore.IsChecked = false;
            // 应用队伍1规则
            RuleTeam1Model.ScoreLimt = 0;
            RuleTeam1Model.ScoreGap = 0;
            RuleTeam1Model.MaxKill = 0;
            RuleTeam1Model.FlagKD = 0;
            RuleTeam1Model.MaxKD = 0.00f;
            RuleTeam1Model.FlagKPM = 0;
            RuleTeam1Model.MaxKPM = 0.00f;
            RuleTeam1Model.MinRank = 0;
            RuleTeam1Model.MaxRank = 0;
            RuleTeam1Model.LifeMaxKD = 0;
            RuleTeam1Model.LifeMaxKPM = 0;
            RuleTeam1Model.LifeMaxWeaponStar = 0;
            RuleTeam1Model.LifeMaxVehicleStar = 0;
            RuleTeam1Model.LifeMaxAccuracyRatioLevel = 0;
            RuleTeam1Model.LifeMaxAccuracyRatio = 0;
            RuleTeam1Model.LifeMaxHeadShotRatioLevel = 0;
            RuleTeam1Model.LifeMaxHeadShotRatio = 0;
            RuleTeam1Model.LifeMaxWRLevel = 0;
            RuleTeam1Model.LifeMaxWR = 0;
            RuleTeam1Model.MaxScore = 0;
            RuleTeam1Model.FlagKDPro = 0;
            RuleTeam1Model.MaxKDPro = 0.00f;
            RuleTeam1Model.FlagKPMPro = 0;
            RuleTeam1Model.MaxKPMPro = 0.00f;
            // 应用队伍2规则
            RuleTeam2Model.MaxKill = 0;
            RuleTeam2Model.FlagKD = 0;
            RuleTeam2Model.MaxKD = 0.00f;
            RuleTeam2Model.FlagKPM = 0;
            RuleTeam2Model.MaxKPM = 0.00f;
            RuleTeam2Model.MinRank = 0;
            RuleTeam2Model.MaxRank = 0;
            RuleTeam2Model.LifeMaxKD = 0;
            RuleTeam2Model.LifeMaxKPM = 0;
            RuleTeam2Model.LifeMaxWeaponStar = 0;
            RuleTeam2Model.LifeMaxVehicleStar = 0;
            RuleTeam2Model.LifeMaxAccuracyRatioLevel = 0;
            RuleTeam2Model.LifeMaxAccuracyRatio = 0;
            RuleTeam2Model.LifeMaxHeadShotRatioLevel = 0;
            RuleTeam2Model.LifeMaxHeadShotRatio = 0;
            RuleTeam2Model.LifeMaxWRLevel = 0;
            RuleTeam2Model.LifeMaxWR = 0;
            RuleTeam2Model.MaxScore = 0;
            RuleTeam2Model.FlagKDPro = 0;
            RuleTeam2Model.MaxKDPro = 0.00f;
            RuleTeam2Model.FlagKPMPro = 0;
            RuleTeam2Model.MaxKPMPro = 0.00f;
            for (int i = 0; i < DataGrid_RuleWeaponModels.Count; i++)
            {
                var item = DataGrid_RuleWeaponModels[i];
                item.Team1 = false;
                item.Team2 = false;
            }
        }
         )
);
    }

    private async Task<bool> SetRuleFromCloud()
    {
        Globals.IsRefreshRule = true;
        var result = await BF1API.RefreshWhiteList(ServerId: Globals.ServerId.ToString());
        if (result.IsSuccess)
        {
            var test = result.Content.Replace("\r", "");

            List<Players> players = JsonConvert.DeserializeObject<List<Players>>(test);
            this.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        ListBox_CustomWhites.Items.Clear();
                    }
                )
            );

            foreach (Players player in players)
            {

                this.Dispatcher.Invoke(
                    new Action(
                        delegate
                        {
                            ListBox_CustomWhites.Items.Add(player.PlayerName);
                        }
                    )
                );
            }
            Globals.IsRefreshRule = false;
            NotifierHelper.Show(NotifierType.Success, "获取数据成功");
        }
        else
        {
            Globals.IsRefreshRule = false;
            NotifierHelper.Show(NotifierType.Success, "获取数据失败,请检查网络问题");

            return false;
        }

        var result2 = await CloudApi.RefreshBlackList(ServerId: Globals.ServerId.ToString());
        if (result2.IsSuccess)
        {
            var test = result2.Content.Replace("\r", "");

            List<Players> players = JsonConvert.DeserializeObject<List<Players>>(test);

            this.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        ListBox_CustomBlacks.Items.Clear();
                    }
                )
            );

            foreach (Players player in players)
            {

                this.Dispatcher.Invoke(
                    new Action(
                        delegate
                        {
                            ListBox_CustomBlacks.Items.Add(player.PlayerName);
                        }
                    )
                );
            }
        }
        else
        {
            return false;
        }

        var result3 = await CloudApi.QueryRule(ServerId: Globals.ServerId);

        if (result3.IsSuccess)
        {
            var tmp = result3.Content.Replace("\r", "");
            CloudRule data = JsonConvert.DeserializeObject<CloudRule>(tmp);
            this.Dispatcher.Invoke(
                  new Action(
                      delegate
                      {
                          CheckBox_WhiteLifeKD.IsChecked = data.WhiteLifeKD;
                          CheckBox_WhiteLifeKPM.IsChecked = data.WhiteLifeKPM;
                          CheckBox_WhiteLifeWeaponStar.IsChecked = data.WhiteLifeWeaponStar;
                          CheckBox_WhiteLifeVehicleStar.IsChecked = data.WhiteLifeVehicleStar;
                          CheckBox_WhiteKill.IsChecked = data.WhiteKill;
                          CheckBox_WhiteKD.IsChecked = data.WhiteKD;
                          CheckBox_WhiteKPM.IsChecked = data.WhiteKPM;
                          CheckBox_WhiteRank.IsChecked = data.WhiteRank;
                          CheckBox_WhiteWeapon.IsChecked = data.WhiteWeapon;
                          CheckBox_WhiteLifeMaxAccuracyRatio.IsChecked = data.WhiteLifeMaxAccuracyRatio;
                          CheckBox_WhiteLifeMaxHeadShotRatio.IsChecked = data.WhiteLifeMaxHeadShotRatio;
                          CheckBox_WhiteLifeMaxWR.IsChecked = data.WhiteLifeMaxWR;
                          CheckBox_WhiteToggleTeamLimt.IsChecked = data.WhiteAllowToggleTeam;
                          CheckBox_AllowToggle2LowScoreTeam.IsChecked = data.Allow2LowScoreTeam;
                          CheckBox_WhiteScore.IsChecked = data.WhiteScore;
                          // 应用队伍1规则
                          RuleTeam1Model.ScoreLimt = data.Team1ScoreLimit;
                          RuleTeam1Model.ScoreGap = data.Team1ScoreGap;
                          RuleTeam1Model.MaxKill = data.Team1MaxKill;
                          RuleTeam1Model.FlagKD = data.Team1FlagKD;
                          RuleTeam1Model.MaxKD = data.Team1MaxKD;
                          RuleTeam1Model.FlagKPM = data.Team1FlagKPM;
                          RuleTeam1Model.MaxKPM = data.Team1MaxKPM;
                          RuleTeam1Model.MinRank = data.Team1MinRank;
                          RuleTeam1Model.MaxRank = data.Team1MaxRank;
                          RuleTeam1Model.LifeMaxKD = data.Team1LifeMaxKD;
                          RuleTeam1Model.LifeMaxKPM = data.Team1LifeMaxKPM;
                          RuleTeam1Model.LifeMaxWeaponStar = data.Team1LifeMaxWeaponStar;
                          RuleTeam1Model.LifeMaxVehicleStar = data.Team1LifeMaxVehicleStar;
                          RuleTeam1Model.LifeMaxAccuracyRatioLevel = data.Team1LifeMaxAccuracyRatioLevel;
                          RuleTeam1Model.LifeMaxAccuracyRatio = data.Team1LifeMaxAccuracyRatio;
                          RuleTeam1Model.LifeMaxHeadShotRatioLevel = data.Team1LifeMaxHeadShotRatioLevel;
                          RuleTeam1Model.LifeMaxHeadShotRatio = data.Team1LifeMaxHeadShotRatio;
                          RuleTeam1Model.LifeMaxWRLevel = data.Team1LifeMaxWRLevel;
                          RuleTeam1Model.LifeMaxWR = data.Team1LifeMaxWR;
                          RuleTeam1Model.MaxScore = data.Team1MaxScore;
                          RuleTeam1Model.FlagKDPro = data.Team1FlagKDPro;
                          RuleTeam1Model.MaxKDPro = data.Team1MaxKDPro;
                          RuleTeam1Model.FlagKPMPro = data.Team1FlagKPMPro;
                          RuleTeam1Model.MaxKPMPro = data.Team1MaxKPMPro;

                          // 应用队伍2规则
                          RuleTeam2Model.MaxKill = data.Team2MaxKill;
                          RuleTeam2Model.FlagKD = data.Team2FlagKD;
                          RuleTeam2Model.MaxKD = data.Team2MaxKD;
                          RuleTeam2Model.FlagKPM = data.Team2FlagKPM;
                          RuleTeam2Model.MaxKPM = data.Team2MaxKPM;
                          RuleTeam2Model.MinRank = data.Team2MinRank;
                          RuleTeam2Model.MaxRank = data.Team2MaxRank;
                          RuleTeam2Model.LifeMaxKD = data.Team2LifeMaxKD;
                          RuleTeam2Model.LifeMaxKPM = data.Team2LifeMaxKPM;
                          RuleTeam2Model.LifeMaxWeaponStar = data.Team2LifeMaxWeaponStar;
                          RuleTeam2Model.LifeMaxVehicleStar = data.Team2LifeMaxVehicleStar;
                          RuleTeam2Model.LifeMaxAccuracyRatioLevel = data.Team2LifeMaxAccuracyRatioLevel;
                          RuleTeam2Model.LifeMaxAccuracyRatio = data.Team2LifeMaxAccuracyRatio;
                          RuleTeam2Model.LifeMaxHeadShotRatioLevel = data.Team2LifeMaxHeadShotRatioLevel;
                          RuleTeam2Model.LifeMaxHeadShotRatio = data.Team2LifeMaxHeadShotRatio;
                          RuleTeam2Model.LifeMaxWRLevel = data.Team2LifeMaxWRLevel;
                          RuleTeam2Model.LifeMaxWR = data.Team2LifeMaxWR;
                          RuleTeam2Model.MaxScore = data.Team2MaxScore;
                          RuleTeam2Model.FlagKDPro = data.Team2FlagKDPro;
                          RuleTeam2Model.MaxKDPro = data.Team2MaxKDPro;
                          RuleTeam2Model.FlagKPMPro = data.Team2FlagKPMPro;
                          RuleTeam2Model.MaxKPMPro = data.Team2MaxKPMPro;
                          List<string> list = new List<string>(data.Team1WeaponLimit.Split(','));
                          List<string> list2 = new List<string>(data.Team2WeaponLimit.Split(','));

                          for (int i = 0; i < DataGrid_RuleWeaponModels.Count; i++)
                          {
                              var item = DataGrid_RuleWeaponModels[i];

                              var v1 = list.IndexOf(item.English);
                              if (v1 != -1)
                                  item.Team1 = true;
                              else
                                  item.Team1 = false;

                              var v2 = list2.IndexOf(item.English);
                              if (v2 != -1)
                                  item.Team2 = true;
                              else
                                  item.Team2 = false;
                          }
                      }
                  )
              );


        }
        else
        {
            return false;
        }
        return true;
    }


    private void SetRuleFromLocalHost()
    {
        Dispatcher.Invoke(
        new Action(
        delegate
        {
            var index = ComboBox_ConfigNames.SelectedIndex;
            if (index == -1)
                return;

            var rule = RuleConfig.RuleInfos[index];
            CheckBox_WhiteLifeKD.IsChecked = rule.WhiteLifeKD;
            CheckBox_WhiteLifeKPM.IsChecked = rule.WhiteLifeKPM;
            CheckBox_WhiteLifeWeaponStar.IsChecked = rule.WhiteLifeWeaponStar;
            CheckBox_WhiteLifeVehicleStar.IsChecked = rule.WhiteLifeVehicleStar;
            CheckBox_WhiteKill.IsChecked = rule.WhiteKill;
            CheckBox_WhiteKD.IsChecked = rule.WhiteKD;
            CheckBox_WhiteKPM.IsChecked = rule.WhiteKPM;
            CheckBox_WhiteRank.IsChecked = rule.WhiteRank;
            CheckBox_WhiteWeapon.IsChecked = rule.WhiteWeapon;
            CheckBox_WhiteLifeMaxAccuracyRatio.IsChecked = rule.WhiteLifeMaxAccuracyRatio;
            CheckBox_WhiteLifeMaxHeadShotRatio.IsChecked = rule.WhiteLifeMaxHeadShotRatio;
            CheckBox_WhiteLifeMaxWR.IsChecked = rule.WhiteMaxWR;
            CheckBox_WhiteToggleTeamLimt.IsChecked = rule.WhiteAllowToggleTeam;
            CheckBox_AllowToggle2LowScoreTeam.IsChecked = rule.Allow2LowScoreTeam;
            CheckBox_WhiteScore.IsChecked = rule.WhiteScore;
            // 应用队伍1规则
            RuleTeam1Model.ScoreLimt = rule.Team1Rule.ScoreLimit;
            RuleTeam1Model.ScoreGap = rule.Team1Rule.ScoreGap;
            RuleTeam1Model.MaxKill = rule.Team1Rule.MaxKill;
            RuleTeam1Model.FlagKD = rule.Team1Rule.FlagKD;
            RuleTeam1Model.MaxKD = rule.Team1Rule.MaxKD;
            RuleTeam1Model.FlagKPM = rule.Team1Rule.FlagKPM;
            RuleTeam1Model.MaxKPM = rule.Team1Rule.MaxKPM;
            RuleTeam1Model.MinRank = rule.Team1Rule.MinRank;
            RuleTeam1Model.MaxRank = rule.Team1Rule.MaxRank;
            RuleTeam1Model.LifeMaxKD = rule.Team1Rule.LifeMaxKD;
            RuleTeam1Model.LifeMaxKPM = rule.Team1Rule.LifeMaxKPM;
            RuleTeam1Model.LifeMaxAccuracyRatioLevel = rule.Team1Rule.LifeMaxAccuracyRatioLevel;
            RuleTeam1Model.LifeMaxAccuracyRatio = rule.Team1Rule.LifeMaxAccuracyRatio;
            RuleTeam1Model.LifeMaxHeadShotRatioLevel = rule.Team1Rule.LifeMaxHeadShotRatioLevel;
            RuleTeam1Model.LifeMaxHeadShotRatio = rule.Team1Rule.LifeMaxHeadShotRatio;
            RuleTeam1Model.LifeMaxWRLevel = rule.Team1Rule.LifeMaxWRLevel;
            RuleTeam1Model.LifeMaxWR = rule.Team1Rule.LifeMaxWR;
            RuleTeam1Model.LifeMaxWeaponStar = rule.Team1Rule.LifeMaxWeaponStar;
            RuleTeam1Model.LifeMaxVehicleStar = rule.Team1Rule.LifeMaxVehicleStar;
            RuleTeam1Model.MaxScore = rule.Team1Rule.MaxScore;
            RuleTeam1Model.FlagKDPro = rule.Team1Rule.FlagKDPro;
            RuleTeam1Model.MaxKDPro = rule.Team1Rule.MaxKDPro;
            RuleTeam1Model.FlagKPMPro = rule.Team1Rule.FlagKPMPro;
            RuleTeam1Model.MaxKPMPro = rule.Team1Rule.MaxKPMPro;

            // 应用队伍2规则
            RuleTeam2Model.MaxKill = rule.Team2Rule.MaxKill;
            RuleTeam2Model.FlagKD = rule.Team2Rule.FlagKD;
            RuleTeam2Model.MaxKD = rule.Team2Rule.MaxKD;
            RuleTeam2Model.FlagKPM = rule.Team2Rule.FlagKPM;
            RuleTeam2Model.MaxKPM = rule.Team2Rule.MaxKPM;
            RuleTeam2Model.MinRank = rule.Team2Rule.MinRank;
            RuleTeam2Model.MaxRank = rule.Team2Rule.MaxRank;
            RuleTeam2Model.LifeMaxKD = rule.Team2Rule.LifeMaxKD;
            RuleTeam2Model.LifeMaxKPM = rule.Team2Rule.LifeMaxKPM;
            RuleTeam2Model.LifeMaxAccuracyRatioLevel = rule.Team2Rule.LifeMaxAccuracyRatioLevel;
            RuleTeam2Model.LifeMaxAccuracyRatio = rule.Team2Rule.LifeMaxAccuracyRatio;
            RuleTeam2Model.LifeMaxHeadShotRatioLevel = rule.Team2Rule.LifeMaxHeadShotRatioLevel;
            RuleTeam2Model.LifeMaxHeadShotRatio = rule.Team2Rule.LifeMaxHeadShotRatio;
            RuleTeam2Model.LifeMaxWRLevel = rule.Team2Rule.LifeMaxWRLevel;
            RuleTeam2Model.LifeMaxWR = rule.Team2Rule.LifeMaxWR;
            RuleTeam2Model.LifeMaxWeaponStar = rule.Team2Rule.LifeMaxWeaponStar;
            RuleTeam2Model.LifeMaxVehicleStar = rule.Team2Rule.LifeMaxVehicleStar;
            RuleTeam2Model.MaxScore = rule.Team2Rule.MaxScore;
            RuleTeam2Model.FlagKDPro = rule.Team2Rule.FlagKDPro;
            RuleTeam2Model.MaxKDPro = rule.Team2Rule.MaxKDPro;
            RuleTeam2Model.FlagKPMPro = rule.Team2Rule.FlagKPMPro;
            RuleTeam2Model.MaxKPMPro = rule.Team2Rule.MaxKPMPro;
            // 白名单特权

            // 读取白名单列表
            ListBox_CustomWhites.Items.Clear();
            if (!Globals.IsCloudMode)
            {
                foreach (var item in rule.WhiteList)
                {
                    ListBox_CustomWhites.Items.Add(item);
                }
            }

            // 读取黑名单列表
            ListBox_CustomBlacks.Items.Clear();
            if (!Globals.IsCloudMode)
            {
                foreach (var item in rule.BlackList)
                {
                    ListBox_CustomBlacks.Items.Add(item);
                }
            }

            // 读取中文ID规则列表
            TranslateKeyRules_LoadFromList(rule.TranslateKeyRuleList);


            // 读取武器限制信息
            if (!Globals.IsCloudMode)
            {
                for (int i = 0; i < DataGrid_RuleWeaponModels.Count; i++)
                {
                    var item = DataGrid_RuleWeaponModels[i];

                    var v1 = rule.Team1Weapon.IndexOf(item.English);
                    if (v1 != -1)
                        item.Team1 = true;
                    else
                        item.Team1 = false;

                    var v2 = rule.Team2Weapon.IndexOf(item.English);
                    if (v2 != -1)
                        item.Team2 = true;
                    else
                        item.Team2 = false;
                }
                SaveConfig();

            }

        }
)
);

    }

    private async void UpdateWhitelist()
    {
        while (MainWindow.IsAppRunning)
        {
            if (Globals.IsCloudMode)
            {
                if (Globals.ServerId != 0)
                {
                    if (Globals.LoginPlayerIsAdmin)
                    {
                        if (Globals.ToggleTeambeforeKick && Globals.TempGameId == 0)
                        {
                            Globals.TempGameId = Globals.GameId;
                        }
                        if (Globals.OffileModeSet)
                        {
                            SetRule2Zero();
                            Globals.OffileModeSet = false;
                            Globals.ISetRule = false;
                        }
                        if (!Globals.ISetRule)
                        {
                            bool ok = await SetRuleFromCloud();
                            if (ok)
                            {
                                Globals.CloudModeSet = true;
                                Globals.ISetRule = true;
                            }
                        }
                        if (Globals.SystemAutoBalance && Globals.Team2PlayerCount - Globals.Team1PlayerCount <= 1 && Globals.Team2PlayerCount - Globals.Team1PlayerCount >= 0 || Globals.Team1PlayerCount - Globals.Team2PlayerCount <= 1 && Globals.Team1PlayerCount - Globals.Team2PlayerCount >= 0)
                        {
                            Globals.SystemAutoBalance = false;
                        }
                    }
                }
                else
                {
                    if (Globals.ToggleTeambeforeKick && Globals.TempGameId != 0)
                    {
                        if (Globals.IsCloudMode)
                        {
                            var result = await CloudApi.RemoveAllToggleTeambeForeKick(Globals.TempGameId.ToString());
                            if (!result.IsSuccess)
                            {
                                Globals.AlreadyToggleTeamPlayer.Clear();
                                Globals.TempGameId= 0;
                            }
                        }
                        else{
                            Globals.AlreadyToggleTeamPlayer.Clear();
                            Globals.TempGameId= 0;
                        }
                    }

                    if (Globals.OffileModeSet)
                    {
                        SetRule2Zero();
                        Globals.OffileModeSet = false;
                    }
                    if (Globals.ISetRule)
                    {
                        SetRule2Zero();
                        Globals.ISetRule = false;
                    }
                    if (Globals.SystemAutoBalance)
                    {
                        Globals.SystemAutoBalance = false;
                    }
                    if (!Globals.AllowAutoChangeMap)
                    {
                        Globals.AllowAutoChangeMap = true;
                    }
                }
            }
            else
            {

                if (Globals.CloudModeSet)
                {
                    SetRule2Zero();
                    Globals.CloudModeSet = false;
                    Globals.ISetRule = false;
                }

                if (!Globals.ISetRule)
                {
                    Globals.ISetRule = true;
                    Globals.OffileModeSet = true;
                    SetRuleFromLocalHost();
                }

                if (Globals.ServerId != 0)
                {
                    if (Globals.LoginPlayerIsAdmin)
                    {
                        if (Globals.SystemAutoBalance && Globals.Team2PlayerCount - Globals.Team1PlayerCount <= 1 && Globals.Team2PlayerCount - Globals.Team1PlayerCount >= 0 || Globals.Team1PlayerCount - Globals.Team2PlayerCount <= 1 && Globals.Team1PlayerCount - Globals.Team2PlayerCount >= 0)
                        {
                            Globals.SystemAutoBalance = false;
                        }
                    }
                }
                else
                {
                    if (Globals.SystemAutoBalance)
                    {
                        Globals.SystemAutoBalance = false;
                    }
                }
            }
                Thread.Sleep(2000);
        }
    }

    private void MainWindow_WindowClosingEvent()
    {
        SaveConfig();
    }

    /// <summary>
    /// 保存配置文件
    /// </summary>
    private void SaveConfig()
    {
        // 更新当前授权信息
        if (!Globals.IsCloudMode)
        {

        
        var index = ComboBox_ConfigNames.SelectedIndex;
        if (index != -1)
        {
            RuleConfig.SelectedIndex = index;
            var rule = RuleConfig.RuleInfos[index];
            rule.WhiteLifeKD = CheckBox_WhiteLifeKD.IsChecked == true;
            rule.WhiteLifeKPM = CheckBox_WhiteLifeKPM.IsChecked == true;
            rule.WhiteLifeWeaponStar = CheckBox_WhiteLifeWeaponStar.IsChecked == true;
            rule.WhiteLifeVehicleStar = CheckBox_WhiteLifeVehicleStar.IsChecked == true;
            rule.WhiteKill = CheckBox_WhiteKill.IsChecked == true;
            rule.WhiteKD = CheckBox_WhiteKD.IsChecked == true;
            rule.WhiteKPM = CheckBox_WhiteKPM.IsChecked == true;
            rule.WhiteRank = CheckBox_WhiteRank.IsChecked == true;
            rule.WhiteWeapon = CheckBox_WhiteWeapon.IsChecked == true;
            rule.WhiteLifeMaxAccuracyRatio = CheckBox_WhiteLifeMaxAccuracyRatio.IsChecked == true;
            rule.WhiteLifeMaxHeadShotRatio = CheckBox_WhiteLifeMaxHeadShotRatio.IsChecked == true;
            rule.WhiteMaxWR = CheckBox_WhiteLifeMaxWR.IsChecked == true;
            rule.WhiteAllowToggleTeam = CheckBox_WhiteToggleTeamLimt.IsChecked == true;
            rule.Allow2LowScoreTeam = CheckBox_AllowToggle2LowScoreTeam.IsChecked == true;
            rule.WhiteScore =CheckBox_WhiteScore.IsChecked == true;

            rule.Team1Rule.MaxKill = RuleTeam1Model.MaxKill;
            rule.Team1Rule.FlagKD = RuleTeam1Model.FlagKD;
            rule.Team1Rule.MaxKD = RuleTeam1Model.MaxKD;
            rule.Team1Rule.FlagKPM = RuleTeam1Model.FlagKPM;
            rule.Team1Rule.MaxKPM = RuleTeam1Model.MaxKPM;
            rule.Team1Rule.MinRank = RuleTeam1Model.MinRank;
            rule.Team1Rule.MaxRank = RuleTeam1Model.MaxRank;
            rule.Team1Rule.LifeMaxKD = RuleTeam1Model.LifeMaxKD;
            rule.Team1Rule.LifeMaxKPM = RuleTeam1Model.LifeMaxKPM;
            rule.Team1Rule.LifeMaxWeaponStar = RuleTeam1Model.LifeMaxWeaponStar;
            rule.Team1Rule.LifeMaxAccuracyRatioLevel = RuleTeam1Model.LifeMaxAccuracyRatioLevel;
            rule.Team1Rule.LifeMaxAccuracyRatio = RuleTeam1Model.LifeMaxAccuracyRatio;
            rule.Team1Rule.LifeMaxHeadShotRatioLevel = RuleTeam1Model.LifeMaxHeadShotRatioLevel;
            rule.Team1Rule.LifeMaxHeadShotRatio = RuleTeam1Model.LifeMaxHeadShotRatio;
            rule.Team1Rule.LifeMaxVehicleStar = RuleTeam1Model.LifeMaxVehicleStar;
            rule.Team1Rule.LifeMaxWRLevel = RuleTeam1Model.LifeMaxWRLevel;
            rule.Team1Rule.LifeMaxWR = RuleTeam1Model.LifeMaxWR;
            rule.Team1Rule.ScoreLimit = RuleTeam1Model.ScoreLimt;
            rule.Team1Rule.ScoreGap = RuleTeam1Model.ScoreGap;
            rule.Team1Rule.MaxScore = RuleTeam1Model.MaxScore;
            rule.Team1Rule.FlagKDPro = RuleTeam1Model.FlagKDPro;
            rule.Team1Rule.MaxKDPro = RuleTeam1Model.MaxKDPro;
            rule.Team1Rule.FlagKPMPro = RuleTeam1Model.FlagKPMPro;
            rule.Team1Rule.MaxKPMPro = RuleTeam1Model.MaxKPMPro;

            rule.Team2Rule.MaxKill = RuleTeam2Model.MaxKill;
            rule.Team2Rule.FlagKD = RuleTeam2Model.FlagKD;
            rule.Team2Rule.MaxKD = RuleTeam2Model.MaxKD;
            rule.Team2Rule.FlagKPM = RuleTeam2Model.FlagKPM;
            rule.Team2Rule.MaxKPM = RuleTeam2Model.MaxKPM;
            rule.Team2Rule.MinRank = RuleTeam2Model.MinRank;
            rule.Team2Rule.MaxRank = RuleTeam2Model.MaxRank;
            rule.Team2Rule.LifeMaxKD = RuleTeam2Model.LifeMaxKD;
            rule.Team2Rule.LifeMaxKPM = RuleTeam2Model.LifeMaxKPM;
            rule.Team2Rule.LifeMaxWeaponStar = RuleTeam2Model.LifeMaxWeaponStar;
            rule.Team2Rule.LifeMaxVehicleStar = RuleTeam2Model.LifeMaxVehicleStar;
            rule.Team2Rule.LifeMaxAccuracyRatioLevel = RuleTeam2Model.LifeMaxAccuracyRatioLevel;
            rule.Team2Rule.LifeMaxAccuracyRatio = RuleTeam2Model.LifeMaxAccuracyRatio;
            rule.Team2Rule.LifeMaxHeadShotRatioLevel = RuleTeam2Model.LifeMaxHeadShotRatioLevel;
            rule.Team2Rule.LifeMaxHeadShotRatio = RuleTeam2Model.LifeMaxHeadShotRatio;
            rule.Team2Rule.LifeMaxWRLevel = RuleTeam2Model.LifeMaxWRLevel;
            rule.Team2Rule.LifeMaxWR = RuleTeam2Model.LifeMaxWR;
            rule.Team2Rule.ScoreLimit = RuleTeam2Model.ScoreLimt;
            rule.Team2Rule.ScoreGap = RuleTeam2Model.ScoreGap;
            rule.Team2Rule.MaxScore = RuleTeam2Model.MaxScore;
            rule.Team2Rule.FlagKDPro = RuleTeam2Model.FlagKDPro;
            rule.Team2Rule.MaxKDPro = RuleTeam2Model.MaxKDPro;
            rule.Team2Rule.FlagKPMPro = RuleTeam2Model.FlagKPMPro;
            rule.Team2Rule.MaxKPMPro = RuleTeam2Model.MaxKPMPro;
            rule.WhiteList.Clear();
            foreach (string name in ListBox_CustomWhites.Items)
            {
                rule.WhiteList.Add(name);
            }

            rule.BlackList.Clear();
            foreach (string name in ListBox_CustomBlacks.Items)
            {
                rule.BlackList.Add(name);
            }
            
            rule.TranslateKeyRuleList.Clear();
            foreach (string translateKey in ListBox_TranslateKeyRules.Items) {
                rule.TranslateKeyRuleList.Add(translateKey);
            }

            rule.Team1Weapon.Clear();
            rule.Team2Weapon.Clear();
            for (int i = 0; i < DataGrid_RuleWeaponModels.Count; i++)
            {
                var item = DataGrid_RuleWeaponModels[i];
                if (item.Team1)
                    rule.Team1Weapon.Add(item.English);

                if (item.Team2)
                    rule.Team2Weapon.Add(item.English);
            }
        }

        File.WriteAllText(F_Rule_Path, JsonHelper.JsonSeri(RuleConfig));
        }
    }

    private void SaveConfigOnSwitch()
    {
        // 更新当前授权信息
        if (!Globals.IsCloudMode)
        {
            if (Globals.previousSelectedIndex != -1)
            {
                RuleConfig.SelectedIndex = Globals.previousSelectedIndex;
                var rule = RuleConfig.RuleInfos[Globals.previousSelectedIndex];
                rule.WhiteLifeKD = CheckBox_WhiteLifeKD.IsChecked == true;
                rule.WhiteLifeKPM = CheckBox_WhiteLifeKPM.IsChecked == true;
                rule.WhiteLifeWeaponStar = CheckBox_WhiteLifeWeaponStar.IsChecked == true;
                rule.WhiteLifeVehicleStar = CheckBox_WhiteLifeVehicleStar.IsChecked == true;
                rule.WhiteKill = CheckBox_WhiteKill.IsChecked == true;
                rule.WhiteKD = CheckBox_WhiteKD.IsChecked == true;
                rule.WhiteKPM = CheckBox_WhiteKPM.IsChecked == true;
                rule.WhiteRank = CheckBox_WhiteRank.IsChecked == true;
                rule.WhiteWeapon = CheckBox_WhiteWeapon.IsChecked == true;
                rule.WhiteLifeMaxAccuracyRatio = CheckBox_WhiteLifeMaxAccuracyRatio.IsChecked == true;
                rule.WhiteLifeMaxHeadShotRatio = CheckBox_WhiteLifeMaxHeadShotRatio.IsChecked == true;
                rule.WhiteMaxWR = CheckBox_WhiteLifeMaxWR.IsChecked == true;
                rule.WhiteAllowToggleTeam = CheckBox_WhiteToggleTeamLimt.IsChecked == true;
                rule.Allow2LowScoreTeam = CheckBox_AllowToggle2LowScoreTeam.IsChecked == true;
                rule.WhiteScore =CheckBox_WhiteScore.IsChecked == true;

                rule.Team1Rule.MaxKill = RuleTeam1Model.MaxKill;
                rule.Team1Rule.FlagKD = RuleTeam1Model.FlagKD;
                rule.Team1Rule.MaxKD = RuleTeam1Model.MaxKD;
                rule.Team1Rule.FlagKPM = RuleTeam1Model.FlagKPM;
                rule.Team1Rule.MaxKPM = RuleTeam1Model.MaxKPM;
                rule.Team1Rule.MinRank = RuleTeam1Model.MinRank;
                rule.Team1Rule.MaxRank = RuleTeam1Model.MaxRank;
                rule.Team1Rule.LifeMaxKD = RuleTeam1Model.LifeMaxKD;
                rule.Team1Rule.LifeMaxKPM = RuleTeam1Model.LifeMaxKPM;
                rule.Team1Rule.LifeMaxWeaponStar = RuleTeam1Model.LifeMaxWeaponStar;
                rule.Team1Rule.LifeMaxAccuracyRatioLevel = RuleTeam1Model.LifeMaxAccuracyRatioLevel;
                rule.Team1Rule.LifeMaxAccuracyRatio = RuleTeam1Model.LifeMaxAccuracyRatio;
                rule.Team1Rule.LifeMaxHeadShotRatioLevel = RuleTeam1Model.LifeMaxHeadShotRatioLevel;
                rule.Team1Rule.LifeMaxHeadShotRatio = RuleTeam1Model.LifeMaxHeadShotRatio;
                rule.Team1Rule.LifeMaxVehicleStar = RuleTeam1Model.LifeMaxVehicleStar;
                rule.Team1Rule.LifeMaxWRLevel = RuleTeam1Model.LifeMaxWRLevel;
                rule.Team1Rule.LifeMaxWR = RuleTeam1Model.LifeMaxWR;
                rule.Team1Rule.ScoreLimit = RuleTeam1Model.ScoreLimt;
                rule.Team1Rule.ScoreGap = RuleTeam1Model.ScoreGap;
                rule.Team1Rule.MaxScore = RuleTeam1Model.MaxScore;
                rule.Team1Rule.FlagKDPro = RuleTeam1Model.FlagKDPro;
                rule.Team1Rule.MaxKDPro = RuleTeam1Model.MaxKDPro;
                rule.Team1Rule.FlagKPMPro = RuleTeam1Model.FlagKPMPro;
                rule.Team1Rule.MaxKPMPro = RuleTeam1Model.MaxKPMPro;

                rule.Team2Rule.MaxKill = RuleTeam2Model.MaxKill;
                rule.Team2Rule.FlagKD = RuleTeam2Model.FlagKD;
                rule.Team2Rule.MaxKD = RuleTeam2Model.MaxKD;
                rule.Team2Rule.FlagKPM = RuleTeam2Model.FlagKPM;
                rule.Team2Rule.MaxKPM = RuleTeam2Model.MaxKPM;
                rule.Team2Rule.MinRank = RuleTeam2Model.MinRank;
                rule.Team2Rule.MaxRank = RuleTeam2Model.MaxRank;
                rule.Team2Rule.LifeMaxKD = RuleTeam2Model.LifeMaxKD;
                rule.Team2Rule.LifeMaxKPM = RuleTeam2Model.LifeMaxKPM;
                rule.Team2Rule.LifeMaxWeaponStar = RuleTeam2Model.LifeMaxWeaponStar;
                rule.Team2Rule.LifeMaxVehicleStar = RuleTeam2Model.LifeMaxVehicleStar;
                rule.Team2Rule.LifeMaxAccuracyRatioLevel = RuleTeam2Model.LifeMaxAccuracyRatioLevel;
                rule.Team2Rule.LifeMaxAccuracyRatio = RuleTeam2Model.LifeMaxAccuracyRatio;
                rule.Team2Rule.LifeMaxHeadShotRatioLevel = RuleTeam2Model.LifeMaxHeadShotRatioLevel;
                rule.Team2Rule.LifeMaxHeadShotRatio = RuleTeam2Model.LifeMaxHeadShotRatio;
                rule.Team2Rule.LifeMaxWRLevel = RuleTeam2Model.LifeMaxWRLevel;
                rule.Team2Rule.LifeMaxWR = RuleTeam2Model.LifeMaxWR;
                rule.Team2Rule.ScoreLimit = RuleTeam2Model.ScoreLimt;
                rule.Team2Rule.ScoreGap = RuleTeam2Model.ScoreGap;
                rule.Team2Rule.MaxScore = RuleTeam2Model.MaxScore;
                rule.Team2Rule.FlagKDPro = RuleTeam2Model.FlagKDPro;
                rule.Team2Rule.MaxKDPro = RuleTeam2Model.MaxKDPro;
                rule.Team2Rule.FlagKPMPro = RuleTeam2Model.FlagKPMPro;
                rule.Team2Rule.MaxKPMPro = RuleTeam2Model.MaxKPMPro;
                rule.WhiteList.Clear();
                foreach (string name in ListBox_CustomWhites.Items)
                {
                    rule.WhiteList.Add(name);
                }

                rule.BlackList.Clear();
                foreach (string name in ListBox_CustomBlacks.Items)
                {
                    rule.BlackList.Add(name);
                }

                rule.Team1Weapon.Clear();
                rule.Team2Weapon.Clear();
                for (int i = 0; i < DataGrid_RuleWeaponModels.Count; i++)
                {
                    var item = DataGrid_RuleWeaponModels[i];
                    if (item.Team1)
                        rule.Team1Weapon.Add(item.English);

                    if (item.Team2)
                        rule.Team2Weapon.Add(item.English);
                }
            }

            File.WriteAllText(F_Rule_Path, JsonHelper.JsonSeri(RuleConfig));
            Globals.previousSelectedIndex = ComboBox_ConfigNames.SelectedIndex;
        }
    }
    /// <summary>
    /// 切回离线模式
    /// </summary>
/*    public void Switch2Offline_Mode()
    {
        var index = ComboBox_ConfigNames.SelectedIndex;
        if (index == -1)
            return;

        var rule = RuleConfig.RuleInfos[index];

        ListBox_CustomWhites.Items.Clear();
        if (!Globals.IsCloudMode)
        {
            foreach (var item in rule.WhiteList)
            {
                ListBox_CustomWhites.Items.Add(item);
            }
        }

        // 读取黑名单列表
        ListBox_CustomBlacks.Items.Clear();

        ListBox_CustomWhites.Items.Clear();
        if (!Globals.IsCloudMode)
        {
            foreach (var item in rule.BlackList)
            {
                ListBox_CustomBlacks.Items.Add(item);
            }
        }


        // 读取武器限制信息
        if (!Globals.IsCloudMode)
        {
            for (int i = 0; i < DataGrid_RuleWeaponModels.Count; i++)
            {
                var item = DataGrid_RuleWeaponModels[i];

                var v1 = rule.Team1Weapon.IndexOf(item.English);
                if (v1 != -1)
                    item.Team1 = true;
                else
                    item.Team1 = false;

                var v2 = rule.Team2Weapon.IndexOf(item.English);
                if (v2 != -1)
                    item.Team2 = true;
                else
                    item.Team2 = false;
            }

        }
    }

*/
    /// <summary>
    /// ComboBox选中项变更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ComboBox_ConfigNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!Globals.IsBoot)
        {
            Globals.IsBoot = true;
        } 
        else
        {
            SaveConfigOnSwitch();
        }
        var index = ComboBox_ConfigNames.SelectedIndex;
        if (index == -1)
            return;

        var rule = RuleConfig.RuleInfos[index];

        if (!Globals.IsCloudMode)
        {
            CheckBox_WhiteLifeKD.IsChecked = rule.WhiteLifeKD;
            CheckBox_WhiteLifeKPM.IsChecked = rule.WhiteLifeKPM;
            CheckBox_WhiteLifeWeaponStar.IsChecked = rule.WhiteLifeWeaponStar;
            CheckBox_WhiteLifeVehicleStar.IsChecked = rule.WhiteLifeVehicleStar;
            CheckBox_WhiteKill.IsChecked = rule.WhiteKill;
            CheckBox_WhiteKD.IsChecked = rule.WhiteKD;
            CheckBox_WhiteKPM.IsChecked = rule.WhiteKPM;
            CheckBox_WhiteRank.IsChecked = rule.WhiteRank;
            CheckBox_WhiteWeapon.IsChecked = rule.WhiteWeapon;
            CheckBox_WhiteLifeMaxAccuracyRatio.IsChecked = rule.WhiteLifeMaxAccuracyRatio;
            CheckBox_WhiteLifeMaxHeadShotRatio.IsChecked = rule.WhiteLifeMaxHeadShotRatio;
            CheckBox_WhiteLifeMaxWR.IsChecked = rule.WhiteMaxWR;
            CheckBox_WhiteToggleTeamLimt.IsChecked = rule.WhiteAllowToggleTeam;
            CheckBox_AllowToggle2LowScoreTeam.IsChecked = rule.Allow2LowScoreTeam;
            CheckBox_WhiteScore.IsChecked = rule.WhiteScore;
            // 应用队伍1规则
            RuleTeam1Model.ScoreLimt = rule.Team1Rule.ScoreLimit;
            RuleTeam1Model.ScoreGap = rule.Team1Rule.ScoreGap;
            RuleTeam1Model.MaxKill = rule.Team1Rule.MaxKill;
            RuleTeam1Model.FlagKD = rule.Team1Rule.FlagKD;
            RuleTeam1Model.MaxKD = rule.Team1Rule.MaxKD;
            RuleTeam1Model.FlagKPM = rule.Team1Rule.FlagKPM;
            RuleTeam1Model.MaxKPM = rule.Team1Rule.MaxKPM;
            RuleTeam1Model.MinRank = rule.Team1Rule.MinRank;
            RuleTeam1Model.MaxRank = rule.Team1Rule.MaxRank;
            RuleTeam1Model.LifeMaxKD = rule.Team1Rule.LifeMaxKD;
            RuleTeam1Model.LifeMaxKPM = rule.Team1Rule.LifeMaxKPM;
            RuleTeam1Model.LifeMaxAccuracyRatioLevel = rule.Team1Rule.LifeMaxAccuracyRatioLevel;
            RuleTeam1Model.LifeMaxAccuracyRatio = rule.Team1Rule.LifeMaxAccuracyRatio;
            RuleTeam1Model.LifeMaxHeadShotRatioLevel = rule.Team1Rule.LifeMaxHeadShotRatioLevel;
            RuleTeam1Model.LifeMaxHeadShotRatio = rule.Team1Rule.LifeMaxHeadShotRatio;
            RuleTeam1Model.LifeMaxWRLevel = rule.Team1Rule.LifeMaxWRLevel;
            RuleTeam1Model.LifeMaxWR = rule.Team1Rule.LifeMaxWR;
            RuleTeam1Model.LifeMaxWeaponStar = rule.Team1Rule.LifeMaxWeaponStar;
            RuleTeam1Model.LifeMaxVehicleStar = rule.Team1Rule.LifeMaxVehicleStar;
            RuleTeam1Model.ScoreLimt = rule.Team1Rule.ScoreLimit;
            RuleTeam1Model.ScoreGap = rule.Team1Rule.ScoreGap;
            RuleTeam1Model.MaxScore = rule.Team1Rule.MaxScore;
            RuleTeam1Model.FlagKDPro = rule.Team1Rule.FlagKDPro;
            RuleTeam1Model.MaxKDPro = rule.Team1Rule.MaxKDPro;
            RuleTeam1Model.FlagKPMPro = rule.Team1Rule.FlagKPMPro;
            RuleTeam1Model.MaxKPMPro = rule.Team1Rule.MaxKPMPro;
            // 应用队伍2规则
            RuleTeam2Model.MaxKill = rule.Team2Rule.MaxKill;
            RuleTeam2Model.FlagKD = rule.Team2Rule.FlagKD;
            RuleTeam2Model.MaxKD = rule.Team2Rule.MaxKD;
            RuleTeam2Model.FlagKPM = rule.Team2Rule.FlagKPM;
            RuleTeam2Model.MaxKPM = rule.Team2Rule.MaxKPM;
            RuleTeam2Model.MinRank = rule.Team2Rule.MinRank;
            RuleTeam2Model.MaxRank = rule.Team2Rule.MaxRank;
            RuleTeam2Model.LifeMaxKD = rule.Team2Rule.LifeMaxKD;
            RuleTeam2Model.LifeMaxKPM = rule.Team2Rule.LifeMaxKPM;
            RuleTeam2Model.LifeMaxAccuracyRatioLevel = rule.Team2Rule.LifeMaxAccuracyRatioLevel;
            RuleTeam2Model.LifeMaxAccuracyRatio = rule.Team2Rule.LifeMaxAccuracyRatio;
            RuleTeam2Model.LifeMaxHeadShotRatioLevel = rule.Team2Rule.LifeMaxHeadShotRatioLevel;
            RuleTeam2Model.LifeMaxHeadShotRatio = rule.Team2Rule.LifeMaxHeadShotRatio;
            RuleTeam2Model.LifeMaxWRLevel = rule.Team2Rule.LifeMaxWRLevel;
            RuleTeam2Model.LifeMaxWR = rule.Team2Rule.LifeMaxWR;
            RuleTeam2Model.LifeMaxWeaponStar = rule.Team2Rule.LifeMaxWeaponStar;
            RuleTeam2Model.LifeMaxVehicleStar = rule.Team2Rule.LifeMaxVehicleStar;
            RuleTeam2Model.ScoreLimt = rule.Team2Rule.ScoreLimit;
            RuleTeam2Model.ScoreGap = rule.Team2Rule.ScoreGap;
            RuleTeam2Model.MaxScore = rule.Team2Rule.MaxScore;
            RuleTeam2Model.FlagKDPro = rule.Team2Rule.FlagKDPro;
            RuleTeam2Model.MaxKDPro = rule.Team2Rule.MaxKDPro;
            RuleTeam2Model.FlagKPMPro = rule.Team2Rule.FlagKPMPro;
            RuleTeam2Model.MaxKPMPro = rule.Team2Rule.MaxKPMPro;
            // 白名单特权

            // 读取白名单列表
            ListBox_CustomWhites.Items.Clear();
            if (!Globals.IsCloudMode)
            {
                foreach (var item in rule.WhiteList)
                {
                    ListBox_CustomWhites.Items.Add(item);
                }
            }

            // 读取黑名单列表
            ListBox_CustomBlacks.Items.Clear();
            if (!Globals.IsCloudMode)
            {
                foreach (var item in rule.BlackList)
                {
                    ListBox_CustomBlacks.Items.Add(item);
                }
            }

            TranslateKeyRules_LoadFromList(rule.TranslateKeyRuleList);


            // 读取武器限制信息
            if (!Globals.IsCloudMode)
            {
                for (int i = 0; i < DataGrid_RuleWeaponModels.Count; i++)
                {
                    var item = DataGrid_RuleWeaponModels[i];

                    var v1 = rule.Team1Weapon.IndexOf(item.English);
                    if (v1 != -1)
                        item.Team1 = true;
                    else
                        item.Team1 = false;

                    var v2 = rule.Team2Weapon.IndexOf(item.English);
                    if (v2 != -1)
                        item.Team2 = true;
                    else
                        item.Team2 = false;
                }

            }
            SaveConfig();

        }



    }

    /// <summary>
    /// 保存配置文件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_SaveConfig_Click(object sender, RoutedEventArgs e)
    {
        SaveConfig();
        NotifierHelper.Show(NotifierType.Success, "保存配置文件成功");
    }

    /// <summary>
    /// 当前配置文件重命名
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_ReNameCurrentConfig_Click(object sender, RoutedEventArgs e)
    {
        var name = TextBox_CurrentConfigName.Text.Trim();
        if (string.IsNullOrEmpty(name))
        {
            NotifierHelper.Show(NotifierType.Warning, "配置文件名称不能为空");
            return;
        }

        var index = ComboBox_ConfigNames.SelectedIndex;
        if (index == -1)
        {
            NotifierHelper.Show(NotifierType.Warning, "请选择正确的配置文件");
            return;
        }

        ConfigNames[index] = name;
        RuleConfig.RuleInfos[index].RuleName = name;

        ComboBox_ConfigNames.SelectedIndex = index;
        NotifierHelper.Show(NotifierType.Success, "当前配置文件重命名成功");
    }

    ////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 打印规则日志
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="t1Value"></param>
    /// <param name="t2Value"></param>
    private void AddRuleLog(string Name, string t1Value = "", string t2Value = "")
    {
        DataGrid_RuleLogs.Add(new RuleLog()
        {
            Name = Name,
            T1Value = t1Value,
            T2Value = t2Value
        });
    }

    /// <summary>
    /// 清空规则日志
    /// </summary>
    private void ClearRuleLog()
    {
        DataGrid_RuleLogs.Clear();
    }


    /// <summary>
    /// 推送规则到云端
    /// </summary>
    private async void PushRule2Cloud()
    {
        string team1weapon = string.Join(",", Globals.CustomWeapons_Team1);
        string team2weapon = string.Join(",", Globals.CustomWeapons_Team2);
        var result = await CloudApi.PushRule(
              whiteLifeKD: Globals.WhiteLifeKD,
              whiteLifeKPM: Globals.WhiteLifeKPM,
              whiteLifeWeaponStar: Globals.WhiteLifeWeaponStar,
              whiteLifeVehicleStar: Globals.WhiteLifeVehicleStar,
              whiteKill: Globals.WhiteKill,
              whiteKD: Globals.WhiteKD,
              whiteKPM: Globals.WhiteKPM,
              whiteRank: Globals.WhiteRank,
              whiteWeapon: Globals.WhiteWeapon,
              whiteToggleTeam: Globals.IsNotAllowToggle,
              whiteLifeMaxAccuracyRatio: Globals.WhiteLifeMaxAccuracyRatio,
              whiteLifeMaxHeadShotRatio: Globals.WhiteLifeMaxHeadShotRatio,
              whiteLifeMaxWR: Globals.WhiteLifeMaxWR,
              whiteAllowToggleTeam: Globals.IsAllowWhlistToggleTeam,
              allow2LowScoreTeam: Globals.Allow2LowScoreTeam,
              whiteScore: Globals.WhiteScore,
              team1MaxKill: Globals.ServerRule_Team1.MaxKill,
              team1FlagKD: Globals.ServerRule_Team1.FlagKD,
              team1MaxKD: Globals.ServerRule_Team1.MaxKD,
              team1FlagKPM: Globals.ServerRule_Team1.FlagKPM,
              team1MaxKPM: Globals.ServerRule_Team1.MaxKPM,
              team1MinRank: Globals.ServerRule_Team1.MinRank,
              team1MaxRank: Globals.ServerRule_Team1.MaxRank,
              team1LifeMaxKD: Globals.ServerRule_Team1.LifeMaxKD,
              team1LifeMaxKPM: Globals.ServerRule_Team1.LifeMaxKPM,
              team1LifeMaxAccuracyRatioLevel: Globals.ServerRule_Team1.LifeMaxAccuracyRatioLevel,
              team1LifeMaxAccuracyRatio: Globals.ServerRule_Team1.LifeMaxAccuracyRatio,
              team1LifeMaxHeadShotRatioLevel: Globals.ServerRule_Team1.LifeMaxHeadShotRatioLevel,
              team1LifeMaxHeadShotRatio: Globals.ServerRule_Team1.LifeMaxHeadShotRatio,
              team1LifeMaxWRLevel: Globals.ServerRule_Team1.LifeMaxWRLevel,
              team1LifeMaxWR: Globals.ServerRule_Team1.LifeMaxWR,
              team1LifeMaxWeaponStar: Globals.ServerRule_Team1.LifeMaxWeaponStar,
              team1LifeMaxVehicleStar: Globals.ServerRule_Team1.LifeMaxVehicleStar,
              team1MaxScore: Globals.ServerRule_Team1.MaxScore,
              team1FlagKDPro: Globals.ServerRule_Team1.FlagKDPro,
              team1MaxKDPro: Globals.ServerRule_Team1.MaxKDPro,
              team1FlagKPMPro: Globals.ServerRule_Team1.FlagKPMPro,
              team1MaxKPMPro: Globals.ServerRule_Team1.MaxKPMPro,
              team2MaxKill: Globals.ServerRule_Team2.MaxKill,
              team2FlagKD: Globals.ServerRule_Team2.FlagKD,
              team2MaxKD: Globals.ServerRule_Team2.MaxKD,
              team2FlagKPM: Globals.ServerRule_Team2.FlagKPM,
              team2MaxKPM: Globals.ServerRule_Team2.MaxKPM,
              team2MinRank: Globals.ServerRule_Team2.MinRank,
              team2MaxRank: Globals.ServerRule_Team2.MaxRank,
              team2LifeMaxKD: Globals.ServerRule_Team2.LifeMaxKD,
              team2LifeMaxKPM: Globals.ServerRule_Team2.LifeMaxKPM,
              team2LifeMaxAccuracyRatioLevel: Globals.ServerRule_Team2.LifeMaxAccuracyRatioLevel,
              team2LifeMaxAccuracyRatio: Globals.ServerRule_Team2.LifeMaxAccuracyRatio,
              team2LifeMaxHeadShotRatioLevel: Globals.ServerRule_Team2.LifeMaxHeadShotRatioLevel,
              team2LifeMaxHeadShotRatio: Globals.ServerRule_Team2.LifeMaxHeadShotRatio,
              team2LifeMaxWRLevel: Globals.ServerRule_Team2.LifeMaxWRLevel,
              team2LifeMaxWR: Globals.ServerRule_Team2.LifeMaxWR,
              team2LifeMaxWeaponStar: Globals.ServerRule_Team2.LifeMaxWeaponStar,
              team2LifeMaxVehicleStar: Globals.ServerRule_Team2.LifeMaxVehicleStar,
              team2MaxScore: Globals.ServerRule_Team2.MaxScore,
              team2FlagKDPro: Globals.ServerRule_Team2.FlagKDPro,
              team2MaxKDPro: Globals.ServerRule_Team2.MaxKDPro,
              team2FlagKPMPro: Globals.ServerRule_Team2.FlagKPMPro,
              team2MaxKPMPro: Globals.ServerRule_Team2.MaxKPMPro,
              serverId: Globals.ServerId,
              gameId: Globals.GameId,
              guid: Globals.PersistedGameId,
              operatorPersonaId: Globals.PersonaId,
              team1Weapon: team1weapon,
              team2Weapon: team2weapon,
              team1ScoreLimit: Globals.ServerRule_Team1.ScoreLimit,
              team1ScoreGap: Globals.ServerRule_Team1.ScoreGap,
              serverName: Globals.ServerName
              ); 

        if (result.IsSuccess)
        {
            NotifierHelper.Show(NotifierType.Success, "查询当前规则成功，同步成功");
        }
        else
        {
            NotifierHelper.Show(NotifierType.Warning, "查询当前规则失败，同步失败");
        }
    }


    /// <summary>
    /// 刷新当前规则
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Buttonn_Refresh_Rule(object sender, RoutedEventArgs e)
    {

        if (Globals.IsCloudMode)
        {


        if (Globals.ServerId != 0)
        {
            if (Globals.LoginPlayerIsAdmin)
            {
                   bool ok = await SetRuleFromCloud();
                    if (ok)
                    {
                        NotifierHelper.Show(NotifierType.Success, "刷新成功");
                    }
                    else
                    {
                        NotifierHelper.Show(NotifierType.Error, "刷新失败，请检查服务端或客户端网络问题");
                    }

                }
                else
            {
                NotifierHelper.Show(NotifierType.Error, $"您不是当前服务器管理员");
            }
        }
        else
        {
            NotifierHelper.Show(NotifierType.Error, $"请进入任意服务器");
        }
        }
    else
    {
            NotifierHelper.Show(NotifierType.Error, $"离线模式无法使用该功能请使用在线模式");
    }
    }

    /// <summary>
    /// 应用并查询当前规则
    /// </summary>
    private void SetAndApplyRule()
    {
        // 重置状态
        Globals.IsSetRuleOK = false;
        Globals.AutoKickBreakRulePlayer = false;
        #region 应用当前规则
        Globals.WhiteLifeKD = CheckBox_WhiteLifeKD.IsChecked == true;
        Globals.WhiteLifeKPM = CheckBox_WhiteLifeKPM.IsChecked == true;
        Globals.WhiteLifeWeaponStar = CheckBox_WhiteLifeWeaponStar.IsChecked == true;
        Globals.WhiteLifeVehicleStar = CheckBox_WhiteLifeVehicleStar.IsChecked == true;
        Globals.WhiteKill = CheckBox_WhiteKill.IsChecked == true;
        Globals.WhiteKD = CheckBox_WhiteKD.IsChecked == true;
        Globals.WhiteKPM = CheckBox_WhiteKPM.IsChecked == true;
        Globals.WhiteRank = CheckBox_WhiteRank.IsChecked == true;
        Globals.WhiteWeapon = CheckBox_WhiteWeapon.IsChecked == true;
        Globals.IsAllowWhlistToggleTeam = CheckBox_WhiteToggleTeamLimt.IsChecked == true;
        Globals.WhiteLifeMaxAccuracyRatio = CheckBox_WhiteLifeMaxAccuracyRatio.IsChecked == true;
        Globals.WhiteLifeMaxHeadShotRatio = CheckBox_WhiteLifeMaxHeadShotRatio.IsChecked == true;
        Globals.WhiteLifeMaxWR = CheckBox_WhiteLifeMaxWR.IsChecked == true;
        Globals.Allow2LowScoreTeam = CheckBox_AllowToggle2LowScoreTeam.IsChecked == true;
        Globals.WhiteScore = CheckBox_WhiteScore.IsChecked == true;
        Globals.ServerRule_Team1.ScoreLimit = RuleTeam1Model.ScoreLimt;
        Globals.ServerRule_Team1.ScoreGap = RuleTeam1Model.ScoreGap;
        Globals.ServerRule_Team1.MaxKill = RuleTeam1Model.MaxKill;
        Globals.ServerRule_Team1.FlagKD = RuleTeam1Model.FlagKD;
        Globals.ServerRule_Team1.MaxKD = RuleTeam1Model.MaxKD;
        Globals.ServerRule_Team1.FlagKPM = RuleTeam1Model.FlagKPM;
        Globals.ServerRule_Team1.MaxKPM = RuleTeam1Model.MaxKPM;
        Globals.ServerRule_Team1.MinRank = RuleTeam1Model.MinRank;
        Globals.ServerRule_Team1.MaxRank = RuleTeam1Model.MaxRank;

        Globals.ServerRule_Team1.LifeMaxKD = RuleTeam1Model.LifeMaxKD;
        Globals.ServerRule_Team1.LifeMaxKPM = RuleTeam1Model.LifeMaxKPM;
        Globals.ServerRule_Team1.LifeMaxAccuracyRatioLevel = RuleTeam1Model.LifeMaxAccuracyRatioLevel;
        Globals.ServerRule_Team1.LifeMaxAccuracyRatio = RuleTeam1Model.LifeMaxAccuracyRatio;
        Globals.ServerRule_Team1.LifeMaxHeadShotRatioLevel = RuleTeam1Model.LifeMaxHeadShotRatioLevel;
        Globals.ServerRule_Team1.LifeMaxHeadShotRatio = RuleTeam1Model.LifeMaxHeadShotRatio;
        Globals.ServerRule_Team1.LifeMaxWRLevel = RuleTeam1Model.LifeMaxWRLevel;
        Globals.ServerRule_Team1.LifeMaxWR = RuleTeam1Model.LifeMaxWR;
        Globals.ServerRule_Team1.LifeMaxWeaponStar = RuleTeam1Model.LifeMaxWeaponStar;
        Globals.ServerRule_Team1.LifeMaxVehicleStar = RuleTeam1Model.LifeMaxVehicleStar;

        Globals.ServerRule_Team1.MaxScore = RuleTeam1Model.MaxScore;
        Globals.ServerRule_Team1.FlagKDPro = RuleTeam1Model.FlagKDPro;
        Globals.ServerRule_Team1.MaxKDPro = RuleTeam1Model.MaxKDPro;
        Globals.ServerRule_Team1.FlagKPMPro = RuleTeam1Model.FlagKPMPro;
        Globals.ServerRule_Team1.MaxKPMPro = RuleTeam1Model.MaxKPMPro;

        Globals.ServerRule_Team2.MaxKill = RuleTeam2Model.MaxKill;
        Globals.ServerRule_Team2.FlagKD = RuleTeam2Model.FlagKD;
        Globals.ServerRule_Team2.MaxKD = RuleTeam2Model.MaxKD;
        Globals.ServerRule_Team2.FlagKPM = RuleTeam2Model.FlagKPM;
        Globals.ServerRule_Team2.MaxKPM = RuleTeam2Model.MaxKPM;
        Globals.ServerRule_Team2.MinRank = RuleTeam2Model.MinRank;
        Globals.ServerRule_Team2.MaxRank = RuleTeam2Model.MaxRank;

        Globals.ServerRule_Team2.LifeMaxKD = RuleTeam2Model.LifeMaxKD;
        Globals.ServerRule_Team2.LifeMaxKPM = RuleTeam2Model.LifeMaxKPM;
        Globals.ServerRule_Team2.LifeMaxAccuracyRatioLevel = RuleTeam2Model.LifeMaxAccuracyRatioLevel;
        Globals.ServerRule_Team2.LifeMaxAccuracyRatio = RuleTeam2Model.LifeMaxAccuracyRatio;
        Globals.ServerRule_Team2.LifeMaxHeadShotRatioLevel = RuleTeam2Model.LifeMaxHeadShotRatioLevel;
        Globals.ServerRule_Team2.LifeMaxHeadShotRatio = RuleTeam2Model.LifeMaxHeadShotRatio;
        Globals.ServerRule_Team2.LifeMaxWRLevel = RuleTeam2Model.LifeMaxWRLevel;
        Globals.ServerRule_Team2.LifeMaxWR = RuleTeam2Model.LifeMaxWR;
        Globals.ServerRule_Team2.LifeMaxWeaponStar = RuleTeam2Model.LifeMaxWeaponStar;
        Globals.ServerRule_Team2.LifeMaxVehicleStar = RuleTeam2Model.LifeMaxVehicleStar;

        Globals.ServerRule_Team2.MaxScore = RuleTeam2Model.MaxScore;
        Globals.ServerRule_Team2.FlagKDPro = RuleTeam2Model.FlagKDPro;
        Globals.ServerRule_Team2.MaxKDPro = RuleTeam2Model.MaxKDPro;
        Globals.ServerRule_Team2.FlagKPMPro = RuleTeam2Model.FlagKPMPro;
        Globals.ServerRule_Team2.MaxKPMPro = RuleTeam2Model.MaxKPMPro;
        /////////////////////////////////////////////////////////////////////////////

        // 检查队伍1等级限制
        if (Globals.ServerRule_Team1.MinRank >= Globals.ServerRule_Team1.MaxRank && Globals.ServerRule_Team1.MinRank != 0 && Globals.ServerRule_Team1.MaxRank != 0)
        {
            Globals.IsSetRuleOK = false;

            NotifierHelper.Show(NotifierType.Warning, "队伍1 限制等级规则设置不正确");
            return;
        }
        // 检查队伍2等级限制
        if (Globals.ServerRule_Team2.MinRank >= Globals.ServerRule_Team2.MaxRank && Globals.ServerRule_Team2.MinRank != 0 && Globals.ServerRule_Team2.MaxRank != 0)
        {
            Globals.IsSetRuleOK = false;

            NotifierHelper.Show(NotifierType.Warning, "队伍2 限制等级规则设置不正确");
            return;
        }
        // 检查重开比分设置 
        if (Globals.ServerRule_Team1.ScoreGap >= Globals.ServerRule_Team1.ScoreLimit && Globals.ServerRule_Team1.ScoreLimit != 0 && Globals.ServerRule_Team1.ScoreGap != 0)
        {
            Globals.IsSetRuleOK = false;
            NotifierHelper.Show(NotifierType.Warning, "计算重开分差限制规则设置不正确");
            return;
        }
        /////////////////////////////////////////////////////////////////////////////

        // 清空限制武器列表
        Globals.CustomWeapons_Team1.Clear();
        Globals.CustomWeapons_Team2.Clear();
        // 添加自定义限制武器
        foreach (var item in DataGrid_RuleWeaponModels)
        {
            if (item.Team1)
                Globals.CustomWeapons_Team1.Add(item.English);

            if (item.Team2)
                Globals.CustomWeapons_Team2.Add(item.English);
        }

        // 清空白名单列表
        Globals.CustomWhites_Name.Clear();
        // 添加自定义白名单列表
        foreach (string name in ListBox_CustomWhites.Items)
        {
            Globals.CustomWhites_Name.Add(name);
        }

        // 清空黑名单列表
        Globals.CustomBlacks_Name.Clear();
        // 添加自定义黑名单列表
        foreach (string name in ListBox_CustomBlacks.Items)
        {
            Globals.CustomBlacks_Name.Add(name);
        }

        Globals.IsSetRuleOK = true;
        #endregion

        AddRuleLog("【当局规则】");
        AddRuleLog("最高击杀", $"{Globals.ServerRule_Team1.MaxKill}", $"{Globals.ServerRule_Team2.MaxKill}");

        AddRuleLog("KD阈值", $"{Globals.ServerRule_Team1.FlagKD}", $"{Globals.ServerRule_Team2.FlagKD}");
        AddRuleLog("最高KD", $"{Globals.ServerRule_Team1.MaxKD}", $"{Globals.ServerRule_Team2.MaxKD}");

        AddRuleLog("KPM阈值", $"{Globals.ServerRule_Team1.FlagKPM}", $"{Globals.ServerRule_Team2.FlagKPM}");
        AddRuleLog("最高KPM", $"{Globals.ServerRule_Team1.MaxKPM}", $"{Globals.ServerRule_Team2.MaxKPM}");

        AddRuleLog("最低等级", $"{Globals.ServerRule_Team1.MinRank}", $"{Globals.ServerRule_Team2.MinRank}");
        AddRuleLog("最高等级", $"{Globals.ServerRule_Team1.MaxRank}", $"{Globals.ServerRule_Team2.MaxRank}");

        AddRuleLog("【生涯规则】");
        AddRuleLog("生涯KD", $"{Globals.ServerRule_Team1.LifeMaxKD}", $"{Globals.ServerRule_Team2.LifeMaxKD}");
        AddRuleLog("生涯KPM", $"{Globals.ServerRule_Team1.LifeMaxKPM}", $"{Globals.ServerRule_Team2.LifeMaxKPM}");

        AddRuleLog("生涯命中率等级阈值", $"{Globals.ServerRule_Team1.LifeMaxAccuracyRatioLevel}", $"{Globals.ServerRule_Team2.LifeMaxAccuracyRatioLevel}");
        AddRuleLog("生涯命中率", $"{Globals.ServerRule_Team1.LifeMaxAccuracyRatio}", $"{Globals.ServerRule_Team2.LifeMaxAccuracyRatio}");
        AddRuleLog("生涯爆头率等级阈值", $"{Globals.ServerRule_Team1.LifeMaxHeadShotRatioLevel}", $"{Globals.ServerRule_Team2.LifeMaxHeadShotRatioLevel}");
        AddRuleLog("生涯爆头率", $"{Globals.ServerRule_Team1.LifeMaxHeadShotRatio}", $"{Globals.ServerRule_Team2.LifeMaxHeadShotRatio}");
        AddRuleLog("生涯胜率等级阈值", $"{Globals.ServerRule_Team1.LifeMaxWRLevel}", $"{Globals.ServerRule_Team2.LifeMaxWRLevel}");
        AddRuleLog("生涯胜率", $"{Globals.ServerRule_Team1.LifeMaxWR}", $"{Globals.ServerRule_Team2.LifeMaxWR}");

        AddRuleLog("【扩展规则】");
        AddRuleLog("最高分数", $"{Globals.ServerRule_Team1.MaxScore}", $"{Globals.ServerRule_Team2.MaxScore}");
        AddRuleLog("计算KD的最低击杀数（150级）", $"{Globals.ServerRule_Team1.FlagKDPro}", $"{Globals.ServerRule_Team2.FlagKDPro}");
        AddRuleLog("最高KD（150级）", $"{Globals.ServerRule_Team1.MaxKDPro}", $"{Globals.ServerRule_Team2.MaxKDPro}");
        AddRuleLog("计算KPM的最低击杀数（150级）", $"{Globals.ServerRule_Team1.FlagKPMPro}", $"{Globals.ServerRule_Team2.FlagKPMPro}");
        AddRuleLog("最高KPM（150级）", $"{Globals.ServerRule_Team1.MaxKPMPro}", $"{Globals.ServerRule_Team2.MaxKPMPro}");


        AddRuleLog("武器星数", $"{Globals.ServerRule_Team1.LifeMaxWeaponStar}", $"{Globals.ServerRule_Team2.LifeMaxWeaponStar}");
        AddRuleLog("载具星数", $"{Globals.ServerRule_Team1.LifeMaxVehicleStar}", $"{Globals.ServerRule_Team2.LifeMaxVehicleStar}");

        AddRuleLog("【禁用武器】");
        int team1 = Globals.CustomWeapons_Team1.Count;
        int team2 = Globals.CustomWeapons_Team2.Count;
        for (int i = 0; i < Math.Max(team1, team2); i++)
        {
            if (i < team1 && i < team2)
            {
                AddRuleLog($"武器名称 {i + 1}", $"{ClientHelper.GetWeaponChsName(Globals.CustomWeapons_Team1[i])}", $"{ClientHelper.GetWeaponChsName(Globals.CustomWeapons_Team2[i])}");
            }
            else if (i < team1)
            {
                AddRuleLog($"武器名称 {i + 1}", $"{ClientHelper.GetWeaponChsName(Globals.CustomWeapons_Team1[i])}");
            }
            else if (i < team2)
            {
                AddRuleLog($"武器名称 {i + 1}", "", $"{ClientHelper.GetWeaponChsName(Globals.CustomWeapons_Team2[i])}");
            }
        }

        AddRuleLog("【白名单特权】");
        if (Globals.WhiteLifeKD)
            AddRuleLog("", "免疫生涯KD限制");
        if (Globals.WhiteLifeKPM)
            AddRuleLog("", "免疫生涯KPM限制");
        if (Globals.WhiteLifeWeaponStar)
            AddRuleLog("", "免疫生涯武器星数限制");
        if (Globals.WhiteLifeVehicleStar)
            AddRuleLog("", "免疫生涯载具星数限制");
        if (Globals.WhiteKill)
            AddRuleLog("", "免疫击杀限制");
        if (Globals.WhiteKD)
            AddRuleLog("", "免疫KD限制");
        if (Globals.WhiteKPM)
            AddRuleLog("", "免疫KPM限制");
        if (Globals.WhiteRank)
            AddRuleLog("", "免疫等级限制");
        if (Globals.WhiteWeapon)
            AddRuleLog("", "免疫武器限制");
        if (Globals.WhiteLifeMaxAccuracyRatio)
            AddRuleLog("", "免疫生涯命中率限制");
        if (Globals.WhiteLifeMaxHeadShotRatio)
            AddRuleLog("", "免疫生涯爆头率限制");
        if (Globals.WhiteLifeMaxWR)
            AddRuleLog("", "免疫生涯胜率限制");
        if (Globals.IsAllowWhlistToggleTeam)
            AddRuleLog("", "免疫更换队伍限制");
        if (Globals.WhiteScore)
            AddRuleLog("", "免疫分数限制");
        int index = 1;
        AddRuleLog("【白名单列表】");
        foreach (var item in Globals.CustomWhites_Name)
        {
            AddRuleLog($"玩家ID {index++}", $"{item}");
        }

        index = 1;
        AddRuleLog("【黑名单列表】");
        foreach (var item in Globals.CustomBlacks_Name)
        {
            AddRuleLog($"玩家ID {index++}", $"{item}");
        }
        AddRuleLog("【是否允许更换至劣势抗压】");
        if (Globals.Allow2LowScoreTeam)
            AddRuleLog("", "允许更换至劣势抗压");
    }
    /// <summary>
    /// 应用并查询当前规则
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_ApplyAndQueryCurrentRule_Click(object sender, RoutedEventArgs e)
    {
        if (Globals.IsRefreshRule)
        {
            NotifierHelper.Show(NotifierType.Warning, "正在获取规则，请稍后再试");
            return;
        }
        ClearRuleLog();
        if (Globals.IsCloudMode)
        {

            if (Globals.ServerId == 0)
            {
                NotifierHelper.Show(NotifierType.Error, "请进入任意服务器");
            }
            else
            {
                if (Globals.LoginPlayerIsAdmin)
                {
                    SetAndApplyRule();
                    PushRule2Cloud();
                }
                else
                {
                    NotifierHelper.Show(NotifierType.Error, "您不是当前服务器管理员");
                }
            }

        }
        else
        {
            SetAndApplyRule();
            NotifierHelper.Show(NotifierType.Success, "查询当前规则成功");
            SaveConfig();
        }
    }

    /// <summary>
    /// 从白名单列表移除选中玩家
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_RemoveSelectedWhite_Click(object sender, RoutedEventArgs e)
    {
            if (Globals.IsCloudMode)
            {
            
            if (ListBox_CustomWhites.SelectedItem is string name)
            {
                ListBox_CustomWhites.Items.Remove(name);
            
                if (Globals.ServerId == 0)
                {
                    NotifierHelper.Show(NotifierType.Error, $"请进入任意服务器");
                    return;
                }
                if (!Globals.LoginPlayerIsAdmin)
                {
                    NotifierHelper.Show(NotifierType.Error, $"您不是当前服务器管理员");
                    return;
                }
                var result = await BF1API.RemoveWhiteList(ServerId: Globals.ServerId.ToString(), PlayerName: name);
                if (result.IsSuccess)
                {
                    NotifierHelper.Show(NotifierType.Success, $"联网白名单删除成功");
                    var results = await BF1API.RefreshWhiteList(ServerId: Globals.ServerId.ToString());
                    if (results.IsSuccess)
                    {
                        var test = results.Content.Replace("\r", "");
            
                        List<Players> players = JsonConvert.DeserializeObject<List<Players>>(test);
            
                        ListBox_CustomWhites.Items.Clear();
            
                        foreach (Players player in players)
                        {
                            ListBox_CustomWhites.Items.Add(player.PlayerName);
                        }
            
                        NotifierHelper.Show(NotifierType.Success, $"联网白名单刷新成功");
                    }
                    else
                    {
                        NotifierHelper.Show(NotifierType.Error, $"联网白名单刷新失败");
                    }
                }
                else
                {
                    NotifierHelper.Show(NotifierType.Error, $"联网白名单删除失败");
                }
                TextBox_NewWhiteName.Clear();
                }
        }
        else
        {
            if (ListBox_CustomWhites.SelectedItem is string name)
            {
                ListBox_CustomWhites.Items.Remove(name);

                NotifierHelper.Show(NotifierType.Success, $"从白名单列表移除玩家 {name} 成功");
            }
        }
    }

        /// <summary>
        /// 刷新联网白名单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_RefreshWhite_Click(object sender, RoutedEventArgs e)
    {
        if (Globals.IsCloudMode)
        {
            NotifierHelper.Show(NotifierType.Information, $"正在刷新联网白名单中");
            if (Globals.ServerId == 0)
            {
                NotifierHelper.Show(NotifierType.Error, $"请进入任意服务器");
                return;
            }
            if (!Globals.LoginPlayerIsAdmin)
            {
                NotifierHelper.Show(NotifierType.Error, $"您不是当前服务器管理员");
                return;
            }
            var result = await BF1API.RefreshWhiteList(ServerId: Globals.ServerId.ToString());
            if (result.IsSuccess)
            {
                var test = result.Content.Replace("\r", "");

                List<Players> players = JsonConvert.DeserializeObject<List<Players>>(test);

                ListBox_CustomWhites.Items.Clear();

                foreach (Players player in players)
                {
                    ListBox_CustomWhites.Items.Add(player.PlayerName);
                }

                NotifierHelper.Show(NotifierType.Success, $"在线白名单刷新成功");
            }
            else
            {
                NotifierHelper.Show(NotifierType.Error, $"在线白名单刷新失败");
            }
        }
        else
        {
            NotifierHelper.Show(NotifierType.Success, $"当前为离线模式无法使用在线白名单");
        }


    }
    /// <summary>
    /// 添加玩家到白名单列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_AddNewWhite_Click(object sender, RoutedEventArgs e)
    {
        if (Globals.IsCloudMode)
        {

           var name = TextBox_NewWhiteName.Text.Trim();
           if (string.IsNullOrWhiteSpace(name))
           {
               NotifierHelper.Show(NotifierType.Warning, "请输入正确的玩家名称");
               return;
           }
           if (Globals.ServerId == 0)
           {
               NotifierHelper.Show(NotifierType.Error, $"请进入任意服务器");
               return;
           }
           if (!Globals.LoginPlayerIsAdmin)
           {
               NotifierHelper.Show(NotifierType.Error, $"您不是当前服务器管理员");
               return;
           }
           var result = await BF1API.AddWhiteList( ServerId: Globals.ServerId.ToString(),Gameid: Globals.GameId.ToString(),Guid: Globals.PersistedGameId, PlayerName: name,ServerName: Globals.ServerName);
           if (result.IsSuccess)
           {
               NotifierHelper.Show(NotifierType.Success, $"联网白名单添加成功");
               var results = await BF1API.RefreshWhiteList(ServerId: Globals.ServerId.ToString());
               if (results.IsSuccess)
               {
                   var test = results.Content.Replace("\r", "");
           
                   List<Players> players = JsonConvert.DeserializeObject<List<Players>>(test);
           
                   ListBox_CustomWhites.Items.Clear();
           
                   foreach (Players player in players)
                   {
                       ListBox_CustomWhites.Items.Add(player.PlayerName);
                   }
           
                   NotifierHelper.Show(NotifierType.Success, $"联网白名单刷新成功");
               }
               else
               {
                   NotifierHelper.Show(NotifierType.Error, $"联网白名单刷新失败");
               }
           }
           else
           {
               NotifierHelper.Show(NotifierType.Error, $"联网白名单添加失败");
           }
           TextBox_NewWhiteName.Clear();
           }
        else
        {
            var name = TextBox_NewWhiteName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                NotifierHelper.Show(NotifierType.Warning, "请输入正确的玩家名称");
                return;
            }

            ListBox_CustomWhites.Items.Add(name);
            TextBox_NewWhiteName.Clear();

            NotifierHelper.Show(NotifierType.Success, $"添加玩家 {name} 到白名单列表成功");
        }
    }



    /// <summary>
    /// 刷新联网黑名单
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_RefreshBlack_Click(object sender, RoutedEventArgs e)
    {


        if(Globals.IsCloudMode)
        {
            NotifierHelper.Show(NotifierType.Information, $"正在刷新联网黑名单中");
            if (Globals.ServerId == 0)
            {
                NotifierHelper.Show(NotifierType.Error, $"请进入任意服务器");
                return;
            }
            if (!Globals.LoginPlayerIsAdmin)
            {
                NotifierHelper.Show(NotifierType.Error, $"您不是当前服务器管理员");
                return;
            }
            var result = await CloudApi.RefreshBlackList(ServerId: Globals.ServerId.ToString());
            if (result.IsSuccess)
            {
                var test = result.Content.Replace("\r", "");

                List<Players> players = JsonConvert.DeserializeObject<List<Players>>(test);

                ListBox_CustomBlacks.Items.Clear();

                foreach (Players player in players)
                {
                    ListBox_CustomBlacks.Items.Add(player.PlayerName);
                }

                NotifierHelper.Show(NotifierType.Success, $"联网黑名单刷新成功");
            }
            else
            {
                NotifierHelper.Show(NotifierType.Error, $"联网黑名单刷新失败");
            }
        }
        else
        {
            NotifierHelper.Show(NotifierType.Success, $"当前为离线模式无法使用在线白名单");

        }



    }
        /// <summary>
        /// 从黑名单列表移除选中玩家
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_RemoveSelectedBlack_Click(object sender, RoutedEventArgs e)
    {


        if (Globals.IsCloudMode)
        {
            if (ListBox_CustomBlacks.SelectedItem is string name)
            {
                ListBox_CustomBlacks.Items.Remove(name);

                if (Globals.ServerId == 0)
                {
                    NotifierHelper.Show(NotifierType.Error, $"请进入任意服务器");
                    return;
                }
                if (!Globals.LoginPlayerIsAdmin)
                {
                    NotifierHelper.Show(NotifierType.Error, $"您不是当前服务器管理员");
                    return;
                }
                var result = await CloudApi.RemoveBlackList(ServerId: Globals.ServerId.ToString(), PlayerName: name);
                if (result.IsSuccess)
                {
                    NotifierHelper.Show(NotifierType.Success, $"联网黑名单删除成功");
                    var results = await CloudApi.RefreshBlackList(ServerId: Globals.ServerId.ToString());
                    if (results.IsSuccess)
                    {
                        var test = results.Content.Replace("\r", "");

                        List<Players> players = JsonConvert.DeserializeObject<List<Players>>(test);

                        ListBox_CustomBlacks.Items.Clear();

                        foreach (Players player in players)
                        {
                            ListBox_CustomBlacks.Items.Add(player.PlayerName);
                        }

                        NotifierHelper.Show(NotifierType.Success, $"联网黑名单刷新成功");
                    }
                    else
                    {
                        NotifierHelper.Show(NotifierType.Error, $"联网黑名单刷新失败");
                    }
                }
                else
                {
                    NotifierHelper.Show(NotifierType.Error, $"联网黑名单删除失败");
                }
                TextBox_NewBlackName.Clear();


            }
        }
        else
        {
            if (ListBox_CustomBlacks.SelectedItem is string name)
            {
                ListBox_CustomBlacks.Items.Remove(name);

                NotifierHelper.Show(NotifierType.Success, $"从黑名单列表移除玩家 {name} 成功");
            }
        }

    }


    /// <summary>
    /// 添加玩家到黑名单列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_AddNewBlack_Click(object sender, RoutedEventArgs e)
    {

        if (Globals.IsCloudMode)
        {
            var name = TextBox_NewBlackName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                NotifierHelper.Show(NotifierType.Warning, "请输入正确的玩家名称");
                return;
            }
            if (Globals.ServerId == 0)
            {
                NotifierHelper.Show(NotifierType.Error, $"请进入任意服务器");
                return;
            }
            if (!Globals.LoginPlayerIsAdmin)
            {
                NotifierHelper.Show(NotifierType.Error, $"您不是当前服务器管理员");
                return;
            }
            var result = await CloudApi.AddBlackList(ServerId: Globals.ServerId.ToString(), Gameid: Globals.GameId.ToString(), Guid: Globals.PersistedGameId, PlayerName: name, ServerName: Globals.ServerName);
            if (result.IsSuccess)
            {
                NotifierHelper.Show(NotifierType.Success, $"联网黑名单添加成功");
                var results = await CloudApi.RefreshBlackList(ServerId: Globals.ServerId.ToString());
                if (results.IsSuccess)
                {
                    var test = results.Content.Replace("\r", "");

                    List<Players> players = JsonConvert.DeserializeObject<List<Players>>(test);

                    ListBox_CustomBlacks.Items.Clear();

                    foreach (Players player in players)
                    {
                        ListBox_CustomBlacks.Items.Add(player.PlayerName);
                    }

                    NotifierHelper.Show(NotifierType.Success, $"联网黑名单刷新成功");
                }
                else
                {
                    NotifierHelper.Show(NotifierType.Error, $"联网黑名单刷新失败");
                }
            }
            else
            {
                NotifierHelper.Show(NotifierType.Error, $"联网黑名单添加失败");
            }
            TextBox_NewBlackName.Clear();
        }
        else 
        {
            var name = TextBox_NewBlackName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                NotifierHelper.Show(NotifierType.Warning, "请输入正确的玩家名称");
                return;
            }

            ListBox_CustomBlacks.Items.Add(name);
            TextBox_NewBlackName.Clear();

            NotifierHelper.Show(NotifierType.Success, $"添加玩家 {name} 到黑名单列表成功");
        }


    }


    /*
        /// <summary>
        /// 从黑名单列表移除选中玩家
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_RemoveSelectedBlack_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox_CustomBlacks.SelectedItem is string name)
            {
                ListBox_CustomBlacks.Items.Remove(name);

                NotifierHelper.Show(NotifierType.Success, $"从黑名单列表移除玩家 {name} 成功");
            }
        }

        /// <summary>
        /// 添加玩家到黑名单列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_AddNewBlack_Click(object sender, RoutedEventArgs e)
        {
            var name = TextBox_NewBlackName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                NotifierHelper.Show(NotifierType.Warning, "请输入正确的玩家名称");
                return;
            }

            ListBox_CustomBlacks.Items.Add(name);
            TextBox_NewBlackName.Clear();

            NotifierHelper.Show(NotifierType.Success, $"添加玩家 {name} 到黑名单列表成功");
        }*/

    ///////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 导入白名单列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MenuItem_ImportCustomWhites_Click(object sender, RoutedEventArgs e)
    {
        if (!Globals.IsCloudMode)
        {
        try
        {
            var fileDialog = new OpenFileDialog
            {
                Title = "批量导入白名单列表",
                RestoreDirectory = true,
                Multiselect = false,
                Filter = "文本文档|*.txt"
            };

            if (fileDialog.ShowDialog() == true)
            {
                ListBox_CustomWhites.Items.Clear();
                foreach (var item in File.ReadAllLines(fileDialog.FileName))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        ListBox_CustomWhites.Items.Add(item);
                }

                NotifierHelper.Show(NotifierType.Success, "批量导入txt文件到白名单列表成功");
            }
        }
        catch (Exception ex)
        {
            NotifierHelper.ShowException(ex);
        }
    }
        else
        {
            NotifierHelper.Show(NotifierType.Error, "在线模式下无法使用该功能，未来将支持该功能");
        }
    }

    /// <summary>
    /// 导出白名单列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MenuItem_ExportCustomWhites_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_CustomWhites.Items.IsEmpty)
        {
            NotifierHelper.Show(NotifierType.Warning, "白名单列表为空，导出操作取消");
            return;
        }

        try
        {
            var fileDialog = new SaveFileDialog
            {
                Title = "批量导出白名单列表",
                RestoreDirectory = true,
                Filter = "文本文档|*.txt"
            };

            if (fileDialog.ShowDialog() == true)
            {
                File.WriteAllText(fileDialog.FileName, string.Join(Environment.NewLine, ListBox_CustomWhites.Items.Cast<string>()));

                NotifierHelper.Show(NotifierType.Success, "批量导出白名单列表到txt文件成功");
            }
        }
        catch (Exception ex)
        {
            NotifierHelper.ShowException(ex);
        }
    }

    /// <summary>
    /// 白名单列表去重
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MenuItem_DistinctCustomWhites_Click(object sender, RoutedEventArgs e)
    {
        if (!Globals.IsCloudMode)
        {
        if (ListBox_CustomWhites.Items.IsEmpty)
        {
            NotifierHelper.Show(NotifierType.Warning, "白名单列表为空，去重操作取消");
            return;
        }

        List<string> tempStr = new();
        foreach (string item in ListBox_CustomWhites.Items)
            tempStr.Add(item);
        ListBox_CustomWhites.Items.Clear();
        foreach (var item in tempStr.Distinct().ToList())
            ListBox_CustomWhites.Items.Add(item);

        NotifierHelper.Show(NotifierType.Success, "白名单列表去重成功");
    }
        else
        {
            NotifierHelper.Show(NotifierType.Error, "在线模式下无法使用该功能，未来将支持该功能");
        }
    }

    /// <summary>
    /// 清空白名单列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MenuItem_ClearCustomWhites_Click(object sender, RoutedEventArgs e)
    {
        if (!Globals.IsCloudMode)
        {
        ListBox_CustomWhites.Items.Clear();
        NotifierHelper.Show(NotifierType.Success, "清空白名单列表成功");
        }
        else
        {
            NotifierHelper.Show(NotifierType.Error, "在线模式下无法使用该功能，未来将支持该功能");
        }
    }

    /// <summary>
    /// 导入黑名单列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MenuItem_ImportCustomBlacks_Click(object sender, RoutedEventArgs e)
    {
        if (!Globals.IsCloudMode)
        {
        try
        {
            var fileDialog = new OpenFileDialog
            {
                Title = "批量导入黑名单列表",
                RestoreDirectory = true,
                Multiselect = false,
                Filter = "文本文档|*.txt"
            };

            if (fileDialog.ShowDialog() == true)
            {
                ListBox_CustomBlacks.Items.Clear();
                foreach (var item in File.ReadAllLines(fileDialog.FileName))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        ListBox_CustomBlacks.Items.Add(item);
                        if (Globals.IsCloudMode)
                        {
                            
                        }

                    }

                }

                NotifierHelper.Show(NotifierType.Success, "批量导入txt文件到黑名单列表成功");
            }
        }
        catch (Exception ex)
        {
            NotifierHelper.ShowException(ex);
        }
        }
        else
        {
            NotifierHelper.Show(NotifierType.Error, "在线模式下无法使用该功能，未来将支持该功能");
        }
    }

    /// <summary>
    /// 导出黑名单列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MenuItem_ExportCustomBlacks_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_CustomBlacks.Items.IsEmpty)
        {
            NotifierHelper.Show(NotifierType.Warning, "黑名单列表为空，导出操作取消");
            return;
        }

        try
        {
            var fileDialog = new SaveFileDialog
            {
                Title = "批量导出黑名单列表",
                RestoreDirectory = true,
                Filter = "文本文档|*.txt"
            };

            if (fileDialog.ShowDialog() == true)
            {
                File.WriteAllText(fileDialog.FileName, string.Join(Environment.NewLine, ListBox_CustomBlacks.Items.Cast<string>()));

                NotifierHelper.Show(NotifierType.Success, "批量导出黑名单列表到txt文件成功");
            }
        }
        catch (Exception ex)
        {
            NotifierHelper.ShowException(ex);
        }
    }

    /// <summary>
    /// 黑名单列表去重
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MenuItem_DistinctCustomBlacks_Click(object sender, RoutedEventArgs e)
    {
        if (!Globals.IsCloudMode)
        {


            if (ListBox_CustomBlacks.Items.IsEmpty)
            {
                NotifierHelper.Show(NotifierType.Warning, "黑名单列表为空，去重操作取消");
                return;
            }

            List<string> tempStr = new();
            foreach (string item in ListBox_CustomBlacks.Items)
                tempStr.Add(item);
            ListBox_CustomBlacks.Items.Clear();
            foreach (var item in tempStr.Distinct().ToList())
                ListBox_CustomBlacks.Items.Add(item);

            NotifierHelper.Show(NotifierType.Success, "黑名单列表去重成功");
        }
        else
        {
            NotifierHelper.Show(NotifierType.Error, "在线模式下无法使用该功能,未来将支持该功能");
        }
    }

    /// <summary>
    /// 清空黑名单列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MenuItem_ClearCustomBlacks_Click(object sender, RoutedEventArgs e)
    {
        if (!Globals.IsCloudMode)
        {
        ListBox_CustomBlacks.Items.Clear();
        NotifierHelper.Show(NotifierType.Success, "清空黑名单列表成功");
        }
        else
        {
            NotifierHelper.Show(NotifierType.Error, "在线模式下无法使用该功能,未来将支持该功能");
        }
    }

    private void TranslateKeyRules_Unimplemented(object sender, RoutedEventArgs e)
    {
        NotifierHelper.Show(NotifierType.Error, "咕咕咕, 这个功能还没写, 但是先把菜单搁这免得咱忘了\n --SakuraKooi");
    }

    private void TranslateKeyRules_AddFromList(object sender, RoutedEventArgs routedEventArgs)
    {
        if (ListBox_TranslateKeyList.SelectedItems.Count == 0)
        {
            NotifierHelper.Show(NotifierType.Error, "请选择要添加到黑名单的翻译词条");
            return;
        }

        var copiedList = new List<string>(ListBox_TranslateKeyList.SelectedItems.Cast<string>());
        foreach (var selectedItem in copiedList)
        {
            ListBox_TranslateKeyRules.Items.Add(selectedItem);
            ListBox_TranslateKeyList.Items.Remove(selectedItem);
        }
        TranslateKeyRules_Helper_ReorderList();
    }

    private void TranslateKeyRules_RemoveFromList(object sender, RoutedEventArgs routedEventArgs)
    {
        if (ListBox_TranslateKeyRules.SelectedItems.Count == 0)
        {
            NotifierHelper.Show(NotifierType.Error, "请选择要从黑名单删除的翻译词条");
            return;
        }

        var copiedList = new List<string>(ListBox_TranslateKeyRules.SelectedItems.Cast<string>());
        foreach (var selectedItem in copiedList)
        {
            ListBox_TranslateKeyList.Items.Add(selectedItem);
            ListBox_TranslateKeyRules.Items.Remove(selectedItem);
        }
        TranslateKeyRules_Helper_ReorderList();
    }

    private void TranslateKeyRules_Clear(object sender, RoutedEventArgs e)
    {
        TranslateKeyRules_LoadFromList(new List<string>());
    }
    private void TranslateKeyRules_AddCustom(object sender, RoutedEventArgs e)
    {
        if (!Regex.IsMatch(TextBox_NewTranslateKeyRule.Text, @"^[\w\d]{8}$"))
        {
            NotifierHelper.Show(NotifierType.Error, "不合法的寒霜引擎翻译词条格式! (请输入8位十六进制哈希值)");
            return;
        }
        ListBox_TranslateKeyRules.Items.Add(TextBox_NewTranslateKeyRule.Text + " 自定义词条");
        foreach (string key in ListBox_TranslateKeyList.Items)
        {
            if (key.Split(' ')[0].Equals(TextBox_NewTranslateKeyRule.Text))
            {
                ListBox_TranslateKeyList.Items.Remove(key);
                break;
            }
        }
        TranslateKeyRules_Helper_ReorderList();
    }

    private void TranslateKeyRules_Preset_TooLong_Checked(object sender, RoutedEventArgs e)
    {
        TranslateKeyRules_Helper_ApplyPreset(TranslateKeyData.TranslateKeyFlag.TOO_LONG, CheckBox_TranslateKeyRulePresets_TooLong.IsChecked.Value);
    }

    private void TranslateKeyRules_Preset_Offensive_Checked(object sender, RoutedEventArgs e)
    {
        TranslateKeyRules_Helper_ApplyPreset(TranslateKeyData.TranslateKeyFlag.OFFENSIVE, CheckBox_TranslateKeyRulePresets_Offensive.IsChecked.Value);
    }

    private void TranslateKeyRules_Preset_Whitespace_Checked(object sender, RoutedEventArgs e)
    {
        TranslateKeyRules_Helper_ApplyPreset(TranslateKeyData.TranslateKeyFlag.WHITESPACE, CheckBox_TranslateKeyRulePresets_Whitespace.IsChecked.Value);
    }

    private void TranslateKeyRules_Preset_FakeEnglish_Checked(object sender, RoutedEventArgs e)
    {
        TranslateKeyRules_Helper_ApplyPreset(TranslateKeyData.TranslateKeyFlag.FAKE_ENGLISH, CheckBox_TranslateKeyRulePresets_FakeEnglish.IsChecked.Value);
    }

    private void TranslateKeyRules_Preset_Multiline_Checked(object sender, RoutedEventArgs e)
    {
        TranslateKeyRules_Helper_ApplyPreset(TranslateKeyData.TranslateKeyFlag.MULTILINE, CheckBox_TranslateKeyRulePresets_Multiline.IsChecked.Value);
    }

    private void TranslateKeyRules_Initialize()
    {
        TranslateKeyData.TranslateKeys.ForEach(key =>
        {
            ListBox_TranslateKeyList.Items.Add(key.ToRule());
        });
    }

    private void TranslateKeyRules_LoadFromList(List<string> ruleTranslateKeyRuleList)
    {
        ListBox_TranslateKeyRules.Items.Clear();
        ListBox_TranslateKeyList.Items.Clear();

        TranslateKeyRules_Initialize();
        ruleTranslateKeyRuleList.ForEach(key =>
        {
            ListBox_TranslateKeyRules.Items.Add(key);
            ListBox_TranslateKeyList.Items.Remove(key);
        });
        TranslateKeyRules_Helper_ReorderList();
    }

    private void TranslateKeyRules_Helper_ApplyPreset(TranslateKeyData.TranslateKeyFlag flag, bool addOrRemove)
    {
        TranslateKeyData.TranslateKeys
            .SkipWhile(key => key.Flag != flag)
            .Select(key => key.ToRule())
            .Each(key =>
            {
                if (addOrRemove)
                {
                    ListBox_TranslateKeyRules.Items.Add(key);
                    ListBox_TranslateKeyList.Items.Remove(key);
                }
                else
                {
                    ListBox_TranslateKeyList.Items.Add(key);
                    ListBox_TranslateKeyRules.Items.Remove(key);
                }
            });
        TranslateKeyRules_Helper_ReorderList();
    }

    private void TranslateKeyRules_Helper_ReorderList()
    {
        var sortedList1 = ListBox_TranslateKeyRules.Items.Cast<string>().Distinct().OrderBy(t => t).ToList();
        ListBox_TranslateKeyRules.Items.Clear();
        sortedList1.ForEach(key => ListBox_TranslateKeyRules.Items.Add(key));

        var sortedList2 = ListBox_TranslateKeyList.Items.Cast<string>().Distinct().OrderBy(t => t).ToList();
        ListBox_TranslateKeyList.Items.Clear();
        sortedList2.ForEach(key => ListBox_TranslateKeyList.Items.Add(key));
    }
}
