using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate1 : MonoBehaviour
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
        power += -Input.GetAxis("L_Stick_V" + playerNum) * speed * Time.deltaTime;
        power = Math.Min(0.08f, Math.Abs(power)) * Math.Sign(power);

        //�͂��������ĂȂ��̂Ȃ猸������
        if (Input.GetAxis("L_Stick_V" + playerNum) == 0 && power != 0) power *= 0.99f;

        //�p�x�����Z���A�͈͓��Ɏ��߂�
        transform.eulerAngles += new Vector3(0, power, 0);

        ///�͈͓��ɂ����߂�
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
