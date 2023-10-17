using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate1 : MonoBehaviour
{

    [SerializeField] private float speed = 1.0f;  // �X�s�[�h
    [SerializeField] private int playerNum;       // �v���C���[�ԍ�

    private float power = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���݂̗͂��擾
        power += -Input.GetAxis("L_Stick_V1") * speed * Time.deltaTime;
        power = Math.Min(0.12f, Math.Abs(power)) * Math.Sign(power);

        //�͂��������ĂȂ��̂Ȃ猸������
        if (Input.GetAxis("L_Stick_V1") == 0 && power != 0) power *= 0.997f;

        //�p�x�����Z���A�͈͓��Ɏ��߂�
        transform.eulerAngles += new Vector3(0, power, 0);

        //�͈͓��ɂ����߂�
        if (transform.eulerAngles.y > 35 && transform.eulerAngles.y < 300) transform.eulerAngles = new Vector3(0, 35, 0);
        if (transform.eulerAngles.y < 305 && transform.eulerAngles.y > 40) transform.eulerAngles = new Vector3(0, 305, 0);

    }
}
