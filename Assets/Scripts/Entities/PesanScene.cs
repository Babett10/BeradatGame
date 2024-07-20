using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPesanScene", menuName ="Data/New Pesan Scene")]
[System.Serializable]
public class PesanScene : GameScene
{
    public List<Sentence> sentences;
    public GameScene nextScene;
    public GameScene previousScene;

    [System.Serializable]
    public struct Sentence
    {
        public string text;
    }
}

