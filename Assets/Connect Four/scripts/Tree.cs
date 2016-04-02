using System;
using System.Collections.Generic;

namespace ConnectFour
{
	public enum Player
	{
		Me, Opponent,
	}

	public class Node
	{
		int[,] field;
		int x previousMove;
		int value;
		Player turn;

		Node parent;
		List<Node> childs;
	}

	public class Tree
	{
		public Tree (int[,] currentField)
		{
		}
	}
}

