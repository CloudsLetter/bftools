using BF1ServerTools.API;
using BF1ServerTools.Utils;
using BF1ServerTools.Views;
using BF1ServerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BF1ServerTools.API.RespJson.GetWeapons.ResultItem.WeaponsItem;
using BF1ServerTools.Data;
using Newtonsoft.Json.Bson;
using static System.Windows.Forms.AxHost;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;


public class RepsoneData
{
    public string Id { get; set; }
}


public static class CloudUtil
{
    public static async void AutTogglTeame(ChangeTeamInfo info)
    {

        if (Globals.LoginPlayerIsAdmin && Globals.IsNotAllowToggle && Globals.IsSetRuleOK)
        {
            bool toggleTeam = false;

            if (info.To == 1 && Globals.AllowTempAloowToggleTeamList1.Count != 0)
            {
                toggleTeam = PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.AllowTempAloowToggleTeamList1);
            }

            if (info.To == 2 && Globals.AllowTempAloowToggleTeamList2.Count != 0)
            {
                toggleTeam = PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.AllowTempAloowToggleTeamList2);

            }

            bool whitelistToggle = false;

            if (Globals.ToggleKickMode)
            {
                if (Globals.IsCloudMode)
                {
                    var results = await BF1API.RefreshWhiteList(ServerId: Globals.ServerId.ToString());
                    if (results.IsSuccess)
                    {
                        try
                        {
                            var test = results.Content.Replace("\r", "");
                            List<Players> players = JsonConvert.DeserializeObject<List<Players>>(test);
                            if (PlayerUtil.IsCloudWhite(info.Name, players))
                            {
                                whitelistToggle = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (PlayerUtil.IsWhite(info.Name, Globals.CustomWhites_Name))
                            {
                                whitelistToggle = true;
                            }
                        }
                    }
                    else
                    {
                        if (PlayerUtil.IsWhite(info.Name, Globals.CustomWhites_Name))
                        {
                            whitelistToggle = true;
                        }
                    }

                }
                else
                {
                    if (Globals.IsAllowWhlistToggleTeam)
                    {
                        if (PlayerUtil.IsWhite(info.Name, Globals.CustomWhites_Name))
                        {
                            whitelistToggle = true;
                        }
                    }
                }



                bool IsAdmin = PlayerUtil.IsAdminVIP(info.PersonaId, Globals.ServerAdmins_PID);
                bool isToggle = false;


                if (!IsAdmin && !whitelistToggle)
                {
                    if (Globals.IsCloudMode)
                    {


                        var result3 = await CloudApi.QueryAutoToggleTeamList(info.PersonaId.ToString());
                        if (result3.IsSuccess)
                        {
                            isToggle = true;

                            if (Globals.TempToggleTeamList.Count != 0)
                            {
                                if (PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.TempToggleTeamList))
                                {
                                    Globals.TempToggleTeamList.Remove(info.PersonaId);
                                }

                            }
                        }
                        else
                        {
                            try
                            {
                                var data = result3.Content.Replace("\r", "");
                                RepsoneData dataObj = JsonConvert.DeserializeObject<RepsoneData>(data);
                                if (dataObj.Id != "0002")
                                {
                                    if (Globals.TempToggleTeamList.Count != 0)
                                    {
                                        isToggle = PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.TempToggleTeamList);
                                        if (isToggle)
                                        {
                                            Globals.TempToggleTeamList.Remove(info.PersonaId);
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                if (Globals.TempToggleTeamList.Count != 0)
                                {
                                    isToggle = PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.TempToggleTeamList);
                                    if (isToggle)
                                    {
                                        Globals.TempToggleTeamList.Remove(info.PersonaId);
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        isToggle = PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.TempToggleTeamList);
                        if (isToggle)
                        {
                            Globals.TempToggleTeamList.Remove(info.PersonaId);
                        }
                    }

                }


                if (!toggleTeam && !IsAdmin && !whitelistToggle && !isToggle)
                {
                    var result = await BF1API.RSPKickPlayer(Globals.SessionId, Globals.GameId, info.PersonaId, "BFTools: 禁止跳邊");
                    if (result.IsSuccess)
                    {
                        info.State = $"将 等级:{info.Rank} 名称: {info.Rank}踢出服务器成功";

                    }
                    else
                    {
                        info.State = $"将 等级:{info.Rank} 名称: {info.Rank}踢出服务器失败";

                    }

                    LogView.ActionAddKickOKLog(new AutoKickInfo()
                    {
                        Time = DateTime.Now,
                        Rank = info.Rank,
                        Name = info.Name,
                        PersonaId = info.PersonaId,
                        Flag = KickFlag.Success,
                        State = info.State,
                        Reason = "禁止跳边"
                    });
                    if (Globals.IsCloudMode)
                    {
                        _ = CloudApi.KickHIstory(Operator: Globals.PersonaId.ToString(), KickedOutPlayerRank: info.Rank.ToString(), KickedOutPlayerName: info.Name, KickedOutPersonaId: info.PersonaId.ToString(), Reason: "BFTools: 禁止跳邊", State: info.State, ServerId: Globals.ServerId.ToString(), Guid: Globals.PersistedGameId, GameId: Globals.GameId.ToString(), Type: "AutoKickToggle");

                    }

                }

                if (info.To == 2 && Globals.AllowTempAloowToggleTeamList2.Count != 0 && toggleTeam)
                {
                    if (PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.AllowTempAloowToggleTeamList2))
                    {
                        Globals.AllowTempAloowToggleTeamList2.Remove(info.PersonaId);
                    }

                }

                if (info.To == 1 && Globals.AllowTempAloowToggleTeamList1.Count != 0 && toggleTeam)
                {
                    if (PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.AllowTempAloowToggleTeamList1))
                    {
                        Globals.AllowTempAloowToggleTeamList1.Remove(info.PersonaId);
                    }

                }
            }
            else
            {
                if (Globals.IsCloudMode)
                {
                    var results = await BF1API.RefreshWhiteList(ServerId: Globals.ServerId.ToString());
                    if (results.IsSuccess)
                    {
                        try
                        {
                            var test = results.Content.Replace("\r", "");
                            List<Players> players = JsonConvert.DeserializeObject<List<Players>>(test);
                            if (PlayerUtil.IsCloudWhite(info.Name, players))
                            {
                                whitelistToggle = true;
                            }
                        }
                        catch (Exception _)
                        {
                            if (PlayerUtil.IsWhite(info.Name, Globals.CustomWhites_Name))
                            {
                                whitelistToggle = true;
                            }
                        }
                    }
                    else
                    {
                        if (PlayerUtil.IsWhite(info.Name, Globals.CustomWhites_Name))
                        {
                            whitelistToggle = true;
                        }
                    }

                }
                else
                {
                    if (Globals.IsAllowWhlistToggleTeam)
                    {
                        if (PlayerUtil.IsWhite(info.Name, Globals.CustomWhites_Name))
                        {
                            whitelistToggle = true;
                        }
                    }
                }



                bool IsAdmin = PlayerUtil.IsAdminVIP(info.PersonaId, Globals.ServerAdmins_PID);
                bool isToggle = false;


                if (!IsAdmin && !whitelistToggle)
                {
                    if (Globals.IsCloudMode)
                    {


                        var result3 = await CloudApi.QueryAutoToggleTeamList(info.PersonaId.ToString());
                        if (result3.IsSuccess)
                        {
                            isToggle = true;

                            if (Globals.TempToggleTeamList.Count != 0)
                            {
                                if (PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.TempToggleTeamList))
                                {
                                    Globals.TempToggleTeamList.Remove(info.PersonaId);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                var data = result3.Content.Replace("\r", "");
                                RepsoneData dataObj = JsonConvert.DeserializeObject<RepsoneData>(data);
                                if (dataObj.Id != "0002")
                                {
                                    if (Globals.TempToggleTeamList.Count != 0)
                                    {
                                        isToggle = PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.TempToggleTeamList);
                                        if (isToggle)
                                        {
                                            Globals.TempToggleTeamList.Remove(info.PersonaId);
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                if (Globals.TempToggleTeamList.Count != 0)
                                {
                                    isToggle = PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.TempToggleTeamList);
                                    if (isToggle)
                                    {
                                        Globals.TempToggleTeamList.Remove(info.PersonaId);
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        isToggle = PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.TempToggleTeamList);
                        if (isToggle)
                        {
                            Globals.TempToggleTeamList.Remove(info.PersonaId);
                        }
                    }

                }


                if (!toggleTeam && !IsAdmin && !whitelistToggle && !isToggle)
                {
                    var result = await BF1API.RSPMovePlayer(Globals.SessionId, Globals.GameId, info.PersonaId, info.To);
                    if (result.IsSuccess)
                    {
                        info.State = $"将 等级:{info.Rank} 名称: {info.Rank}切换回原有队伍成功";

                    }
                    else
                    {
                        info.State = $"将 等级:{info.Rank} 名称: {info.Rank}切换回原有队伍失败";

                    }
                    if (Globals.IsCloudMode)
                    {
                        var result2 = await CloudApi.AddAutoToggleTeamList(info.PersonaId.ToString());
                        if (result2.IsSuccess)
                        {

                        }
                        else
                        {
                            try
                            {
                                var data = result2.Content.Replace("\r", "");
                                RepsoneData dataObj = JsonConvert.DeserializeObject<RepsoneData>(data);
                                if (dataObj.Id != "0003")
                                {
                                    Globals.TempToggleTeamList.Add(info.PersonaId);
                                }
                            }
                            catch (Exception)
                            {
                                Globals.TempToggleTeamList.Add(info.PersonaId);
                            }
                        }
                    }
                    else
                    {
                        Globals.TempToggleTeamList.Add(info.PersonaId);

                    }


                    LogView.ActionAddChangeTeamInfoLog(info);
                    if (Globals.IsCloudMode)
                    {
                        var _ = CloudApi.PushToggleHistory(PlayerRank: info.Rank.ToString(), PlayerName: info.Name, PersonaId: info.PersonaId.ToString(), GameMode: info.GameMode, MapName: info.MapName, Team1Name: info.Team1Name, Team2Name: info.Team2Name, State: info.State, ServerId: Globals.ServerId.ToString(), Guid: Globals.PersistedGameId, GameId: Globals.GameId.ToString());
                    }

                }
                if (info.To == 2 && Globals.AllowTempAloowToggleTeamList2.Count != 0 && toggleTeam)
                {
                    if (PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.AllowTempAloowToggleTeamList2))
                    {
                        Globals.AllowTempAloowToggleTeamList2.Remove(info.PersonaId);
                    }

                }
                if (info.To == 1 && Globals.AllowTempAloowToggleTeamList1.Count != 0 && toggleTeam)
                {
                    if (PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.AllowTempAloowToggleTeamList1))
                    {
                        Globals.AllowTempAloowToggleTeamList1.Remove(info.PersonaId);
                    }

                }
            }

        }

    }

/*    public static int GetMapID(){
        if (Globals.CurrentMapName == "ID_M_LEVEL_MENU" || Globals.CurrentMapName == "大厅菜单")
        {
            return 0;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_MOUNTAIN_FORT" || Globals.CurrentMapName == "格拉巴山")
        {
            return 4;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_FOREST" || Globals.CurrentMapName == "阿尔贡森林")
        {
            return 11;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_ITALIAN_COAST" || Globals.CurrentMapName == "帝国边境")
        {
            return 2;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_CHATEAU" || Globals.CurrentMapName == "流血宴厅")
        {
            return 7;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_SCAR" || Globals.CurrentMapName == "圣康坦的伤痕")
        {
            return 8;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_DESERT" || Globals.CurrentMapName == "西奈沙漠")
        {
            return 10;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_AMIENS" || Globals.CurrentMapName == "亚眠")
        {
            return 0;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_SUEZ" || Globals.CurrentMapName == "苏伊士")
        {
            return 9;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_FAO_FORTRESS" || Globals.CurrentMapName == "法欧堡")
        {
            return 6;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_GIANT" || Globals.CurrentMapName == "庞然暗影")
        {
            return 12;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_FIELDS" || Globals.CurrentMapName == "苏瓦松")
        {
            return 16;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_GRAVEYARD" || Globals.CurrentMapName == "决裂")
        {
            return 5;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_UNDERWORLD" || Globals.CurrentMapName == "法乌克斯要塞")
        {
            return 15;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_VERDUN" || Globals.CurrentMapName == "凡尔登高地")
        {
            return 13;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_TRENCH" || Globals.CurrentMapName == "尼维尔之夜")
        {
            return 14;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_SHOVELTOWN" || Globals.CurrentMapName == "攻占托尔")
        {
            return 3;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_BRIDGE" || Globals.CurrentMapName == "勃鲁希洛夫关口")
        {
            return 18;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_ISLANDS" || Globals.CurrentMapName == "阿尔比恩")
        {
            return 22;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_RAVINES" || Globals.CurrentMapName == "武普库夫山口")
        {
            return 20;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_VALLEY" || Globals.CurrentMapName == "加利西亚")
        {
            return 17;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_TSARITSYN" || Globals.CurrentMapName == "察里津")
        {
            return 19;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_VOLGA" || Globals.CurrentMapName == "窝瓦河")
        {
            return 21;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_BEACHHEAD" || Globals.CurrentMapName == "海丽丝岬")
        {
            return 23;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_HARBOR" || Globals.CurrentMapName == "泽布吕赫")
        {
            return 24;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_NAVAL" || Globals.CurrentMapName == "黑尔戈兰湾")
        {
            return 29;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_RIDGE" || Globals.CurrentMapName == "阿奇巴巴")
        {
            return 25;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_OFFENSIVE" || Globals.CurrentMapName == "索姆河")
        {
            return 28;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_HELL" || Globals.CurrentMapName == "帕斯尚尔")
        {
            return 27;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_RIVER" || Globals.CurrentMapName == "卡波雷托")
        {
            return 26;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_ALPS" || Globals.CurrentMapName == "剃刀边缘")
        {
            return 32;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_BLITZ" || Globals.CurrentMapName == "伦敦的呼唤：夜袭")
        {
            return 30;
        }
        if (Globals.CurrentMapName == "ID_M_MP_LEVEL_LONDON" || Globals.CurrentMapName == "伦敦的呼唤：灾祸")
        {
            return 31;
        }

        return 0;

    }*/

}