using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerUiControl : MonoBehaviour
{
    public Button buttonLeft;
    public Button buttonRight;

    private PlayerMovement playerMovement;

    private bool moveLeft;
    private bool moveRight;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        // Menghubungkan Event Triggers
        AddEventTrigger(buttonLeft.gameObject, EventTriggerType.PointerDown, (data) => { OnLeftButtonDown(); });
        AddEventTrigger(buttonLeft.gameObject, EventTriggerType.PointerUp, (data) => { OnButtonUp(); });

        AddEventTrigger(buttonRight.gameObject, EventTriggerType.PointerDown, (data) => { OnRightButtonDown(); });
        AddEventTrigger(buttonRight.gameObject, EventTriggerType.PointerUp, (data) => { OnButtonUp(); });
    }

    private void Update()
    {
        if (moveLeft)
        {
            playerMovement.SetMoveInput(-1);
        }
        else if (moveRight)
        {
            playerMovement.SetMoveInput(1);
        }
        else
        {
            playerMovement.SetMoveInput(0);
        }
    }

    public void OnLeftButtonDown()
    {
        moveLeft = true;
        moveRight = false;
    }

    public void OnRightButtonDown()
    {
        moveRight = true;
        moveLeft = false;
    }

    public void OnButtonUp()
    {
        moveLeft = false;
        moveRight = false;
    }

    private void AddEventTrigger(GameObject obj, EventTriggerType type, System.Action<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener((data) => action.Invoke(data));
        trigger.triggers.Add(entry);
    }
}
