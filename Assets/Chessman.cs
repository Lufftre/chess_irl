using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chessman : MonoBehaviour {

    public int CurrentX { set; get; }
    public int CurrentY { set; get; }
    public bool isWhite;
    public int nMoves { set; get; }

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public bool PossibleMove(int x, int y)
    {
        return PossibleMoves()[x, y];
    }


    public virtual bool[,] PossibleMoves()
    {
        return new bool[8,8];
    }
}
