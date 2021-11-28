using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb; // ��������� � ���������
    public float speed; //����������� ����� public �������� speed � ������ ����� 
    public float jumpHeight; // jumpHeight - ���������� ������ ������
    public Transform groundCheck; //�������� ������� �����
    bool isGrounded; // ���������� ��������� ��������� �� ������ �� �����
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

    
    void Update() // ����� �������� ���� ������ ���������� 
    {
        
        
        // !!! ���� ������ if ������ ���� ������� �� �������� ������ ����� �� �������. ���� 2 � ����� �� ��� �� ����� �����������, ��� �� ��������� ���� ������� �������� ������.
        CheckGround(); 
        if (Input.GetAxis("Horizontal") == 0 && isGrounded) // ���� (�� ������ �� ���� �������) � (��������� �� �����) �� 
        {
            anim.SetInteger("State", 1); // { ����������/������������� �������� ( "State", 1) } 1 - ��� ����� ��������
        }
        else
        {
            Flip(); // Flip �������� ������ ����� ������������ ������ ����� ��� ��� ������� ����� �����������. ���� ��������� �� ������ ����� �� ����� ����������� ��������� �.�. ����� ����������� ������ ����.
            if (isGrounded)
                anim.SetInteger("State", 2);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // ��� ��� ������ ������ "������" 
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse); // ��� ��� ������.
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y); // ����������� ����� ������� ������ ��� �������� � �������� � ���������
        
    }

    void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0) // "Input.GetAxis("Horizontal") > 0" �������� ������� ������ "�������" (-1---0---1)
            transform.localRotation = Quaternion.Euler(0, 0, 0); //transform.localRotation - �������� �� ������� ���������. � ������ ������ 0 0 0 �������� �������� �������� � ������� ������ �������.
        if (Input.GetAxis("Horizontal") < 0) // "< 0
            transform.localRotation = Quaternion.Euler(0, 180, 0); //
    }

    void CheckGround () // ����� ��������������� ��������� � ������������
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f); // ��� ������� � ������� 0.2f ����� ������ ����������. �������� ����� ��� ������.
        isGrounded = colliders.Length > 1; // ���� ��������� ��� ��� ��������, ���� ������ 1�� ���������� ������ ���� ������������� � ������ ��� ������� ��������. 
        
        if (!isGrounded)                    // ���� (��� �������������)
            anim.SetInteger("State", 3);    // ��������.����������("State", 3)
    }

    public void RecountHp(int deltaHp)
    {
        curHp = curHp + deltaHp;
        print(curHp);
        if (curHp<=0)
        {
            print("��������� ������");
        }
    }

}
