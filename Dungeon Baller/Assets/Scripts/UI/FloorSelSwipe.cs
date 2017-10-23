﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSelSwipe : MonoBehaviour {

	private Vector3 touchPos;
	private float initPos;
	public float towerHeight;

	private float lastY = 1;

	public void towerScroll(Vector2 vector){
		print (vector.y);
		transform.position = new Vector3 (transform.position.x, transform.position.y - (lastY - vector.y) * 600, transform.position.z);
		lastY = vector.y;
	}
}
