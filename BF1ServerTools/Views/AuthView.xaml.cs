﻿using BF1ServerTools.API;
using BF1ServerTools.API.RespJson;
using BF1ServerTools.SDK;
using BF1ServerTools.Utils;
using BF1ServerTools.Models;
using BF1ServerTools.Helper;
using BF1ServerTools.Configs;
using BF1ServerTools.Windows;

using Microsoft.Web.WebView2.Core;
using CommunityToolkit.Mvvm.Messaging;
using System.Xml.Linq;
using System;

namespace BF1ServerTools.Views;

/// <summary>
/// AuthView.xaml 的交互逻辑
/// </summary>
public partial class AuthView : UserControl
{
    /// <summary>
    /// 数据模型绑定
    /// </summary>
    public AuthModel AuthModel { get; set; } = new();

    /// <summary>
    /// 配置文件路径
    /// </summary>
    private readonly string F_Auth_Path = FileUtil.D_Config_Path + @"\AuthConfig.json";

    /// <summary>
    /// 配置文件，以json格式保存到本地
    /// </summary>
    private AuthConfig AuthConfig = new();

    /// <summary>
    /// 配置文件名称动态集合
    /// </summary>
    public ObservableCollection<string> ConfigNames { get; set; } = new();
    public AuthView()
    {
        InitializeComponent();
        this.DataContext = this;
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

        #region 配置文件
        // 如果配置文件不存在就创建
        if (!File.Exists(F_Auth_Path))
        {
            AuthConfig.IsUseMode1 = true;
            AuthConfig.SelectedIndex = 0;
            AuthConfig.ReverseOrder = false;
            AuthConfig.CurrentBF1SpartaGatewayProxyAddr = "https://sparta-gw.battlelog.com";
            AuthConfig.AutoApplyRuleOffline = false;
            AuthConfig.AutoApplyRuleOnline = false;
            AuthConfig.AuthInfos = new();
            AuthConfig.BF1SpartaGatewayProxyAddr = new();
            // 初始化10个配置文件槽
            for (int i = 0; i < 10; i++)
            {
                AuthConfig.AuthInfos.Add(new()
                {
                    Avatar2 = "",
                    DisplayName2 = $"配置槽名称 {i + 1}",
                    PersonaId2 = 0,
                    Sid = "",
                    Remid = "",
                    AccessToken = "",
                    SessionId2 = ""
                });
            }
            // 保存配置文件
            SaveConfig();
        }

        // 如果配置文件存在就读取
        if (File.Exists(F_Auth_Path))
        {
            using StreamReader streamReader = new(F_Auth_Path);
            AuthConfig = JsonHelper.JsonDese<AuthConfig>(streamReader.ReadToEnd());
            // 读取配置文件名称
            foreach (var item in AuthConfig.AuthInfos)
                ConfigNames.Add(item.DisplayName2);
            // 读取选中配置文件索引
            ComboBox_ConfigNames.SelectedIndex = AuthConfig.SelectedIndex;
        }
        #endregion
        /////////////////////////////////////////////////////////////////////

        // 用于接收WebView2传回的数据
        WeakReferenceMessenger.Default.Register<string, string>(this, "SendRemidSid", (s, e) =>
        {
            this.Dispatcher.Invoke(() =>
            {
                AuthModel.Avatar2 = Globals.Avatar2;
                AuthModel.DisplayName2 = Globals.DisplayName2;
                AuthModel.PersonaId2 = Globals.PersonaId2;

                AuthModel.Sid = Globals.Sid;
                AuthModel.Remid = Globals.Remid;
                AuthModel.AccessToken = Globals.AccessToken;
                AuthModel.SessionId2 = Globals.SessionId2;

                SaveConfig();
            });
        });

        // 模式1 定时内存扫描SessionId 周期5分钟
        var AutoRefreshTimerModel1 = new Timer
        {
            AutoReset = true,
            Interval = TimeSpan.FromMinutes(5).TotalMilliseconds
        };
        AutoRefreshTimerModel1.Elapsed += AutoRefreshTimerModel1_Elapsed;
        AutoRefreshTimerModel1.Start();

        // 模式2 定时更新玩家Cookies 周期30分钟
        var AutoRefreshTimerModel2 = new Timer
        {
            AutoReset = true,
            Interval = TimeSpan.FromMinutes(30).TotalMilliseconds
        };
        AutoRefreshTimerModel2.Elapsed += AutoRefreshTimerModel2_Elapsed;
        AutoRefreshTimerModel2.Start();

        ////////////////////////////////////////////

        if (Globals.IsUseMode1)
            AutoRefreshTimerModel1_Elapsed(null, null);
        else
            AutoRefreshTimerModel2_Elapsed(null, null);
        SelectdEAGatewayAddrProxy.Text = AuthConfig.CurrentBF1SpartaGatewayProxyAddr;
        int index = AuthConfig.BF1SpartaGatewayProxyAddr.FindIndex(Item => Item == "https://sparta-gw.battlelog.com");
        if (index == -1)
        {
            AuthConfig.BF1SpartaGatewayProxyAddr.Add("https://sparta-gw.battlelog.com");
            AuthConfig.CurrentBF1SpartaGatewayProxyAddr = "https://sparta-gw.battlelog.com";
            SaveConfig();
        }
        SelectdEAGatewayAddrProxy.ItemsSource = AuthConfig.BF1SpartaGatewayProxyAddr;
        SelectdEAGatewayAddrProxy.SelectedItem = AuthConfig.CurrentBF1SpartaGatewayProxyAddr;
        Globals.BF1SpartaGateWayAddrProxy = AuthConfig.CurrentBF1SpartaGatewayProxyAddr;

        Globals.AutoApplyRuleOffline = AuthConfig.AutoApplyRuleOffline;
        Globals.AutoApplyRuleOnline = AuthConfig.AutoApplyRuleOnline;

        if (Globals.AutoApplyRuleOffline || Globals.AutoApplyRuleOnline)
        {
            Globals.AutoApplyRule = true;
            RefreshAuthInfo();
        }
    }

