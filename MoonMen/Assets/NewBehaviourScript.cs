using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public string a = "lalala";
    int b;
    float c;
    public bool d = false; // ��������� public ��������� ������� �������� � ������ �����. ��� ����� ����� ����� ��������� ��������� ������ � ����� �� �������� �������� ����

    // Start is called before the first frame update
    void Start()    // void ��� �����. void Start ���������� ������ 1 ���
    {
        print("��� ����� �����");
    }

    // Update is called once per frame
    void Update() // void Update ���������� ������ ����
    {
        print("��� ��� ����� �������!");
    }


    void Calling()
    {
        print("��� �� ��� �����?");
    }
}