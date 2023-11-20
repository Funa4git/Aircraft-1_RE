// using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private Image image;
    private Sprite defaultSprite;
    public bool interactable = true;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private Sprite lockedSprite;

    [SerializeField] private int id = 0;
    [SerializeField] private float pressOffsetY = 0f;
    public UnityEvent onClick = null; // 外部からEventを設定するためpublic

    private Transform child;
    private float defaultY;
    private ButtonController[] buttonControllers;

    private bool isPushed = false;

    void Awake()
    {
        image = GetComponent<Image>();
        defaultSprite = image.sprite;
        child = transform.GetChild(0);
        defaultY = child.localPosition.y;
        Transform canvas = GameObject.Find("Canvas").transform;
        buttonControllers = canvas.GetComponentsInChildren<ButtonController>();
    }

    // ボタンロック時のときの共通処理
    public void InitializeInteractable(){
        if(interactable) return;
        gameObject.AddComponent<CanvasGroup>();
        CanvasGroup buttonGroup = this.GetComponent<CanvasGroup>();
        buttonGroup.alpha = 0.9f;
        if (lockedSprite != null) image.sprite = lockedSprite;
    }

    void OnEnable()
    {
        ButtonActive(true);
    }

    public void ButtonActive(bool active)
    {
        isPushed = !active;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPushed || !interactable) return;
        Vector3 pos = child.localPosition;
        pos.y = defaultY - pressOffsetY;
        child.localPosition = pos;
        if (pressedSprite != null) image.sprite = pressedSprite;
        OnButtonDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPushed || !interactable) return;
        Vector3 pos = child.localPosition;
        pos.y = defaultY;
        child.localPosition = pos;
        image.sprite = defaultSprite;
        OnButtonUp();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable) return;
        if (id != -1)
        {
            // idが-1のやつ以外は、idが同じならfalse,idが違うならtrue
            foreach (var controller in buttonControllers)
            {
                // idが同じ→ButtonActiveにfalseが渡される→
                // isPushedがtrueになる→他のボタンの押下防止
                controller.ButtonActive(controller.id != this.id);
            }
        }

        OnButtonClick();
        onClick?.Invoke();
    }

    public void AllButtonReset()
    {
        foreach (var controller in buttonControllers)
        {
            controller.ButtonActive(true);
        }
    }

    private void OnButtonDown()
    {
        // Down時の共通処理
        Debug.Log("Downされた");
    }

    private void OnButtonUp()
    {
        // Up時の共通処理
        Debug.Log("Upされた");
    }

    private void OnButtonClick()
    {
        // Click時の共通処理（SE鳴らすなど）
        Debug.Log("Clickされた");
        AllButtonReset();
    }
}