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

    //public void MoveTo(Vector3 coordinates) {

    //}

    //private IEnumerator MoveToAnimate(Vector3 coordinates) {
    //    float elapsedTime = 0;
    //    while (elapsedTime < moveTime) {
    //        float normalized = (elapsedTime / animateTime);
    //        currentTextIndex = (int)(chars * normalized);
    //        if (currentTextIndex != indexPrevFrame) {
    //            textComponent.text += originalText[currentTextIndex];
    //            print(currentTextIndex);
    //        }
    //        elapsedTime += Time.deltaTime;
    //        indexPrevFrame = currentTextIndex;

    //        yield return null;
    //    }
        
    //}
}
