using System;
public class Piece {
    public string type;
    public int side;

    public Piece(string type, int side) {
        if (!isLegalType(type)) {
            Console.WriteLine("wrong piece type");
            this.side = 42;
            this.type = "Wrong";
            return;
        }
        this.type = type;
        if (!(side == 0 || side == 1)) {
            Console.WriteLine("side should be 0 or 1");
            this.side = 42;
            this.type = "Wrong";
            return;
        }
        this.side = side;
        return;
    }

    public string getType() {
        return type;
    }

    public int getSide() {
        return side;
    }
    public bool isLegalType(string type) {
        switch (type) {
            case "King": return true;
            case "Queen": return true;
            case "Bishop": return true;
            case "Knight": return true;
            case "Rook": return true;
            case "Pawn": return true;
            default: return false;
        }
    }
}