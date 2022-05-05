using System;

public class Game {
    Piece[,] board;
    int currentTurn;

    public Game() {
        board = new Piece[9,9];
        // init pawns
        for (int i = 1 ; i < 9; i++) {
            board[2, i] = new Piece("Pawn", 1);
            board[7, i] = new Piece("Pawn", 0);
        }
        // init white side
        board[1, 1] = new Piece("Rook", 1);
        board[1, 8] = new Piece("Rook", 1);
        board[1, 2] = new Piece("Knight", 1);
        board[1, 7] = new Piece("Knight", 1);
        board[1, 3] = new Piece("Bishop", 1);
        board[1, 6] = new Piece("Bishop", 1);
        board[1, 4] = new Piece("Queen", 1);
        board[1, 5] = new Piece("King", 1);
        // init black side
        board[8, 1] = new Piece("Rook", 1);
        board[8, 8] = new Piece("Rook", 1);
        board[8, 2] = new Piece("Knight", 1);
        board[8, 7] = new Piece("Knight", 1);
        board[8, 3] = new Piece("Bishop", 1);
        board[8, 6] = new Piece("Bishop", 1);
        board[8, 4] = new Piece("Queen", 1);
        board[8, 5] = new Piece("King", 1);
        currentTurn = 1;
        return;
    }

    public int getTurn() {
        return currentTurn;
    }

    public Piece[,] getBoard() {
        return board;
    }

    public void movePiece((int, int) from, (int, int) to) {
        if (isMoveLegal(from, to)) {
            board[to.Item1, to.Item2] = board[from.Item1, from.Item2];
            board[from.Item1, from.Item2] = null;
            changeTurn();
        }
        return;
    }

    public bool isMoveLegal((int, int) from, (int, int) to) {
        if (board[from.Item1, from.Item2] == null) return false;
        if ((8 < to.Item1 && to.Item1 < 0) || (8 < to.Item2 && to.Item2 < 0)) return false;
        switch (board[from.Item1, from.Item2].getType()) {
            case "King": return moveKingLegal(from, to);
            case "Queen": return moveQueenLegal(from, to);
            case "Bishop": return moveBishopLegal(from, to);
            case "Knight": return moveKnightLegal(from, to);
            case "Rook": return moveRookLegal(from, to);
            case "Pawn": return movePawnLegal(from, to);
            default: return false;
        }
    }

    public void changeTurn() {
        currentTurn = currentTurn ^ 1;
    }

    static public void Main(string[] args) {
        Piece noPiece = null;
        Game game = new Game();
        Console.WriteLine("Nobody won yet");
        game.movePiece((2, 2), (4, 2));
    }

    public bool moveKingLegal((int, int) from, (int, int) to) {
        return false;
    }

    public bool moveQueenLegal((int, int) from, (int, int) to) {
        return false;
    }

    public bool moveBishopLegal((int, int) from, (int, int) to) {
        return false;
    }

    public bool moveKnightLegal((int, int) from, (int, int) to) {
        return false;
    }

    public bool moveRookLegal((int, int) from, (int, int) to) {
        if (!(from.Item1 == to.Item1 || from.Item2 == to.Item2)) return false;

        return false;
    }

    public bool movePawnLegal((int, int) from, (int, int) to) {
        Piece piece = board[from.Item1, from.Item2];
        int side = piece.getSide();
        bool destIsEmpty = board[to.Item1, to.Item2] == null;
        if (side == 1) {
            int verticalMoveDistance = to.Item1 - from.Item1; 
            if ((1 > verticalMoveDistance && verticalMoveDistance > 2) && to.Item2 == from.Item2) {
                return (destIsEmpty && board[from.Item1 + 1, from.Item2] == null);
            }
            else if (destIsEmpty) return false; // cannot move to other squares if not attacking
            else if (to.Item1 == from.Item1 + 1 && 
                    (to.Item2 == from.Item2 - 1 || to.Item2 == from.Item2 + 1)) {
                return (side != board[to.Item1, to.Item2].getSide()); // can attack non-allies diagonally 
            } else {
                return false;
            }
        } else {
            
        }
        return false;
    }
}

