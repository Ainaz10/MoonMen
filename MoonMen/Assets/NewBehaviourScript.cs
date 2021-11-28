using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public string a = "lalala";
    int b;
    float c;
    public bool d = false; // выражение public позволяет вынести значение в панель юнити. тем самым можно будет управлять значением только с юнити не открывая редактор кода

    // Start is called before the first frame update
    void Start()    // void это метод. void Start вызывается только 1 раз
    {
        print("Это метод Старт");
    }

    // Update is called once per frame
    void Update() // void Update вызывается каждый кадр
    {
        print("ААА как много апдейта!");
    }


    void Calling()
    {
        print("Где же наш метод?");
    }
}