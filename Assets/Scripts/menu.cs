using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{
    public string menuName;
    public bool open;
   public void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }
    public void close()
    {
        open=false;
        gameObject.SetActive(false);
    }
}
