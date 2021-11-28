using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb; // обращение к персонажу
    public float speed; //вытаскиваем через public значение speed в панель юнити 
    public float jumpHeight; // jumpHeight - объ€вление высоты прыжка
    public Transform groundCheck; //проверка наличи€ земли
    bool isGrounded; // логическое выражение находитс€ ли объект на земле
    Animator anim;
    int curHp;
    int maxHp = 3;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curHp = maxHp;
    }

    
    void Update() // метод проверки кода каждый ежекадрово 
    {
        
        
        // !!! ≈сли внутри if только одна команда то фигурные скобки можно не ставить. ≈сли 2 и более то они не будут считыватьс€, что бы считалось надо ставить фигурные скобки.
        CheckGround(); 
        if (Input.GetAxis("Horizontal") == 0 && isGrounded) // если (не нажата ни одна клавиша) и (находитс€ на земле) то 
        {
            anim.SetInteger("State", 1); // { установить/задействовать анимацию ( "State", 1) } 1 - это номер анимации
        }
        else
        {
            Flip(); // Flip наход€сь именно здесь активируетс€ только когда вот это условие будет выполн€тьс€. если поставить на другое место то будет перегружать процессор т.к. будет провер€тьс€ каждый кадр.
            if (isGrounded)
                anim.SetInteger("State", 2);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // код дл€ кнопки прыжка "пробел" 
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse); // код дл€ прыжка.
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y); // считываетс€ кака€ клавиша нажата что приводит в движение в персонажа
        
    }

    void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0) // "Input.GetAxis("Horizontal") > 0" имитаци€ нажати€ кнопки "направо" (-1---0---1)
            transform.localRotation = Quaternion.Euler(0, 0, 0); //transform.localRotation - отвечает за поворот персонажа. в данном случае 0 0 0 €вл€етс€ исходной позицией и смотрит объект направо.
        if (Input.GetAxis("Horizontal") < 0) // "< 0
            transform.localRotation = Quaternion.Euler(0, 180, 0); //
    }

    void CheckGround () // метод соприкосновени€ персонажа с поверхностью
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f); // все объекты в радиусе 0.2f будут внутри коллайдера. имитаци€ земли под ногами.
        isGrounded = colliders.Length > 1; // один коллайдер это наш персонаж, если больше 1го коллайдера значит есть прикосновение с землей или твердым объектом. 
        
        if (!isGrounded)                    // если (нет прикосновени€)
            anim.SetInteger("State", 3);    // анимацию.установить("State", 3)
    }

    public void RecountHp(int deltaHp)
    {
        curHp = curHp + deltaHp;
        print(curHp);
        if (curHp<=0)
        {
            print("ѕроизошла смерть");
        }
    }

}
