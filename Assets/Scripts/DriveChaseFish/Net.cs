using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Net : MonoBehaviour
{

    [SerializeField] private Vector3 impositionScale; //����������̊g�嗦
    private Vector3 impositionPos; //����������̈ʒu
    private Vector3 impositionPos2;//����������̈ʒu2
    private Vector3 initialPos;    //�����ʒu
    private Vector3 initialScale;  //�����g�嗦

    // Start is called before the first frame update
    void Start()
    {
        //�ʒu�Ɗg�嗦��ۑ����Ă���
        initialPos = transform.position;
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�Ԃ𔭓�
    public void NetExecute()
    {
        
    }
}
