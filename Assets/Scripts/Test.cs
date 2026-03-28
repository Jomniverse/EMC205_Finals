using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour, IPointerClickHandler
{
    [Header("Panel to spawn (UI Prefab)")]
    [SerializeField] private GameObject panelPrefab;

    [Header("Where to put it (Canvas or a UI container under Canvas)")]
    [SerializeField] private RectTransform canvasParent;

    [Header("Optional: prevent opening multiple copies")]
    [SerializeField] private bool allowOnlyOne = true;

    private GameObject spawnedPanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount != 2) return;

        if (allowOnlyOne && spawnedPanel != null)
        {
            spawnedPanel.SetActive(true);
            spawnedPanel.transform.SetAsLastSibling(); // bring to front
            CenterPanel(spawnedPanel.GetComponent<RectTransform>());
            return;
        }

        if (panelPrefab == null || canvasParent == null) return;

        spawnedPanel = Instantiate(panelPrefab, canvasParent, false);
        spawnedPanel.transform.SetAsLastSibling(); // bring to front

        RectTransform rt = spawnedPanel.GetComponent<RectTransform>();
        CenterPanel(rt);
    }

    private void CenterPanel(RectTransform rt)
    {
        if (rt == null) return;

        // Center anchors/pivot so anchoredPosition=0 is true center of parent
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;
        rt.localRotation = Quaternion.identity;
    }
}