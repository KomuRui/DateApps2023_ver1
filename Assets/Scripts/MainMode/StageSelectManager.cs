using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{

    [SerializeField] private float notSelectAlpha = 0.2f;
    [SerializeField] private Vector3 notSelectScale = new Vector3(2.4f,1.8f,1.0f);
    [SerializeField] private List<GameObject> stageImageObj;

    private float selectAlpha = 1.0f;
    private Vector3 selectScale = new Vector3(3.4f, 2.6f, 1.0f);

    private int lookMiniGameNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        //‚P•bŒã‚ÉŠJŽn
        StartCoroutine(RandomMiniGameSelectStart(1.0f));
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator RandomMiniGameSelectStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        for(int i = 0; i < stageImageObj.Count; i++)
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

        //Œ©‚éˆÊ’u•Ï‚¦‚é
        lookMiniGameNum++;
        if (lookMiniGameNum >= stageImageObj.Count) lookMiniGameNum = 0;

        //0.3•bŒã‚ÉŠJŽn
        StartCoroutine(RandomMiniGameSelectStart(0.3f));
    }
}
