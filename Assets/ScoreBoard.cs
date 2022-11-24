using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ScoreBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;
    [SerializeField] CanvasGroup canvasGroup;

   
    Dictionary<Player,Scoreboarditem> scoreboarditems=new Dictionary<Player,Scoreboarditem>();
  
    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScoreboardItem(player);
        }
     
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreboardItem(newPlayer);
    }
    public override void OnPlayerLeftRoom(Player Otherplayer)
    {
        RemoveScoreboardItem(Otherplayer);
    }

    void AddScoreboardItem(Player player)
    {
        Scoreboarditem item = Instantiate(scoreboardItemPrefab, container).GetComponent<Scoreboarditem>();
        item.Initialize(player);
        scoreboarditems[player] = item;
    }

    
   
    void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboarditems[player].gameObject);
        scoreboarditems.Remove(player);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            canvasGroup.alpha = 1;
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            canvasGroup.alpha = 0;
        }
    }
}