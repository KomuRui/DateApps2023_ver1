using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonThreePlayer : GimmickBase
{
    [SerializeField] private GameObject shotPos;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 15f;

    private bool isHit = false;

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
        if ((Input.GetButtonDown("Abutton2") && isHit) || (Input.GetButtonDown("Abutton3") && isHit) || (Input.GetButtonDown("Abutton4") && isHit))
            this.SetIsInput(true);
    }

    //球を発射
    public void Shot()
    {
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

    void OnTriggerStay(Collider other)
    {
        isHit = true;
    }

    void OnTriggerExit(Collider other)
    {
        isHit = false;
    }
}
