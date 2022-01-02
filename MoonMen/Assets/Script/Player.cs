using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    float hitTimer = 0f;
    public Image PlayerCountdown;
    float insideTimer = -1f;
    public float insideTimerUp = 30f;
    public Image insideCountdown;
    public Inventory inventory;
    public Soundeffector soundeffector;
    public Joystick joystick;

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
            if (joystick.Horizontal >= 0.3f || joystick.Horizontal <= -0.3f)
                Flip(); 
        }
        else
        {

            // !!! Если внутри if только одна команда то фигурные скобки можно не ставить. Если 2 и более то они не будут считываться, что бы считалось надо ставить фигурные скобки.
            CheckGround();
            if (joystick.Horizontal < 0.3f && joystick.Horizontal > -0.3f && (isGrounded) && !isClimbing) // если (не нажата ни одна клавиша) и (находится на земле) то 
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
        if (insideTimer >= 0f)
        {
            insideTimer += Time.deltaTime;
            if (insideTimer >= insideTimerUp)
            {
                insideTimer = 0f;
                RecountHp(-1);
            }
            else
                insideCountdown.fillAmount = 1 - (insideTimer / insideTimerUp);
        }

    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse); // код для прыжка.
            soundeffector.PlayJumpSound(); //вызов звука прыжка
        }
    }


    void FixedUpdate()
    {
        if (joystick.Horizontal >= 0.2f)
            rb.velocity = new Vector2(speed, rb.velocity.y); // считывается какая клавиша нажата что приводит в движение в персонажа
        else if (joystick.Horizontal <= -0.2f)
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        else
            rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    void Flip()
    {
        if (joystick.Horizontal >= 0.3f) // "Input.GetAxis("Horizontal") > 0" имитация нажатия кнопки "направо" (-1---0---1)
            transform.localRotation = Quaternion.Euler(0, 0, 0); //transform.localRotation - отвечает за поворот персонажа. в данном случае 0 0 0 является исходной позицией и смотрит объект направо.
        if (joystick.Horizontal <= -0.3f) // "< 0
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
            inventory.Add_key();

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
            soundeffector.PlayCoinSound(); // вызов звука монет

        }

        // собираем сердечки
        if (collision.gameObject.tag == "Heart")
        {
            Destroy(collision.gameObject);
            // RecountHp(1);
            inventory.Add_hp();
            
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
            // StartCoroutine(NoHit());
            inventory.Add_bg();
        }

        // подбор зеленого кристалла
        if (collision.gameObject.tag == "GreenGem")
        {
            Destroy(collision.gameObject);
            // StartCoroutine(SpeedBonus());
            inventory.Add_gg();
        }

        // кнопка старт стоп
        if (collision.gameObject.tag == "TimerButtonStart")
        {
            insideTimer = 0f;
        }

        if (collision.gameObject.tag == "TimerButtonStop")
        {
            insideTimer = -1f;
            insideCountdown.fillAmount = 0f;
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

        if (collision.gameObject.tag == "Icy") // условие для скольжения (вход в лед(скользкая поверхность))
        {
            if (rb.gravityScale == 1f)
            {
                rb.gravityScale = 7f;
                speed *= 0.25f;
            }
            
        }

        if(collision.gameObject.tag == "Lava")
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= 3f)
            {
                hitTimer = 0f;
                PlayerCountdown.fillAmount = 1f;
                RecountHp(-1);

            }
            else
                PlayerCountdown.fillAmount = 1 - (hitTimer / 3f);
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

        if (collision.gameObject.tag == "Icy") // условие для скольжения (выход изо льда (наступаем на обычную поверхность))
        {
            if (rb.gravityScale == 7f)
            {
                rb.gravityScale = 1f;
                speed *= 4f;
            }
            
        }

        if (collision.gameObject.tag == "Lava")
        {
            hitTimer = 0f;
            PlayerCountdown.fillAmount = 0f;
        }
             

    }

    // создание батута
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trampoline")
            StartCoroutine(TrampolineAnim(collision.gameObject.GetComponentInParent<Animator>()));

        if (collision.gameObject.tag == "Quicksand") // условия для зыбучего песка
        {
            speed *= 0.25f;
            rb.mass *= 100f; 
        }
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Quicksand") // условия для зыбучего песка
        {
            speed *= 4f;
            rb.mass *= 0.01f;
        }
    }

    //доступ к значениям собранных монет
    public int GetCoins()
    {
        return coins;
    }

    public int GetHP()
    {
        return curHp;
    }

    //делаем публичными методы для синего и зеленого кристалла для того что бы можно было использовать в скрипте inventory
    public void BlueGem()
    {
        StartCoroutine(NoHit());
    }

    public void GreenGem()
    {
        StartCoroutine(SpeedBonus());
    }
}


