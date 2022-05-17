using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace Chess.Tests;

[TestClass]
public class UnitTest1 {
    [TestMethod]
    public void TurnChangeTest() {
        Game game= new Game();
        Assert.IsTrue(game.getTurn() == 1, "Turn should be 1 in the start");
        game.changeTurn();
        Assert.IsTrue(game.getTurn() == 0, "Turn should be 0 after turn change");
        game.changeTurn();
        Assert.IsTrue(game.getTurn() == 1, "Turn should be 1 after two turn changes");
    }

    [TestMethod]
    public void MoveNullPiece() {
        Game game = new Game();
        bool isMoveLegal = game.isMoveLegal((4, 4), (5, 5));
        Assert.IsFalse(isMoveLegal, "You cannot move nothing");
    }

    [TestMethod]
    public void CannotMoveToCurrentLocation() {
        Game game = new Game();
        bool isLegalMove = game.isMoveLegal((2, 2), (2, 2));
        Assert.IsFalse(isLegalMove, "Pawn cannot move to current location");
    }

    [TestMethod]
    public void TurnChangeOnLegalMove() {
        Game game= new Game();
        game.movePiece((2, 4), (4, 4));
        Assert.IsTrue(game.getTurn() == 0, "Turn should have changed on move");
    }

    [TestMethod]
    public void LegalWhitePawnMoveTest() {
        Game game = new Game();
        bool isLegal = game.isMoveLegal((2, 5), (4, 5));
        Assert.IsTrue(isLegal, "This should be a legal move");

        bool isLegal2 = game.isMoveLegal((2, 4), (3, 4));
        Assert.IsTrue(isLegal2, "This should also be a legal move");
    }

    [TestMethod]
    public void IllegalWhitePawnMoveTest() {
        Game game = new Game();
        bool isLegal = game.isMoveLegal((2, 5), (3, 6));
        Assert.IsFalse(isLegal, "This should be an illegal move");

        bool isLegal2 = game.isMoveLegal((2, 4), (2, 6));
        Assert.IsFalse(isLegal2, "This should also be an illegal move");
    }

    [TestMethod]
    public void PawnMovedTest() {
        Game game = new Game();
        bool destIsEmpty = game.getBoard()[4, 4] == null;
        Assert.IsTrue(destIsEmpty, "Destination should be empty");
        
        game.movePiece((2, 4), (4, 4));
        bool destNotEmpty = game.getBoard()[4, 4] != null;
        Assert.IsTrue(destNotEmpty, "There should be a piece now");

        bool fromIsNowEmpty = game.getBoard()[2, 4] == null;
        Assert.IsTrue(fromIsNowEmpty, "From should now be empty");
    }

    [TestMethod]
    public void WhiteCannotMoveTwice() {
        Game game = new Game();
        game.movePiece((2, 4), (4, 4));
        bool firstDestNotEmpty = game.getBoard()[4, 4] != null;
        Assert.IsTrue(firstDestNotEmpty, "First piece should have been moved");

        game.movePiece((2, 5), (3, 5));
        bool secondDestEmpty = game.getBoard()[3, 5] == null;
        Assert.IsTrue(secondDestEmpty, "White should not be able to move twice");
    }

    [TestMethod]
    public void LegalBlackPawnMoveTest() {
        Game game = new Game();
        game.changeTurn();
        bool isLegal = game.isMoveLegal((7, 5), (5, 5));
        Assert.IsTrue(isLegal, "This should be a legal move");

        bool isLegal2 = game.isMoveLegal((7, 4), (6, 4));
        Assert.IsTrue(isLegal2, "This should also be a legal move");
    }

    [TestMethod]
    public void IllegalBlackPawnMoveTest() {
        Game game = new Game();
        bool isLegal = game.isMoveLegal((7, 5), (4, 4));
        Assert.IsFalse(isLegal, "This should be an illegal move");

        bool isLegal2 = game.isMoveLegal((7, 4), (3, 3));
        Assert.IsFalse(isLegal2, "This should also be an illegal move");
    }

    [TestMethod]
    public void BlackPawnMove() {
        Game game = new Game();
        game.changeTurn();

        game.movePiece((7, 4), (5, 4));
        bool firstDestNotEmpty = game.getBoard()[5, 4] != null;
        Assert.IsTrue(firstDestNotEmpty, "First piece should have been moved");
    }

