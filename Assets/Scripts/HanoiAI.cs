using UnityEngine;
using System.Collections;

public class HanoiAI : MonoBehaviour {
	public GameObject bigRing, mediumRing, smallRing;
	public Transform tower1, tower2, tower3;

	private float bottomRingY, middleRingY, topRingY;

	void Awake() {
		bigRing = GameObject.Find ("BigRing");
		mediumRing = GameObject.Find ("MediumRing");
		smallRing = GameObject.Find ("SmallRing");

		tower1 = GameObject.Find ("Tower1").transform;
		tower2 = GameObject.Find ("Tower2").transform;
		tower3 = GameObject.Find ("Tower3").transform;

		bottomRingY = bigRing.transform.position.y;
		middleRingY = mediumRing.transform.position.y;
		topRingY = smallRing.transform.position.y;
	}

	// Use this for initialization
	void Start () {
		bigRing.transform.position = tower3.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
