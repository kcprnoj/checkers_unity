using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class UIData
{
    public static string Color{ get; set; }

    public static string Winner{ get; set; }

    public static string GameMode { get; set; }

    public static string Name { get; set; }

    public static int StartTime { get; set; }

    public static bool IsHost { get; set; }

    public static int GetUnixTime()
    {
        return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }
}
