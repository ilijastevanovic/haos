﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ElementPlacing : MonoBehaviour {

	static public string currHold;
	static public bool holding;
	public GameObject setDir;
	public GameObject block;
	public GameObject ramp;
	public GameObject curve;
	public GameObject pistonBlock;
	public int setdirNum, blockNum, rampNum, curveNum, pistonBlockNum;
	private GameObject ground;
	private GameObject invPanel;
	private GameObject spawnedObjects;
	static public GameObject LeftRotButton;
	static public GameObject RightRotButton;
	static public GameObject CheckButton;
	static public GameObject RemoveButton;
	static public Image leftImage;
	static public Image rightImage;
	static public Image checkImage;
	public GameObject tower;
	public GameObject transpWall;
	public bool removeToggle;
	public GameObject panel;
	public GameObject canvas;

	// Use this for initialization
	void Awake () {


		LeftRotButton = GameObject.Find ("RotLeftButton");
		RightRotButton = GameObject.Find ("RotRightButton");
		CheckButton = GameObject.Find ("CheckButton");
		RemoveButton = GameObject.Find ("RemoveButton");
		leftImage = LeftRotButton.GetComponent<Image> ();
		rightImage = RightRotButton.GetComponent<Image> ();
		checkImage = CheckButton.GetComponent<Image> ();
		ground = GameObject.Find ("Plane");
		holding = false;
		setdirNum = blockNum = rampNum = curveNum = pistonBlockNum = 0;
		invPanel = GameObject.Find ("InvPanel");
		spawnedObjects = GameObject.Find ("SpawnedObjects");
		removeToggle = false;
	}

	public void TakeElement(string elem){
		BlockHover.showGrid ();
		holding = true;
		currHold = elem;
		if (elem == "remove") {

			removeToggle = !removeToggle;
			if (!removeToggle) {
				holding = false;
				//currHold = "";
			}

		}

		print (currHold);

		var manager = GameObject.Find ("UIManager");
		if (manager.GetComponent<InvOpen> ().open) {
			manager.GetComponent<InvOpen> ().open = false;
			panel.transform.Translate (panel.transform.right * 135 * canvas.GetComponent<Canvas>().scaleFactor);
		}

	}

	public int getObjectNum(string name){

		/*int num = 0;

		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Spawned Objects")) {

			if (g.name.Contains (name))
				num++;

		}

		return num;*/

		switch (name) {

		case "block":
			return blockNum;
		case "setdir":
			return setdirNum;
		case "ramp":
			return rampNum;
		case "curve":
			return curveNum;
		case "pistonblock":
			return pistonBlockNum;

		}

		return 0;

	}

	public string truncateNumbers(string name){
		string nameRoot = name;

		while((nameRoot[nameRoot.Length - 1] >= '0') && (nameRoot[nameRoot.Length - 1] <= '9'))
			nameRoot = nameRoot.Remove(nameRoot.Length - 1);

		return nameRoot;
	}

	public void decNum(string name){
		string nameRoot = truncateNumbers(name);
		//while((nameRoot[nameRoot.Length - 1] >= '0') && (nameRoot[nameRoot.Length - 1] <= '9'))
		//	nameRoot = nameRoot.Remove(nameRoot.Length - 1);
		//print (nameRoot);
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Spawned Objects")) {

			if (g.name.Contains (nameRoot)) {

				switch (nameRoot) {
				case "block":
					blockNum--;
					break;
				case "setdir":
					setdirNum--;
					break;
				case "ramp":
					rampNum--;
					break;
				case "curve":
					curveNum--;
					break;
				case "pistonblock":
					pistonBlockNum--;
					break;
				}
				return;

			}

		}

	}

	void activateButtons(){

		Positioning.showButtons ();


	}

	bool overlaping(Vector3 point){

		GameObject detector = GameObject.CreatePrimitive (PrimitiveType.Cube);
		detector.transform.position = new Vector3 (Mathf.Round (point.x), point.y + 0.2f, Mathf.Round (point.z));
		detector.AddComponent<BoxCollider> ();
		//detector.AddComponent<Rigidbody> ();
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Spawned Objects")) {
			
			if (detector.GetComponent<BoxCollider> ().bounds.Contains (obj.transform.position)) {
				Destroy (detector);
				return true;

			}
		}
		Destroy (detector);
		return false;

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {

			if (!PlaySimulation.isSimActive && (!EventSystem.current.IsPointerOverGameObject())) {

				Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
				Ray ray;
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				ground.GetComponent<MeshCollider> ().enabled = true;
				BlockHover.hideGrid ();
				if (Physics.Raycast (ray, out hit, 100)) {
					if (holding) {
						
						if (hit.collider.gameObject != null && (hit.collider.gameObject.tag == "Spawned Objects")) {

							Positioning.showButtons ();
							//if (hit.collider.gameObject.name.Contains ("halfcurve")) {
							//	Positioning.placedElem = hit.collider.gameObject.transform.parent.gameObject;
							//}else
							Positioning.placedElem = hit.collider.gameObject;

						} else {


							Bounds bounds = ground.GetComponent<MeshCollider> ().bounds;
							if (bounds.Contains (hit.point) && !overlaping(hit.point)) {
								var script = invPanel.GetComponent<AvailElemManager> ();
								bool placed = false;

								GameObject newObj = null;

								if (currHold == "setdir") {
								
									newObj = Instantiate (setDir);
									setdirNum++;
									newObj.name = "setdir" + setdirNum;
									newObj.GetComponent<MonoBehaviour> ().enabled = false;
									newObj.transform.position = new Vector3 (Mathf.Round (hit.point.x), hit.point.y - 0.49f, Mathf.Round (hit.point.z));
									placed = true;
								}
								else if (currHold == "block") {

									newObj = Instantiate (block);
									blockNum++;
									newObj.name = "block" + blockNum;
									newObj.transform.position = new Vector3 (Mathf.Round (hit.point.x), hit.point.y + 0.45f, Mathf.Round (hit.point.z));
									placed = true;

								} else if (currHold == "ramp") {

									newObj = Instantiate (ramp);
									rampNum++;
									newObj.name = "ramp" + rampNum;
									newObj.transform.position = new Vector3 (Mathf.Round (hit.point.x), hit.point.y + 0.4f, Mathf.Round (hit.point.z));
									placed = true;

								} else if (currHold == "curve") {

									newObj = Instantiate (curve);
									curveNum++;
									newObj.name = "curve" + curveNum;
									newObj.transform.position = new Vector3 (Mathf.Round (hit.point.x), hit.point.y + 0.375f - 0.08f, Mathf.Round (hit.point.z));
									placed = true;

								
								} else if (currHold == "pistonblock") {

									newObj = Instantiate (pistonBlock);
									pistonBlockNum++;
									newObj.name = "pistonblock" + pistonBlockNum;
									newObj.transform.position = new Vector3 (Mathf.Round (hit.point.x), hit.point.y + 0.45f, Mathf.Round (hit.point.z));
									placed = true;

								}
								if (placed && newObj) {

									newObj.transform.parent = spawnedObjects.transform;
									activateButtons ();

									Positioning.placedElem = newObj;
										
								}
							}
						}
						
					} else {
						
						if (hit.collider.gameObject && (hit.collider.gameObject.tag == "Spawned Objects")) {

							activateButtons ();
							BlockHover.showGrid ();
							//if (hit.collider.gameObject.name.Contains ("halfcurve")) {
								
							//	Positioning.placedElem = hit.collider.gameObject.transform.parent.gameObject;
								//print (hit.collider.gameObject.transform.gameObject.name);
							//}else
							Positioning.placedElem = hit.collider.gameObject;

						}

					}

				}
				ground.GetComponent<MeshCollider> ().enabled = false;
				if(holding)
					BlockHover.showGrid ();
			}

		}
	}

}