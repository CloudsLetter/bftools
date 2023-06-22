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

        if (Globals.LoginPlayerIsAdmin && Globals.IsAllowToggle && Globals.IsSetRuleOK)
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

            if (Globals.IsAllowWhlistToggleTeam)
            {
                if (PlayerUtil.IsWhite(info.Name, Globals.CustomWhites_Name))
                {
                    whitelistToggle = true;
                }
            }

            bool IsAdmin = PlayerUtil.IsAdminVIP(info.PersonaId, Globals.ServerAdmins_PID);
            bool isToggle = false;


            if (!IsAdmin && !whitelistToggle)
            {
            var result3 = await CloudApi.QueryAutoToggleTeamList(info.PersonaId.ToString());
            if (result3.IsSuccess)
            {
                isToggle = true;

                if (Globals.TempToggleTeamList.Count != 0)
                {
                    Globals.TempToggleTeamList.Remove(info.PersonaId);
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
                            Globals.TempToggleTeamList.Remove(info.PersonaId);
                        }
                    }
                    }
                catch (Exception)
                {
                        if (Globals.TempToggleTeamList.Count != 0)
                        {
                            isToggle = PlayerUtil.IsAtTempTempAloowToggleTeamList(info.PersonaId, Globals.TempToggleTeamList);
                            Globals.TempToggleTeamList.Remove(info.PersonaId);
                        }
                    }

                }
            }


            if (!toggleTeam && !IsAdmin && !whitelistToggle && !isToggle )
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
                    catch (Exception) {
                        Globals.TempToggleTeamList.Add(info.PersonaId);
                    }
                }

                LogView.ActionAddChangeTeamInfoLog(info);

            }
            if (info.To == 2 && Globals.AllowTempAloowToggleTeamList2.Count != 0 && toggleTeam)
            {
                Globals.AllowTempAloowToggleTeamList2.Remove(info.PersonaId);

            }
            if (info.To == 1 && Globals.AllowTempAloowToggleTeamList1.Count != 0 && toggleTeam)
            {
                Globals.AllowTempAloowToggleTeamList1.Remove(info.PersonaId);

            }
            /*bool Kick = false;


            //&& !IsAdmin
            if (!Kick && !WhitelistKick && Globals.IsSetRuleOK)
            {
                var result = await BF1API.RSPMovePlayer(Globals.SessionId, Globals.GameId, info.PersonaId, info.To);
                if (result.IsSuccess)
                {
    *//*                lock (this)
                    {
                        AppendChangeTeamLog($"操作时间: {DateTime.Now}");
                        AppendChangeTeamLog($"等级: {info.Rank}");
                        AppendChangeTeamLog($"玩家ID: {info.Name}");
                        AppendChangeTeamLog($"数字ID: {info.PersonaId}");
                        AppendChangeTeamLog($"当前地图: {info.GameMode} - {info.MapName}");
                        AppendChangeTeamLog($"状态: {info.State}");
                        AppendChangeTeamLog($"自动更换至原队伍成功\n");
                    }
    *//*
                    info.State = $"将 等级:{info.Rank} 名称: {info.Rank}切换回原有队伍成功";


                }
                else
                {
    *//*                lock (this)
                    {
                        AppendChangeTeamLog($"操作时间: {DateTime.Now}");
                        AppendChangeTeamLog($"等级: {info.Rank}");
                        AppendChangeTeamLog($"玩家ID: {info.Name}");
                        AppendChangeTeamLog($"数字ID: {info.PersonaId}");
                        AppendChangeTeamLog($"当前地图: {info.GameMode} - {info.MapName}");
                        AppendChangeTeamLog($"状态: {info.State}");
                        AppendChangeTeamLog($"自动更换至原队伍失败\n");
                    }*
                    info.State = $"将 等级:{info.Rank} 名称: {info.Rank}切换回原有队伍失败";
                }
                ChatView.ActionSendTextToBf1Game(info.State);
                RobotView.ActionSendChangeTeamLogToQQ(info);


            }

*/
        }

    }

}