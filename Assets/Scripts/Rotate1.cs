using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate1 : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;
    private float power = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���݂̗͂��擾
        power += -Input.GetAxis("L_Stick_H") * speed * Time.deltaTime;
        power = Math.Min(0.03f, Math.Abs(power)) * Math.Sign(power);

        //�͂��������ĂȂ��̂Ȃ猸������
        if (Input.GetAxis("L_Stick_H") == 0 && power != 0) power *= 0.997f;

        transform.eulerAngles += new Vector3(0, power, 0);
    }
}
