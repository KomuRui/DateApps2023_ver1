using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : GimmickBase
{
    [SerializeField] private GameObject shotPos;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 1500f;

    public override void Action()
    {
        //���𔭎�
        Shot();
    }

    public override void GimmickStart()
    {
    }

    public override void GimmickUpdate()
    {
    }

    //���𔭎�
    public void Shot()
    {
        GameObject createdBullet = Instantiate(bullet) as GameObject;
        createdBullet.transform.position = shotPos.transform.position;

        //���˃x�N�g��
        Vector3 force = new Vector3(1,0,0);

        //���˂̌����Ƒ��x������
        force *= bulletSpeed * Time.deltaTime;

        // Rigidbody�ɗ͂������Ĕ���
        createdBullet.GetComponent<Rigidbody>().AddForce(force);
    }
}
