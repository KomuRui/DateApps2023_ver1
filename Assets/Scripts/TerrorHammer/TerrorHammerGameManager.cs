using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrorHammerGameManager : MiniGameManager
{
    [NonSerialized] public List<byte> goalPlayer = new List<byte>();
    public GameObject hitEffectParent;

    /// Start is called before the first frame update
    public override void SceneStart()
    {

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

        //�����c���Ă���l�ɏ��ʂ�����
        foreach (var player in goalPlayer)
        {
            ScoreManager.AddScore(player, nowRank);
            nowRank++;
        }

        //3�l���̓��_���\�[�g�ŕ��ѕς���
        foreach (var item in threePlayer)
        {
            //�S�[�����Ă���̂Ȃ炱�̐揈�����Ȃ�
            if (goalPlayer.Contains(item.Key)) continue;

            //�X�R�A���Z
            ScoreManager.AddScore(item.Key, nowRank);
        }
    }

    //�n���}�[�̃q�b�g�G�t�F�N�g��\��
    public void HammerHitEffect(Vector3 pos)
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
       
        if(ef != null)
        {
            ef.transform.position = pos;
            ef.SetActive(true);
            ef.GetComponent<ParticleSystem>().Play();
        }
    }
}
