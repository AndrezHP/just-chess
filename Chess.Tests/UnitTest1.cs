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
    public void LegalPawnMoveTest() {
        Game game = new Game();
        bool isLegal = game.isMoveLegal((2, 5), (4, 5));
        Assert.IsTrue(isLegal, "This should be a legal move");

        bool isLegal2 = game.isMoveLegal((2, 4), (3, 4));
        Assert.IsTrue(isLegal2, "This should also be a legal move");
    }

    [TestMethod]
    public void IllegalPawnMoveTest() {
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
    public void PawnCannotAlly() {

    }
}