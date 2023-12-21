using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] public Transform[] goalPoint; //巡回するときの巡るポイント
    [SerializeField] public Transform[] fallPoint; //プールに魚が落ちる時のポイント
    public int playerNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
