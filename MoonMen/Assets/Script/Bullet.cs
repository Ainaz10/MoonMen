using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 3f; // скорость падения бомб
    float TimeToDisable = 10f; //время исчезнования бомб


    void Start()
    {
        StartCoroutine(SetDisabled()); //запуск корутин
    }

     
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime); // установка для падения бомб 
    }

    IEnumerator SetDisabled() // корутина для уничтожения бомб
    {
        yield return new WaitForSeconds(TimeToDisable);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision) // код для уничтожения бомбы после столкновения с физ.объектом
    {
        StopCoroutine(SetDisabled());
        gameObject.SetActive(false);
    }

}