    private void Button_AutoAutoAndApplyModeSwitch(object sender, RoutedEventArgs e)
    {
        if (!Globals.AutoApplyRuleOnline && !Globals.AutoApplyRuleOffline)
        {
            Globals.AutoApplyRuleOnline = true;
            NotifierHelper.Show(NotifierType.Success, "开启自动验证SessionId并自动设置规则为在线规则");
            AuthConfig.AutoApplyRuleOnline = true;
            SaveConfig();
            return;
        }

        if (Globals.AutoApplyRuleOnline)
        {
            Globals.AutoApplyRuleOffline = true;
            Globals.AutoApplyRuleOnline = false;
            NotifierHelper.Show(NotifierType.Success, "开启自动验证SessionId并自动设置规则为离线规则");
            AuthConfig.AutoApplyRuleOnline = false;
            AuthConfig.AutoApplyRuleOffline = true;

            SaveConfig();

            return;
        }

        if (Globals.AutoApplyRuleOffline)
        {
            Globals.AutoApplyRuleOffline = false;
            Globals.AutoApplyRuleOnline = false;
            NotifierHelper.Show(NotifierType.Success, "关闭自动验证SessionId与自动设置规则");
            AuthConfig.AutoApplyRuleOnline = false;
            AuthConfig.AutoApplyRuleOffline = false;
            SaveConfig();

            return;
        }


    }

