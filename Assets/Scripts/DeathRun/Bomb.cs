using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : GimmickBase
{
    //�������Ă��邩�ǂ���
    private bool isExplode = false;

    [SerializeField] private float explodeTime = 3f;
    [SerializeField] private GameObject explodeParticle;

    //����̃A�N�V�������N����
    public override void Action()
    {
        Explode();
    }

    public override void GimmickStart()
    {
    }

    public override void GimmickUpdate()
    {
    }

    void OnTriggerStay(Collider other)
    {
        //�������������Ă�����
        if (isExplode && other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
        }
        //Debug.Log(other.gameObject.name); // �Ԃ���������̖��O���擾
    }

    //��������
    public void Explode()
    {
        isExplode = true;
        Instantiate(explodeParticle, this.gameObject.transform);
        Invoke("ExplodeEnd", explodeTime);
    }

    //�������I�������
    public void ExplodeEnd()
    {
        Destroy(gameObject);
    }
}
