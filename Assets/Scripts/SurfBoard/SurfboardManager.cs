using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfboardManager : MiniGameManager
{
    [SerializeField] List<SurfboardPlayer> threePlayerList;

    //Startと同じ
    public override void SceneStart()
    {
    }

    //Updateと同じ
    public override void MiniGameUpdate()
    {
        ///////////a版のみ////////////////

        bool flag = true;
        foreach (var item in threePlayerList)
        {
            if (!item.isDead)
            {
                flag = false;
            }
        }

        if (flag)
        {
            GameManager.nowMiniGameManager.SetMiniGameFinish();
        }
        //////////////////////////////////
    }

    //ミニゲームが終わったら呼ばれる
    public override void MiniGameFinish()
    {
    }
}
