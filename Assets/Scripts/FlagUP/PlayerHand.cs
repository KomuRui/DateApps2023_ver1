using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private int playerNum;                   // ÉvÉåÉCÉÑÅ[î‘çÜ
    public GameObject redOb;
    public GameObject whiteOb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("RBbutton" + playerNum))
        {
            redOb.transform.eulerAngles = (new Vector3(0, 0, 90));
        }
        else if (Input.GetButtonDown("LBbutton" + playerNum))
        {
            whiteOb.transform.eulerAngles = (new Vector3(0, 0, 90));
        }
    }
}
