using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexManager : MonoBehaviour
{
    [SerializeField] private float instanceTime = 5f;
    [SerializeField] private List<Vortex> vortexList;
    [SerializeField] private GameObject stage;
    [SerializeField] private float radius = 6f;
    [SerializeField] private float VertexRadius = 4f;
    private bool isAppearanceVotex = true;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator VortexInstance()
    {
        yield return new WaitForSeconds(instanceTime);

        Vector3 prePos = Vector3.positiveInfinity;

        //渦の位置を決める
        for (int i = 0; i < vortexList.Count; i++)
        {
            //渦をアクティブに
            vortexList[i].gameObject.SetActive(true);

            //渦の位置を変更
            Vector3 tmp = VertexPositionChange();
            vortexList[i].transform.position = new Vector3(tmp.x, vortexList[i].transform.position.y, tmp.z);

            //渦同士がぶつからないように
            float distance = 9999;

            //二回目なら
            if (prePos != Vector3.positiveInfinity)
            {
                //渦同士の距離を計算
                distance = Vector3.Distance(prePos, vortexList[i].transform.position);
            }

            if (distance < VertexRadius && prePos != Vector3.positiveInfinity)
            {
                i--;
                continue;
            }
            prePos = vortexList[i].transform.position;
        }
        if (isAppearanceVotex) 
            //もう一回コルーチンを呼ぶ
            StartCoroutine(VortexInstance());
    }

    //渦の位置
    public Vector3 VertexPositionChange()
    {
        //ランダムにベクトルを作成
        return stage.transform.position + (RandDirection() * Random.Range(0f, radius));
    }

    public Vector3 RandDirection()
    {
        return new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)).normalized;
    }

    //渦のコルーチン開始
    public void StartVortexCoroutine()
    {
        //コルーチン実行
        StartCoroutine(VortexInstance());
    }

    public void SetIsAppearanceVotex(bool a)
    {
        isAppearanceVotex = a;
    }
}
