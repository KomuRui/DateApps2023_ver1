using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaSelectManager : MonoBehaviour
{
    //ÉâÉCÉìêî
    enum LineNum
    {
        ONE,
        TWO,
        THREE,
        MAX_LINE
    }

    [SerializeField] private List<GameObject> line1Chara;
    [SerializeField] private List<GameObject> line2Chara;
    [SerializeField] private List<GameObject> line3Chara;

    private Dictionary<byte, List<GameObject>> lineCharaTable = new Dictionary<byte, List<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        lineCharaTable[(byte)LineNum.ONE] = line1Chara;
        lineCharaTable[(byte)LineNum.TWO] = line2Chara;
        lineCharaTable[(byte)LineNum.THREE] = line3Chara;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
