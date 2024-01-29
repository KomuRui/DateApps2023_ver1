using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeathRunGameManager : MiniGameManager
{
    [SerializeField] private Vector3 goal2DPos;
    [SerializeField] private Vector3 start2DPos;
    [SerializeField] private Vector3 goal3DPos;
    [SerializeField] private Vector3 start3DPos;
    [SerializeField] private GameObject startToGoalCanvas;
    [NonSerialized] public List<byte> goalPlayer = new List<byte>();

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

    //�Q�[���I�����ɌĂ΂��
    public override void MiniGameFinish()
    {

        //�P�l�������������ǂ���
        bool isWinOnePLayer = false;

        //�S�[�������v���C���[�����Ȃ��̂Ȃ�
        if (goalPlayer.Count <= 0)
        {
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
            isWinOnePLayer = true;
        }
        else
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);

        //���ʂ��m�F
        byte nowRank = (isWinOnePLayer ? (byte)2 : (byte)1);
        byte sameRank = 0;

        //�����c���Ă���l�ɏ��ʂ�����
        foreach (var player in goalPlayer)
        {
            ScoreManager.AddScore(player, nowRank);
            nowRank++;
        }

        Dictionary<byte, float> deadDis = new Dictionary<byte, float>();
        int lookNum = 0;
        foreach (var item in threePlayer)
        {
            deadDis[item.Key] = threePlayerImage[lookNum].transform.localPosition.y - start2DPos.y;
            lookNum++;
        }

        //3�l���̓��_���\�[�g�ŕ��ѕς���
        var sortedDictionary = deadDis.OrderByDescending(pair => pair.Value);
        float beforeValue = -1;
        foreach (var item in sortedDictionary)
        {
            //�S�[�����Ă���̂Ȃ炱�̐揈�����Ȃ�
            if (goalPlayer.Contains(item.Key)) continue;

            //�O��̒l�ƈႤ�̂Ȃ��
            if (beforeValue != item.Value)
            {
                nowRank += sameRank;
                sameRank = 1;
            }
            else
                sameRank++;

            beforeValue = item.Value;
            ScoreManager.AddScore(item.Key, nowRank);
        }
    }

}
