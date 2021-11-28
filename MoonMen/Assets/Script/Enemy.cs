using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //МЕТОДЫ СТОЛКНОВЕНИЯ С ВРАГОМ
    //1) метод столкновения двух объектов
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Player") {
            // print("Минус 1 жизнь");
            collision.gameObject.GetComponent<Player>().RecountHp(-1);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 8f, ForceMode2D.Impulse);
        }
        
    }

   /* 
    *2) Метод когда объекты соприкоснулись
    *private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("Stay");
        }
    }
   3) метод когда объекты отсоединяются/отходят друг от друга (убрал ногу с мины - убился)
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("Объекты разошлись");
        }
    }*/
}
