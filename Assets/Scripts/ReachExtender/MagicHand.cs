using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicHand : MonoBehaviour
{
    //[SerializeField] private GameObject onePlayer;
    // Start is called before the first frame update

    private bool bigMax = false;        //マジックハンドが伸びきったかどうか
    [SerializeField] private GameObject nextArmParent;


    void Start()
    {
        //1秒後に伸びきる
        Invoke("BigMax", 5f);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!bigMax)
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + 0.001f, transform.localScale.z );
    }

    public void BigMax()
    {
        bigMax = true;
        if(nextArmParent != null)
        {
            nextArmParent.SetActive(true);
        }
    }
}
