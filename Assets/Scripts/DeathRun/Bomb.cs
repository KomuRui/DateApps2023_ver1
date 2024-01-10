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
    }

    void OnTriggerStay(Collider other)
    {
        //もしも爆発していたら
        if (isExplode && other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
        }
        //Debug.Log(other.gameObject.name); // ぶつかった相手の名前を取得
    }

    //爆発処理
    public void Explode()
    {
        isExplode = true;
        Instantiate(explodeParticle, this.gameObject.transform);
        Invoke("ExplodeEnd", explodeTime);
    }

    //爆発が終わったら
    public void ExplodeEnd()
    {
        Destroy(gameObject);
    }
}
