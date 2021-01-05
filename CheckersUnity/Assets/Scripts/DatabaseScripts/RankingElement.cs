using System.Collections;
using System.Collections.Generic;

public class RankingElement
{
    public int RankNumber { get; set; }
    public string Name { get; set; }
    public int NumberOfWins { get; set; }
    public int NumberOfDefeats { get; set; }
    public string Side { get; set; }
    public int Time { get; set; }
    public int Points { get; set; }

    public RankingElement(int rank, string name, int wins, int defeats,string side, int time, int points)
    {
        this.RankNumber = rank;
        this.Name = name;
        this.NumberOfWins = wins;
        this.NumberOfDefeats = defeats;
        this.Side = side;
        this.Time = time;
        this.Points = points;
    }

}
