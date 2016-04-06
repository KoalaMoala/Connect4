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

    public Node (Field field, bool myTurn) {
      wins = 0;
      plays = 0;
      this.myTurn = myTurn;
      this.field = field;
      childs = new Dictionary<int, Node> ();
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

    public int FindBestMove(Field field, bool myTurn) {
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
	}
}