    private async void AutoAuthSession()
    {
        if (Globals.AutoApplyRuleOffline)
        {
            if (string.IsNullOrEmpty(Globals.SessionId))
            {
                NotifierHelper.Show(NotifierType.Warning, "玩家SessionId为空，请先获取玩家SessionId");
                return;
            }

            TextBlock_SessionIdState.Text = "正在验证中，请稍后...";
            Border_SessionIdState.Background = Brushes.Gray;
            NotifierHelper.Show(NotifierType.Information, "正在验证中，请稍后...");

            _ = BF1API.SetAPILocale(Globals.SessionId);

            var result = await BF1API.GetWelcomeMessage(Globals.SessionId);
            if (result.IsSuccess)
            {
                var welcomeMsg = JsonHelper.JsonDese<WelcomeMsg>(result.Content);
                var firstMessage = ChsUtil.ToSimplified(welcomeMsg.result.firstMessage);

                TextBlock_SessionIdState.Text = firstMessage;
                Border_SessionIdState.Background = Brushes.Green;

                if (Globals.IsCloudMode)
                {
                    var ress = await CloudApi.RemoveAllToggleTeambeForeKick(Globals.TempGameId.ToString());
                    if (!ress.IsSuccess)
                    {
                        if (Globals.AlreadyToggleTeamPlayer.Count != 0)
                        {
                            if (Globals.AlreadyToggleTeamPlayer.Count != 0)
                            {
                                Globals.AlreadyToggleTeamPlayer.Clear();
                            }
                            if (Globals.TempToggleTeamList.Count != 0)
                            {
                                Globals.TempToggleTeamList.Clear();
                            }
                            if (Globals.AllowTempAloowToggleTeamList1.Count != 0)
                            {
                                Globals.AllowTempAloowToggleTeamList1.Clear();
                            }
                            if (Globals.AllowTempAloowToggleTeamList2.Count != 0)
                            {
                                Globals.AllowTempAloowToggleTeamList2.Clear();
                            }
                        }
                    }
                }
                Globals.IsAuth = true;
                NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  验证成功\n{firstMessage}");
                if (Globals.IsCloudMode)
                {
                    Globals.IsCloudMode = false;
                    NotifierHelper.Show(NotifierType.Success, "已切回离线模式");
                }
            }
            else
            {
                TextBlock_SessionIdState.Text = "验证失败";
                Border_SessionIdState.Background = Brushes.OrangeRed;
                NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  验证失败\n{result.Content}");
            }
        }

        if (Globals.AutoApplyRuleOnline)
        {
            if (string.IsNullOrEmpty(Globals.SessionId))
            {
                NotifierHelper.Show(NotifierType.Warning, "玩家SessionId为空，请先获取玩家SessionId");
                return;
            }

            TextBlock_SessionIdState.Text = "正在验证中，请稍后...";
            Border_SessionIdState.Background = Brushes.Gray;
            NotifierHelper.Show(NotifierType.Information, "正在验证中，请稍后...");

            _ = BF1API.SetAPILocale(Globals.SessionId);

            var result = await BF1API.GetWelcomeMessage(Globals.SessionId);
            if (result.IsSuccess)
            {
                var welcomeMsg = JsonHelper.JsonDese<WelcomeMsg>(result.Content);
                var firstMessage = ChsUtil.ToSimplified(welcomeMsg.result.firstMessage);

                TextBlock_SessionIdState.Text = firstMessage;
                Border_SessionIdState.Background = Brushes.Green;

                if (!Globals.IsCloudMode)
                {
                    if (Globals.AlreadyToggleTeamPlayer.Count != 0)
                    {
                        Globals.AlreadyToggleTeamPlayer.Clear();
                    }
                    if (Globals.TempToggleTeamList.Count != 0)
                    {
                        Globals.TempToggleTeamList.Clear();
                    }
                    if (Globals.AllowTempAloowToggleTeamList1.Count != 0)
                    {
                        Globals.AllowTempAloowToggleTeamList1.Clear();
                    }
                    if (Globals.AllowTempAloowToggleTeamList2.Count != 0)
                    {
                        Globals.AllowTempAloowToggleTeamList2.Clear();
                    }
                }

                var result1 = await CloudApi.CheckAlive();
                if (result1.IsSuccess)
                {
                    Globals.IsAuth = true;
                    Globals.IsCloudMode = true;
                    Globals.ISetRule = false;
                    NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  验证成功\n{firstMessage}在线模式");

                }
                else
                {
                    Globals.IsCloudMode = false;
                    NotifierHelper.Show(NotifierType.Warning, $"[{result.ExecTime:0.00} 秒]  验证成功\n{firstMessage}服务器无响应回退到离线模式");
                }
            }
            else
            {
                TextBlock_SessionIdState.Text = "验证失败";
                Border_SessionIdState.Background = Brushes.OrangeRed;
                NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  验证失败\n{result.Content}");
            }
        }
    }

