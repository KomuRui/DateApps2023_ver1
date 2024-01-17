using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreGenerationMetor : MonoBehaviour
{
    [SerializeField] private List<GameObject> metorPointObj;
    [SerializeField] private List<Transform> generationPos;
    [SerializeField] private List<ScoreMetor> scoreMetor;
    [SerializeField] private float generationTime;
    private List<bool> isFinish = new List<bool>();
    public List<int> generationNum;
    public int pointNum;

    // Start is called before the first frame update
    void Start()
    {
        pointNum = 1;
        for (int i = 0; i < metorPointObj.Count; i++)
            isFinish.Add(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    //����
    IEnumerator Generation(float delay,int num,int point)
    {
        yield return new WaitForSeconds(delay);

        if (generationNum[num] > 0)
        {
            //�|�C���g����
            GameObject g = Instantiate(metorPointObj[num], generationPos[num].position, Quaternion.identity);
            g.GetComponent<ScoreMetorPoint>().myPosNum = point;
            point++;
            generationNum[num]--;
            StartCoroutine(Generation(generationTime, num, point));
        }
        else
            StartCoroutine(finish(3, num));
    }

    //����
    IEnumerator finish(float delay, int num)
    {
        yield return new WaitForSeconds(delay);
        isFinish[num] = true;
    }

    //�����J�n
    public void GenerationStart()
    {
        //����
        for(int i = 0; i < metorPointObj.Count; i++)
        {
            if (generationNum[i] > 0)
                StartCoroutine(Generation(2,i, pointNum));
        }
    }

    //�������I���������ǂ���
    public bool IsGeneratioonFinish()
    {
        for (int i = 0; i < isFinish.Count; i++)
            if (!isFinish[i]) return false;

        return true;
    }
}
