using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : GimmickBase
{
    //特定のアクションを起こす
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
        Debug.Log(other.gameObject.name); // ぶつかった相手の名前を取得
    }
}
