using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickScript : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Image joystickBGImage;
    [SerializeField]
    private Image joystickButtonImage;

    [HideInInspector]
    public Vector3 inputDirection;

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBGImage.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 position))
        {
            position.x /= joystickBGImage.rectTransform.sizeDelta.x;
            position.y /= joystickBGImage.rectTransform.sizeDelta.y;

            inputDirection = new Vector3(position.x * 2, position.y * 2, 0);
            if (inputDirection.magnitude > 1) inputDirection = inputDirection.normalized;

            joystickButtonImage.rectTransform.anchoredPosition = new Vector3(inputDirection.x * (joystickBGImage.rectTransform.sizeDelta.x / 3.5f), inputDirection.y * (joystickBGImage.rectTransform.sizeDelta.y / 3.5f));
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        ResetJoystick();
    }

    public float Horizontal()
    {
        if (inputDirection.x != 0) return inputDirection.x;
        else return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        if (inputDirection.y != 0) return inputDirection.y;
        else return Input.GetAxis("Vertical");
    }

    public void ResetJoystick()
    {
        inputDirection = Vector3.zero;
        joystickButtonImage.rectTransform.anchoredPosition = inputDirection;
    }
}
