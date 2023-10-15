using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Floor : MonoBehaviour
{

    //制限時間テキスト
    [SerializeField] private TextMeshProUGUI[] timeTextMeshPro;

    //現在制限時間
    private float time = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].text = "30";
    }

    // Update is called once per frame
    void Update()
    {
        //時間計算・表示
        time -= Time.deltaTime;
        for (int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].text = ((int)time).ToString();
    }
}
