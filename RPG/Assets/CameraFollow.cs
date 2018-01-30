using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	GameObject target;

	void Start () {
		target = GameObject.FindGameObjectWithTag("Player");
	}
	
	void LateUpdate () {
		transform.position = target.transform.position;
	}
}
