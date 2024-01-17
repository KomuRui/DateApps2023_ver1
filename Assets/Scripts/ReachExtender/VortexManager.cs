using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexManager : MonoBehaviour
{
    [SerializeField] private float instanceTime = 5f;
    [SerializeField] private List<Vortex> vortexList;

    // Start is called before the first frame update
    void Start()
    {
        //コルーチン実行
        StartCoroutine(VortexInstance());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator VortexInstance()
    {
        yield return new WaitForSeconds(instanceTime);
        foreach (var vortex in vortexList)
        {
            vortex.gameObject.SetActive(true);
        }

        //もう一回コルーチンを呼ぶ
        StartCoroutine(VortexInstance());
    }
}
