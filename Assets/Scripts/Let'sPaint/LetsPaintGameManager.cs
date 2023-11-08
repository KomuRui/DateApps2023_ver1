using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetsPaintGameManager : MonoBehaviour
{
    [SerializeField] private PaintTarget target;
    [SerializeField] private TextMeshProUGUI[] playerPercentText; //�p�[�Z���g

    private int[] playerPercent;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < playerPercentText.Length; i++)
            playerPercentText[i].SetText("0%");
    }

    // Update is called once per frame
    void Update()
    {
        //�e�v���C���[�̃p�[�Z���g�v�Z
        playerPercentCalc();

    }

    //�e�v���C���[�̃p�[�Z���g�v�Z
    private void playerPercentCalc()
    {
        playerPercent = target.GetPercent(target);
        for (int i = 0; i < playerPercentText.Length; i++)
            playerPercentText[i].SetText(playerPercent[i].ToString() + "%");
    }
}
