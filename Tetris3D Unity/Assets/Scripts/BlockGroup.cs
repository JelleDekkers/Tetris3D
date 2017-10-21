using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGroup {

    public Block[] Blocks { get; private set; }
    public Block RotationPivotBlock { get; private set; }
    
    public BlockGroup(Block[] blocks) {
        Blocks = blocks;
        RotationPivotBlock = blocks[0];
    } 
}