    /// <summary>
    /// 主窗口关闭事件
    /// </summary>
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
        var index = ComboBox_ConfigNames.SelectedIndex;
        if (index != -1)
        {
            AuthConfig.IsUseMode1 = Globals.IsUseMode1;
            AuthConfig.SelectedIndex = index;

            var auth = AuthConfig.AuthInfos[index];

            auth.Avatar2 = Globals.Avatar2;
            auth.DisplayName2 = Globals.DisplayName2;
            auth.PersonaId2 = Globals.PersonaId2;
            auth.Sid = Globals.Sid;
            auth.Remid = Globals.Remid;
            auth.AccessToken = Globals.AccessToken;
            auth.SessionId2 = Globals.SessionId2;
        }
        // 写入到Json文件
        File.WriteAllText(F_Auth_Path, JsonHelper.JsonSeri(AuthConfig));
    }


    /// <summary>
    /// ComboBox选中项变更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ComboBox_ConfigNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var index = ComboBox_ConfigNames.SelectedIndex;
        if (index == -1)
            return;

        Globals.IsUseMode1 = AuthConfig.IsUseMode1;

        if (Globals.IsUseMode1)
            RadioButton_Mode1.IsChecked = true;
        else
            RadioButton_Mode2.IsChecked = true;

        ///////////////////////////////////////////

        var auth = AuthConfig.AuthInfos[index];

        AuthModel.Avatar2 = auth.Avatar2;
        AuthModel.DisplayName2 = auth.DisplayName2;
        AuthModel.PersonaId2 = auth.PersonaId2;
        AuthModel.Sid = auth.Sid;
        AuthModel.Remid = auth.Remid;
        AuthModel.AccessToken = auth.AccessToken;
        AuthModel.SessionId2 = auth.SessionId2;

        Globals.Avatar2 = auth.Avatar2;
        Globals.DisplayName2 = auth.DisplayName2;
        Globals.PersonaId2 = auth.PersonaId2;
        Globals.Sid = auth.Sid;
        Globals.Remid = auth.Remid;
        Globals.AccessToken = auth.AccessToken;
        Globals.SessionId2 = auth.SessionId2;

        SaveConfig();
    }

    /// <summary>
    /// 保存配置文件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_SaveConfig_Click(object sender, RoutedEventArgs e)
    {
        SaveConfig();

        var index = ComboBox_ConfigNames.SelectedIndex;
        if (index != -1)
        {
            ConfigNames[index] = Globals.DisplayName2;
            ComboBox_ConfigNames.SelectedIndex = index;
        }

        NotifierHelper.Show(NotifierType.Success, "保存配置文件成功");
    }

    ////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 模式1 定时内存扫描SessionId 周期5分钟
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private async void AutoRefreshTimerModel1_Elapsed(object sender, ElapsedEventArgs e)
    {
        if (!Globals.IsUseMode1)
            return;

        var sessionId = await Scan.GetGatewaySession();
        if (sessionId != string.Empty)
        {
            Globals.SessionId1 = sessionId;
            LoggerHelper.Info($"内存扫描SessionId成功 {Globals.SessionId}");
        }
        else
        {
            LoggerHelper.Error("内存扫描SessionID失败");
        }
    }

    /// <summary>
    /// 模式2 定时更新玩家Cookies 周期30分钟
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void AutoRefreshTimerModel2_Elapsed(object sender, ElapsedEventArgs e)
    {
        if (Globals.IsUseMode1)
            return;

        if (string.IsNullOrEmpty(Globals.Remid) || string.IsNullOrEmpty(Globals.Sid))
            return;

        var respAuth = await EA1API.GetAuthCode(Globals.Remid, Globals.Sid);
        if (respAuth.IsSuccess)
        {
            if (!string.IsNullOrEmpty(respAuth.Remid))
                Globals.Remid = respAuth.Remid;
            if (!string.IsNullOrEmpty(respAuth.Sid))
                Globals.Sid = respAuth.Sid;

            var result = await EA2API.GetAccessToken(Globals.Remid, Globals.Sid);
            if (result.IsSuccess)
            {
                var jNode = JsonNode.Parse(result.Content);
                Globals.AccessToken = jNode["access_token"].GetValue<string>();
                AuthModel.AccessToken = Globals.AccessToken;
                LoggerHelper.Info("刷新玩家access_token成功");
            }

            result = await BF1API.GetEnvIdViaAuthCode(respAuth.Code);
            if (result.IsSuccess)
            {
                var envIdViaAuthCode = JsonHelper.JsonDese<EnvIdViaAuthCode>(result.Content);
                Globals.SessionId2 = envIdViaAuthCode.result.sessionId;
                Globals.PersonaId2 = long.Parse(envIdViaAuthCode.result.personaId);

                result = await BF1API.GetPersonasByIds(Globals.SessionId2, Globals.PersonaId);
                if (result.IsSuccess)
                {
                    var jNode = JsonNode.Parse(result.Content);
                    var personas = jNode["result"]![$"{Globals.PersonaId}"];
                    if (personas != null)
                    {
                        Globals.Avatar2 = personas!["avatar"].GetValue<string>();
                        Globals.DisplayName2 = personas!["displayName"].GetValue<string>();

                        AuthModel.Avatar2 = Globals.Avatar2;
                        AuthModel.DisplayName2 = Globals.DisplayName2;
                        AuthModel.PersonaId2 = Globals.PersonaId2;

                        AuthModel.Sid = Globals.Sid;
                        AuthModel.Remid = Globals.Remid;
                        AuthModel.SessionId2 = Globals.SessionId2;

                        LoggerHelper.Info("刷新玩家Cookies数据成功");
                    }
                }
            }
        }
    }

    /// <summary>
    /// 选择工作模式事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RadioButton_Mode12_Click(object sender, RoutedEventArgs e)
    {
        Globals.IsUseMode1 = RadioButton_Mode1.IsChecked == true;
    }

    /// <summary>
    /// 获取玩家Cookies数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_GetPlayerCookies_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(CoreWebView2Environment.GetAvailableBrowserVersionString()))
        {
            NotifierHelper.Show(NotifierType.Warning, "未检测到WebView2对应依赖，请安装对应依赖");
            return;
        }

        var webView2Window = new WebView2Window()
        {
            Owner = MainWindow.MainWindowInstance
        };
        webView2Window.ShowDialog();
    }

    /// <summary>
    /// 刷新玩家Cookies数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_RefreshAuthInfo_Click(object sender, RoutedEventArgs e)
    {
        if (Globals.IsUseMode1)
        {
            NotifierHelper.Show(NotifierType.Information, "正在内存扫描中，请稍后...");

            var sessionId = await Scan.GetGatewaySession();
            if (sessionId != string.Empty)
            {
                Globals.SessionId1 = sessionId;
                NotifierHelper.Show(NotifierType.Information, $"内存扫描SessionId成功 {Globals.SessionId1}");
            }
            else
            {
                NotifierHelper.Show(NotifierType.Information, "内存扫描SessionId失败");
            }
        }
        else
        {
            if (string.IsNullOrEmpty(Globals.Remid) || string.IsNullOrEmpty(Globals.Sid))
            {
                NotifierHelper.Show(NotifierType.Warning, "玩家Remid或Sid为空，请先获取玩家Cookies");
                return;
            }

            NotifierHelper.Show(NotifierType.Information, "正在刷新中，请稍后...");

            var respAuth = await EA1API.GetAuthCode(Globals.Remid, Globals.Sid);
            if (respAuth.IsSuccess)
            {
                if (!string.IsNullOrEmpty(respAuth.Remid))
                    Globals.Remid = respAuth.Remid;
                if (!string.IsNullOrEmpty(respAuth.Sid))
                    Globals.Sid = respAuth.Sid;

                var result = await EA2API.GetAccessToken(Globals.Remid, Globals.Sid);
                if (result.IsSuccess)
                {
                    var jNode = JsonNode.Parse(result.Content);
                    Globals.AccessToken = jNode["access_token"].GetValue<string>();
                    AuthModel.AccessToken = Globals.AccessToken;
                    NotifierHelper.Show(NotifierType.Success, "刷新玩家access_token成功");
                }

                result = await BF1API.GetEnvIdViaAuthCode(respAuth.Code);
                if (result.IsSuccess)
                {
                    var envIdViaAuthCode = JsonHelper.JsonDese<EnvIdViaAuthCode>(result.Content);
                    Globals.SessionId2 = envIdViaAuthCode.result.sessionId;
                    Globals.PersonaId2 = long.Parse(envIdViaAuthCode.result.personaId);

                    result = await BF1API.GetPersonasByIds(Globals.SessionId2, Globals.PersonaId);
                    if (result.IsSuccess)
                    {
                        var jNode = JsonNode.Parse(result.Content);
                        var personas = jNode["result"]![$"{Globals.PersonaId}"];
                        if (personas != null)
                        {
                            Globals.Avatar2 = personas!["avatar"].GetValue<string>();
                            Globals.DisplayName2 = personas!["displayName"].GetValue<string>();

                            AuthModel.Avatar2 = Globals.Avatar2;
                            AuthModel.DisplayName2 = Globals.DisplayName2;
                            AuthModel.PersonaId2 = Globals.PersonaId2;

                            AuthModel.Sid = Globals.Sid;
                            AuthModel.Remid = Globals.Remid;
                            AuthModel.SessionId2 = Globals.SessionId2;

                            NotifierHelper.Show(NotifierType.Success, "刷新玩家Cookies数据成功");
                        }
                    }
                    else
                    {
                        NotifierHelper.Show(NotifierType.Error, $"刷新失败\n{result.Content}");
                    }
                }
                else
                {
                    NotifierHelper.Show(NotifierType.Error, $"刷新失败\n{result.Content}");
                }
            }
            else
            {
                NotifierHelper.Show(NotifierType.Error, "刷新失败，玩家Remid或Sid可能已过期");
            }
        }
    }



    private async void RefreshAuthInfo()
    {
        if (Globals.IsUseMode1)
        {
            NotifierHelper.Show(NotifierType.Information, "正在内存扫描中，请稍后...");

            var sessionId = await Scan.GetGatewaySession();
            if (sessionId != string.Empty)
            {
                Globals.SessionId1 = sessionId;
                NotifierHelper.Show(NotifierType.Information, $"内存扫描SessionId成功 {Globals.SessionId1}");
            }
            else
            {
                NotifierHelper.Show(NotifierType.Information, "内存扫描SessionId失败");
            }
        }
        else
        {
            if (string.IsNullOrEmpty(Globals.Remid) || string.IsNullOrEmpty(Globals.Sid))
            {
                NotifierHelper.Show(NotifierType.Warning, "玩家Remid或Sid为空，请先获取玩家Cookies");
                return;
            }

            NotifierHelper.Show(NotifierType.Information, "正在刷新中，请稍后...");

            var respAuth = await EA1API.GetAuthCode(Globals.Remid, Globals.Sid);
            if (respAuth.IsSuccess)
            {
                if (!string.IsNullOrEmpty(respAuth.Remid))
                    Globals.Remid = respAuth.Remid;
                if (!string.IsNullOrEmpty(respAuth.Sid))
                    Globals.Sid = respAuth.Sid;

                var result = await EA2API.GetAccessToken(Globals.Remid, Globals.Sid);
                if (result.IsSuccess)
                {
                    var jNode = JsonNode.Parse(result.Content);
                    Globals.AccessToken = jNode["access_token"].GetValue<string>();
                    AuthModel.AccessToken = Globals.AccessToken;
                    NotifierHelper.Show(NotifierType.Success, "刷新玩家access_token成功");
                }

                result = await BF1API.GetEnvIdViaAuthCode(respAuth.Code);
                if (result.IsSuccess)
                {
                    var envIdViaAuthCode = JsonHelper.JsonDese<EnvIdViaAuthCode>(result.Content);
                    Globals.SessionId2 = envIdViaAuthCode.result.sessionId;
                    Globals.PersonaId2 = long.Parse(envIdViaAuthCode.result.personaId);

                    result = await BF1API.GetPersonasByIds(Globals.SessionId2, Globals.PersonaId);
                    if (result.IsSuccess)
                    {
                        var jNode = JsonNode.Parse(result.Content);
                        var personas = jNode["result"]![$"{Globals.PersonaId}"];
                        if (personas != null)
                        {
                            Globals.Avatar2 = personas!["avatar"].GetValue<string>();
                            Globals.DisplayName2 = personas!["displayName"].GetValue<string>();

                            AuthModel.Avatar2 = Globals.Avatar2;
                            AuthModel.DisplayName2 = Globals.DisplayName2;
                            AuthModel.PersonaId2 = Globals.PersonaId2;

                            AuthModel.Sid = Globals.Sid;
                            AuthModel.Remid = Globals.Remid;
                            AuthModel.SessionId2 = Globals.SessionId2;

                            NotifierHelper.Show(NotifierType.Success, "刷新玩家Cookies数据成功");
                        }
                    }
                    else
                    {
                        NotifierHelper.Show(NotifierType.Error, $"刷新失败\n{result.Content}");
                    }
                }
                else
                {
                    NotifierHelper.Show(NotifierType.Error, $"刷新失败\n{result.Content}");
                }
            }
            else
            {
                NotifierHelper.Show(NotifierType.Error, "刷新失败，玩家Remid或Sid可能已过期");
            }
        }
        AutoAuthSession();
    }


    /// <summary>
    /// 验证玩家SessionId有效性，默认离线版本
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_VerifySessionId_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(Globals.SessionId))
        {
            NotifierHelper.Show(NotifierType.Warning, "玩家SessionId为空，请先获取玩家SessionId");
            return;
        }

        TextBlock_SessionIdState.Text = "正在验证中，请稍后...";
        Border_SessionIdState.Background = Brushes.Gray;
        NotifierHelper.Show(NotifierType.Information, "正在验证中，请稍后...");

        _ = BF1API.SetAPILocale(Globals.SessionId);

        var result = await BF1API.GetWelcomeMessage(Globals.SessionId);
        if (result.IsSuccess)
        {
            var welcomeMsg = JsonHelper.JsonDese<WelcomeMsg>(result.Content);
            var firstMessage = ChsUtil.ToSimplified(welcomeMsg.result.firstMessage);

            TextBlock_SessionIdState.Text = firstMessage;
            Border_SessionIdState.Background = Brushes.Green;

            if (Globals.IsCloudMode)
            {
                var ress = await CloudApi.RemoveAllToggleTeambeForeKick(Globals.TempGameId.ToString());
                if (!ress.IsSuccess)
                {
                    if (Globals.AlreadyToggleTeamPlayer.Count != 0)
                    {
                        if (Globals.AlreadyToggleTeamPlayer.Count != 0)
                        {
                            Globals.AlreadyToggleTeamPlayer.Clear();
                        }
                        if (Globals.TempToggleTeamList.Count != 0)
                        {
                            Globals.TempToggleTeamList.Clear();
                        }
                        if (Globals.AllowTempAloowToggleTeamList1.Count != 0)
                        {
                            Globals.AllowTempAloowToggleTeamList1.Clear();
                        }
                        if (Globals.AllowTempAloowToggleTeamList2.Count != 0)
                        {
                            Globals.AllowTempAloowToggleTeamList2.Clear();
                        }
                    }
                }
            }
            Globals.IsAuth = true;
            NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  验证成功\n{firstMessage}");
            if (Globals.IsCloudMode)
            {
                Globals.IsCloudMode = false;
                NotifierHelper.Show(NotifierType.Success, "已切回离线模式");
            }
        }
        else
        {
            TextBlock_SessionIdState.Text = "验证失败";
            Border_SessionIdState.Background = Brushes.OrangeRed;
            NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  验证失败\n{result.Content}");
        }
    }


    /// <summary>
    /// 验证玩家SessionId有效性，在线版本
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_VerifySessionId_Online_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(Globals.SessionId))
        {
            NotifierHelper.Show(NotifierType.Warning, "玩家SessionId为空，请先获取玩家SessionId");
            return;
        }

        TextBlock_SessionIdState.Text = "正在验证中，请稍后...";
        Border_SessionIdState.Background = Brushes.Gray;
        NotifierHelper.Show(NotifierType.Information, "正在验证中，请稍后...");

        _ = BF1API.SetAPILocale(Globals.SessionId);

        var result = await BF1API.GetWelcomeMessage(Globals.SessionId);
        if (result.IsSuccess)
        {
            var welcomeMsg = JsonHelper.JsonDese<WelcomeMsg>(result.Content);
            var firstMessage = ChsUtil.ToSimplified(welcomeMsg.result.firstMessage);

            TextBlock_SessionIdState.Text = firstMessage;
            Border_SessionIdState.Background = Brushes.Green;

            if (!Globals.IsCloudMode)
            {
                if (Globals.AlreadyToggleTeamPlayer.Count != 0)
                {
                    Globals.AlreadyToggleTeamPlayer.Clear();
                }
                if (Globals.TempToggleTeamList.Count != 0)
                {
                    Globals.TempToggleTeamList.Clear();
                }
                if (Globals.AllowTempAloowToggleTeamList1.Count != 0)
                {
                   Globals.AllowTempAloowToggleTeamList1.Clear();
                }
                if (Globals.AllowTempAloowToggleTeamList2.Count != 0)
                {
                    Globals.AllowTempAloowToggleTeamList2.Clear();
                }
            }

            var result1 = await CloudApi.CheckAlive();
            if (result1.IsSuccess)
            {
                Globals.IsCloudMode = true;
                Globals.ISetRule = false;
                Globals.IsAuth = true;
                NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  验证成功\n{firstMessage}在线模式");

            }
            else
            {
                Globals.IsCloudMode = false;
                NotifierHelper.Show(NotifierType.Warning, $"[{result.ExecTime:0.00} 秒]  验证成功\n{firstMessage}服务器无响应回退到离线模式");
            }
        }
        else
        {
            TextBlock_SessionIdState.Text = "验证失败";
            Border_SessionIdState.Background = Brushes.OrangeRed;
            NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  验证失败\n{result.Content}");
        }
    }
    /// <summary>
    /// 网络检测
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_DNS_Click(object sender, RoutedEventArgs e)
    {
        var btnContent = (e.OriginalSource as Button).Content.ToString();

        switch (btnContent)
        {
            case "查询DNS缓存":
                ProcessUtil.RunCMD("ipconfig /displaydns");
                break;
            case "刷新DNS缓存":
                ProcessUtil.RunCMD("ipconfig /flushdns");
                break;
            case "检测EA服务器本地解析":
                ProcessUtil.RunCMD("nslookup accounts.ea.com & nslookup signin.ea.com & nslookup gateway.ea.com & nslookup eaassets-a.akamaihd.net & nslookup sparta-gw.battlelog.com");
                break;
            case "批量Ping检测":
                ProcessUtil.RunCMD("ping accounts.ea.com & ping signin.ea.com & ping gateway.ea.com & ping eaassets-a.akamaihd.net & ping sparta-gw.battlelog.com");
                break;
            case "编辑Host文件":
                ProcessUtil.OpenPath("notepad.exe", @"C:\windows\system32\drivers\etc\hosts");
                break;
        }
    }

    /// <summary>
    /// 打开配置文件夹
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_OpenConfigFolder_Click(object sender, RoutedEventArgs e)
    {
        ProcessUtil.OpenPath(FileUtil.Default_Path);
    }

    /// <summary>
    /// 通用超链接点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Hyperlink_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Hyperlink link)
            ProcessUtil.OpenPath(link.NavigateUri.AbsoluteUri);
    }

    private void Button_ReverseOrder_Click(object sender, RoutedEventArgs e)
    {
        if (Globals.ReverseOrder)
        {
            Globals.ReverseOrder = false;
            NotifierHelper.Show(NotifierType.Success, "正序输出");
            AuthConfig.ReverseOrder = false;
            SaveConfig();
        }
        else
        {
            Globals.ReverseOrder = true;
            NotifierHelper.Show(NotifierType.Success, "反序输出");
            AuthConfig.ReverseOrder = true;
            SaveConfig();
        }
    }

    private void Button_ADD_EAGatewayProxyAddr(object sender, RoutedEventArgs e)
    {
        int index = AuthConfig.BF1SpartaGatewayProxyAddr.FindIndex(Item => Item == EAGatewayProxyAddr.Text.Trim());

        if (index != -1)
        {
            EAGatewayProxyAddr.Clear();
            NotifierHelper.Show(NotifierType.Error, "已存在相同Gateway地址");
        }
        else
        {
            string pattern = @"https?://\S+";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(EAGatewayProxyAddr.Text.Trim());
            if (match.Success)
            {
                AuthConfig.BF1SpartaGatewayProxyAddr.Add(EAGatewayProxyAddr.Text.Trim().TrimEnd('/'));
                SaveConfig();
                EAGatewayProxyAddr.Clear();
                NotifierHelper.Show(NotifierType.Success, "添加Gateway代理地址成功");
                Dispatcher.Invoke(() =>
                {
                    SelectdEAGatewayAddrProxy.ItemsSource = AuthConfig.BF1SpartaGatewayProxyAddr;
                    SelectdEAGatewayAddrProxy.Items.Refresh();

                });
            } else
            {
                NotifierHelper.Show(NotifierType.Error, "输入不合法");
            }
        }

    }

    private void Button_Remove_EAGatewayProxyAddr(object sender, RoutedEventArgs e)
    {
        if (SelectdEAGatewayAddrProxy.Text != "https://sparta-gw.battlelog.com")
        {
            if (Globals.BF1SpartaGateWayAddrProxy == SelectdEAGatewayAddrProxy.Text.Trim())
            {
                Globals.BF1SpartaGateWayAddrProxy = "https://sparta-gw.battlelog.com";
                AuthConfig.CurrentBF1SpartaGatewayProxyAddr = "https://sparta-gw.battlelog.com";
                BF1API.SetBF1BF1SpartaGateWayAddrProxy();
            }
            AuthConfig.BF1SpartaGatewayProxyAddr.Remove(SelectdEAGatewayAddrProxy.Text.Trim());
            SaveConfig();
            NotifierHelper.Show(NotifierType.Success, "删除Sparta Gateway代理地址成功");
            Dispatcher.Invoke(() =>
            {
                SelectdEAGatewayAddrProxy.Text = "https://sparta-gw.battlelog.com";
                SelectdEAGatewayAddrProxy.ItemsSource = AuthConfig.BF1SpartaGatewayProxyAddr;
                SelectdEAGatewayAddrProxy.Items.Refresh();
            });

        }
        else
        {
            NotifierHelper.Show(NotifierType.Error, "默认Sparta Gateway地址不允许删除");

        }
    }
    private void Button_Set_EAGatewayProxyAddr(object sender, RoutedEventArgs e)
    {
        Globals.BF1SpartaGateWayAddrProxy = SelectdEAGatewayAddrProxy.Text.Trim();
        AuthConfig.CurrentBF1SpartaGatewayProxyAddr = SelectdEAGatewayAddrProxy.Text.Trim();
        BF1API.SetBF1BF1SpartaGateWayAddrProxy();
        SaveConfig();
        NotifierHelper.Show(NotifierType.Success, "应用当前Sparta Gateway代理地址");
    }
}
