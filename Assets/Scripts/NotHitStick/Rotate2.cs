using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate2 : MonoBehaviour
{
    [SerializeField] private float speed;         // �X�s�[�h
    [SerializeField] public int playerNum;       // �v���C���[�ԍ�

    private float power = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //�J�n���Ă��Ȃ����I����Ă���̂Ȃ�
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish()) return;

        //���݂̗͂��擾
        float nowPower = Input.GetAxis("L_Stick_V" + playerNum) * speed;
        power += nowPower;
        power = Math.Min(30.0f, Math.Abs(power)) * Math.Sign(power);

        //�͂��������ĂȂ��̂Ȃ猸������
        if (Input.GetAxis("L_Stick_V" + playerNum) == 0 && power != 0.0f) power *= 0.99f;

        transform.eulerAngles += new Vector3(0, power * Time.deltaTime, 0);
        //�͈͓��ɂ����߂�
        if (transform.eulerAngles.y > 35 && transform.eulerAngles.y < 300)
        {
            transform.eulerAngles = new Vector3(0, 35, 0);
            power = 0.0f;
        }
        if (transform.eulerAngles.y < 305 && transform.eulerAngles.y > 40)
        {
            transform.eulerAngles = new Vector3(0, 305, 0);
            power = 0.0f;
        }
    }
}
