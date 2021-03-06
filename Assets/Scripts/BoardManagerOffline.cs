﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoardManagerOffline : BoardBoi
{

    

    public Chessman selectedChessman;

    const float TILE_SIZE = 1.0f;
    const float TILE_OFFSET = 0.5f;

    int selectionX = -1;
    int selectionY = -1;
    int oldSelectionX;
    int oldSelectionY;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman;
    // Use this for initialization

    public bool isWhiteTurn = true;
    
    private AudioSource audioSource;
    public AudioClip moveSound;
    public AudioClip captureSound;
    public AudioClip hoverSound;
    public AudioClip pickSound;
      
    void Start()
    {
        Instace = this;
        audioSource = GetComponent<AudioSource>();
        SpawnAllChessmans();
    }



    void UpdateSelection()
    {
        if (!Camera.main)
            return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Chess Plane"))){
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
		} else {
            selectionX = -1;
            selectionY = -1;
		}

        if(selectedChessman == null){
            if(!(oldSelectionX == selectionX && oldSelectionY == selectionY)){
                if(selectionX >= 0 && selectionY >= 0 &&
                    Chessmans[selectionX, selectionY] != null &&
                    Chessmans[selectionX, selectionY].isWhite == isWhiteTurn){
                    // Handheld.Vibrate();
                    audioSource.PlayOneShot(hoverSound, 0.25f);
                    Chessmans[selectionX, selectionY].transform.localScale = new Vector3(.12f, .12f, .12f);
                    Chessmans[selectionX, selectionY].GetComponent<Renderer>().material.color = Color.yellow;

                }
                if(oldSelectionX >= 0 && oldSelectionY >= 0 && Chessmans[oldSelectionX, oldSelectionY] != null){
                    Chessmans[oldSelectionX, oldSelectionY].transform.localScale = new Vector3(.1f,.1f,.1f);
                    Chessmans[oldSelectionX, oldSelectionY].GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }

        oldSelectionX = selectionX;
        oldSelectionY = selectionY;
    }

    Vector3 GetTileCenter(int x, int y)
    {
        Vector3 pos = Vector3.zero;
        pos.x += (TILE_SIZE * x) + TILE_OFFSET;
        pos.z += (TILE_SIZE * y) + TILE_OFFSET;
        return pos;
    }

    void SpawnChessman(int index, int x, int y, int team)
    {
        Quaternion orientation = Quaternion.Euler(0, 90 - 180*team, 0);

        GameObject go = Instantiate(chessmanPrefabs[index + team*6], GetTileCenter(x, y), orientation) as GameObject;
        go.transform.SetParent(GameObject.Find("Chessmans").transform);
        Chessmans[x, y] = go.GetComponent<Chessman> ();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
        // NetworkServer.Spawn(go);
    }

    void SpawnAllChessmans()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];
        print("Spawning chessbois");
        SpawnChessman(0, 4, 0, 0);
        SpawnChessman(1, 3, 0, 0);

        SpawnChessman(2, 0, 0, 0);
        SpawnChessman(2, 7, 0, 0);

        SpawnChessman(3, 2, 0, 0);
        SpawnChessman(3, 5, 0, 0);

        SpawnChessman(4, 1, 0, 0);
        SpawnChessman(4, 6, 0, 0);

        for(int i = 0; i < 8; i++)
            SpawnChessman(5, i, 1, 0);

        SpawnChessman(0, 4, 7, 1);
        SpawnChessman(1, 3, 7, 1);

        SpawnChessman(2, 0, 7, 1);
        SpawnChessman(2, 7, 7, 1);

        SpawnChessman(3, 2, 7, 1);
        SpawnChessman(3, 5, 7, 1);

        SpawnChessman(4, 1, 7, 1);
        SpawnChessman(4, 6, 7, 1);

        for (int i = 0; i < 8; i++)
            SpawnChessman(5, i, 6, 1);

    }

    void DrawChessboard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            start = Vector3.right * i;
            Debug.DrawLine(start, start + heightLine);

        }


        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(new Vector3(selectionX, 0, selectionY), new Vector3(selectionX + 1, 0, selectionY + 1));

        }
    }

    // Update is called once per frame
    void Update()
    {

        UpdateSelection();

        // print(selectionX + " : " + selectionY);

        if (selectionX >= 0 && selectionY >= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectChessman(selectionX, selectionY);
            }
            if (Input.GetMouseButton(0) && selectedChessman != null)
            {
                PreviewMove(selectionX, selectionY);
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedChessman != null)
            {
                int x = (int)selectedChessman.transform.position.x;
                int y = (int)selectedChessman.transform.position.z;
                selectedChessman.transform.position = GetTileCenter(selectedChessman.CurrentX, selectedChessman.CurrentY);
                CmdMoveChessman(selectedChessman.CurrentX, selectedChessman.CurrentY, x, y);
            }

    }

    public void SelectChessman(int x, int y)
    {
        if (Chessmans[x, y] == null)
            return;

        if (Chessmans[x, y].isWhite != isWhiteTurn)
            return;
        print("Select");
        audioSource.PlayOneShot(pickSound, 0.2f);
        selectedChessman = Chessmans[x, y];
        Vector3 pos = selectedChessman.transform.position;
        Quaternion rot = selectedChessman.transform.rotation;
        pos.y = .6f;
        selectedChessman.transform.position = pos;
        selectedChessman.transform.rotation = Quaternion.Euler(12f, rot.eulerAngles.y, 0f);


        this.allowedMoves = selectedChessman.PossibleMoves();
        BoardHighlightsOffline.Instace.HighlightAllowedMoves(allowedMoves);
 
    }


    void ClearBoard()
    {
        foreach(GameObject cm in activeChessman){
            Destroy(cm);
        }
    }


    void CmdMoveChessman(int x0, int y0, int x, int y){
        RpcMoveChessman(x0, y0, x, y);
        if(!isWhiteTurn)
            AIMakeMove();
    }
    public void RpcMoveChessman(int x0, int y0, int x, int y)
    {
        if (Chessmans[x0, y0].PossibleMove(x, y))
        {

            Chessman c = Chessmans[x, y];
            if (c != null && c.isWhite != isWhiteTurn)
            {
                activeChessman.Remove(c.gameObject);
                if(c.IsKing()){
                    ClearBoard();
                    SpawnAllChessmans();
                    isWhiteTurn = true;
                    BoardHighlightsOffline.Instace.HideHighlights();
                    return;
                }
                Destroy(c.gameObject);
                audioSource.PlayOneShot(captureSound, 1f);
            } else {
                audioSource.PlayOneShot(moveSound, .6f);
            }

            Chessmans[x0, y0].transform.position = GetTileCenter(x, y);
            Chessmans[x0, y0].SetPosition(x, y);
            Chessmans[x0, y0].nMoves += 1;
            Chessmans[x, y] = Chessmans[x0, y0];
            Chessmans[x0, y0] = null;
            isWhiteTurn = !isWhiteTurn;
        }
        selectedChessman.transform.localScale = new Vector3(.1f,.1f,.1f);
        selectedChessman.GetComponent<Renderer>().material.color = Color.white;
        oldSelectionX = -1;
        oldSelectionY = -1;
        Vector3 pos = selectedChessman.transform.position;
        Quaternion rot = selectedChessman.transform.rotation;
        pos.y = 0f;
        selectedChessman.transform.position = pos;
        selectedChessman.transform.rotation = Quaternion.Euler(0f, rot.eulerAngles.y, 0f);
        // selectedChessman.transform.position = GetTileCenter(selectedChessman.CurrentX, selectedChessman.CurrentY);
        BoardHighlightsOffline.Instace.HideHighlights();
        selectedChessman = null;
    }


    public void PreviewMove(int x, int y)
    {
        //if (selectedChessman.PossibleMove(x, y) || (x == selectedChessman.CurrentX && y == selectedChessman.CurrentY))
        //{
            bool[,] moves = selectedChessman.PossibleMoves();

            int closestX = x;
            int closestY = y;
            float closestDist = 16f;
            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (moves[i, j] || (i==selectedChessman.CurrentX && j==selectedChessman.CurrentY)) {
                        //int dist = Mathf.Abs(x-i) + Mathf.Abs(y-j);
                        float dist = Mathf.Sqrt((x-i)*(x-i) + (y-j)*(y-j));
                        if (dist < closestDist){
                            closestDist = dist;
                            closestX = i;
                            closestY = j;
                        }
                    } 
                }
            }

            Vector3 pos = GetTileCenter(closestX, closestY);
            pos.y = selectedChessman.transform.position.y;
            selectedChessman.transform.position = pos;
        //}

    }

    void AIMakeMove(){
        int c = 0;
        while(c < 1000){
            int x = Random.Range(0, 8);
            int y = Random.Range(0, 8);
            if(Chessmans[x,y] != null && !Chessmans[x,y].isWhite){
                bool[,] possibleMoves = Chessmans[x,y].PossibleMoves();
                List<List<int>> moves = new List<List<int>>();
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if(possibleMoves[i,j]){
                            List<int> move = new List<int>();
                            move.Add(i);
                            move.Add(j);
                            moves.Add(move);
                        }
                    }
                }
                print(moves.Count);
                if(moves.Count > 0){
                    int index = Random.Range(0, moves.Count);
                    print(index);
                    int xNew = moves[index][0];
                    int yNew = moves[index][1];
                    SelectChessman(x, y);
                    CmdMoveChessman(x, y, xNew, yNew);
                    return;
                }
            }
            c++;
        }
    }


}