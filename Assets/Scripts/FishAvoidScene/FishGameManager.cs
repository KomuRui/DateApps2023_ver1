using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishGameManager : MiniGameManager
{

    [SerializeField] private List<GameObject> image;
    [SerializeField] private List<GameObject> imageTutorial;
    public GameObject waterEffectParent;

    // Start is called before the first frame update
    public override void SceneStart()
    {
        //�`���[�g���A����������Ă��Ȃ��̂Ȃ炱�̐揈�����Ȃ�
        if (!TutorialManager.isTutorialFinish) return;

        //�L���Ɩ�����K�p
        for(int i = 0; i < image.Count; i++)
        {
            image[i].SetActive(true);
            imageTutorial[i].SetActive(false);
        }

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


    //�C���J�̐����Ԃ��G�t�F�N�g
    public void WaterEffect(Vector3 pos)
    {
        GameObject ef = null;

        for (int i = 0; i < waterEffectParent.transform.childCount; i++)
        {
            if (!waterEffectParent.transform.GetChild(i).gameObject.activeSelf)
            {
                ef = waterEffectParent.transform.GetChild(i).gameObject;
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

    public void WaterUpEffect(Vector3 pos)
    {
        GameObject ef = null;

        for (int i = 0; i < waterEffectParent.transform.childCount; i++)
        {
            if (!waterEffectParent.transform.GetChild(i).gameObject.activeSelf)
            {
                ef = waterEffectParent.transform.GetChild(i).gameObject;
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
