using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class LetsPaintGameManager : MiniGameManager
{
    [SerializeField] private PaintTarget target;
    [SerializeField] private GameObject floor;
    public GameObject splashEffectParent;
    public GameObject hitEffectParent;
    private int[] playerPercent;

    //�X�V
    public override void MiniGameUpdate() 
    {
        //�e�v���C���[�̃p�[�Z���g�v�Z
        playerPercentCalc();
    }

    //�e�v���C���[�̃p�[�Z���g�v�Z
    private void playerPercentCalc()
    {
        //�e�N�X�`���擾
        Material m = floor.GetComponent<Renderer>().material;
        Texture t = m.GetTexture("_MainTex");
        Texture2D texture2D = (Texture2D)t;

        ///�s�N�Z�����Ƃ̐F�Ƒ��s�N�Z�������擾
        Color[] pixels = texture2D.GetPixels();
        int totalPixels = texture2D.width * texture2D.height;
        int[] percent = { 0, 0, 0, 0 };
        foreach (Color pixel in pixels)
        {
            Color c = pixel;
            if (pixel.r > .5) { percent[1]++; continue; }
            if (pixel.g > .5) { percent[0]++; continue; }
            if (pixel.b > .5) { percent[2]++; continue; }
            if (pixel.a > .5) { percent[3]++; continue; }
        }

        percent[0] = (int)(((float)percent[0] / totalPixels) * 100);
        percent[1] = (int)(((float)percent[1] / totalPixels) * 100);
        percent[2] = (int)(((float)percent[2] / totalPixels) * 100);
        percent[3] = (int)(((float)percent[3] / totalPixels) * 100);

        int sum = percent[0] + percent[1] + percent[2] + percent[3];
        if (sum > 100)
        {
            sum = sum - 100;
           percent[0] -= sum;
        }

       // return percent;

        //playerPercent = target.GetPercent(target);
    }

    //�Q�[���I�����ɌĂ΂��
    public override void MiniGameFinish()
    {
        int threePlayer = playerPercent[1] + playerPercent[2] + playerPercent[3];
        int onePlayer = playerPercent[0];

        bool isWinOnePLayer = false;

        //1�l�����������̂Ȃ�
        if (threePlayer <= onePlayer)
        {
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
            isWinOnePLayer = true;
        }
        else
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);

        //3�l���̓��_���\�[�g�ŕ��ѕς���
        var dict = new Dictionary<int, int>();
        for (int i = 1; i < playerPercent.Length; i++)
            dict.Add(i, playerPercent[i]);

        var sortedDictionary = dict.OrderByDescending(pair => pair.Value);

        //���ʂ��m�F
        byte nowRank = (isWinOnePLayer ? (byte)1 : (byte)0);
        byte sameRank = 1;
        byte lookNum = 1;
        float beforeValue = -1;
        foreach (var item in sortedDictionary)
        {
            if (beforeValue != item.Value)
            {
                nowRank += sameRank;
                sameRank = 1;
            }
            else
                sameRank++;

            beforeValue = item.Value;
            ScoreManager.AddScore(threePlayerObj[item.Key - 1].GetComponent<PlayerNum>().playerNum, nowRank);
            lookNum++;
        }
    }

    //�C�ɗ��������̃G�t�F�N�g��\��
    public void SplashEffect(Vector3 pos)
    {
        GameObject ef = null;

        for (int i = 0; i < splashEffectParent.transform.childCount; i++)
        {
            if (!splashEffectParent.transform.GetChild(i).gameObject.activeSelf)
            {
                ef = splashEffectParent.transform.GetChild(i).gameObject;
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

    //�����������̃G�t�F�N�g��\��
    public void HitEffect(Vector3 pos)
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
