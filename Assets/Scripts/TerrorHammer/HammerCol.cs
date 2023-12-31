using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerCol : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Floor")
        {
            ((TerrorHammerGameManager)GameManager.nowMiniGameManager).HammerHitEffect(new Vector3(other.contacts[0].point.x, other.contacts[0].point.y + 0.1f, other.contacts[0].point.z));
        }
    }

}
