using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Floor : MonoBehaviour
{

    //�������ԃe�L�X�g
    [SerializeField] private TextMeshProUGUI[] timeTextMeshPro;

    //���ݐ�������
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
        //���Ԍv�Z�E�\��
        time -= Time.deltaTime;
        for (int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].text = ((int)time).ToString();
    }
}
