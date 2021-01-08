using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseViewManager : MonoBehaviour
{
    public GameObject singlePlayerRankPrefab;
    public GameObject multiPlayerRankPrefab;
    public Transform singlePlayerRankingParent;
    public Transform multiPlayerRankingParent;
    private DatabaseManager dbManager;

    void Start()
    {
        this.dbManager = new DatabaseManager();
        ShowRankingSinglePlayer();
        ShowRankingMultiPlayer();
    }

    private void ShowRankingSinglePlayer()
    {
        foreach (var elem in dbManager.RankingElementsSingle)
        {
                GameObject tmpObject = Instantiate(singlePlayerRankPrefab);
                tmpObject.GetComponent<RankingElementSinglePlayerDisplay>().SetRankingElement(elem);
                tmpObject.transform.SetParent(singlePlayerRankingParent);
                tmpObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void ShowRankingMultiPlayer()
    {
        foreach (var elem in dbManager.DbElementsMulti)
        {
                GameObject tmpObject = Instantiate(multiPlayerRankPrefab);
                tmpObject.GetComponent<DatabaseElementMultiPlayerDisplay>().SetMultiPlayerElement(elem);
                tmpObject.transform.SetParent(multiPlayerRankingParent);
                tmpObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
