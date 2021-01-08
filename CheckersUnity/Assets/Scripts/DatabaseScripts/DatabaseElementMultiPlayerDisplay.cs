using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseElementMultiPlayerDisplay : MonoBehaviour
{
    public GameObject playerName;
    public GameObject enemyName;
    public GameObject winner;
    public GameObject time;
    public void SetMultiPlayerElement(DatabaseElementMultiPlayer elem)
    {
        this.playerName.GetComponent<TMPro.TextMeshProUGUI>().text = elem.PlayerName + "\n( " + elem.PlayerSide + " )";
        this.enemyName.GetComponent<TMPro.TextMeshProUGUI>().text = elem.EnemyName + "\n( " + elem.EnemySide + " )"; ;
        this.winner.GetComponent<TMPro.TextMeshProUGUI>().text = elem.Winner;
        this.time.GetComponent<TMPro.TextMeshProUGUI>().text = elem.Time.ToString();
    }
}
