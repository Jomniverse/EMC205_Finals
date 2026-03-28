using UnityEngine;

public class HotlineRewardManager : MonoBehaviour
{
    [Header("Optional UI Refresh References")]
    [SerializeField] private Report_Manager employeeReportManager;
    [SerializeField] private MarketPlace_UI_Manager marketplaceUIManager;
    [SerializeField] private Shop_UI_Manager shopUIManager;

    public void GiveMoney()
    {
        if (Profile_Manager.Instance == null || Profile_Manager.Instance.currentProfile == null)
        {
            Debug.LogWarning("No current profile selected.");
            return;
        }

        Profile_Manager.Instance.currentProfile.player_money += 10000;
        Profile_Manager.Instance.SaveCurrentProfile();

        RefreshEverything();
    }

    public void GiveExperience()
    {
        if (Profile_Manager.Instance == null || Profile_Manager.Instance.currentProfile == null)
        {
            Debug.LogWarning("No current profile selected.");
            return;
        }

        Profile_Manager.Instance.currentProfile.player_experience += 100000;
        Profile_Manager.Instance.SaveCurrentProfile();

        RefreshEverything();
    }

    private void RefreshEverything()
    {
        if (employeeReportManager != null)
        {
            employeeReportManager.RefreshReport();
        }

        if (marketplaceUIManager != null)
        {
            marketplaceUIManager.RefreshMoney();
            marketplaceUIManager.RefreshTransactionItems();
        }

        if (shopUIManager != null)
        {
            shopUIManager.RefreshShopList();
        }
    }
}