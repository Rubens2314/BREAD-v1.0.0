using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuOpciones : MonoBehaviour
{
    public static MenuOpciones Instance { get; private set; }   

    [SerializeField] menu[] menus;
    private void Awake()
    {

     Instance = this;
    }
    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName==menuName)
            {
                menus[i].Open();
            }else if (menus[i].open)
            {
                CloseMenu(menus[i]);    
            }
        }
    }
    public void OpenMenu(menu menu)
    {

        for (int i = 0; i < menus.Length; i++)
        {
           if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }
    public void CloseMenu(menu menu)
    {
        menu.close();
    }
    public void Jugar()
    {
        SceneManager.LoadScene("Juego");
    }
    public void Multijugador()
    {
        SceneManager.LoadScene("Launcher");
    }
  
    public void Salir()
    {
        Application.Quit();
            
    }
}