    [TestMethod]
    public void PawnCannotAttackAlly() {
        Game game = new Game();

        game.movePiece((2, 4), (4, 4));
        bool firstDestNotEmpty = game.getBoard()[4, 4] != null;
        Assert.IsTrue(firstDestNotEmpty, "First piece should have been moved");

        game.changeTurn();
        game.movePiece((2, 5), (3, 5));
        bool secondDestEmpty = game.getBoard()[3, 5] != null;
        Assert.IsTrue(secondDestEmpty, "White should be able to move after turn change");

        game.changeTurn();
        bool isMoveLegal = game.isMoveLegal((3, 5), (4, 4));
        Assert.IsFalse(isMoveLegal, "White pawn should not be able to attack ally");
    }

    [TestMethod]
    public void WhitePawnCanTakeBlackPawn() {
        Game game = new Game();

        game.movePiece((2, 5), (4, 5));
        game.movePiece((7, 4), (5, 4));
        
        bool isMoveLegal = game.isMoveLegal((4, 5), (5, 4));
        Assert.IsTrue(isMoveLegal, "White is allowed to take black pawn here");

        game.movePiece((4, 5), (5, 4));
        bool pawnIsNowWhite = game.getBoard()[5, 4].getSide() == 1;
        Assert.IsTrue(pawnIsNowWhite, "White should have taken and replaced this pawn");
    }

    [TestMethod]
    public void RookCannotMoveDiagonally() {
        Game game = new Game();
        Piece rook = new Piece("Rook", 1);
        game.cleanBoard();
        game.setPiece(rook, (5, 5));

        bool firstMoveIsLegal = game.isMoveLegal((5, 5), (7, 7));
        Assert.IsFalse(firstMoveIsLegal, "Illegal diagonal rook move");
        bool secondMoveIsLegal = game.isMoveLegal((5, 5), (3, 4));
        Assert.IsFalse(secondMoveIsLegal, "Also illegal diagonal rook move");
    }

    [TestMethod]
    public void RookCanMoveStraight() {
        Game game = new Game();
        Piece rook = new Piece("Rook", 1);
        game.cleanBoard();
        game.setPiece(rook, (5, 5));

        bool firstMoveIsLegal = game.isMoveLegal((5, 5), (5, 8));
        bool secondMoveIsLegal = game.isMoveLegal((5, 5), (1, 5));
        Assert.IsTrue(firstMoveIsLegal, "This rook move should be legal");
        Assert.IsTrue(secondMoveIsLegal, "This rook move should also be legal");
    }

    [TestMethod]
    public void RooksCannotJumpOverPawn() {
        Game game = new Game();
        Assert.IsTrue(game.getBoard()[1,1].getType().Equals("Rook"), "This piece should be a rook");

        bool firstMoveIsLegal = game.isMoveLegal((1, 1), (5, 1));
        bool secondMoveIsLegal = game.isMoveLegal((1, 8), (6, 8));
        Assert.IsFalse(firstMoveIsLegal, "Rooks cannot jump over pawns");
        Assert.IsFalse(secondMoveIsLegal, "Rooks cannot jump over pawns here either");
    }

    [TestMethod]
    public void RookCannotJumpOverEnemyHorizontally() {
        Game game = new Game();
        Piece rook = new Piece("Rook", 1);
        game.setPiece(rook, (5, 5));
        Piece pawn = new Piece("Pawn", 0);
        game.setPiece(pawn, (5, 6));

        bool isMoveLegal = game.isMoveLegal((5, 5), (5, 8));
        Assert.IsFalse(isMoveLegal, "Rooks cannot jump over pieces horizontally either");
    }

    [TestMethod]
    public void IllegalKingMoves() {
        Game game = new Game();
        game.cleanBoard();
        Piece king = new Piece("King", 1);
        game.setPiece(king, (5, 5));

        bool move1Legal = game.isMoveLegal((5, 5), (5, 7));
        bool move2Legal = game.isMoveLegal((5, 5), (3, 7));
        bool move3Legal = game.isMoveLegal((5, 5), (3, 5));
        bool move4Legal = game.isMoveLegal((5, 5), (4, 3));
        bool move5Legal = game.isMoveLegal((5, 5), (8, 7));
        Assert.IsFalse(move1Legal || move2Legal || move3Legal || move4Legal || move5Legal, "These should all be illegal king moves");
    }

