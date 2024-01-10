using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : GimmickBase
{
    //爆発しているかどうか
    private bool isExplode = false;

    [SerializeField] private float explodeTime = 3f;
    [SerializeField] private GameObject explodeParticle;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed = 180.0f;    // プレイヤーの回転速度

    private int sign = 1;

    //特定のアクションを起こす
    public override void Action()
    {
        Explode();
    }

    public override void GimmickStart()
    {
    }

    public override void GimmickUpdate()
    {
        // 移動方向を計算
        Vector3 moveDirection = Vector3.right * sign;

        // 移動
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerStay(Collider other)
    {
        //もしも爆発していたら
        if (isExplode && other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<DeathRunPlayer>().FallPlayer();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player") sign *= -1;
    }

    //爆発処理
    public void Explode()
    {
        isExplode = true;
        explodeParticle.transform.position = transform.position;
        GameObject o = Instantiate(explodeParticle, this.transform);
        o.transform.position = transform.position;
        o.transform.localScale = transform.localScale;
        Invoke("ExplodeEnd", explodeTime);
    }

    //爆発が終わったら
    public void ExplodeEnd()
    {
        Destroy(gameObject);
    }
}
