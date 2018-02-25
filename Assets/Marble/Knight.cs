using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chessman
{

    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[8, 8];
        Chessman c;
        int[] target;


        target = new int[] { CurrentX + 2, CurrentY + 1 };
        if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
        {
            c = BoardManager.Instace.Chessmans[target[0], target[1]];
            if (c == null || c.isWhite != isWhite)
            {
                r[target[0], target[1]] = true;
            }
        }

        target = new int[] { CurrentX + 2, CurrentY - 1 };
        if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
        {
            c = BoardManager.Instace.Chessmans[target[0], target[1]];
            if (c == null || c.isWhite != isWhite)
            {
                r[target[0], target[1]] = true;
            }
        }

        target = new int[] { CurrentX - 2, CurrentY + 1 };
        if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
        {
            c = BoardManager.Instace.Chessmans[target[0], target[1]];
            if (c == null || c.isWhite != isWhite)
            {
                r[target[0], target[1]] = true;
            }
        }

        target = new int[] { CurrentX - 2, CurrentY - 1 };
        if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
        {
            c = BoardManager.Instace.Chessmans[target[0], target[1]];
            if (c == null || c.isWhite != isWhite)
            {
                r[target[0], target[1]] = true;
            }
        }




        target = new int[] { CurrentX + 1, CurrentY + 2 };
        if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
        {
            c = BoardManager.Instace.Chessmans[target[0], target[1]];
            if (c == null || c.isWhite != isWhite)
            {
                r[target[0], target[1]] = true;
            }
        }

        target = new int[] { CurrentX + 1, CurrentY - 2 };
        if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
        {
            c = BoardManager.Instace.Chessmans[target[0], target[1]];
            if (c == null || c.isWhite != isWhite)
            {
                r[target[0], target[1]] = true;
            }
        }

        target = new int[] { CurrentX - 1, CurrentY + 2 };
        if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
        {
            c = BoardManager.Instace.Chessmans[target[0], target[1]];
            if (c == null || c.isWhite != isWhite)
            {
                r[target[0], target[1]] = true;
            }
        }

        target = new int[] { CurrentX - 1, CurrentY - 2 };
        if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
        {
            c = BoardManager.Instace.Chessmans[target[0], target[1]];
            if (c == null || c.isWhite != isWhite)
            {
                r[target[0], target[1]] = true;
            }
        }




        return r;
    }
}
