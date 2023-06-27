using BF1ServerTools.API;
using BF1ServerTools.Data;
using BF1ServerTools.Helper;
using BF1ServerTools.Utils;

namespace BF1ServerTools.Views;

/// <summary>
/// LogView.xaml 的交互逻辑
/// </summary>
public partial class LogView : UserControl
{
    /// <summary>
    /// 添加手动操作日志委托
    /// </summary>
    public static Action<AutoKickInfo> ActionScoreKickLog;

    /// <summary>
    /// 添加踢人成功日志委托
    /// </summary>
    public static Action<AutoKickInfo> ActionAddKickOKLog;
    /// <summary>
    /// 添加踢人失败日志委托
    /// </summary>
    public static Action<AutoKickInfo> ActionAddKickNOLog;
    /// <summary>
    /// 添加换边日志委托
    /// </summary>
    public static Action<ChangeTeamInfo> ActionAddChangeTeamInfoLog;

    public LogView()
    {
        InitializeComponent();
        this.DataContext = this;
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

        ActionScoreKickLog = ScoreKickLog;

        ActionAddKickOKLog = AddKickOKLog;
        ActionAddKickNOLog = AddKickNOLog;

        ActionAddChangeTeamInfoLog = AddChangeTeamInfoLog;
    }

    /// <summary>
    /// 主窗口关闭事件
    /// </summary>
    private void MainWindow_WindowClosingEvent()
    {

    }

    /////////////////////////////////////////////////////

    /// <summary>
    /// 增加踢人CD信息
    /// </summary>
    /// <param name="info"></param>
    private void AddKickCDInfo(AutoKickInfo info)
    {
        if (Globals.IsEnableKickCoolDown)
        {
            var index = Globals.KickCoolDownInfos.FindIndex(var => var.PersonaId == info.PersonaId);
            if (index == -1)
            {
                Globals.KickCoolDownInfos.Add(new()
                {
                    Rank = info.Rank,
                    Name = info.Name,
                    PersonaId = info.PersonaId,
                    Date = DateTime.Now
                });
            }
        }
    }

