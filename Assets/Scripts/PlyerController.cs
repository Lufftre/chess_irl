using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PlyerController : NetworkBehaviour {


    // int selectionX = -1;
    // int selectionY = -1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) return;

		transform.position = Camera.main.transform.position;
		transform.rotation = Camera.main.transform.rotation;

        // UpdateSelection();
        // if (selectionX >= 0 && selectionY >= 0)
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         BoardManager.Instace.CmdSelectChessman(selectionX, selectionY);
        //     }
        //     if (Input.GetMouseButton(0) && BoardManager.Instace.selectedChessman != null)
        //     {
        //         BoardManager.Instace.CmdPreviewMove(selectionX, selectionY);
        //     }
        // }

        // if (Input.GetMouseButtonUp(0) && BoardManager.Instace.selectedChessman != null)
        //     {
        //         BoardManager.Instace.CmdMoveChessman(selectionX, selectionY);
        //     }
	}

    // void UpdateSelection()
    // {
    //     if (!Camera.main)
    //         return;

    //     Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
	// 	RaycastHit hit;
	// 	if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Chess Plane"))){
    //         selectionX = (int)hit.point.x;
    //         selectionY = (int)hit.point.z;
	// 	} else {
    //         selectionX = -1;
    //         selectionY = -1;
	// 	}


    // }
}
