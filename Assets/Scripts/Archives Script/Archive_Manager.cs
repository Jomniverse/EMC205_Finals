using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Archive_Manager : MonoBehaviour
{
    [Header("Archive Data")]
    [SerializeField] private Archive_Scriptable[] entries;

    [Header("Managers")]
    [SerializeField] private Call_Manager callManager;

    [Header("Archive List")]
    [SerializeField] private Transform archive_list_holder;
    [SerializeField] private GameObject archive_prefab;

    [Header("Right Panel")]
    [SerializeField] private TMP_Text archive_entry_name;
    [SerializeField] private Image archive_image;
    [SerializeField] private TMP_Text archive_description;
    [SerializeField] private TMP_Text archive_Risk;
    [SerializeField] private TMP_Text archive_Risk_Bullets;
    [SerializeField] private TMP_Text archive_Solution;

    [Header("Scroll")]
    [SerializeField] private ScrollRect archive_scroll_rect;
    [SerializeField] private RectTransform content_holder;

    [Header("Text Resize")]
    [SerializeField] private float extraHeightPadding = 10f;

    [Header("Image Resize")]
    [SerializeField] private float archiveImageWidth = 300f;

    private void OnEnable()
    {
        RefreshEntriesList();
        ShowFirstUnlockedEntry();
    }

    public void RefreshArchivesForCurrentDay()
    {
        RefreshEntriesList();
        ShowFirstUnlockedEntry();
    }

    public bool IsArchiveUnlocked(Archive_Scriptable archiveData)
    {
        if (archiveData == null) return false;

        int currentDay = 1;

        if (callManager != null)
            currentDay = callManager.CurrentDayNumber;

        return archiveData.IsUnlocked(currentDay);
    }

    public void RefreshEntriesList()
    {
        if (archive_list_holder == null || archive_prefab == null) return;

        for (int i = archive_list_holder.childCount - 1; i >= 0; i--)
        {
            Destroy(archive_list_holder.GetChild(i).gameObject);
        }

        if (entries == null) return;

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i] == null) continue;

            GameObject item = Instantiate(archive_prefab, archive_list_holder);
            Archives_Item itemUI = item.GetComponent<Archives_Item>();

            if (itemUI != null)
                itemUI.Setup(entries[i], this);
        }
    }

    private void ShowFirstUnlockedEntry()
    {
        if (entries == null || entries.Length == 0)
        {
            ClearArchiveDisplay();
            return;
        }

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i] != null && IsArchiveUnlocked(entries[i]))
            {
                ShowArchive(entries[i]);
                return;
            }
        }

        ClearArchiveDisplay();
    }

    public void ShowArchive(Archive_Scriptable archiveData)
    {
        if (archiveData == null || !IsArchiveUnlocked(archiveData))
            return;

        if (archive_entry_name != null)
            archive_entry_name.text = archiveData.archive_name;

        if (archive_image != null)
        {
            archive_image.sprite = archiveData.archive_image;
            archive_image.enabled = archiveData.archive_image != null;

            if (archiveData.archive_image != null)
                ResizeImageToWidth(archive_image, archiveImageWidth);
        }

        if (archive_description != null)
            archive_description.text = archiveData.description;

        if (archive_Risk != null)
            archive_Risk.text = archiveData.risk;

        if (archive_Risk_Bullets != null)
            archive_Risk_Bullets.text = archiveData.risk_bullets;

        if (archive_Solution != null)
            archive_Solution.text = archiveData.solution;

        ResizeTextField(archive_entry_name);
        ResizeTextField(archive_description);
        ResizeTextField(archive_Risk);
        ResizeTextField(archive_Risk_Bullets);
        ResizeTextField(archive_Solution);

        StartCoroutine(RefreshLayoutAndResetScroll());
    }

    private void ResizeTextField(TMP_Text textField)
    {
        if (textField == null) return;

        textField.textWrappingMode = TextWrappingModes.Normal;
        textField.overflowMode = TextOverflowModes.Overflow;

        RectTransform rect = textField.rectTransform;

        float width = rect.rect.width;
        if (width <= 0f)
            width = rect.sizeDelta.x;

        Vector2 preferredSize = textField.GetPreferredValues(textField.text, width, 0f);
        float preferredHeight = preferredSize.y + extraHeightPadding;

        LayoutElement layoutElement = textField.GetComponent<LayoutElement>();
        if (layoutElement == null)
            layoutElement = textField.gameObject.AddComponent<LayoutElement>();

        layoutElement.minHeight = preferredHeight;
        layoutElement.preferredHeight = preferredHeight;
        layoutElement.flexibleHeight = 0f;

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight);

        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    private void ResizeImageToWidth(Image targetImage, float targetWidth)
    {
        if (targetImage == null || targetImage.sprite == null) return;

        RectTransform rect = targetImage.rectTransform;

        float spriteWidth = targetImage.sprite.rect.width;
        float spriteHeight = targetImage.sprite.rect.height;

        float aspectRatio = spriteHeight / spriteWidth;
        float targetHeight = targetWidth * aspectRatio;

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
    }

    private IEnumerator RefreshLayoutAndResetScroll()
    {
        yield return null;
        yield return null;

        Canvas.ForceUpdateCanvases();

        ResizeTextField(archive_entry_name);
        ResizeTextField(archive_description);
        ResizeTextField(archive_Risk);
        ResizeTextField(archive_Risk_Bullets);
        ResizeTextField(archive_Solution);

        if (content_holder != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(content_holder);

        if (archive_scroll_rect != null)
            archive_scroll_rect.verticalNormalizedPosition = 1f;
    }

    private void ClearArchiveDisplay()
    {
        if (archive_entry_name != null)
            archive_entry_name.text = "";

        if (archive_image != null)
        {
            archive_image.sprite = null;
            archive_image.enabled = false;
        }

        if (archive_description != null)
            archive_description.text = "";

        if (archive_Risk != null)
            archive_Risk.text = "";

        if (archive_Risk_Bullets != null)
            archive_Risk_Bullets.text = "";

        if (archive_Solution != null)
            archive_Solution.text = "";
    }
}