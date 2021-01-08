public class DatabaseElementMultiPlayer
{
    public string PlayerName { get; set; }
    public string PlayerSide { get; set; }
    public string EnemyName { get; set; }
    public string EnemySide { get; set; }
    public string Winner { get; set; }
    public int Time { get; set; }

    public DatabaseElementMultiPlayer(string playerName, string playerSide, string enemyName, string enemySide, string winner, int time)
    {
        this.PlayerName = playerName;
        this.PlayerSide = playerSide;
        this.EnemySide = enemySide;
        this.EnemyName = enemyName;
        this.Time = time;
        this.Winner = winner;
    }
}
