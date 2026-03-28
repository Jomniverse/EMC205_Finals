using TMPro;
using UnityEngine;

public class Mail_Manager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private Call_Manager callManager;

    [Header("Mail Day Data")]
    [SerializeField] private DayMail_Scriptable[] mailDays;

    [Header("Mail List")]
    [SerializeField] private Transform mail_list_holder;
    [SerializeField] private GameObject mail_prefab;

    [Header("Right Panel")]
    [SerializeField] private TMP_Text sender_Text;
    [SerializeField] private TMP_Text subject_Text;
    [SerializeField] private TMP_Text message_Text;

    private DayMail_Scriptable currentMailDay;
    private Mail_Scriptable[] currentMails;

    private void OnEnable()
    {
        RefreshMailsForCurrentDay();
    }

    public void RefreshMailsForCurrentDay()
    {
        currentMailDay = GetMailDayByNumber(GetCurrentDayNumber());
        currentMails = currentMailDay != null ? currentMailDay.mails : null;

        RefreshMailList();

        if (currentMails != null && currentMails.Length > 0 && currentMails[0] != null)
        {
            ShowMail(currentMails[0]);
        }
        else
        {
            ClearMailDisplay();
        }
    }

    private int GetCurrentDayNumber()
    {
        if (callManager != null)
            return callManager.CurrentDayNumber;

        return 1;
    }

    private DayMail_Scriptable GetMailDayByNumber(int dayNumber)
    {
        if (mailDays == null) return null;

        for (int i = 0; i < mailDays.Length; i++)
        {
            if (mailDays[i] == null) continue;

            if (mailDays[i].dayNumber == dayNumber)
                return mailDays[i];
        }

        return null;
    }

    public void RefreshMailList()
    {
        if (mail_list_holder == null || mail_prefab == null) return;

        for (int i = mail_list_holder.childCount - 1; i >= 0; i--)
        {
            Destroy(mail_list_holder.GetChild(i).gameObject);
        }

        if (currentMails == null) return;

        for (int i = 0; i < currentMails.Length; i++)
        {
            if (currentMails[i] == null) continue;

            GameObject item = Instantiate(mail_prefab, mail_list_holder);
            Mail_Item itemUI = item.GetComponent<Mail_Item>();

            if (itemUI != null)
            {
                itemUI.Setup(currentMails[i], this);
            }
        }
    }

    public void ShowMail(Mail_Scriptable mailData)
    {
        if (mailData == null)
        {
            ClearMailDisplay();
            return;
        }

        if (sender_Text != null)
            sender_Text.text = "From: " + mailData.sender;

        if (subject_Text != null)
            subject_Text.text = "Subject Line: " + mailData.subject;

        if (message_Text != null)
            message_Text.text = mailData.message;
    }

    private void ClearMailDisplay()
    {
        if (sender_Text != null)
            sender_Text.text = "";

        if (subject_Text != null)
            subject_Text.text = "";

        if (message_Text != null)
            message_Text.text = "";
    }
}