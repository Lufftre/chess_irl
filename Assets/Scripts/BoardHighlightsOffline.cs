﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class BoardHighlightsOffline : MonoBehaviour {

    public static BoardHighlightsOffline Instace { set; get; }
    public GameObject highlightPrefab;
    public GameObject highlightPrefabChessman;
    private List<GameObject> highlights;
    private BoardManagerOffline boardManager;



    private void Start()
    {
        Instace = this;
        highlights = new List<GameObject>();
        boardManager = GetComponent<BoardManagerOffline>();
        
    }

    private GameObject GetHighLightObject()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);
        if(go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }

        return go;
    }

    public void HighlightAllowedMoves(bool[,] moves)
    {
        for(int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (moves[i, j]) { 
                    //GameObject go = GetHighLightObject();
                    GameObject go;
                    if(boardManager.Chessmans[i,j] == null)
                        go = Instantiate(highlightPrefab);
                    else
                        go = Instantiate(highlightPrefabChessman);
                    highlights.Add(go);
                    // go.SetActive(true);
                    go.transform.position = new Vector3(i, 0, j);
                }
            }
        }
    }

    public void HideHighlights()
    {
        foreach (GameObject go in highlights)
            // go.SetActive(false);
            Destroy(go);
    }

}
