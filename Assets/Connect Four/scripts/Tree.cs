using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ConnectFour
{
  public class NodeEvaluator
  {
    static double explorationParameter = Math.Sqrt (2);

    static double Evaluate (Node node)
    {
      return 0;
    }
  }

  public class Node
  {
    // le nombre de simulation gagnées depuis ce noeud
    private int wins;

    public int Wins {
      get { return wins; }
    }
    // Le nombre de simulation jouées depuis ce noeud
    private int plays;

    public int Plays {
      get { return plays; }
    }
    // vrai si c'est le joueur qui a appelé MCTS qui doit joueur à ce noeud
    bool isPlayersTurn { get; set; }
    // le noeud parent
    Node parent { get; set; }
    // on associe état enfant à l'action qui mène à cette état
    // Dictionary<int, Node> children;
		Dictionary<Node, int> children;
    // référence vers le jeu utilisé pour la simulation
		// public Field field;

    /*public Node (Field field, Node parentNode = null)
    {
      wins = 0;
      plays = 0;
      this.isPlayersTurn = field.IsPlayersTurn;
      // this.field = field;
      children = new Dictionary<int, Node> ();
      parent = parentNode;
    }*/

		public Node (Node parentNode = null)
		{
			wins = 0;
			plays = 0;
			this.isPlayersTurn = MonteCarloSearchTree.simulatedStateField.IsPlayersTurn;
			// this.field = field;
			// children = new Dictionary<int, Node> ();
			children = new Dictionary<Node, int> ();
			parent = parentNode;
		}

    /// <summary>
    /// Adds a child to the node.
    /// </summary>
    /// <param name="node">The node to add to the child pool</param>
    /// <param name="line">The line number used to get to this child play</param>
    public void addChild (Node node, int line)
    {
			children.Add (node, line);
    }

		/// <summary>
		/// Gets the child action number.
		/// </summary>
		/// <returns>The child action.</returns>
		/// <param name="node">A node.</param>
		public int getChildAction(Node node) {
			return children [node];
		}

		/// <summary>
		/// Gets the children number.
		/// </summary>
		/// <returns>The children number.</returns>
		public int getChildrenNumber() {
			return children.Count;
		}

    /// <summary>
    /// Select a node to expand in the current MCTS
    /// </summary>
    /// <returns>
    /// The selected node
    /// </returns>
    public Node Select (int nbSimulation)
    {
      //check for end game
			if (!MonteCarloSearchTree.simulatedStateField.ContainsEmptyCell () || MonteCarloSearchTree.simulatedStateField.CheckForVictory ()) {
        return this;
      }

			// If not all plays have been tried
      if (children.Keys.Count != MonteCarloSearchTree.simulatedStateField.GetPossibleDrops ().Count )
				return this;
      
      Node bestNode = null;
			bestNode = selectBestChild (nbSimulation);
      // TODO : Evaluer l'ensemble des enfants et appeler récursivement Select dessus
      // (ne pas oublier d'appeler field.dropInColumn(meilleurMouvement) pour mettre à jour le field
			MonteCarloSearchTree.simulatedStateField.DropInColumn (children[bestNode]);
			MonteCarloSearchTree.simulatedStateField.SwitchPlayer ();
      return bestNode.Select (nbSimulation);
    }

    /// <summary>
    /// Instantiate a child below the selected node and attach it to the tree.
    /// </summary>
    /// <returns>The new node created</returns>
    public Node Expand ()
    {

      //if selected node is a leaf
      if (!MonteCarloSearchTree.simulatedStateField.ContainsEmptyCell () || MonteCarloSearchTree.simulatedStateField.CheckForVictory ())
        return this;

      // Copy of the possible plays list
			List<int> drops = new List<int> (MonteCarloSearchTree.simulatedStateField.GetPossibleDrops ());

      // For each available plays, remove the ones that have already been play.
      foreach (int column in children.Values) {
        if (drops.Contains (column))
          drops.Remove (column);
      }
      // Gets a line to play on.
      int colToPlay = drops[ UnityEngine.Random.Range (0, drops.Count) ]; 
      Node n = new Node (this);
      // Adds the child to the tree
			addChild (n, colToPlay);
			MonteCarloSearchTree.simulatedStateField.DropInColumn (colToPlay);
			MonteCarloSearchTree.simulatedStateField.SwitchPlayer ();
      return n;
    }

    /// <summary>
    /// Simulate a game play based on the specified baseNode.
    /// </summary>
    /// <returns>True if the simulation leads to a win for the main player</returns>
    public bool Simulate ()
    {
      if (MonteCarloSearchTree.simulatedStateField.CheckForVictory ()) {
        return !MonteCarloSearchTree.simulatedStateField.IsPlayersTurn;
      }
			while (MonteCarloSearchTree.simulatedStateField.ContainsEmptyCell ()) {
				int column = MonteCarloSearchTree.simulatedStateField.GetRandomMove ();
				MonteCarloSearchTree.simulatedStateField.DropInColumn (column);
        if (MonteCarloSearchTree.simulatedStateField.CheckForVictory ()) {
          return MonteCarloSearchTree.simulatedStateField.IsPlayersTurn;
        }
				MonteCarloSearchTree.simulatedStateField.SwitchPlayer ();
      }
//      Debug.Log (MonteCarloSearchTree.simulatedStateField.ToString ());
			return true;
    }

    /// <summary>
    /// Does the back propagation from this simulated game to the root.
    /// </summary>
    /// <param name="playersVictory">True if the value to propagate is a victory for the main player</param>
    public void BackPropagate (bool playersVictory)
    {
      plays++;
      if (isPlayersTurn == playersVictory) {
        wins++;
      }
      if (parent != null) {
        parent.BackPropagate (playersVictory);
      }
    }

		/// <summary>
		/// Selects the best child from this node with UBC1 technique.
		/// </summary>
		/// <returns>The best child.</returns>
		public Node selectBestChild (int nbSimulation) {
			double maxValue = -1;
			Node bestNode = null;
			foreach (Node child in children.Keys) {
				double evaluation = child.wins / child.plays + Math.Sqrt (2 * Math.Log (nbSimulation) / child.plays);
				if (maxValue < evaluation) {
					maxValue = evaluation;
					bestNode = child;
				}
			}
			return bestNode;
		}

    public int MostSelectedMove ()
    {
      double maxValue = -1;
      int bestMove = -1;
      Debug.Log ("Child selection :");
      foreach (var child in children) {
        Debug.Log (child.Value + " - " + child.Key.Wins + " - " + child.Key.Plays);
        if ((double)child.Key.Wins/(double)child.Key.Plays > maxValue) {
          bestMove = child.Value;
          maxValue = (double)child.Key.Wins/(double)child.Key.Plays;
        }
      }
      return bestMove;
    }
  }

  public class MonteCarloSearchTree
  {
    // correspond à l'état actuel du jeu
    // /!\ c'est une référence vers l'état du jeu, NE PAS MODIFIER
    Field currentStateField;
    // état du jeu utilisé pour la simulation
    public static Field simulatedStateField;
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
			simulatedStateField = currentStateField.Clone ();
			rootNode = new Node ();

      Node selectedNode;
      Node expandedNode;
			int choosedColumn = -1;
			// Inutile de lancer MCST le premier tour
			if (field.PiecesNumber != 0) {
				int nbIteration = 1000;
				for (int i = 0; i < nbIteration; i++) {
					// copie profonde
					simulatedStateField = currentStateField.Clone ();

					selectedNode = rootNode.Select (rootNode.Plays);
					expandedNode = selectedNode.Expand ();
					expandedNode.BackPropagate (expandedNode.Simulate ());
				}
				//choosedColumn = rootNode.getChildAction (rootNode.selectBestChild (rootNode.Plays));
        choosedColumn = rootNode.MostSelectedMove();
			} else
				choosedColumn = field.GetRandomMove ();
				
			return choosedColumn;
    }
  }
}

