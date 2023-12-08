using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField] private List<Transform> dokanPosList = new List<Transform>();
    [SerializeField] private int oneTimeFishMax = 10;
    [SerializeField] private List<GameObject> noActiveFish = new List<GameObject>();
    [SerializeField] private CountDownAndTimer timer;
    private List<GameObject> ActiveFish = new List<GameObject>();
    private int goldFishCount;
    

    // Start is called before the first frame update
    void Start()
    {
        goldFishCount = 0;
        StartCoroutine(FishInstantiate(4.0f));
    }

    // Update is called once per frame
    void Update()
    {
    }

    //ãõê∂ê¨
    IEnumerator FishInstantiate(float delay)
    {
        yield return new WaitForSeconds(delay);

        //Ç«ÇÃìyä«Ç…âΩïCèoåªÇ≥ÇπÇÈÇ©åàÇﬂÇÈ
        int[] fishCount = new int[dokanPosList.Count];
        for (int i = 0; i < oneTimeFishMax; i++)
        {
            if(fishCount[Random.Range(0, fishCount.Length)] < 5)
                fishCount[Random.Range(0, fishCount.Length)]++;
            else
            {
                bool isOK = false;
                for(int j = 0; j < fishCount.Length;j++)
                {
                    if (fishCount[j] < 5 && !isOK)
                    {
                        fishCount[j]++;
                        isOK = true;
                    }
                }
            }
        }

        //ãõê∂ê¨Ç∑ÇÈ
        for (int i = 0; i < fishCount.Length; i++)
        {
            for (int j = 0; j < fishCount[i]; j++)
            {
                GameObject fish = noActiveFish[0];
                fish.SetActive(true);
                fish.transform.position = dokanPosList[i].transform.position;
                ActiveFish.Add(fish);
                noActiveFish.Remove(fish);
            }
        }

        StartCoroutine(FishInstantiate(10.0f));
    }

}
