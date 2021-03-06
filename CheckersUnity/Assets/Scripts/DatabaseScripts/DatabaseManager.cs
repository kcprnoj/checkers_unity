﻿using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using Mono.Data.Sqlite;
using UnityEngine;
public class DatabaseManager
{
    private string connectionString;
    private List<DatabaseElementSinglePlayer> dbElementsSingle = new List<DatabaseElementSinglePlayer>();
    public List<DatabaseElementMultiPlayer> DbElementsMulti = new List<DatabaseElementMultiPlayer>();

    public List<RankingElementSinglePlayer> RankingElementsSingle = new List<RankingElementSinglePlayer>();

    public DatabaseManager()
    {
        connectionString = "URI=file:" + Application.dataPath + "/Database.db";
        GetScores();
        CreateRankingTableSinglePlayer();
    }

    public void InsertScoreIntoSinglePlayerTable(string name, string side,string status, int time)
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

    public void InsertScoreIntoMultiPlayerTable(string playerName, string playerSide, string enemyName, string enemySide, string winner, int time)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("INSERT INTO MultiPlayerTable(PlayerName,PlayerSide,EnemyName,EnemySide,Winner,Time) VALUES(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")", playerName, playerSide, enemyName, enemySide, winner, time);

                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }

    private void GetScores()
    {
        dbElementsSingle.Clear();

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM SinglePlayerScores";
                string sqlQuery2 = "SELECT * FROM MultiPlayerTable";

                dbCmd.CommandText = sqlQuery;

                using(IDataReader reader = dbCmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        dbElementsSingle.Add(new DatabaseElementSinglePlayer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4)));
                    }
                    reader.Close();
                }

                dbCmd.CommandText = sqlQuery2;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DbElementsMulti.Add(new DatabaseElementMultiPlayer(reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetInt32(6)));
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
    }

    private void CreateRankingTableSinglePlayer()
    {
        RankingElementsSingle.Clear();
        var dbElemsGroupedByName = dbElementsSingle.GroupBy(elem =>elem.Name);
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

                if (time > groupElem.Time && groupElem.Status == "Win")
                {
                    time = groupElem.Time;
                    side = groupElem.Side;
                }     
            }
            RankingElementsSingle.Add(new RankingElementSinglePlayer(0, group.Key, wins, defeats, side, time, wins-defeats));
        }

        RankingElementsSingle.Sort(
            delegate(RankingElementSinglePlayer r1, RankingElementSinglePlayer r2)
            {
                return r2.Points.CompareTo(r1.Points);
            });
        for(int i=0; i < RankingElementsSingle.Count; i++)
        {
            RankingElementsSingle[i].RankNumber = i + 1;
        }
    }
}
