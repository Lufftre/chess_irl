using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class BoardHighlights : NetworkBehaviour {

    public static BoardHighlights Instace { set; get; }
    public GameObject highlightPrefab;
    private List<GameObject> highlights;

    private void Start()
    {
        Instace = this;
        highlights = new List<GameObject>();
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
                    GameObject go = Instantiate(highlightPrefab);
                    highlights.Add(go);
                    // go.SetActive(true);
                    go.transform.position = new Vector3(i, 0, j);
                    NetworkServer.Spawn(go);
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
