using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetsPaintGameManager : MonoBehaviour
{
    [SerializeField] private PaintTarget target;
    [SerializeField] private TextMeshProUGUI[] playerPercent; //パーセント

    private float player0 = 0;
    private float player1 = 0;
    private float player2 = 0;
    private float player3 = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < playerPercent.Length; i++)
            playerPercent[i].SetText("0");
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < target.m_Splats.Count; i++)
        {
            if (target.m_Splats[i].name == "1") player0++;
            if (target.m_Splats[i].name == "2") player1++;
            if (target.m_Splats[i].name == "3") player2++;
            if (target.m_Splats[i].name == "4") player3++;
        }

        float sum = player0 + player1 + player2 + player3;

        if(sum == 0)
        {
            playerPercent[0].SetText("0%");
            playerPercent[1].SetText("0%");
            playerPercent[2].SetText("0%");
            playerPercent[3].SetText("0%");
            return;
        }

        playerPercent[0].SetText(((int)((player0 / sum) * 100)).ToString() + "%");
        playerPercent[1].SetText(((int)((player1 / sum) * 100)).ToString() + "%");
        playerPercent[2].SetText(((int)((player2 / sum) * 100)).ToString() + "%");
        playerPercent[3].SetText(((int)((player3 / sum) * 100)).ToString() + "%");

    }
}
