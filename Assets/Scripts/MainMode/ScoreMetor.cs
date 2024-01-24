using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreMetor : MonoBehaviour
{

    [SerializeField] private List<GameObject> metor;
    [SerializeField] private Material metorMaterial;
    [SerializeField] private Material metorMaterialAlpha;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int nowScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        nowScore = 4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //現在のスコアにメーターを初期化する
    public void NowScoreMetorInitializ(int score)
    {
        for(int i = 0; i < score; i++)
            metor[i].GetComponent<MeshRenderer>().material = metorMaterial;
    }

    //メーター動く
    IEnumerator MetorMove(float delay,int myPosNum,int nowLookNum)
    {
        yield return new WaitForSeconds(delay);

        if (myPosNum <= nowLookNum)
        {
            metor[nowLookNum].GetComponent<MeshRenderer>().material = metorMaterial;

            //前回見ていた場所が要素オーバーしていないのなら
            if (nowLookNum + 1 < metor.Count)
                metor[nowLookNum + 1].GetComponent<MeshRenderer>().material = metorMaterialAlpha;

            //見ている場所を減らす
            nowLookNum--;
            nowLookNum = Mathf.Max(0, nowLookNum);

            //次を呼ぶ
            if (nowLookNum != 0)
                StartCoroutine(MetorMove(0.01f, myPosNum, nowLookNum));
            else
            {
                nowScore++;
                scoreText.text = nowScore.ToString();
            }
        }
        else
        {
            nowScore++;
            scoreText.text = nowScore.ToString();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //ポイントに当たったら
        if (collision.gameObject.tag == "MetorPoint")
        {
            StartCoroutine(MetorMove(0, collision.gameObject.GetComponent<ScoreMetorPoint>().myPosNum, metor.Count - 1));
            Destroy(collision.gameObject);
        }
    }

}
