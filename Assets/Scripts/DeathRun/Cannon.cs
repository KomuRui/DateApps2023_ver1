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
        GameObject createdBullet = Instantiate(bullet) as GameObject;
        createdBullet.transform.position = shotPos.transform.position;

        //発射ベクトル
        Vector3 force = new Vector3(1,0,0);

        //発射の向きと速度を決定
        force *= bulletSpeed * Time.deltaTime;

        // Rigidbodyに力を加えて発射
        createdBullet.GetComponent<Rigidbody>().AddForce(force);
    }
}
