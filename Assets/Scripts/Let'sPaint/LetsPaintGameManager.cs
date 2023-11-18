using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetsPaintGameManager : MiniGameManager
{
    [SerializeField] private PaintTarget target;
    [SerializeField] private TextMeshProUGUI[] playerPercentText; //パーセント

    private int[] playerPercent;


    // Update is called once per frame
    void Update()
    {
        //各プレイヤーのパーセント計算
        playerPercentCalc();
    }

    //各プレイヤーのパーセント計算
    private void playerPercentCalc()
    {
        playerPercent = target.GetPercent(target);
        for (int i = 0; i < playerPercentText.Length; i++)
            playerPercentText[i].SetText(playerPercent[i].ToString() + "%");
    }
}
