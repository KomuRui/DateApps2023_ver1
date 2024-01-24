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

    //�V�[���J�n
    public override void SceneStart() 
    {
        //�e���������߂�
        startToGoalDis3D = goal3DPos.z - start3DPos.z;
        startToGoalDis2D = goal2DPos.y - start2DPos.y;
        
        //�`���[�g���A�����I�����Ă���̂Ȃ�
        if(TutorialManager.isTutorialFinish)
            startToGoalCanvas.SetActive(true);
    }

    //�X�V
    public override void MiniGameUpdate() 
    {
        //�J�n���Ă��Ȃ����I����Ă���̂Ȃ�
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish()) return;

        //�摜�̈ʒu��ς���
        for (int i = 0; i < threePlayerImage.Count; i++)
        {
            if (threePlayerObj[i] == null) continue;

            //�摜�̈ʒu�����߂�
            float dis3D = threePlayerObj[i].transform.position.z - start3DPos.z;
            float ratio = dis3D / startToGoalDis3D;
            threePlayerImage[i].transform.localPosition = new Vector3(threePlayerImage[i].transform.localPosition.x,
                                                                      start2DPos.y + (startToGoalDis2D * ratio),
                                                                      threePlayerImage[i].transform.localPosition.z);
        }
        


    }

}
