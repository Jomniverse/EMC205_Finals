using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mail_Item : MonoBehaviour
{
    [SerializeField] private TMP_Text senderText;
    [SerializeField] private TMP_Text subjectText;
    [SerializeField] private Button button;

    private Mail_Scriptable current_Mail;
    private Mail_Manager mailbox_Manager;

    public void Setup(Mail_Scriptable mailData, Mail_Manager manager)
    {
        current_Mail = mailData;
        mailbox_Manager = manager;

        if (senderText != null)
            senderText.text = mailData.sender;

        if (subjectText != null)
            subjectText.text = mailData.subject;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClickMail);
        }
    }

    public void OnClickMail()
    {
        if (mailbox_Manager != null && current_Mail != null)
        {
            mailbox_Manager.ShowMail(current_Mail);
        }
    }
}