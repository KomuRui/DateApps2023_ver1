using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarryBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f; 　　　　　　　　//移動速度
    [SerializeField] private GameObject cannon;      　　　　　　　　//大砲オブジェクト
    [SerializeField] private CarryToTheGoalGameManager manager;      //マネジャー                                                        //マネージャー
    [SerializeField] private CarrySE se;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //オブジェクトがアクティブなら移動
        if (this.gameObject.activeSelf)
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CarryStage")
        {
            this.gameObject.SetActive(false);
            transform.position = cannon.gameObject.transform.position;
        }
        if(other.gameObject.tag == "Player")
        {
            se.RockAudio();
            this.gameObject.SetActive(false);
            transform.position = cannon.gameObject.transform.position;

            if (!other.gameObject.GetComponent<CarryToTheGoalPlayer>().isMuteki)
            {
                other.GetComponent<CarryToTheGoalPlayer>().Damege();
                manager.Damege(other.gameObject.GetComponent<PlayerNum>().playerNum);
            }

        }
    }
}
