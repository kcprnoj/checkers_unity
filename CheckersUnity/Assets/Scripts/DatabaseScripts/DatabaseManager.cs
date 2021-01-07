using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using Mono.Data.Sqlite;
using UnityEngine;
public class DatabaseManager
{
    private string connectionString;
    private List<DatabaseElement> dbElements = new List<DatabaseElement>();

    public List<RankingElement> RankingElements = new List<RankingElement>();

    public DatabaseManager()
    {
        connectionString = "URI=file:" + Application.dataPath + "/Database.db";
        GetScores();
        CreateRankingTable();
    }

    public void InsertScore(string name, string side,string status, int time)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("INSERT INTO SinglePlayerScores(Name,Side,Status,Time) VALUES(\"{0}\",\"{1}\",\"{2}\",\"{3}\")", name,side,status,time);

                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }

    private void GetScores()
    {
        dbElements.Clear();

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM SinglePlayerScores";

                dbCmd.CommandText = sqlQuery;

                using(IDataReader reader = dbCmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        dbElements.Add(new DatabaseElement(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4)));
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
    }

    private void CreateRankingTable()
    {
        RankingElements.Clear();
        var dbElemsGroupedByName = dbElements.GroupBy(elem =>elem.Name);
        foreach(var group in dbElemsGroupedByName)
        {
            int wins=0;
            int defeats=0;
            int time=int.MaxValue;
            string side="";
            foreach(var groupElem in group)
            {
                if (groupElem.Status == "Win")
                    wins++;
                else if (groupElem.Status == "Defeat")
                    defeats++;

                if (time > groupElem.Time)
                {
                    time = groupElem.Time;
                    side = groupElem.Side;
                }     
            }
            RankingElements.Add(new RankingElement(0, group.Key, wins, defeats, side, time, wins-defeats));
        }

        RankingElements.Sort(
            delegate(RankingElement r1, RankingElement r2)
            {
                return r2.Points.CompareTo(r1.Points);
            });
        for(int i=0; i < RankingElements.Count; i++)
        {
            RankingElements[i].RankNumber = i + 1;
        }
    }
}
