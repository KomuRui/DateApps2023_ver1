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
        int redNum = target.GetRedPercent(target);
        int orengeNum = target.GetOrengePercent(target);
        playerPercent[0].SetText(redNum.ToString() + "%");
        playerPercent[1].SetText(orengeNum.ToString() + "%");
        playerPercent[2].SetText(orengeNum.ToString() + "%");
        playerPercent[3].SetText(orengeNum.ToString() + "%");
    }
}
