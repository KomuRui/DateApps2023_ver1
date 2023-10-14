using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate2 : MonoBehaviour
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
        //Œ»İ‚Ì—Í‚ğæ“¾
        power += -Input.GetAxis("R_Stick_H1") * speed * Time.deltaTime;
        power = Math.Min(0.09f, Math.Abs(power)) * Math.Sign(power);

        //—Í‚ª‰Á‚¦‚ç‚ê‚Ä‚È‚¢‚Ì‚È‚çŒ¸‘¬‚·‚é
        if (Input.GetAxis("R_Stick_H1") == 0 && power != 0.0f) power *= 0.997f;

        transform.eulerAngles += new Vector3(0, power, 0);

    }
}
