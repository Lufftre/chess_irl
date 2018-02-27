using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chessman
{

    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[8, 8];
        Chessman c;
        int[] target;


        // Diagonal Left
        target = new int[] { CurrentX - 1, isWhite ? CurrentY + 1 : CurrentY - 1 };
        if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
        {
            c = BoardBoi.Instace.Chessmans[target[0], target[1]];
            if (c != null && c.isWhite != isWhite)
            {
                r[target[0], target[1]] = true;
            }
        }

        // Diagonal Right
        target = new int[] { CurrentX + 1, isWhite ? CurrentY + 1 : CurrentY - 1 };
        if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
        {
            c = BoardBoi.Instace.Chessmans[target[0], target[1]];
            if (c != null && c.isWhite != isWhite)
            {
                r[target[0], target[1]] = true;
            }
        }

        // Forward One
        target = new int[] { CurrentX, isWhite ? CurrentY + 1 : CurrentY - 1 };
        if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
        {
            c = BoardBoi.Instace.Chessmans[target[0], target[1]];
            if (c == null)
            {
                r[target[0], target[1]] = true;
                // Forward Two
                target = new int[] { CurrentX, isWhite ? CurrentY + 2 : CurrentY - 2 };
                if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
                {
                    c = BoardBoi.Instace.Chessmans[target[0], target[1]];
                    if (c == null && nMoves == 0)
                    {
                        r[target[0], target[1]] = true;
                    }
                }
            }
        }


        return r;
    }
}
