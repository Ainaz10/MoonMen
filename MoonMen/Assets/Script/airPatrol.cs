using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airPatrol : MonoBehaviour
{
    public Transform point1; // первая точка
    public Transform point2; // вторая точка
    public float speed = 2f; // скорость лета
    public float waitTime = 3f; // ожидание в точке после долета
    bool CanGo = true; // может ли муха двигаться

    void Start()
    {
        gameObject.transform.position = new Vector3(point1.position.x, point1.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanGo)
            transform.position = Vector3.MoveTowards(transform.position, point1.position, speed * Time.deltaTime);

        if (transform.position == point1.position)
        {
            Transform t = point1;
            point1 = point2;
            point2 = t;
            CanGo = false;
            StartCoroutine(Waiting());

        }
        IEnumerator Waiting() // корутина для ожидания
        {
            yield return new WaitForSeconds(waitTime);
            if (transform.rotation.y == 0)
                transform.eulerAngles = new Vector3(0, 180, 0);
            else
                transform.eulerAngles = new Vector3(0, 0, 0);
            CanGo = true;
        }
    }
}
