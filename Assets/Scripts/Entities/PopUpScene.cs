using UnityEngine;

[CreateAssetMenu(fileName = "NewPopUpScene", menuName = "Data/New PopUp Scene")]
[System.Serializable]
public class PopUpScene : GameScene
{
    public Sprite popupImage;
    public GameScene nextSceneAfterPopup;
}
