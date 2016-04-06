using System;
using System.Collections;
using System.Collections.Generic;

namespace ConnectFour
{
	public class Node
	{
		// le nombre de simulation gagnées depuis ce noeud
		int wins;
		// Le nombre de simulation jouées depuis ce noeud
		int plays;
		// vrai si c'est le joueur qui a appelé MCTS qui doit joueur à ce noeud
		bool myTurn;
		// le noeud parent
		Node parent;
		// on associe état enfant à l'action qui mène à cette état
		Dictionary<int, Node> childs;
		// référence vers le jeu utilisé pour la simulation
		Field field;

		public Node (Field field, bool myTurn)
		{
			wins = 0;
			plays = 0;
			this.myTurn = myTurn;
			this.field = field;
			childs = new Dictionary<int, Node> ();
		}

		public Field getField ()
		{
			return this.field;
		}

		public bool getTurn ()
		{
			return this.myTurn;
		}

		public void setWins (int wins)
		{
			this.wins = wins;
		}

		public int getWins ()
		{
			return this.wins;
		}

		public void setPlays (int plays)
		{
			this.plays = plays;
		}

		public int getPlays ()
		{
			return this.plays;
		}

		public Dictionary<int, Node> getChildren ()
		{
			return this.childs;
		}
	}

	public class MonteCarloSearchTree
	{
		// correspond à l'état actuel du jeu
		// /!\ c'est une référence vers l'état du jeu, NE PAS MODIFIER
		Field currentStateField;
		// état du jeu utilisé pour la simulation
		Field simulatedStateField;
		// Pour éviter de faire des copies profondes du jeu pour chaque noeud
		// je pense que chaque noeud doit juste avoir une référence vers cet état
		// lors de chaque itération de l'arbre, copier le contenu de currentStateField
		// dans simulatedStateField (fonction Clone()), puis pour itérer sur l'arbre
		// faire comme si on jouait avec ce Field (fonction DropInColumn(int)).

		// racine de l'arbre
		Node rootNode;

		/// <summary>
		/// Select a node to expand in the current MCTS
		/// </summary>
		/// <returns>
		/// The selected node
		/// </returns>
		private Node select () {
			return null;
		}

		/// <summary>
		/// Instantiate a child below the selected node;
		/// </summary>
		/// <param name="selectedNode">The node selected before</param>
		private Node expand(Node selectedNode) {
			return null;
		}

		/// <summary>
		/// Simulate a game paly based on the specified baseNode.
		/// </summary>
		/// <param name="baseNode">The node to start the simulation from</param>
		private void simulate(Node baseNode) {
			return;
		}

		/// <summary>
		/// Does the back propagation from the last leaf of the simulated game.
		/// </summary>
		/// <param name="leaf">The leaf to start the back propagation from</param>
		private void doBackPropagation(Node leaf) {
			return;
		}

		// retourne le coup le plus avantageux
		public int FindBestMove (Field field, bool myTurn)
		{
			currentStateField = field;
			rootNode = new Node (simulatedStateField, myTurn);

			int nbIteration = 1000;
			for (int i = 0; i < nbIteration; i++) {
				// copie profonde
				simulatedStateField = currentStateField.Clone ();   
				// TODO : Node selectedNode = rootNode.selection()
				// TODO : Node expandedNode = selectedNode.expansion()
				// TODO : expandedNode.simulation()
				// TODO : expandedNode.retroPropagation()

			}
			return 0;
		}

		// joue une partie aléatoire en partant du noeud sélectionné, puis met à jour les statistiques du chemin parcouru
		public void SimulatePlay (Node node)
		{
			// ligne et colonne dans laquelle vient d'être posée la pièce
			int moveLine = 0;
			int moveColumn = 0;
			bool turn = true;
			// copie de la liste parents-enfants
			//Dictionary<int, Node> simulatedchilds = node.getChildren ();
			// tant que la partie n'est pas terminée
			while (node.getField ().CheckForWinner()) {
				// on sélectionne un coup aléatoire dans la liste des coups possibles et on le joue
				moveColumn = node.getField ().GetRandomMove ();
				moveLine = node.getField ().DropInColumn (moveColumn);
				//simulatedchilds.Add (movecolumn, new Node (node.getField (), turn));
				// on passe au tour de l'autre joueur
				node.getField ().SwitchPlayer ();
				if (turn == true) {
					turn = false;
				} else {
					turn = true;
				}
			}
				
			// mise à jour des statistiques pour tout le chemin traversé
			foreach (Node n in node.getChildren()) {

				// le joueur actif (ordinateur) a gagné
				if (n.getField ().isPlayersTurn == true) {
					// si le noeud appartient au joueur actif
					if (n.getTurn () == true) {
						n.setWins (node.getWins () + 1);
						n.setPlays (node.getPlays () + 1);
					} else {
						n.setPlays (node.getPlays () + 1);
					}
				} 
			// le joueur adverse a gagné
			else {
					// si le noeud appartient au joueur actif
					if (n.getTurn () == true) {
						n.setPlays (node.getPlays () + 1);
					} else {
						n.setWins (node.getWins () + 1);
						n.setPlays (node.getPlays () + 1);
					}
				}
			}
		}
	}
}