    [TestMethod]
    public void LegalKingMoves() {
        Game game = new Game();
        game.cleanBoard();
        Piece king = new Piece("King", 1);
        game.setPiece(king, (5, 5));

        bool move1Legal = game.isMoveLegal((5, 5), (6, 6));
        bool move2Legal = game.isMoveLegal((5, 5), (4, 4));
        bool move3Legal = game.isMoveLegal((5, 5), (4, 5));
        bool move4Legal = game.isMoveLegal((5, 5), (5, 4));
        bool move5Legal = game.isMoveLegal((5, 5), (6, 4));
        Assert.IsTrue(move1Legal && move2Legal && move3Legal && move4Legal && move5Legal, "These should all be legal king moves");
    }
    
    [TestMethod]
    public void IllegalKnightMoves() {
        Game game = new Game();

        bool move1Legal = game.isMoveLegal((1, 2), (3, 2));

        game.cleanBoard();
        Piece knight = new Piece("Knight", 1);
        game.setPiece(knight, (5, 5));

        bool move2Legal = game.isMoveLegal((5, 5), (3, 3));
        bool move3Legal = game.isMoveLegal((5, 5), (6, 6));
        bool move4Legal = game.isMoveLegal((5, 5), (5, 7));
        bool move5Legal = game.isMoveLegal((5, 5), (8, 8));

        Assert.IsFalse(move1Legal || move2Legal || move3Legal || move4Legal || move5Legal, "These should all be illegal knight moves");
    }

    [TestMethod]
    public void LegalKnightMoves() {
        Game game = new Game();

        bool move1Legal = game.isMoveLegal((1, 2), (3, 1));
        bool move2Legal = game.isMoveLegal((1, 2), (3, 3));
        game.changeTurn();
        bool move3Legal = game.isMoveLegal((8, 2), (6, 3));
        game.changeTurn();

        game.cleanBoard();
        Piece knight = new Piece("Knight", 1);
        game.setPiece(knight, (5, 5));

        bool move4Legal = game.isMoveLegal((5, 5), (3, 6));
        bool move5Legal = game.isMoveLegal((5, 5), (4, 3));

        Assert.IsTrue(move1Legal && move2Legal && move3Legal && move4Legal && move5Legal, "These should all be legal knight moves");
    }

    [TestMethod]
    public void IllegalBishopMoves() {
        Game game = new Game();

        bool move1Legal = game.isMoveLegal((1, 2), (3, 2));

        game.cleanBoard();
        Piece bishop = new Piece("Bishop", 1);
        game.setPiece(bishop, (5, 5));

        bool move2Legal = game.isMoveLegal((5, 5), (3, 2));
        bool move3Legal = game.isMoveLegal((5, 5), (7, 6));

        game.setPiece(new Piece("Pawn", 1), (6, 6));
        game.setPiece(new Piece("Knight", 0), (4, 4)); // testing that it cannot jump over pieces
        bool move4Legal = game.isMoveLegal((5, 5), (7, 7));
        bool move5Legal = game.isMoveLegal((5, 5), (3, 3));

        Assert.IsFalse(move1Legal || move2Legal || move3Legal || move4Legal || move5Legal, "These should all be illegal bishop moves");
    }

    [TestMethod]
    public void LegalBishopMoves() {
        Game game = new Game();
        game.movePiece((2, 5), (4, 5));
        game.changeTurn();
        bool move1Legal = game.isMoveLegal((1, 6), (4, 3));
        bool move2Legal = game.isMoveLegal((1, 6), (6, 1));

        game.cleanBoard();
        Piece bishop = new Piece("Bishop", 1);
        game.setPiece(bishop, (5, 5));

        bool move3Legal = game.isMoveLegal((5, 5), (7, 3));
        bool move4Legal = game.isMoveLegal((5, 5), (8, 8));
        bool move5Legal = game.isMoveLegal((5, 5), (1, 1));

        Assert.IsTrue(move1Legal && move2Legal && move3Legal && move4Legal && move5Legal, "These should all be legal bishop moves");
    }
    
    [TestMethod]
    public void CannotMoveFromProtectingKing() {
        Game game = new Game();
        game.cleanBoard();
        game.createAndSetPiece("King", 1, (1, 1));
        game.createAndSetPiece("Rook", 1, (2, 1));
        game.createAndSetPiece("Rook", 0, (8, 1));

        game.printBoard();
        game.movePiece((2, 1), (2, 2));
        game.printBoard();
        bool rookMoved = game.getBoard()[2, 2] != null;

        Assert.IsFalse(rookMoved, "You cannot leave your king unprotected!");
    }

