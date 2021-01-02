using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIData
{
    private static string color;
    private static string winner;

    public static string Color
    {
        get { return color; }
        set { color = value; }
    }

    public static string Winner
    {
        get { return winner; }
        set { winner = value; }
    }
}
