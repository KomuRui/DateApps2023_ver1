using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreGenerationMetor : MonoBehaviour
{
    [SerializeField] private List<GameObject> metorPointObj;
    [SerializeField] private List<Transform> generationPos;
    [SerializeField] private float generationTime;
    public List<int> generationNum;
    public int pointNum;

    // Start is called before the first frame update
    void Start()
    {
        pointNum = 1;
        //GenerationStart();
    }

    // Update is called once per frame
    void Update()
    {
    }

    //����
    IEnumerator Generation(float delay,int num,int point,Vector3 pos, GameObject metorPointObj)
    {
        yield return new WaitForSeconds(delay);

        if (num > 0)
        {
            //�|�C���g����
            GameObject g = Instantiate(metorPointObj, pos, Quaternion.identity);
            g.GetComponent<ScoreMetorPoint>().myPosNum = point;
            point++;
            num--;
            StartCoroutine(Generation(generationTime, num, point, pos, metorPointObj));
        }
    }

    //�����J�n
    public void GenerationStart()
    {
        //����
        for(int i = 0; i < metorPointObj.Count; i++)
        {
            if (generationNum[i] > 0)
                StartCoroutine(Generation(2,generationNum[i], pointNum, generationPos[i].position, metorPointObj[i]));
        }
    }
}
