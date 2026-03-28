using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Software_Set : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Software_Scriptable SS;
    public TMP_Text software_name;
    public Image software_icon;

    public TMP_Text software_description;
    public RectTransform description_background;

    [Header("Description Box Settings")]
    public float maxBackgroundWidth = 400f;
    public float horizontalPadding = 20f;
    public float verticalPadding = 20f;


    public GameObject holder_description;

    public bool isHover = false;
    public float description_appear;
    public float t;

    private void Start()
    {
        Software_information();

        holder_description.SetActive(false);

        
        t = 0f;
    }

    private void Update()
    {
        hover();
    }

    public void Software_information()
    {
        software_name.text = SS.software_name;
        software_icon.sprite = SS.software_icon;
        software_description.text = SS.software_description;

        ResizeDescriptionBox();
    }

    public void hover()
    {
        if (isHover)
        {
            t += Time.deltaTime;

            if (t > description_appear)
            {
                holder_description.SetActive(true);
                isHover = false;
            }
        }
    }

    void ResizeDescriptionBox()
    {
        float maxTextWidth = maxBackgroundWidth - (horizontalPadding * 2);

        // Get size the text wants, while allowing wrapping
        Vector2 preferredSize = software_description.GetPreferredValues(software_description.text, maxTextWidth, 0f);

        float backgroundWidth = Mathf.Min(preferredSize.x + (horizontalPadding * 2), maxBackgroundWidth);
        float textWidth = backgroundWidth - (horizontalPadding * 2);

        // Recalculate height based on final width
        preferredSize = software_description.GetPreferredValues(software_description.text, textWidth, 0f);

        float backgroundHeight = preferredSize.y + (verticalPadding * 2);

        // Resize background
        description_background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, backgroundWidth);
        description_background.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, backgroundHeight);

        // Resize text area
        RectTransform textRect = software_description.rectTransform;
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textWidth);
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredSize.y);
    }


    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        isHover = true;
        t = 0f;
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        isHover = false;
        holder_description.SetActive(false);
        t = 0f;
    }

    
}
