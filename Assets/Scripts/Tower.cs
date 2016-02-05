using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour {
	public int TowerId;
	public LinkedList<Ring> rings = new LinkedList<Ring>();

	private GameManager _gm;

	void Awake() {
		_gm = GameObject.Find ("GameManager").GetComponent<GameManager>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PutRing(Tower tower) {
		Ring r = tower.rings.Last.Value;

		if (rings.Count > 0) {
			Ring lr = rings.Last.Value;
			if (r.RingId > lr.RingId)
				throw new UnityException ("Invalid ringL You trying to put the ring " + r.RingId + " over the ring " + lr.RingId);
		}

		tower.RemoveRing ();
		r.CurrentTower = this;
		rings.AddLast (r);

		r.transform.position = new Vector3(transform.position.x, _gm.TowerHeights[rings.Count], -0.1f);
	}

	public Ring RemoveRing() {
		Ring r = rings.Last.Value;
		rings.RemoveLast ();
		r.CurrentTower = null;
		return r;
	}
}
