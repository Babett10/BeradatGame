using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPuzzleScene", menuName = "Data/New Puzzle Scene")]
[System.Serializable]
public class PuzzleScene : GameScene
{
    public List<PuzzleImage> puzzleImages;
    public GameScene nextSceneAfterPuzzle;

    [System.Serializable]
    public struct PuzzleImage
    {
        public Sprite image;
        public float correctRotation;
        public float currentRotation;

        // Method to update the current rotation
        public void UpdateRotation(float rotation)
        {
            currentRotation = rotation;
        }
    }
}
