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
            other.gameObject.GetComponent<DeathRunPlayer>().FallPlayer();
        }
    }

    //爆発処理
    public void Explode()
    {
        isExplode = true;
        explodeParticle.transform.position = transform.position;
        Instantiate(explodeParticle, explodeParticle.transform);
        Invoke("ExplodeEnd", explodeTime);
    }

    //爆発が終わったら
    public void ExplodeEnd()
    {
        Destroy(gameObject);
    }
}
