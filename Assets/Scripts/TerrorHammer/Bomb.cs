using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : GimmickBase
{
    //����̃A�N�V�������N����
    public override void Action()
    {
    }

    public override void GimmickStart()
    {
    }

    public override void GimmickUpdate()
    {
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name); // �Ԃ���������̖��O���擾
    }
}
