using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RankingElementDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject rank;
    public GameObject playerName;
    public GameObject gamesWonNumber;
    public GameObject gamesLostNumber;
    public GameObject bestGame;
    public GameObject rankDetails;

    public void OnPointerEnter(PointerEventData eventData)
    {
        rankDetails.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rankDetails.SetActive(false);
    }

    public void SetRankingElement(RankingElement elem)
    {
        this.rank.GetComponent<TMPro.TextMeshProUGUI>().text = "# " + elem.RankNumber.ToString();
        this.playerName.GetComponent<TMPro.TextMeshProUGUI>().text = elem.Name;
        this.gamesWonNumber.GetComponent<TMPro.TextMeshProUGUI>().text = elem.NumberOfWins.ToString();
        this.gamesLostNumber.GetComponent<TMPro.TextMeshProUGUI>().text = elem.NumberOfDefeats.ToString();
        this.bestGame.GetComponent<TMPro.TextMeshProUGUI>().text = "Best Game:\nSide -> " + elem.Side + "\tTime -> " + elem.Time.ToString() + " s";
    }

    public void ToggleDetails()
    {
        if (rankDetails.activeSelf)
            rankDetails.SetActive(false);
        else
        {
            rankDetails.GetComponent<RectTransform>().SetAsLastSibling();
            rankDetails.SetActive(true); 
        }
    }

}
