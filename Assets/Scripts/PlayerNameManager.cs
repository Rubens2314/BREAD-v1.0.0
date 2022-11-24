using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
  [SerializeField] TMP_InputField UserNameInput;

    private void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            UserNameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");

        }
        else
        {
            UserNameInput.text ="Panesito# "+Random.Range(0,1000).ToString("0000");
            OnUserNameInputValueChange();
        }
    }
    public void OnUserNameInputValueChange()
    {
        PhotonNetwork.NickName = UserNameInput.text;
        PlayerPrefs.SetString("username", UserNameInput.text);  
    }
}
