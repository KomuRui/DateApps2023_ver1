using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetsPaintGameManager : MonoBehaviour
{
    [SerializeField] private PaintTarget target;

    private int player0 = 0;
    private int player1 = 0;
    private int player2 = 0;
    private int player3 = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < target.m_Splats.Count; i++)
        {
            if (target.m_Splats[i].name == "1") player0++;
            if (target.m_Splats[i].name == "2") player1++;
            if (target.m_Splats[i].name == "3") player2++;
            if (target.m_Splats[i].name == "4") player3++;
        }

        Debug.Log(player0);
        Debug.Log(player1);
        Debug.Log(player2);
        Debug.Log(player3);
    }
}
