using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb; // обращение к персонажу
    public float speed; //вытаскиваем через public значение speed в панель юнити 
    public float jumpHeight; // jumpHeight - объявление высоты прыжка
    public Transform groundCheck; //проверка наличия земли
    bool isGrounded; // логическое выражение находится ли объект на земле
    Animator anim;
    int curHp;
    int maxHp = 3;
    bool isHit = false; // условие для персонажа если его бьют
    public Main main; //обращение к файлу Main
    public bool key = false; // параметр отвечающий есть ключ или нет
    bool canTP = true;
    public bool inWater = false; // параметр для воды
    bool isClimbing = false; // параметр для движения по лестнице // параметр "!isClimbing" необходимо добавить во все анимации 
    int coins = 0; // для монет
    bool canHit = true; // можем ли бить врага
    public GameObject blueGem, greenGem;
    int gemCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curHp = maxHp;
    }


    void Update() // метод проверки кода ежекадрово 
    {
        if (inWater && !isClimbing) // условия для персонажа когда он в воде и не на лестнице 
        {
            anim.SetInteger("State", 4);
            isGrounded = true; // возможность прыгать в воде
            if (Input.GetAxis("Horizontal") != 0)
                Flip(); 
        }
        else
        {

            // !!! Если внутри if только одна команда то фигурные скобки можно не ставить. Если 2 и более то они не будут считываться, что бы считалось надо ставить фигурные скобки.
            CheckGround();
            if (Input.GetAxis("Horizontal") == 0 && (isGrounded) && !isClimbing) // если (не нажата ни одна клавиша) и (находится на земле) то 
            {
                anim.SetInteger("State", 1); // { установить/задействовать анимацию ( "State", 1) } 1 - это номер анимации
            }
            else
            {
                Flip(); // Flip находясь именно здесь активируется только когда вот это условие будет выполняться. если поставить на другое место то будет перегружать процессор т.к. будет проверяться каждый кадр.
                if (isGrounded && !isClimbing)
                    anim.SetInteger("State", 2);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // код для кнопки прыжка "пробел" 
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse); // код для прыжка.
    }


    void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y); // считывается какая клавиша нажата что приводит в движение в персонажа

    }

    void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0) // "Input.GetAxis("Horizontal") > 0" имитация нажатия кнопки "направо" (-1---0---1)
            transform.localRotation = Quaternion.Euler(0, 0, 0); //transform.localRotation - отвечает за поворот персонажа. в данном случае 0 0 0 является исходной позицией и смотрит объект направо.
        if (Input.GetAxis("Horizontal") < 0) // "< 0
            transform.localRotation = Quaternion.Euler(0, 180, 0); //
    }

    void CheckGround() // метод соприкосновения персонажа с поверхностью
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f); // все объекты в радиусе 0.2f будут внутри коллайдера. имитация земли под ногами.
        isGrounded = colliders.Length > 1; // один коллайдер это наш персонаж, если больше 1го коллайдера значит есть прикосновение с землей или твердым объектом. 

        if (!isGrounded && !isClimbing)     // если (нет прикосновения)
            anim.SetInteger("State", 3);    // анимацию.установить("State", 3)
    }

    // метод для жизней
    public void RecountHp(int deltaHp)
    {
         
        if (deltaHp < 0 && canHit)
        {
            curHp = curHp + deltaHp;
            StopCoroutine(OnHit());
            canHit = false;
            isHit = true;
            StartCoroutine(OnHit());
        }
        // кол-во жизней может быть равен только максимальному значению т.е. к трем
        else if (deltaHp > 0){
            if (curHp >= maxHp)
                curHp = maxHp;
            else
                curHp = curHp + deltaHp;
        }
            
        print(curHp);
        if (curHp <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }


    IEnumerator OnHit() // анимация цвета если у персонажа отнимается жизнь
    {
        if (isHit)
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.04f, GetComponent<SpriteRenderer>().color.b - 0.04f);
        else 
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.04f, GetComponent<SpriteRenderer>().color.b + 0.04f);

        if (GetComponent<SpriteRenderer>().color.g >= 1f)
        {
            canHit = true;
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
            yield break;
        }
            

        if (GetComponent<SpriteRenderer>().color.g <= 0)
        {
            isHit = false;
            GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);

        }
         
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());

    }


    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }

    // Метод для ключа и дверей
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            Destroy(collision.gameObject);
            key = true;
        }
        if(collision.gameObject.tag == "Door")
        {
            if (collision.gameObject.GetComponent<Door>().isOpen && canTP)
            { 
                collision.gameObject.GetComponent<Door>().Teleport(gameObject);
                canTP = false;
                StartCoroutine(TPwait());
            }
            else if (key)
                collision.gameObject.GetComponent<Door>().Unlock();
        }

        // собираем монетки
        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
            coins++;
            print("Кол-во монеток равно " + coins);
        }

        // собираем сердечки
        if (collision.gameObject.tag == "Heart")
        {
            Destroy(collision.gameObject);
            RecountHp(1);
            
        }
        // подбор мухамора
        if (collision.gameObject.tag == "Mushroom")
        {
            Destroy(collision.gameObject);
            RecountHp(-1);
        }

        // подбор синего кристалла
        if (collision.gameObject.tag == "BlueGem")
        {
            Destroy(collision.gameObject);
            StartCoroutine(NoHit());
        }

        // подбор зеленого кристалла
        if (collision.gameObject.tag == "GreenGem")
        {
            Destroy(collision.gameObject);
            StartCoroutine(SpeedBonus());
        }

    }

    IEnumerator TPwait()
    {
        yield return new WaitForSeconds(1f);
        canTP = true;
    }

    // Подъем по лестнице
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isClimbing = true;
            //rb.bodyType = RigidbodyType2D.Kinematic; // убираем гравитацию
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0, 0);
            if (Input.GetAxis("Vertical") == 0) //условия для анимации при нажатии клавиша вверх 
            {
                anim.SetInteger("State", 5);
            }
            else
            {
                anim.SetInteger("State", 6);
                transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * 0.5f * Time.deltaTime);  // translate меняет позицию персонажа когда он находится в зоне лестницы
            }
            
        }
    }

    //создаем метод позволяющий возвращаться на землю после схода с лестницы
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isClimbing = false;
            // rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;
        }
        
    }

    // создание батута
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trampoline")
            StartCoroutine(TrampolineAnim(collision.gameObject.GetComponentInParent<Animator>()));
    }

    IEnumerator TrampolineAnim(Animator an)
    {
        an.SetBool("isJump", true);
        yield return new WaitForSeconds(0.5f);
        an.SetBool("isJump", false);
    }

    IEnumerator NoHit()
    {
        gemCount++;
        blueGem.SetActive(true);
        CheckGems(blueGem);

        canHit = false;
        blueGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        //print("Неуязвимость активирована");
        yield return new WaitForSeconds(4f);
        StartCoroutine(Invis(blueGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f); 
        canHit = true;
        //print("Персонажа можно бить!");
        gemCount--;
        blueGem.SetActive(false);
        CheckGems(greenGem);
           
    }
    // корутина для зеленого кристалла
    IEnumerator SpeedBonus()
    {
        gemCount++;
        greenGem.SetActive(true);
        CheckGems(greenGem);

        speed = speed * 2;
        greenGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        // print("Скорость увеличена в два раза");
        yield return new WaitForSeconds(9f);
        StartCoroutine(Invis(greenGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        speed = speed / 2;
        // print("Скорость стала обычной");
        gemCount--;
        greenGem.SetActive(false);
        CheckGems(blueGem);

    }

    void CheckGems(GameObject obj)
    {
        if (gemCount == 1)
            obj.transform.localPosition = new Vector3(0f, 0.6f, obj.transform.localPosition.z);
        else if (gemCount == 2)
        {
            blueGem.transform.localPosition = new Vector3(-0.5f, 0.5f, blueGem.transform.localPosition.z);
            greenGem.transform.localPosition = new Vector3(0.5f, 0.5f, blueGem.transform.localPosition.z);
        }
    }

    IEnumerator Invis(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
            StartCoroutine(Invis(spr, time));
    }
}


