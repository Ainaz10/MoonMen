using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    public Button[] lvls;
    public Text coinText;


    void Start() // метод сверяющий кол-во пройденных уровней и в зависимости от пройденных уровней открывает доступ к другим уровням
    {
        if (PlayerPrefs.HasKey("Lvl"))
            for (int i = 0; i < lvls.Length; i++) 
            {
                if (i <= PlayerPrefs.GetInt("Lvl"))
                    lvls[i].interactable = true;
                else
                    lvls[i].interactable = false;
            }
    }

    private void Update() // метод для вывода общего кол-ва монеток в меню
    {
        if (PlayerPrefs.HasKey("coins"))
            coinText.text = PlayerPrefs.GetInt("coins").ToString();
        else
            coinText.text = "0";
    }

    
    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void DelKeys()
    {
        PlayerPrefs.DeleteAll();
    }
}
