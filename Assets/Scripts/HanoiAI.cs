using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

//Based on article: http://waset.org/publications/11095/on-the-solution-of-the-towers-of-hanoi-problem

public class HanoiAI : MonoBehaviour {
	public float TimeSecsPerAction = 1;

	private Queue<MoveCommand> _commands = new Queue<MoveCommand>();
	private GameManager _gm;

	public class MoveCommand {
		private int _ring;
		private int _towerFrom;
		private int _towerTo;

		public MoveCommand(int ring, int towerFrom, int towerTo) {
			_ring = ring;
			_towerFrom = towerFrom;
			_towerTo = towerTo;
		}

		public int RingId {
			get { return _ring; }
			set { _ring = value; } 
		}

		public int TowerFrom {
			get { return _towerFrom; }
			set { _towerFrom = value; }
		}

		public int TowerTo {
			get { return _towerTo; }
			set { _towerTo = value; }
		}
	}

	void Awake() {
		_gm = GameObject.Find ("GameManager").GetComponent<GameManager>();
	}

	void Start () {
		HanoiLoopless (_gm.TotalRings);
		StartCoroutine (ShowResults());
	}

	//Notes: The Hanoi Tower AI is based on the traversal of a binary tree in-order (LPR - Left-Parent-Right).
	//The number of disks determines what will be the level of the tree, being each level filled completely
	//An even tree level has the default behavior: 1-2, 2-3, 3-1; or xyz'
	//An odd tree level has the default behavior: 1-3, 3-2, 2-1; or zy'x'
	void Hanoi(int n) {
		//The first three digits are sequences of steps to cases where (n - i, or n - ringToMove) is even. The last three to odd case.
		int[] pegs = new int[]{1, 2, 3, 1, 3, 2};

		//Calculates how many moves are needed (2^n - 1)
		int move = (1 << n) - 1;

		//For each movement
		for (int currentMoveCount = 1; currentMoveCount <= move; currentMoveCount++) {
			//Calculates which disk will be moved.
			//The disk to be moved is defined by the number of zeros on the right that the current action has counted
			//In other words, how many times we divide that number by 2
			//So ringToMove is an exponent, whose base is 2
			//Later, +1 will be added to this exponent, which is the final value of the disk id to be moved
			int ringToMove = 0;
			while ((currentMoveCount >> ringToMove & 1) == 0)
				ringToMove++;

			//Checks if the resulting number by subtracting the total of discs by the current disc is even or odd
			int evenOrOdd = (n - ringToMove) & 1;

			//The tower we'll remove the current disk is {1, 2, 3}, if pair, or {1, 3, 2}, if odd
			//The shift operator removes all zeros
			int helper = (currentMoveCount >> ++ringToMove) % 3;
			int fromTower = pegs[helper + (evenOrOdd * 3)];

			//The tower that we will take the current disc is the fromTower +1 (in even) or +2 (in odd)
			int toTower = (fromTower + evenOrOdd) % 3 + 1;
			_commands.Enqueue (new MoveCommand(ringToMove, fromTower, toTower));
		}
	}

	void HanoiLoopless(int n) {
		int i, j, k, l, q;
		int move = (1 << n) - 1;
		int[] pegs = new int[]{1, 2, 3, 1, 3, 2};

		for (j = 1; j <= move; j++) {
			//The purpose of the log is to find i such that 2 ^ i = (j & ~ (j - 1)).
			//With this simple calculation, we count how many zeros there are after 1, the reason which we used the loop before
			i = (int)(Mathf.Log(j & ~(j - 1), 2));

			l = (n - i) & 1;
			k = (j >> ++i) % 3;
			k = pegs[k + (l * 3)];
			q = (k + l) % 3 + 1;
			_commands.Enqueue (new MoveCommand(i, k, q));
		}
	}

	private Dictionary<int, Ring> _ringsCache = new Dictionary<int, Ring> ();
	IEnumerator ShowResults() {
		yield return new WaitForSeconds (1.0f);

		while (_commands.Count > 0) {
			var command = _commands.Dequeue ();
			Debug.Log ("move ring " + command.RingId + " from " + command.TowerFrom + " to " + command.TowerTo + " (" + GetLetterRelated(command.TowerFrom, command.TowerTo) + ")");

			if(!_ringsCache.ContainsKey(command.RingId))
				_ringsCache[command.RingId] = GameObject.Find ("Ring" + command.RingId).GetComponent<Ring>();

			Ring currentRing = _ringsCache[command.RingId];
			_gm.Towers [command.TowerTo].PutRing (currentRing.CurrentTower);

			yield return new WaitForSeconds (TimeSecsPerAction);
		}
	}

	string GetLetterRelated (int from, int to)
	{
		switch (from) {
		case 1:
			switch (to) {
			case 2:
				return "x";
			case 3:
				return "z";
			}
			break;
		case 2:
			switch (to) {
			case 1:
				return "x'";
			case 3:
				return "y";
			}
			break;
		case 3:
			switch (to) {
			case 1:
				return "z'";
			case 2:
				return "y'";
			}
			break;
		}

		return null;
	}
}