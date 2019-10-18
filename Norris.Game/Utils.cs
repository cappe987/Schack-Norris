using System;
using System.Linq;
using System.Collections.Generic;
using Norris.Data.Models.Board;

namespace Norris.Game {
  static class Utils {

    static string GetStringRep(PieceType type){
      switch(type){
        case PieceType.Bishop: return "B";
        case PieceType.Pawn  : return "P";
        case PieceType.Rook  : return "R";
        case PieceType.Knight: return "N";
        case PieceType.Queen : return "Q";
        case PieceType.King  : return "K";
        default: return "";
      }
    }

    static string GetColorString(Color c) => c == Color.White ? "W" : "B";

    public static void PrettyPrint(ChessBoard board){
        Console.WriteLine("\n   A  B  C  D  E  F  G  H ");
      for(int i = 0; i < 8; i++){
        Console.WriteLine($"   -------------------------");
        Console.Write($"{8-i} |");
        for(int j = 0; j < 8; j++){

          string a = board[i,j] == null ? "  " : GetColorString(
            board[i,j].Color) + GetStringRep(board[i,j].Type
          );

          Console.Write(a + "|");
        }
        if(i == 7){
          Console.Write($" {8-i}");
        }
        else{
          Console.WriteLine($" {8-i}");
        }
      }
      Console.WriteLine($"\n   -------------------------");
      Console.WriteLine("   A  B  C  D  E  F  G  H ");
    }

    public static void PrintMoveset(
      ChessBoard board, IEnumerable<Point> arr, Color player){
      var enemy = player == Color.White ? Color.Black : Color.White;
      for(int i = 0; i < 8; i++){
        for(int j = 0; j < 8; j++){
          if((board[i,j] == null || board[i,j].Color == enemy) && arr.Contains(new Point{Y=i,X=j})){
            Console.Write(" x ");
          }
          else{
            Console.Write(board[i,j] != null ? " p " :  " - ");
          }
        }
        Console.WriteLine();
      }
    }

    public static Color GetOppositeColor(Color color){
      return color == Color.White ? Color.Black : Color.White;
    }

    public static Point FindFirst(ChessBoard board, Func<PieceModel, bool> f){
      for(int i = 0; i < 8; i++){
        for(int j = 0; j < 8; j++){
          if(board[i,j] != null && f(board[i,j])){
            return new Point(){ Y = i, X = j};
          }
        }
      }
      throw new Exception($"\nError, tile not found");
 
    }

    public static Point FindKing(ChessBoard board, Color player){
      try{
        return FindFirst(
          board, t => t.Type == PieceType.King && t.Color == player
        );
      }
      catch{
        throw new Exception($"\nError, {player} king not found");
      }
    }

    static PieceModel ClonePiece(PieceModel piece){
      return new PieceModel(){
        Type = piece.Type,
        Color = piece.Color
      };
    }

    public static ChessBoard CloneBoard(ChessBoard board){

      PieceModel[,] clone = new PieceModel[8,8];
      for(int i = 0; i < 8; i++){
        for(int j = 0; j < 8; j++){
          if(board[i,j] != null){
            clone[i,j] = ClonePiece(board[i,j]);
          }
        }
      }

      return new ChessBoard(clone);
    }

    
    public static IEnumerable<Point> FoldIEnum(IEnumerable<Point>[] arr){
      IEnumerable<Point> list = new List<Point>();
      foreach(var ienum in arr){
        list = list.Concat(ienum);
      }
      return list;
    }

    public static IEnumerable<Point> GetAllMovesFor(ChessBoard board, Func<PieceModel, bool> f){
      IEnumerable<Point> list = new List<Point>(){};
      for(int i = 0; i < 8; i++){
        for(int j = 0; j < 8; j++){
          if(board[i,j] != null && f(board[i, j])){
            var p = new Point(){ Y = i, X = j};
            list = list.Concat(
              Logic.GetMovesFor(board, board[i,j].Color, p)
            );
          }
        }
      }
      return list;
    }

    public static PieceModel NewPiece(PieceType type, Color color){
      return new PieceModel(){
          Type = type, Color = color
      };
    }

    public static Point StringToPoint(string pos){
      if (pos.Length < 2){
        throw new ArgumentException($"\nString \"{pos}\" is too short");
      }
      Point p = new Point();
      switch (Char.ToLower(pos[0])){
        case 'a': p.X = 0; break;
        case 'b': p.X = 1; break;
        case 'c': p.X = 2; break;
        case 'd': p.X = 3; break;
        case 'e': p.X = 4; break;
        case 'f': p.X = 5; break;
        case 'g': p.X = 6; break;
        case 'h': p.X = 7; break;
        default: throw new ArgumentException($"\nInvalid file: {pos[0]} ");
      }
      p.Y = 8 - Convert.ToInt32(pos[1].ToString());
      return p;
    }

    public static ValueTuple<Point, Point> MoveToPoints(string move){
      var from = StringToPoint(move.Substring(0, 2));
      var to   = StringToPoint(move.Substring(3, 2));
      return new ValueTuple<Point, Point>(from, to);
    }

    public static Color CharToColor(char c){
      return c == 'w' ? Color.White : Color.Black;
    }

    public static string PieceToString(PieceModel p){
      string pos = "";
      pos += p.Color == Color.White ? "w" : "b";
      pos += GetStringRep(p.Type);
      return pos;
    }
    
  }
}