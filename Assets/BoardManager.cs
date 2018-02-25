using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public static BoardManager Instace { set; get; }
    private bool[,] allowedMoves { set; get; }

    public Chessman[,] Chessmans { set; get; }
    private Chessman selectedChessman;

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
        SpawnAllChessmans();
    }

    void UpdateSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Chess Plane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        } else
        {
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
        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<Chessman> ();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
    }

    void SpawnAllChessmans()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];

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
        DrawChessboard();
        UpdateSelection();

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
                MoveChessman(selectionX, selectionY);
            }


        

    }

    void SelectChessman(int x, int y)
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

    void MoveChessman(int x, int y)
    {

        x = (int)selectedChessman.transform.position.x;
        y = (int)selectedChessman.transform.position.z;
        if (selectedChessman.PossibleMove(x, y))
        {

            Chessman c = Chessmans[x, y];
            if (c != null && c.isWhite != isWhiteTurn)
            {
                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);
            selectedChessman.nMoves += 1;
            Chessmans[x, y] = selectedChessman;
            isWhiteTurn = !isWhiteTurn;
        }
        selectedChessman.transform.position = GetTileCenter(selectedChessman.CurrentX, selectedChessman.CurrentY);
        BoardHighlights.Instace.HideHighlights();
        selectedChessman = null;
    }

    void PreviewMove(int x, int y)
    {
        if (selectedChessman.PossibleMove(x, y) || (x == selectedChessman.CurrentX && y == selectedChessman.CurrentY))
        {
            selectedChessman.transform.position = GetTileCenter(x, y);
        }

    }


}