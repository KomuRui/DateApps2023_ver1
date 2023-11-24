using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private Fade fade;
    [SerializeField] private float fadeTime;
    [SerializeField] private float notSelectAlpha = 0.2f;
    [SerializeField] private Vector3 notSelectScale = new Vector3(2.4f, 1.8f, 1.0f);
    [SerializeField] private  List<GameObject> stageImageObj;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private GameObject rankText;   

    
    private float changeTime = 0.1f;
    private int finishImage = 0;
    private int count = 0;
    private bool isFinish = false;

    private float selectAlpha = 1.0f;
    private Vector3 selectScale = new Vector3(3.4f, 2.6f, 1.0f);

    private int lookMiniGameNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.Initializ();
        ScoreManager.Initializ();

        //fade.FadeOut(fadeTime);
        roundText.text = StageSelectManager.GetNowRound() + "/4";

        //順位テキスト表示
        for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
        {
            rankText.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text
                = ScoreManager.GetRank((byte)(i + 1)).ToString() + "位 " + ScoreManager.GetScore((byte)(i + 1)).ToString() + "P";
        }
        Instantiate(rankText, new Vector3(0, 0, 0), Quaternion.identity);

        //2秒後に開始
        StartCoroutine(RandomMiniGameSelectStart(2.0f));
    }

    // Update is called once per frame
    void Update()
    {
    }

    //シーン遷移開始
    IEnumerator StartSceneChange(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("Let'sPaint");
        //fade.FadeIn(fadeTime);
    }

    IEnumerator RandomMiniGameSelectStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        count++;

        for (int i = 0; i < stageImageObj.Count; i++)
        {
            Color c = stageImageObj[i].GetComponent<MeshRenderer>().material.color;

            if (lookMiniGameNum == i)
            {
                stageImageObj[i].transform.localScale = selectScale;
                c.a = selectAlpha;

            }
            else
            {
                stageImageObj[i].transform.localScale = notSelectScale;
                c.a = notSelectAlpha;
            }

            stageImageObj[i].GetComponent<MeshRenderer>().material.color = c;
        }

        //見る位置変える
        lookMiniGameNum++;
        if (lookMiniGameNum >= stageImageObj.Count)
        {
            lookMiniGameNum = 0;
            changeTime += Random.Range(1 + count, 3 + count) / 100.0f;
            if (changeTime > 0.6f)
            {
                isFinish = true;
                finishImage = Random.Range(0, stageImageObj.Count);
            }
        }

        //終わりなら
        if (isFinish && lookMiniGameNum == finishImage)
            StartCoroutine(StartSceneChange(2.0f));
        else
            //0.3秒後に開始
            StartCoroutine(RandomMiniGameSelectStart(changeTime));

    }
}
