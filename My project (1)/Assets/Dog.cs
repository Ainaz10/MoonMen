using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    float weight = 32f; //���
    int height = 40; // ����
    bool owner = true; // ������
    string breed = "Haski"; //������

    void Start ()
    {
        if (weight > 35)
        {
            print("����� ������ ���������� �������� ���");
        }else
        {
            print("� ����� ������ ��� ������� � �����!");
        }


        if (height <= 40 && breed != "��������")
        {
            print("���� ������ ��� �� �������");
        }
    }

}
