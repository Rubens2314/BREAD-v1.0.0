using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Desconectar : MonoBehaviourPunCallbacks
{
  
    public void Salir()
    {
        Application.Quit();

    }

}
