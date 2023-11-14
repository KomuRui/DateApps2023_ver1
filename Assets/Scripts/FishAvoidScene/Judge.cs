using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;

    public GameObject endText;
    public GameObject[] ranking;
    public List<int> temRanking;

    bool isJudge;
    FishCountDown fishCountDown;
    GameObject obj;
    
    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("GameManager");
        fishCountDown = obj.GetComponent<FishCountDown>();
        isJudge = false;
    }

    // Update is called once per frame
    void Update()
    {
        //‚¾‚ê‚à‚¢‚È‚¢
        if((Player1 == null && Player2 == null && Player3 == null) || fishCountDown.isFinish)
        {
            Instantiate(endText, new Vector3(0, 0, 0), Quaternion.identity);
        }

        if(fishCountDown.isFinish)
        {
            isJudge = true;
            fishCountDown.isFinish = false;
        }


        if(isJudge)
        {
            if(Player1 == null && Player2 == null && Player3 == null)
            {
                ranking[0] = Player4;
                //‚Á‚Ä‚«‚Ä
                //temRanking[0];
            }
            else
            {
                ranking[3] = Player4;
                //‚Á‚Ä‚«‚Ä

            }

        }
    }
}