    /// <summary>
    /// 追加手动操作日志
    /// </summary>
    /// <param name="info"></param>
    private void ScoreKickLog(AutoKickInfo info)
    {
        lock (this)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                if (TextBox_ScoreKickLog.LineCount >= 1000)
                    TextBox_ScoreKickLog.Clear();

                if (Globals.ReverseOrder)
                {
                    AppendScoreKickLog($"状态: {info.State}\n");
                    AppendScoreKickLog($"踢出理由: {info.Reason}");
                    AppendScoreKickLog($"数字ID: {info.PersonaId}");
                    AppendScoreKickLog($"玩家ID: {info.Name}");
                    AppendScoreKickLog($"等级: {info.Rank}");
                    AppendScoreKickLog($"操作时间: {DateTime.Now}");

                }
                else
                {
                    AppendScoreKickLog($"操作时间: {DateTime.Now}");
                    AppendScoreKickLog($"等级: {info.Rank}");
                    AppendScoreKickLog($"玩家ID: {info.Name}");
                    AppendScoreKickLog($"数字ID: {info.PersonaId}");
                    AppendScoreKickLog($"踢出理由: {info.Reason}");
                    AppendScoreKickLog($"状态: {info.State}\n");
                }



                SQLiteHelper.AddLog("score_kick", new()
                {
                    Rank = info.Rank,
                    Name = info.Name,
                    PersonaId = info.PersonaId,
                    Type = "手动踢人",
                    Message1 = info.Reason,
                    Message2 = info.State,
                    Message3 = ""
                });
            });
            if (Globals.IsCloudMode)
            {
                var _ = CloudApi.KickHIstory(Operator: Globals.PersonaId.ToString(),KickedOutPlayerRank: info.Rank.ToString(), KickedOutPlayerName: info.Name,KickedOutPersonaId: info.PersonaId.ToString(), Reason: info.Reason, State: info.State, ServerId: Globals.ServerId.ToString(), Guid: Globals.PersistedGameId,GameId: Globals.GameId.ToString(),Type: "Manual");
            }
            AddKickCDInfo(info);
        }
    }

    /// <summary>
    /// 追加踢人成功日志
    /// </summary>
    /// <param name="info"></param>
    private void AddKickOKLog(AutoKickInfo info)
    {
        lock (this)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                if (TextBox_KickOKLog.LineCount >= 1000)
                    TextBox_KickOKLog.Clear();
                if (Globals.ReverseOrder)
                {
                    AppendKickOKLog($"状态: {info.State}\n");
                    AppendKickOKLog($"踢出理由: {info.Reason}");
                    AppendKickOKLog($"数字ID: {info.PersonaId}");
                    AppendKickOKLog($"玩家ID: {info.Name}");
                    AppendKickOKLog($"等级: {info.Rank}");
                    AppendKickOKLog($"操作时间: {DateTime.Now}");

                }
                else
                {
                    AppendKickOKLog($"操作时间: {DateTime.Now}");
                    AppendKickOKLog($"等级: {info.Rank}");
                    AppendKickOKLog($"玩家ID: {info.Name}");
                    AppendKickOKLog($"数字ID: {info.PersonaId}");
                    AppendKickOKLog($"踢出理由: {info.Reason}");
                    AppendKickOKLog($"状态: {info.State}\n");
                }


                SQLiteHelper.AddLog("kick_ok", new()
                {
                    Rank = info.Rank,
                    Name = info.Name,
                    PersonaId = info.PersonaId,
                    Type = "自动踢人",
                    Message1 = info.Reason,
                    Message2 = info.State,
                    Message3 = ""
                });
            });
            if (Globals.IsCloudMode)
            {
                var _ = CloudApi.KickHIstory(Operator: Globals.PersonaId.ToString(), KickedOutPlayerRank: info.Rank.ToString(), KickedOutPlayerName: info.Name, KickedOutPersonaId: info.PersonaId.ToString(), Reason: info.Reason, State: info.State, ServerId: Globals.ServerId.ToString(), Guid: Globals.PersistedGameId, GameId: Globals.GameId.ToString(), Type: "Success");
            }
            AddKickCDInfo(info);
        }
    }

    /// <summary>
    /// 追加踢人失败日志
    /// </summary>
    /// <param name="info"></param>
    private void AddKickNOLog(AutoKickInfo info)
    {
        lock (this)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                if (TextBox_KickNOLog.LineCount >= 1000)
                    TextBox_KickNOLog.Clear();
                if (Globals.ReverseOrder)
                {
                    AppendKickNOLog($"状态: {info.State}\n");
                    AppendKickNOLog($"踢出理由: {info.Reason}");
                    AppendKickNOLog($"数字ID: {info.PersonaId}");
                    AppendKickNOLog($"玩家ID: {info.Name}");
                    AppendKickNOLog($"等级: {info.Rank}");
                    AppendKickNOLog($"操作时间: {DateTime.Now}");

                }
                else
                {
                    AppendKickNOLog($"操作时间: {DateTime.Now}");
                    AppendKickNOLog($"等级: {info.Rank}");
                    AppendKickNOLog($"玩家ID: {info.Name}");
                    AppendKickNOLog($"数字ID: {info.PersonaId}");
                    AppendKickNOLog($"踢出理由: {info.Reason}");
                    AppendKickNOLog($"状态: {info.State}\n");
                }


                SQLiteHelper.AddLog("kick_no", new()
                {
                    Rank = info.Rank,
                    Name = info.Name,
                    PersonaId = info.PersonaId,
                    Type = "自动踢人",
                    Message1 = info.Reason,
                    Message2 = info.State,
                    Message3 = ""
                });
            });
            if (Globals.IsCloudMode)
            { 
                var _ = CloudApi.KickHIstory(Operator: Globals.PersonaId.ToString(), KickedOutPlayerRank: info.Rank.ToString(), KickedOutPlayerName: info.Name, KickedOutPersonaId: info.PersonaId.ToString(), Reason: info.Reason, State: info.State, ServerId: Globals.ServerId.ToString(), Guid: Globals.PersistedGameId, GameId: Globals.GameId.ToString(), Type: "Fail");
            }
        }
    }

    /// <summary>
    /// 追加更换队伍日志
    /// </summary>
    /// <param name="info"></param>
    private void AddChangeTeamInfoLog(ChangeTeamInfo info)
    {
        lock (this)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                if (TextBox_ChangeTeamLog.LineCount >= 1000)
                    TextBox_ChangeTeamLog.Clear();
                if (Globals.ReverseOrder)
                {
                    AppendChangeTeamLog($"状态: {info.State}\n");
                    AppendChangeTeamLog($"当前地图: {info.GameMode} - {info.MapName}");
                    AppendChangeTeamLog($"数字ID: {info.PersonaId}");
                    AppendChangeTeamLog($"玩家ID: {info.Name}");
                    AppendChangeTeamLog($"等级: {info.Rank}");
                    AppendChangeTeamLog($"操作时间: {DateTime.Now}");

                }
                else
                {
                    AppendChangeTeamLog($"操作时间: {DateTime.Now}");
                    AppendChangeTeamLog($"等级: {info.Rank}");
                    AppendChangeTeamLog($"玩家ID: {info.Name}");
                    AppendChangeTeamLog($"数字ID: {info.PersonaId}");
                    AppendChangeTeamLog($"当前地图: {info.GameMode} - {info.MapName}");
                    AppendChangeTeamLog($"状态: {info.State}\n");

                }

                SQLiteHelper.AddLog("change_team", new()
                {
                    Rank = info.Rank,
                    Name = info.Name,
                    PersonaId = info.PersonaId,
                    Type = "更换队伍",
                    Message1 = $"{info.GameMode} - {info.MapName}",
                    Message2 = info.State,
                    Message3 = ""
                });
            });
            ChatView.ActionChangeTeamNotice(info);
            RobotView.ActionSendChangeTeamLogToQQ(info);
            if (Globals.IsCloudMode)
            {
            var _ = CloudApi.PushToggleHistory(PlayerRank: info.Rank.ToString(), PlayerName: info.Name, PersonaId: info.PersonaId.ToString(), GameMode: info.GameMode,MapName: info.MapName,Team1Name: info.Team1Name, Team2Name: info.Team2Name, State: info.State, ServerId: Globals.ServerId.ToString(), Guid: Globals.PersistedGameId, GameId: Globals.GameId.ToString());
            }
        }

    }

    /////////////////////////////////////////////////////

    /// <summary>
    /// UI添加手动操作日志
    /// </summary>
    /// <param name="log"></param>
    private void AppendScoreKickLog(string log)
    {

        if (Globals.ReverseOrder)
        {
            TextBox_ScoreKickLog.Text = $"{log}\r\n{TextBox_ScoreKickLog.Text}";

        }
        else
        {
            TextBox_ScoreKickLog.AppendText($"{log}\n");

        }
    }

    /// <summary>
    /// UI添加踢人成功日志
    /// </summary>
    /// <param name="log"></param>
    private void AppendKickOKLog(string log)
    {
        if (Globals.ReverseOrder)
        {
            TextBox_KickOKLog.Text = $"{log}\r\n{TextBox_KickOKLog.Text}";
        }
        else
        {
            TextBox_KickOKLog.AppendText($"{log}\n");

        }
    }

    /// <summary>
    /// UI添加踢人失败日志
    /// </summary>
    /// <param name="log"></param>
    private void AppendKickNOLog(string log)
    {
        if (Globals.ReverseOrder)
        {
            TextBox_KickNOLog.Text = $"{log}\r\n{TextBox_KickNOLog.Text}";

        }
        else
        {
            TextBox_KickNOLog.AppendText($"{log}\n");
        }
    }

    /// <summary>
    /// UI添加换边日志
    /// </summary>
    /// <param name="log"></param>
    private void AppendChangeTeamLog(string log)
    {
        if (Globals.ReverseOrder)
        {
            TextBox_ChangeTeamLog.Text = $"{log}\r\n{TextBox_ChangeTeamLog.Text}";

        }
        else
        {
            TextBox_ChangeTeamLog.AppendText($"{log}\n");

        }

    }

    /////////////////////////////////////////////////////

    private void MenuItem_ClearScoreKickLog_Click(object sender, RoutedEventArgs e)
    {
        TextBox_ScoreKickLog.Clear();
        NotifierHelper.Show(NotifierType.Success, "清空手动操作日志成功");
    }

    private void MenuItem_ClearKickOKLog_Click(object sender, RoutedEventArgs e)
    {
        TextBox_KickOKLog.Clear();
        NotifierHelper.Show(NotifierType.Success, "清空踢人成功日志成功");
    }

    private void MenuItem_ClearKickNOLog_Click(object sender, RoutedEventArgs e)
    {
        TextBox_KickNOLog.Clear();
        NotifierHelper.Show(NotifierType.Success, "清空踢人失败日志成功");
    }

    private void MenuItem_ClearChangeTeamLog_Click(object sender, RoutedEventArgs e)
    {
        TextBox_ChangeTeamLog.Clear();
        NotifierHelper.Show(NotifierType.Success, "清空更换队伍日志成功");
    }
}
