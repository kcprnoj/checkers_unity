using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseViewManager : MonoBehaviour
{
    public GameObject rankPrefab;
    public Transform rankingParent;
    private DatabaseManager dbManager;

    void Start()
    {
        this.dbManager = new DatabaseManager();
        ShowRanking();
    }

    private void ShowRanking()
    {
        foreach (var elem in dbManager.RankingElements)
        {
            if(elem.RankNumber < 11)
            {
                GameObject tmpObject = Instantiate(rankPrefab);
                tmpObject.GetComponent<RankingElementDisplay>().SetRankingElement(elem);
                tmpObject.transform.SetParent(rankingParent);
                tmpObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
