using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public int TotalRings = 0;

	public IDictionary<int, Tower> Towers = new Dictionary<int, Tower>();
	public IDictionary<int,float> RingSizes = new Dictionary<int,float>();
	public IDictionary<int,float> TowerHeights = new Dictionary<int,float>();
	public IDictionary<int,Color> Colors = new Dictionary<int,Color>();

	public Tower Tower1, Tower2, Tower3;
	private List<Color> _colors = new List<Color> ();

	void Awake() {
		Tower1 = GameObject.Find ("Tower1").GetComponent<Tower>();
		Tower2 = GameObject.Find ("Tower2").GetComponent<Tower>();
		Tower3 = GameObject.Find ("Tower3").GetComponent<Tower>();

		Tower1.TowerId = 1;
		Tower2.TowerId = 2;
		Tower3.TowerId = 3;
		Towers.Add (Tower1.TowerId, Tower1);
		Towers.Add (Tower2.TowerId, Tower2);
		Towers.Add (Tower3.TowerId, Tower3);

		_colors.Add (Color.red);
		_colors.Add (Color.green);
		_colors.Add (Color.blue);
		_colors.Add (Color.gray);
		_colors.Add (Color.cyan);
		_colors.Add (Color.yellow);

		TotalRings = Mathf.Max (3, Mathf.Min (6, TotalRings));

		for (int i = TotalRings; i > 0; i--) {
			RingSizes.Add (i, 230 - (i - 1) * 30);
			TowerHeights.Add(i, 155 + (i - 1) * 48);
			Colors.Add (i, _colors[(i - 1) % TotalRings]);
		}

		var ringPrototype = GameObject.Find ("RingPrototype");
		for (int i = TotalRings; i > 0; i--) {
			var index = i;
			var id = TotalRings - i + 1;

			var ring = (GameObject)Instantiate (ringPrototype, new Vector3(Tower1.transform.position.x, TowerHeights [index], -0.1f), Quaternion.identity);
			var tmp = ring.transform.localScale;
			ring.transform.localScale = new Vector3 (RingSizes[index], tmp.y, tmp.z);
			ring.GetComponent<Renderer> ().material.color = Colors [index];

			Ring ringC = ring.GetComponent<Ring> ();
			ringC.RingId = id;
			ring.name = "Ring" + ringC.RingId;
			ringC.CurrentTower = Tower1;
			Tower1.rings.AddFirst(ringC);
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
