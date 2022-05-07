using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Chess.Tests;

[TestClass]
public class UnitTest1 {
    [TestMethod]
    public void TurnChangeTest() {
        Game game= new Game();
        Assert.IsTrue(game.getTurn() == 1, "Turn should be 1 in the start");
        game.changeTurn();
        Assert.IsTrue(game.getTurn() == 0, "Turn should be 0 after turn change");
    }

    [TestMethod]
    public void MoveNullPiece() {
        Game game = new Game();
        bool isMoveLegal = game.isMoveLegal((4, 4), (5, 5));
        Assert.IsFalse(isMoveLegal, "You cannot move nothing");
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


}