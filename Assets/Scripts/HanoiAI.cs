using UnityEngine;
using System.Collections;

public class HanoiAI : MonoBehaviour {
	public GameObject bigRing, mediumRing, smallRing;

	private float tower1X, tower2X, tower3X;
	private float bottomRingY, middleRingY, topRingY;

	void Awake() {
		bigRing = GameObject.Find ("BigRing");
		mediumRing = GameObject.Find ("MediumRing");
		smallRing = GameObject.Find ("SmallRing");

		tower1X = GameObject.Find ("Tower1").transform.position.x;
		tower2X = GameObject.Find ("Tower2").transform.position.x;
		tower3X = GameObject.Find ("Tower3").transform.position.x;

		bottomRingY = bigRing.transform.position.y;
		middleRingY = mediumRing.transform.position.y;
		topRingY = smallRing.transform.position.y;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
