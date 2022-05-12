using System;
using System.Collections;

/*
    The board is a 9x9 array with the 0-indexes unused.
    First entry (1, .) is vertical and second entry is horizontal.
    (2, 5) to (4, 5) would then correspond to E2 to E4.
*/

public class Game {
    Piece[,] board;
    int currentTurn;
    Hashtable whitePieces;
    Hashtable blackPieces;
    (int, int) whiteKingPosition;
    (int, int) blackKingPosition;

    public Game() {
        board = new Piece[9,9];
        currentTurn = 1;
        whitePieces = new Hashtable();
        blackPieces = new Hashtable();
        // init pawns
        for (int i = 1 ; i < 9; i++) {
            createAndSetPiece("Pawn", 1, (2, i));
            createAndSetPiece("Pawn", 0, (7, i));
        }
        // init white side;
        createAndSetPiece("Rook", 1, (1, 1));
        createAndSetPiece("Knight", 1, (1, 2));
        createAndSetPiece("Bishop", 1, (1, 3));
        createAndSetPiece("Queen", 1, (1, 4));
        createAndSetPiece("King", 1, (1, 5));
        createAndSetPiece("Bishop", 1, (1, 6));
        createAndSetPiece("Knight", 1, (1, 7));
        createAndSetPiece("Rook", 1, (1, 8));
        // init black side
        createAndSetPiece("Rook", 0, (8, 1));
        createAndSetPiece("Knight", 0, (8, 2));
        createAndSetPiece("Bishop", 0, (8, 3));
        createAndSetPiece("Queen", 0, (8, 4));
        createAndSetPiece("King", 0, (8, 5));
        createAndSetPiece("Bishop", 0, (8, 6));
        createAndSetPiece("Knight", 0, (8, 7));
        createAndSetPiece("Rook", 0, (8, 8));
        return;
    }

    public void printBoard() {
        for (int i = 1 ; i <= 8 ; i++) {
            for (int j = 1 ; j <= 8 ; j++) {
                Piece piece = board[i, j];
                if (piece != null) {
                    Console.WriteLine(piece.getType() + " " + piece.getSide() + "(" + i + ", " + j + ")");
                }
            }
        }
        Console.WriteLine("\n");
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

    public void createAndSetPiece(String type, int side, (int, int) coordinate) {
        Piece piece = new Piece(type, side);
        setPiece(piece, coordinate);
    }

    public void setPiece(Piece piece, (int, int) coordinate) {
        board[coordinate.Item1, coordinate.Item2] = piece;
        if (piece.getSide() == 1) {
            if (piece.getType() == "King") whiteKingPosition = coordinate;
            whitePieces.Add(piece, coordinate);
        }
        else {
            if (piece.getType() == "King") blackKingPosition = coordinate;
            blackPieces.Add(piece, coordinate);
        }
    }

    public void cleanBoard() {
        board = new Piece[9,9];
        whitePieces = new Hashtable();
        blackPieces = new Hashtable();
        whiteKingPosition = (0, 0);
        blackKingPosition = (0, 0);
    }

    public int getTurn() {
        return currentTurn;
    }

    public Piece[,] getBoard() {
        return board;
    }

    public Hashtable getPiecesOf(int side) {
        if (side == 1) return whitePieces;
        else return blackPieces;
    }

    public (int, int) getKingPositionOf(int side) {
        if (side == 1) return whiteKingPosition;
        else return blackKingPosition;
    }

    public bool currentTurnKingIsChecked() {
        if (currentTurn == 1) {
            foreach ((int, int) opponentPiece in blackPieces.Values) {
                changeTurn(); // Change the turn to make opponent moves legal
                bool isLegal = isMoveLegal(opponentPiece, whiteKingPosition);
                changeTurn();
                if (isLegal) return true;
            }
        } else {
            foreach ((int, int) opponentPiece in whitePieces.Values) {
                changeTurn();
                bool isLegal = isMoveLegal(opponentPiece, blackKingPosition);
                changeTurn();
                if (isLegal) return true;
            }
        }
        return false;
    }

    public void movePiece((int, int) from, (int, int) to) {
        Piece piece = board[from.Item1, from.Item2];
        Piece savedPiece = board[to.Item1, to.Item2];
        if (isMoveLegal(from, to)) {
            board[to.Item1, to.Item2] = board[from.Item1, from.Item2];
            board[from.Item1, from.Item2] = null;
            if (piece.getType() == "King") updateKingPosition(to);
            // you cannot make a move that puts your king in check
            if (!currentTurnKingIsChecked()) {
                // remove captured piece if there is any
                if (savedPiece != null) removeCapturedPieceFromTable(savedPiece);
                // update piece location
                if (currentTurn == 1) whitePieces[piece] = to;
                else blackPieces[piece] = to;
                changeTurn();
            } else { // else move the pieces back
                if (piece.getType() == "King") updateKingPosition(from);
                board[from.Item1, from.Item2] = piece;
                board[to.Item1, to.Item2] = savedPiece;
            }
        }
        return;
    }

    public void updateKingPosition((int, int) pos) {
        if (currentTurn == 1) whiteKingPosition = pos;
        else blackKingPosition = pos;
    }

    public void removeCapturedPieceFromTable(Piece piece) {
        if (currentTurn == 1) blackPieces.Remove(piece);
        else whitePieces.Remove(piece);
    }

    public bool isMoveLegal((int, int) from, (int, int) to) {
        Piece piece = board[from.Item1, from.Item2];
        if (piece == null) return false; // cannot move nothing 
        if (from == to) return false; // cannot move to the same square
        if ((to.Item1 < 1 && 8 < to.Item1) || (to.Item2 < 1 && 8 < to.Item2)) return false; // cannot move outside the board
        if (currentTurn != piece.getSide()) return false; // cannot move opponents piece
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
        if (moveRookLegal(from, to)) return true;
        else return moveBishopLegal(from, to);
    }

    public bool moveBishopLegal((int, int) from, (int, int) to) {
        int index = 0;
        (int, int) signs = (0, 0);
        for (int i = 1 ; i < 8 ; i++) { // check if move is diagonal and how far
            if ((from.Item1 + i, from.Item2 + i) == to) { index = i ; signs = (1, 1); }
            else if ((from.Item1 + i, from.Item2 - i) == to) { index = i ; signs = (1, -1); }
            else if ((from.Item1 - i, from.Item2 + i) == to) { index = i ; signs = (-1, 1); }
            else if ((from.Item1 - i, from.Item2 - i) == to) { index = i ; signs = (-1, -1); }
        }
        if (index == 0) return false;

        bool freePath = true;
        for (int i = 1 ; i < index ; i++) {
            (int, int) pos = (from.Item1 + (signs.Item1 * i), from.Item2 + (signs.Item2 * i));
            if (board[pos.Item1, pos.Item2] != null) {
                freePath = false;
            }
        }
        return freePath;
    }

    public bool moveKnightLegal((int, int) from, (int, int) to) {
        (int, int)[] legalDistances = new (int, int)[] { (1, 2), (1, -2), (-1, 2), (-1, -2), (2, 1), (2, -1), (-2, 1), (-2, -1) };
        foreach ((int, int) legalDistance in legalDistances) {
            if (tupleAddition(from, legalDistance) == to) return true;
        }
        return false;
    }

    private (int, int) tupleAddition((int, int) a, (int, int) b) {
        (int, int) result;
        result.Item1 = a.Item1 + b.Item1;
        result.Item2 = a.Item2 + b.Item2;
        return result;
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

