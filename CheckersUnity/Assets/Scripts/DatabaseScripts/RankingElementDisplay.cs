using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingElementDisplay : MonoBehaviour
{
    public GameObject rank;
    public GameObject playerName;
    public GameObject gamesWonNumber;
    public GameObject gamesLostNumber;
    public GameObject bestGame;

    public void SetRankingElement(RankingElement elem)
    {
        this.rank.GetComponent<TMPro.TextMeshProUGUI>().text = "# " + elem.RankNumber.ToString();
        this.playerName.GetComponent<TMPro.TextMeshProUGUI>().text = elem.Name;
        this.gamesWonNumber.GetComponent<TMPro.TextMeshProUGUI>().text = elem.NumberOfWins.ToString();
        this.gamesLostNumber.GetComponent<TMPro.TextMeshProUGUI>().text = elem.NumberOfDefeats.ToString();
        this.bestGame.GetComponent<TMPro.TextMeshProUGUI>().text = "Best Game: Side -> " + elem.Side + "Time -> " + elem.Time.ToString() + " s";
    }
}
