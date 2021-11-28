using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    float weight = 32f; //вес
    int height = 40; // рост
    bool owner = true; // хозяин
    string breed = "Haski"; //порода

    void Start()
    {
        if (weight > 35)
        {
            print("Вашей собаке желательно сбросить вес");
        }
        else
        {
            print("У вашей собаки все отлично с весом!");
        }


        if (height <= 40 && breed != "чихуахуа")
        {
            print("Ваша собака еще не выросла");
        }
    }

}
