using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PlyerControllerOffline : MonoBehaviour {

	void Update () {

		transform.position = Camera.main.transform.position;
		transform.rotation = Camera.main.transform.rotation;

	}

}