    [TestMethod]
    public void TestUpdatePieceLocation() {
        Game game = new Game();
        game.cleanBoard();
        game.createAndSetPiece("King", 1, (1, 1));
        Piece king = game.getBoard()[1, 1];

        bool kingAt11 = game.getKingPositionOf(1) == (1, 1);
        bool kingInTable11 = game.getPiecesOf(1)[king].Equals((1, 1));

        Assert.IsTrue(kingAt11, "king position is not setup correctly");
        Assert.IsTrue(kingInTable11, "white king has wrong position in table");

        game.movePiece((1, 1), (2, 2));

        bool kingAt22 = game.getKingPositionOf(1) == (2, 2);
        bool kingInTable22 = game.getPiecesOf(1)[king].Equals((2, 2));

        Assert.IsTrue(kingAt22, "king position is not updated correctly");
        Assert.IsTrue(kingInTable22, "white king in table is not updated correctly");
    }

    [TestMethod]
    public void KingCannotMoveIntoDanger() {
        Game game = new Game();
        game.cleanBoard();
        game.createAndSetPiece("King", 1, (1, 1));
        game.createAndSetPiece("Rook", 0, (2, 2));

        game.movePiece((1, 1), (1, 2));
        bool kingMovedIntoDanger = game.getBoard()[1, 2] != null;
        Assert.IsFalse(kingMovedIntoDanger, "You cannot lose your king!");

        game.movePiece((1, 1), (2, 2));
        bool kingCapturedRook = game.getBoard()[2, 2].getType() == "King";
        Assert.IsTrue(kingCapturedRook, "The king should have captured");
    }

    [TestMethod]
    public void PieceCannotMoveFromProtectingKing() {
        Game game = new Game();
        game.cleanBoard();
        game.createAndSetPiece("King", 1, (1, 1));
        game.createAndSetPiece("Bishop", 0, (8, 8));
        game.createAndSetPiece("Pawn", 1, (2, 2));
        
        game.movePiece((2, 2), (3, 2));

        bool pieceMoved = game.getBoard()[3, 2] != null;
        Assert.IsFalse(pieceMoved, "The pawn should not have moved from the king");
    }

    [TestMethod]
    public void WhiteWinsTheGame() {
        Game game = new Game();
        game.cleanBoard();

        game.createAndSetPiece("King", 1, (1, 1));
        game.createAndSetPiece("King", 0, (8, 8));
        game.createAndSetPiece("Pawn", 1, (6, 6));
        game.createAndSetPiece("Queen", 1, (2, 7));

        bool isGameOver1 = game.isGameOver();
        Assert.IsFalse(isGameOver1, "The game should not be over yet");

        game.movePiece((2, 7), (7, 7));

        bool isGameOver2 = game.isGameOver();

        Assert.IsTrue(isGameOver2, "Game should have been over now");

        bool whiteWon = game.getGameConclusion() == "White won";
        Assert.IsTrue(whiteWon, "White should have won now");
    }

    [TestMethod]
    public void BlackWinsTheGame() {
        Game game = new Game();
        game.cleanBoard();

        game.changeTurn();

        game.createAndSetPiece("King", 1, (1, 1));
        game.createAndSetPiece("King", 0, (8, 8));
        game.createAndSetPiece("Pawn", 0, (3, 3));
        game.createAndSetPiece("Queen", 0, (7, 2));

        bool isGameOver1 = game.isGameOver();
        Assert.IsFalse(isGameOver1, "The game should not be over yet");

        game.movePiece((7, 2), (2, 2));

        bool isGameOver2 = game.isGameOver();

        Assert.IsTrue(isGameOver2, "Game should have been over now");
        
        bool blackWon = game.getGameConclusion() == "Black won";
        Assert.IsTrue(blackWon, "Black should have won now");
    }

    [TestMethod]
    public void StaleMate() {
        Game game = new Game();
        game.cleanBoard();

        game.createAndSetPiece("King", 1, (1, 1));
        game.createAndSetPiece("King", 0, (8, 8));
        game.createAndSetPiece("Pawn", 1, (6, 6));
        game.createAndSetPiece("Rook", 1, (2, 7));

        bool isGameOver1 = game.isGameOver();
        Assert.IsFalse(isGameOver1, "The game should not be over yet");

        game.movePiece((2, 7), (7, 7));

        bool isGameOver2 = game.isGameOver();

        Assert.IsTrue(isGameOver2, "Game should have been over now");

        bool staleMate = game.getGameConclusion() == "Stalemate";
        Assert.IsTrue(staleMate, "Should have been statemate now");
    }
}