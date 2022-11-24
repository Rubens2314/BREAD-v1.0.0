using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon.StructWrapping;

public class Scoreboarditem : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameText;
    public TMP_Text killstext;
    public TMP_Text deathText;
    Player player;
    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
        this.player = player;
        UpdateStats();
    }
    public void Update()
    {
        UpdateStats();
    }
    void UpdateStats()
    {
        if(player.CustomProperties.TryGetValue("kills",out object kills))
        {
            killstext.text = kills.ToString();
        }
        if (player.CustomProperties.TryGetValue("deaths", out object deaths))
        {
            deathText.text = deaths.ToString();
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer,Hashtable changedProps)
    {
        if (targetPlayer==player)
        {
            if (changedProps.ContainsKey("Kills")|| changedProps.ContainsKey("deaths"))
            {
                UpdateStats();
            }
        }
    }
}
