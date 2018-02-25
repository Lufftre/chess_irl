﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Chessman
{
    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[8, 8];
        Chessman c;
        int[] target;


        for (int x = 1; x < 8; x++)
        {
            target = new int[] { CurrentX + x, CurrentY + x };
            if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
            {
                c = BoardManager.Instace.Chessmans[target[0], target[1]];
                if (c == null)
                {
                    r[target[0], target[1]] = true;
                }
                else
                {
                    if (c.isWhite != isWhite)
                        r[target[0], target[1]] = true;
                    break;
                }
            }
        }

        for (int x = 1; x < 8; x++)
        {
            target = new int[] { CurrentX - x, CurrentY + x };
            if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
            {
                c = BoardManager.Instace.Chessmans[target[0], target[1]];
                if (c == null)
                {
                    r[target[0], target[1]] = true;
                }
                else
                {
                    if (c.isWhite != isWhite)
                        r[target[0], target[1]] = true;
                    break;
                }
            }
        }

        for (int x = 1; x < 8; x++)
        {
            target = new int[] { CurrentX + x, CurrentY - x };
            if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
            {
                c = BoardManager.Instace.Chessmans[target[0], target[1]];
                if (c == null)
                {
                    r[target[0], target[1]] = true;
                }
                else
                {
                    if (c.isWhite != isWhite)
                        r[target[0], target[1]] = true;
                    break;
                }
            }
        }

        for (int x = 1; x < 8; x++)
        {
            target = new int[] { CurrentX - x, CurrentY - x };
            if (target[0] >= 0 && target[0] < 8 && target[1] >= 0 && target[1] < 8)
            {
                c = BoardManager.Instace.Chessmans[target[0], target[1]];
                if (c == null)
                {
                    r[target[0], target[1]] = true;
                }
                else
                {
                    if (c.isWhite != isWhite)
                        r[target[0], target[1]] = true;
                    break;
                }
            }
        }



        return r;
    }
}
