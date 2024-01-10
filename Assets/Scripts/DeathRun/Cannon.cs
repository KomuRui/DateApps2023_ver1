using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : GimmickBase
{
    [SerializeField] private GameObject shotPos;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private GameObject shootParticle;
    [SerializeField] private GameObject shootParticlePos;

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
        //�G�t�F�N�g
        Instantiate(shootParticle, shootParticlePos.transform);

        //�e��\��
        bullet.gameObject.SetActive(true);

        //�e�𔭎ˈʒu��
        bullet.transform.position = shotPos.transform.position;

        //���˃x�N�g��
        Vector3 force = transform.forward;
        bullet.GetComponent<DeathRunBullet>().SetMoveDirection(force);

        //���x������
        bullet.GetComponent<DeathRunBullet>().SetMoveSpeed(bulletSpeed);


        //force *= bulletSpeed * Time.deltaTime;

        // Rigidbody�ɗ͂������Ĕ���
        //Rigidbody rb = bullet.GetComponent<Rigidbody>();
        //rb.AddForce(force, ForceMode.Impulse);
    }
}
