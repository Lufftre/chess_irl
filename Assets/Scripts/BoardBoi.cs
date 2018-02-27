using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBoi : MonoBehaviour {
    public static BoardBoi Instace { set; get; }

    public bool[,] allowedMoves { set; get; }

    public Chessman[,] Chessmans { set; get; }
}
