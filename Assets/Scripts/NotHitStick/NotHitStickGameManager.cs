using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NotHitStickGameManager : MiniGameManager
{
    public int kill = 0;
    public Rotate1 rotate1;
    public Rotate2 rotate2;
    public GameObject[] floor;
    public GameObject hitEffectParent;
    public GameObject tyakutiEffectParent;

    public override void SceneStart()
    {
        rotate1.playerNum = onePlayerObj.GetComponent<PlayerNum>().playerNum;
        rotate2.playerNum = onePlayerObj.GetComponent<PlayerNum>().playerNum;

        int i = 2;
        foreach (var player in threePlayerObj)
        {
            player.GetComponent<NotHitStickPlayer>().stage = floor;
            player.GetComponent<NotHitStickPlayer>().nowStageNum = i;
            i--;
        }
    }

    public override void MiniGameUpdate()
    {
    }

    //�Q�[���I�����ɌĂ΂��
    public override void MiniGameFinish()
    {

        //�P�l�������������ǂ���
        bool isWinOnePLayer = false;

        //�v���C���[�����ׂĎ���ł���̂Ȃ�
        if (isPlayerAllDead)
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
        foreach (var player in threePlayer)
        {
            //�����Ă���̂Ȃ�
            if (!player.Value)
            {
                ScoreManager.AddScore(player.Key, nowRank);
                sameRank++;
            }
        }

        //3�l���̓��_���\�[�g�ŕ��ѕς���
        var sortedDictionary = lifeTime.OrderByDescending(pair => pair.Value);
        float beforeValue = -1;
        foreach (var item in sortedDictionary)
        {
            //�����Ă���̂Ȃ炱�̐揈�����Ȃ�
            if (!threePlayer[item.Key]) continue;

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

    //���n�G�t�F�N�g��\��
    public void tyakutiEffect(Vector3 pos)
    {
        GameObject ef = null;

        for (int i = 0; i < tyakutiEffectParent.transform.childCount; i++)
        {
            if (!tyakutiEffectParent.transform.GetChild(i).gameObject.activeSelf)
            {
                ef = tyakutiEffectParent.transform.GetChild(i).gameObject;
                break;
            }
        }

        if (ef != null)
        {
            ef.transform.position = pos;
            ef.SetActive(true);
            ef.GetComponent<ParticleSystem>().Play();
        }
    }

    //�v���C���[���񂾎��̃G�t�F�N�g��\��
    public void hitEffect(Vector3 pos)
    {
        GameObject ef = null;

        for (int i = 0; i < hitEffectParent.transform.childCount; i++)
        {
            if (!hitEffectParent.transform.GetChild(i).gameObject.activeSelf)
            {
                ef = hitEffectParent.transform.GetChild(i).gameObject;
                break;
            }
        }

        if (ef != null)
        {
            ef.transform.position = pos;
            ef.SetActive(true);
            ef.GetComponent<ParticleSystem>().Play();
        }
    }

}
