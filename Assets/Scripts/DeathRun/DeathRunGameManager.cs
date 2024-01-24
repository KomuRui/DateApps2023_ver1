using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathRunGameManager : MiniGameManager
{
    [SerializeField] private Vector3 goal2DPos;
    [SerializeField] private Vector3 start2DPos;
    [SerializeField] private Vector3 goal3DPos;
    [SerializeField] private Vector3 start3DPos;
    [SerializeField] private GameObject startToGoalCanvas;

    private float startToGoalDis3D;
    private float startToGoalDis2D;

    //シーン開始
    public override void SceneStart() 
    {
        //各距離を求める
        startToGoalDis3D = goal3DPos.z - start3DPos.z;
        startToGoalDis2D = goal2DPos.y - start2DPos.y;
        
        //チュートリアルが終了しているのなら
        if(TutorialManager.isTutorialFinish)
            startToGoalCanvas.SetActive(true);
    }

    //更新
    public override void MiniGameUpdate() 
    {
        //開始していないか終わっているのなら
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish()) return;

        //画像の位置を変える
        for (int i = 0; i < threePlayerImage.Count; i++)
        {
            if (threePlayerObj[i] == null) continue;

            //画像の位置を求める
            float dis3D = threePlayerObj[i].transform.position.z - start3DPos.z;
            float ratio = dis3D / startToGoalDis3D;
            threePlayerImage[i].transform.localPosition = new Vector3(threePlayerImage[i].transform.localPosition.x,
                                                                      start2DPos.y + (startToGoalDis2D * ratio),
                                                                      threePlayerImage[i].transform.localPosition.z);
        }
        


    }

}
