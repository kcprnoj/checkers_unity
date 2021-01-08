public class DatabaseElementSinglePlayer
{
    public string Name { get; set; }
    public string Side { get; set; }
    public string Status { get; set; }
    public int Time { get; set; }
    public int Id { get; set; }

    public DatabaseElementSinglePlayer(int id, string name, string side,string status, int time)
    {
        this.Id = id;
        this.Name = name;
        this.Side = side;
        this.Time = time;
        this.Status = status;
    }
}
