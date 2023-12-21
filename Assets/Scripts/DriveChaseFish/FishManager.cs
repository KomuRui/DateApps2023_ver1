using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField] private List<Transform> dokanPosList = new List<Transform>();
    [SerializeField] private int oneTimeFishMax = 10;
    [SerializeField] private List<GameObject> noActiveFish = new List<GameObject>();
    [SerializeField] private List<GameObject> noActiveGoldFish = new List<GameObject>();
    [SerializeField] private CountDownAndTimer timer;
    private List<GameObject> activeFish = new List<GameObject>();
    private List<GameObject> activeGoldFish = new List<GameObject>();
    public int fishSumCount;
    public int goldFishCount;
    

    // Start is called before the first frame update
    void Start()
    {
        fishSumCount = 0;
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
                GameObject fish = null;
                //1/15ÇÃämó¶Ç≈â©ã‡ÇÃãõÇê∂ê¨Ç∑ÇÈ
                if(Random.Range(0,15) == 1 && goldFishCount < 3 && fishSumCount < 35)
                {
                    fish = noActiveGoldFish[0];
                    activeGoldFish.Add(fish);
                    noActiveGoldFish.Remove(fish);
                    fish.SetActive(true);
                    fish.transform.position = dokanPosList[i].transform.position;
                    goldFishCount++;
                    fishSumCount++;
                }
                else if(fishSumCount < 35)
                {
                    fish = noActiveFish[0];
                    activeFish.Add(fish);
                    noActiveFish.Remove(fish);
                    fish.SetActive(true);
                    fish.transform.position = dokanPosList[i].transform.position;
                    fishSumCount++;
                }

            }
        }

        StartCoroutine(FishInstantiate(10.0f));
    }

}
