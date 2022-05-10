using System;

/*
    The board is a 9x9 array with the 0-indexes unused.
    First entry (1, .) is vertical and second entry is horizontal.
    (2, 5) to (4, 5) would then correspond to E2 to E4.
*/

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

    // custom board setup for testing or
    // playing a game from a certain position
    public void setupBoard(Piece[] pieces, (int, int)[] coordinates) {
        if (pieces.Length != coordinates.Length) return; 
        board = new Piece[9,9];
        for (int i = 0 ; i < pieces.Length ; i++) {
            board[coordinates[i].Item1, coordinates[i].Item2] = pieces[i];
        }
        return;
    }

    public void setPiece(Piece piece, (int, int) coordinate) {
        board[coordinate.Item1, coordinate.Item2] = piece;
    }

    public void cleanBoard() {
        board = new Piece[9,9];
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
        Piece piece = board[from.Item1, from.Item2];
        if (piece == null) return false; // cannot move nothing 
        if (from == to) return false; // cannot move to the same square
        if ((to.Item1 < 1 && 8 < to.Item1) || (to.Item2 < 1 && 8 < to.Item2)) return false; // cannot move outside the board
        if (getTurn() != piece.getSide()) return false; // cannot move opponents piece
        if (board[to.Item1, to.Item2] != null && board[to.Item1, to.Item2].getSide() == getTurn()) return false;
        switch (piece.getType()) {
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
        Game game = new Game();
        Console.WriteLine("Nobody won yet");
        game.movePiece((2, 2), (4, 2));
    }

    public bool moveKingLegal((int, int) from, (int, int) to) {
        for (int i = -1 ; i <= 1 ; i++) {
            for (int j = -1 ; j <= 1 ; j++) {
                if ((from.Item1 + i, from.Item2 + j) == to) {
                    return true;
                }
            }
        }
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
        bool moveIsVertical = from.Item2 == to.Item2;
        bool moveIsHorizontal = from.Item1 == to.Item1;
        if (!(moveIsVertical || moveIsHorizontal)) return false;

        if (moveIsVertical) {
            int distance = to.Item1 - from.Item1;
            bool freePath = true;
            if (distance > 0) {
                for (int i = from.Item1 + 1 ; i < to.Item1 ; i++) {
                    Console.WriteLine(i);
                    if (board[i, from.Item2] != null) freePath = false;
                }
            } else {
                for (int i = from.Item1 - 1 ; i > to.Item1 ; i--) {
                    if (board[i, from.Item2] != null) freePath = false;
                }
            }
            return freePath;
        } 
        else if (moveIsHorizontal) {
            int distance = to.Item2 - from.Item2;
            bool freePath = true;
            if (distance > 0) {
                for (int i = from.Item2 + 1 ; i < to.Item2 ; i++) {
                    if (board[from.Item1, i] != null) freePath = false;
                }
            } else {
                for (int i = from.Item2 - 1 ; i > to.Item2 ; i--) {
                    if (board[from.Item1, i] != null) freePath = false;
                }
            }
            return freePath;
        }
        return false;
    }

    public bool movePawnLegal((int, int) from, (int, int) to) {
        Piece piece = board[from.Item1, from.Item2];
        bool destIsEmpty = board[to.Item1, to.Item2] == null;
        if (piece.getSide() == 1) {
            int verticalMoveDistance = to.Item1 - from.Item1; 
            if (!(verticalMoveDistance < 1 && 2 < verticalMoveDistance) && to.Item2 == from.Item2) {
                return (destIsEmpty && board[from.Item1 + 1, from.Item2] == null);
            }
            else if (destIsEmpty) return false; // cannot move to other squares if not attacking
            else if (to.Item1 == from.Item1 + 1 && 
                    (to.Item2 == from.Item2 - 1 || to.Item2 == from.Item2 + 1)) {
                return true; // can attack non-allies diagonally 
            } else {
                return false;
            }
        } else {
            int verticalMoveDistance = from.Item1 - to.Item1; 
            if (!(verticalMoveDistance < 1 && 2 < verticalMoveDistance) && to.Item2 == from.Item2) {
                return (destIsEmpty && board[from.Item1 - 1, from.Item2] == null);
            }
            else if (destIsEmpty) return false; // cannot move to other squares if not attacking
            else if (to.Item1 == from.Item1 - 1 && 
                    (to.Item2 == from.Item2 - 1 || to.Item2 == from.Item2 + 1)) {
                return true; // can attack non-allies diagonally 
            } else {
                return false;
            }
        }
    }
}

