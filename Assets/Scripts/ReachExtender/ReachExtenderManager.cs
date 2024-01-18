using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReachExtenderManager : MiniGameManager
{
    [SerializeField] VortexManager vortexManager;

    //Startと同じ
    public override void SceneStart()
    {
    }

    //ゲーム開始時に呼ばれる
    public override void MiniGameStart()
    {
        vortexManager.StartVortexCoroutine();
    }

    //Updateと同じ
    public override void MiniGameUpdate()
    {
    }

    //ミニゲームが終わったら呼ばれる
    public override void MiniGameFinish()
    {
        vortexManager.SetIsAppearanceVotex(false);
    }
}
