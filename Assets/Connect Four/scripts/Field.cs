using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ConnectFour
{
  public class Field {

    enum Piece
    {
      Empty = 0,
      Blue = 1,
      Red = 2
    }

    private int numRows;
    public int NumRows {
      get {return numRows;}
    }
    
    private int numColumns;
    public int NumColumns {
      get {return numColumns;}
    }

    private int numPiecesToWin;

    private bool allowDiagonally = true;

    protected int[,] field;

    private bool isPlayersTurn;
    public bool IsPlayersTurn {
      get { return isPlayersTurn; }
    }

  	// Field constructor
    public Field (int numRows, int numColumns, int numPiecesToWin, bool allowDiagonally) {
      this.numRows = numRows;
      this.numColumns = numColumns;
      this.numPiecesToWin = numPiecesToWin;
      this.allowDiagonally = allowDiagonally;

      isPlayersTurn = System.Convert.ToBoolean(UnityEngine.Random.Range (0, 1));

      field = new int[numColumns, numRows];
      for (int x = 0; x < numColumns; x++) {
        for (int y = 0; y < numRows; y++) {
          field [x, y] = (int)Piece.Empty;
        }
      }
  	}

    public Field (int numRows, int numColumns, int numPiecesToWin, bool allowDiagonally, bool isPlayersTurn, int[,] field) {
      this.numRows = numRows;
      this.numColumns = numColumns;
      this.numPiecesToWin = numPiecesToWin;
      this.allowDiagonally = allowDiagonally;
      this.isPlayersTurn = isPlayersTurn;

      this.field = new int[numColumns, numRows];
      for (int x = 0; x < numColumns; x++) {
        for (int y = 0; y < numRows; y++) {
          this.field [x, y] = field [x, y];
        }
      }
    }
  	
    // Renvoie la liste des cases où le joueur peut ajouter une pièce
    public Dictionary<int, int> GetPossibleCells() {
      Dictionary<int, int> possibleCells = new Dictionary<int, int>();
      for (int x = 0; x < numColumns; x++)
      {
        for(int y = numRows - 1; y >= 0; y--)
        {
          if(field[x, y] == (int)Piece.Empty)
          {
            possibleCells.Add(x, y);
            break;
          }
        }
      }
      return possibleCells;
    }

    // Renvoie la liste des colonnes où le joueur peut lâcher une pièce
    public List<int> GetPossibleDrops() {
      List<int> possibleDrops = new List<int>();
      for (int x = 0; x < numColumns; x++)
      {
        for(int y = numRows - 1; y >= 0; y--)
        {
          if(field[x, y] == (int)Piece.Empty)
          {
            possibleDrops.Add(x);
            break;
          }
        }
      }
      return possibleDrops;
    }


    // TODO : renvoie le meilleur mouvement
    public int GetBestMove() {
      return 0;
    }
     
    // renvoie un mouvement aléatoire parmi tous ceux possibles
    public int GetRandomMove() {
      List<int> moves = GetPossibleDrops();

      if(moves.Count > 0)
      {
        return moves[UnityEngine.Random.Range (0, moves.Count)];
      }
      return -1;
    }

    // Lâche une pièce dans la colonne i, renvoie la ligne où elle tombe
    public int DropInColumn(int col) {
      for(int i = numRows-1; i >= 0; i--)
      {
        if(field[col, i] == 0)
        {
//        foundFreeCell = true;
          field[col, i] = isPlayersTurn ? (int)Piece.Blue : (int)Piece.Red;
//        endPosition = new Vector3(x, i * -1, startPosition.z);
          return i;
        }
      }
      return -1;
    }

    // Alterne de joueur
    public void SwitchPlayer() {
      isPlayersTurn = !isPlayersTurn;
    }

    // Vérifie si la partie est gagnée (se référer à isPlayerTurn pour savoir qui est le gagnant)
    public bool CheckForWinner () {
      for (int x = 0; x < numColumns; x++) {
        for (int y = 0; y < numRows; y++) {
          // Get the Laymask to Raycast against, if its Players turn only include
          // Layermask Blue otherwise Layermask Red
          int layermask = isPlayersTurn ? (1 << 8) : (1 << 9);

          // If its Players turn ignore red as Starting piece and wise versa
          if (field [x, y] != (isPlayersTurn ? (int)Piece.Blue : (int)Piece.Red)) {
            continue;
          }

          // shoot a ray of length 'numPiecesToWin - 1' to the right to test horizontally
          RaycastHit[] hitsHorz = Physics.RaycastAll (
                                    new Vector3 (x, y * -1, 0), 
                                    Vector3.right, 
                                    numPiecesToWin - 1, 
                                    layermask);

          // return true (won) if enough hits
          if (hitsHorz.Length == numPiecesToWin - 1) {
            return true;
          }

          // shoot a ray up to test vertically
          RaycastHit[] hitsVert = Physics.RaycastAll (
                                    new Vector3 (x, y * -1, 0), 
                                    Vector3.up, 
                                    numPiecesToWin - 1, 
                                    layermask);

          if (hitsVert.Length == numPiecesToWin - 1) {
            return true;
          }

          // test diagonally
          if (allowDiagonally) {
            // calculate the length of the ray to shoot diagonally
            float length = Vector2.Distance (new Vector2 (0, 0), new Vector2 (numPiecesToWin - 1, numPiecesToWin - 1));

            RaycastHit[] hitsDiaLeft = Physics.RaycastAll (
                                         new Vector3 (x, y * -1, 0), 
                                         new Vector3 (-1, 1), 
                                         length, 
                                         layermask);

            if (hitsDiaLeft.Length == numPiecesToWin - 1) {
              return true;
            }

            RaycastHit[] hitsDiaRight = Physics.RaycastAll (
                                          new Vector3 (x, y * -1, 0), 
                                          new Vector3 (1, 1), 
                                          length, 
                                          layermask);

            if (hitsDiaRight.Length == numPiecesToWin - 1) {
              return true;
            }
          }
        }
      }
      return false;
    }

    // Vérifie si la grille contient encore des cellules vides
    public bool ContainsEmptyCell() {
      for(int x = 0; x < numColumns; x++)
      {
        for(int y = 0; y < numRows; y++)
        {
          if(field[x, y] == (int)Piece.Empty)
            return true;
        }
      }
      return false;
    }

    // Execute une copie profonde de l'état du jeu
    public Field Clone() {
      return new Field (numRows, numColumns, numPiecesToWin, allowDiagonally, isPlayersTurn, field);
    }
  }
}
