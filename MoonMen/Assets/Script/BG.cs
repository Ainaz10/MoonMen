using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour
{
    float length, startpos;
    public GameObject cam;
    public float parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1 - parallaxEffect);
        float dist = cam.transform.position.x * parallaxEffect; // dist будет отвечать за то, насколько сильным будет смещение фона относительно камеры (эффект параллакса)

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);


        // условие для того что бы переместить один из картинок в противоположную сторону, когда крайние фоны перестают быть видны камере
        if (temp > startpos + length)  
            startpos += length;
        else if (temp < startpos - length)
            startpos -= length;
    }
}
