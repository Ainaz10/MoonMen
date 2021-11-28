using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //������ ������������ � ������
    //1) ����� ������������ ���� ��������
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Player") {
            // print("����� 1 �����");
            collision.gameObject.GetComponent<Player>().RecountHp(-1);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 8f, ForceMode2D.Impulse);
        }
        
    }

   /* 
    *2) ����� ����� ������� ��������������
    *private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("Stay");
        }
    }
   3) ����� ����� ������� �������������/������� ���� �� ����� (����� ���� � ���� - ������)
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("������� ���������");
        }
    }*/
}
