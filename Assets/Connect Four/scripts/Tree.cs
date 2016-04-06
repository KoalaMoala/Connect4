using System;
using System.Collections;
using System.Collections.Generic;

namespace ConnectFour
{
	public class Node
	{
		// le nombre de simulation gagnées depuis ce noeud
    int wins {get; set;}
		// Le nombre de simulation jouées depuis ce noeud
    int plays {get; set;}
		// vrai si c'est le joueur qui a appelé MCTS qui doit joueur à ce noeud
    bool isPlayersTurn {get; set;}
		// le noeud parent
    Node parent {get; set;}
		// on associe état enfant à l'action qui mène à cette état
		Dictionary<int, Node> childs;
		// référence vers le jeu utilisé pour la simulation
    Field field {get; set;}

		public Node (Field field, Node parentNode = null)
		{
			wins = 0;
			plays = 0;
      this.isPlayersTurn = field.IsPlayersTurn;
			this.field = field;
			childs = new Dictionary<int, Node> ();
			parent = parentNode;
		}

		/// <summary>
		/// Adds a child to the node.
		/// </summary>
		/// <param name="node">The node to add to the child pool</param>
		/// <param name="line">The line number used to get to this child play</param>
		public void addChild(Node node, int line) {
			childs.Add (line, node);
		}

    /// <summary>
    /// Select a node to expand in the current MCTS
    /// </summary>
    /// <returns>
    /// The selected node
    /// </returns>
    public Node Select () {
      return null;
    }

    /// <summary>
    /// Instantiate a child below the selected node and attach it to the tree.
    /// </summary>
		/// <returns>The new node created</returns>
    public Node Expand() {
			// Copy of the possible plays list
			List<int> drops = new List<int>(field.GetPossibleDrops ());
			// For each available plays, remove the ones that have already been play.
			foreach (int column in childs.Keys) {
				if (drops.Contains (column))
					drops.Remove (column);
			}
			// Gets a line to play on.
			int lineToPlay = UnityEngine.Random.Range (0, drops.Count);
			Node n = new Node (field, this);
			// Adds the child to the tree
			addChild (n, lineToPlay);
      return n;
    }

    /// <summary>
    /// Simulate a game play based on the specified baseNode.
    /// </summary>
    /// <returns>True if the simulation leads to a win for the main player</returns>
    public bool Simulate() {
      while (field.ContainsEmptyCell() && !field.CheckForWinner()) {
        int nextMove = field.GetRandomMove ();
        field.DropInColumn (nextMove);
        field.SwitchPlayer ();
      }
      return !field.IsPlayersTurn;
    }

    /// <summary>
    /// Does the back propagation from this simulated game to the root.
    /// </summary>
    /// <param name="playersVictory">True if the value to propagate is a victory for the main player</param>
    public void BackPropagate(bool playersVictory) {
      plays++;
      if (isPlayersTurn == playersVictory) {
        wins++;
      }
      if (parent != null) {
        parent.BackPropagate (playersVictory);
      }
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

		// retourne le coup le plus avantageux
		public int FindBestMove (Field field)
		{
			currentStateField = field;
			rootNode = new Node (simulatedStateField);

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
/*		public void SimulatePlay (Node node)
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
		}*/
	}
}

