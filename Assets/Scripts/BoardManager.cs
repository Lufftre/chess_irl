using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoardManager : NetworkBehaviour
{
    public static BoardManager Instace { set; get; }
    private bool[,] allowedMoves { set; get; }

    public Chessman[,] Chessmans { set; get; }

    public Chessman selectedChessman;

    const float TILE_SIZE = 1.0f;
    const float TILE_OFFSET = 0.5f;

    int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman;
    // Use this for initialization

    public bool isWhiteTurn = true;
      
    void Start()
    {
        Instace = this;
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];

        if(isLocalPlayer)
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
        if(!isLocalPlayer)
            return;
        // DrawChessboard();
        UpdateSelection();

        print(selectionX + " : " + selectionY);

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
        selectedChessman = Chessmans[x, y];
        allowedMoves = selectedChessman.PossibleMoves();
        BoardHighlights.Instace.HighlightAllowedMoves(allowedMoves);
 
    }


    [Command]
    void CmdMoveChessman(int x0, int y0, int x, int y){
        RpcMoveChessman(x0, y0, x, y);
    }
    [ClientRpc]
    public void RpcMoveChessman(int x0, int y0, int x, int y)
    {
        if(!isLocalPlayer) return;
        if (Chessmans[x0, y0].PossibleMove(x, y))
        {

            Chessman c = Chessmans[x, y];
            if (c != null && c.isWhite != isWhiteTurn)
            {
                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            Chessmans[x0, y0].transform.position = GetTileCenter(x, y);
            Chessmans[x0, y0].SetPosition(x, y);
            Chessmans[x0, y0].nMoves += 1;
            Chessmans[x, y] = Chessmans[x0, y0];
            Chessmans[x0, y0] = null;
            isWhiteTurn = !isWhiteTurn;
        }
        // selectedChessman.transform.position = GetTileCenter(selectedChessman.CurrentX, selectedChessman.CurrentY);
        BoardHighlights.Instace.HideHighlights();
        selectedChessman = null;
    }


    public void PreviewMove(int x, int y)
    {
        //if (selectedChessman.PossibleMove(x, y) || (x == selectedChessman.CurrentX && y == selectedChessman.CurrentY))
        //{
            bool[,] moves = selectedChessman.PossibleMoves();

            int closestX = x;
            int closestY = y;
            int closestDist = 16;
            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (moves[i, j] || (i==selectedChessman.CurrentX && j==selectedChessman.CurrentY)) {
                        int dist = Mathf.Abs(x-i) + Mathf.Abs(y-j);
                        if (dist < closestDist){
                            closestDist = dist;
                            closestX = i;
                            closestY = j;
                        }
                    } 
                }
            }

            selectedChessman.transform.position = GetTileCenter(closestX, closestY);
        //}

    }


}