using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockGroupTypes {

    public static Block[] Type_I = new Block[] {
        new Block(new IntVector3(-1, 0, 0)),
        new Block(new IntVector3(0, 0, 0)),
        new Block(new IntVector3(1, 0, 0)),
        new Block(new IntVector3(2, 0, 0))
    };
    public static Block[] Type_J = new Block[] {
        new Block(new IntVector3(-1, 0, 0)),
        new Block(new IntVector3(0, 0, 0)),
        new Block(new IntVector3(1, 0, 0)),
        new Block(new IntVector3(1, 0, -1))
    };
    public static Block[] Type_L = new Block[] {
        new Block(new IntVector3(-1, 0, 0)),
        new Block(new IntVector3(0, 0, 0)),
        new Block(new IntVector3(1, 0, 0)),
        new Block(new IntVector3(1, 0, 1))
    };
    public static Block[] Type_T = new Block[] {
        new Block(new IntVector3(-1, 0, 0)),
        new Block(new IntVector3(0, 0, 0)),
        new Block(new IntVector3(1, 0, 0)),
        new Block(new IntVector3(0, 0, 1))
    };
    public static Block[] Type_S = new Block[] {
        new Block(new IntVector3(0, 0, -1)),
        new Block(new IntVector3(0, 0, 0)),
        new Block(new IntVector3(1, 0, 0)),
        new Block(new IntVector3(1, 0, 1))
    };
    public static Block[] Type_Z = new Block[] {
        new Block(new IntVector3(0, 0, -1)),
        new Block(new IntVector3(0, 0, 0)),
        new Block(new IntVector3(-1, 0, 0)),
        new Block(new IntVector3(-1, 0, 1))
    };
    public static Block[] Type_O = new Block[] {
        new Block(new IntVector3(0, 0, 0)),
        new Block(new IntVector3(0, 0, -1)),
        new Block(new IntVector3(-1, 0, 0)),
        new Block(new IntVector3(-1, 0, -1))
    };

    private const int AMOUNT = 7;

    public static Block[] GetRandom() {
        int rnd = Random.Range(0, AMOUNT);
        switch(rnd) {
            case 0:
                return Type_I;
            case 1:
                return Type_J;
            case 2:
                return Type_L;
            case 3:
                return Type_T;
            case 4:
                return Type_S;
            case 5:
                return Type_Z;
            case 6:
                return Type_O;
        }
        return Type_I;
    }
}
