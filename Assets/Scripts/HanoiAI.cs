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
		Hanoi (_gm.TotalRings);
		StartCoroutine (ShowResults());
	}

	//Observações: A AI da torre de Hanói baseia-se no percorrimento de uma árvore binária in-order (LPR - Left-Parent-Right).
	//O número de discos determinam qual será o nível da árvore, sendo cada nível preenchido completamente
	//Um nível par da árvore possui o padrão de comportamento: 1-2, 2-3, 3-1; ou xyz'
	//Um nível ímpar da árvore possui o padrão de comportamento: 1-3, 3-2, 2-1; ou zy'x'
	void Hanoi(int n) {
		//Os três primeiros digitos são sequências de passos para casos onde (n - i, ou n - ringToMove) é par. Os três últimos para caso ímpar.
		int[] pegs = new int[]{1, 2, 3, 1, 3, 2};

		//Calcula quantos movimentos serão necessários (2^n - 1)
		int move = (1 << n) - 1;

		//Para cada movimento
		for (int currentMoveCount = 1; currentMoveCount <= move; currentMoveCount++) {
			//Calcula qual será o disco a ser movimentado.
			//O disco a ser movimentado é definido pelo número de zeros à direita que o contador de ação atual possui
			//Ou seja, quantas vezes podemos dividir este número por 2
			//Sendo assim ringToMove é um expoente, cuja base é 2
			//Mais tarde, será somado +1 a esse expoente, sendo este o valor final do disco a ser movido
			int ringToMove = 0;
			while ((currentMoveCount >> ringToMove & 1) == 0)
				ringToMove++;

			//Verifica se o número resultante da subtração do total de discos pelo disco atual é par ou impar
			int evenOrOdd = (n - ringToMove) & 1;

			//A torre que removeremos o disco atual será {1, 2, 3}, se par, ou {1, 3, 2}, se impar
			//O shift remove todos os zeros
			int helper = (currentMoveCount >> ++ringToMove) % 3;
			int fromTower = pegs[helper + (evenOrOdd * 3)];

			//A torre que levaremos o discuto atual é a torre da partida +1 (em even), ou +2 (em odd)
			int toTower = (fromTower + evenOrOdd) % 3 + 1;
			_commands.Enqueue (new MoveCommand(ringToMove, fromTower, toTower));
		}
	}

	void HanoiLoopless(int n) {
		int i, j, k, l, q;
		int move = (1 << n) - 1;
		int[] pegs = new int[]{1, 2, 3, 1, 3, 2};

		for (j = 1; j <= move; j++) {
			//O objetivo do logaritmo é descobrir i, tal que 2^i = (j & ~(j - 1)). 
			//Com esse simples cálculo, contamos quantos zeros existem após o 1
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