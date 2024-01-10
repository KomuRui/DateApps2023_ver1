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
        //球を発射
        Shot();
    }

    public override void GimmickStart()
    {
    }

    public override void GimmickUpdate()
    {
    }

    //球を発射
    public void Shot()
    {
        //エフェクト
        Instantiate(shootParticle, shootParticlePos.transform);

        //弾を表示
        bullet.gameObject.SetActive(true);

        //弾を発射位置に
        bullet.transform.position = shotPos.transform.position;

        //発射ベクトル
        Vector3 force = transform.forward;
        bullet.GetComponent<DeathRunBullet>().SetMoveDirection(force);

        //速度を決定
        bullet.GetComponent<DeathRunBullet>().SetMoveSpeed(bulletSpeed);


        //force *= bulletSpeed * Time.deltaTime;

        // Rigidbodyに力を加えて発射
        //Rigidbody rb = bullet.GetComponent<Rigidbody>();
        //rb.AddForce(force, ForceMode.Impulse);
    }
}
