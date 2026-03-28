using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Call_Manager : MonoBehaviour
{
    public static Call_Manager Instance;


    [Header("Managers")]
    [SerializeField] private Mail_Manager mailManager;
    [SerializeField] private Archive_Manager archiveManager;

    [Header("Panel")]
    [SerializeField] private GameObject call_panel;

    [Header("Day Data")]
    [SerializeField] private Day_Scriptable[] days;
    [SerializeField] private int currentDayIndex;
    public int current_day_counter = 1;

    [Header("All Archives")]
    [SerializeField] private Archive_Scriptable[] allArchives;

    [Header("Call Window UI")]
    [SerializeField] private TMP_Text callerNameText;
    [SerializeField] private TMP_Text callerDialogueText;
    [SerializeField] private Button submitReportButton;
    [SerializeField] private Button zoomButton;

    [Header("Zoomed Call Window UI")]
    [SerializeField] private GameObject zoomPanel;
    [SerializeField] private TMP_Text callerNameText_zoomed;
    [SerializeField] private TMP_Text callerDialogueText_zoomed;

    [Header("Submit Panel UI")]
    [SerializeField] private GameObject submitPanel;
    [SerializeField] private TMP_Dropdown archiveDropdown;

    [Header("Report Success Panel")]
    [SerializeField] private GameObject reportSuccessPanel;

    [Header("Shift Ended Panel")]
    [SerializeField] private GameObject shiftEndedPanel;
    [SerializeField] private TMP_Text shiftEndTitleText;
    [SerializeField] private TMP_Text accuracyText;
    [SerializeField] private TMP_Text salaryText;
    [SerializeField] private TMP_Text goalText;

    [Header("Goal Settings")]
    [SerializeField] private int salaryGoal = 100;
    [SerializeField] private string goalReachText = "Summary Report";
    [SerializeField] private string goalFailText = "Game Over";

    private Day_Scriptable currentDay;
    private int currentCallerIndex = 0;
    private Caller_Scriptable currentCaller;

    private readonly List<Archive_Scriptable> dropdownArchiveReferences = new List<Archive_Scriptable>();

    private int totalCorrectAnswers = 0;
    private int totalWrongAnswers = 0;

    private int dayCorrectAnswers = 0;
    private int dayWrongAnswers = 0;

    private bool daySalaryGranted = false;
    private bool didReachGoalThisShift = false;

    public int CurrentDayNumber
    {
        get
        {
            if (currentDay == null) return 1;
            return currentDay.dayNumber;
        }
    }
    public int TotalCorrectAnswers => totalCorrectAnswers;
    public int TotalWrongAnswers => totalWrongAnswers;
    public int DayCorrectAnswers => dayCorrectAnswers;
    public int DayWrongAnswers => dayWrongAnswers;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (submitPanel != null)
            submitPanel.SetActive(false);

        if (reportSuccessPanel != null)
            reportSuccessPanel.SetActive(false);

        if (shiftEndedPanel != null)
            shiftEndedPanel.SetActive(false);

        LoadDay(currentDayIndex);
    }

    public void LoadDay(int dayIndex)
    {
        if (days == null || days.Length == 0) return;
        if (dayIndex < 0 || dayIndex >= days.Length) return;

       

        currentDayIndex = dayIndex;
        currentDay = days[currentDayIndex];
        currentCallerIndex = 0;

        dayCorrectAnswers = 0;
        dayWrongAnswers = 0;
        daySalaryGranted = false;
        didReachGoalThisShift = false;

        if (submitPanel != null)
            submitPanel.SetActive(false);

        if (reportSuccessPanel != null)
            reportSuccessPanel.SetActive(false);

        if (shiftEndedPanel != null)
            shiftEndedPanel.SetActive(false);

        ShowCurrentCaller();

        if (archiveManager != null)
            archiveManager.RefreshArchivesForCurrentDay();
    }

    public void ShowCurrentCaller()
    {
        if (CurrentDayNumber == 3)
        {
            currentCaller = null;

            if (callerNameText != null)
                callerNameText.text = "No calls";

            if (callerDialogueText != null)
                callerDialogueText.text = "There are no callers for this day.";

            if (submitReportButton != null)
                submitReportButton.interactable = false;

            return;
        }

        if (currentDay == null || currentDay.callers == null || currentDay.callers.Length == 0)
        {
            currentCaller = null;
            ClearCallWindow();
            OpenShiftEndedPanel();
            return;
        }

        if (currentCallerIndex >= currentDay.callers.Length)
        {
            currentCaller = null;
            ClearCallWindow();
            OpenShiftEndedPanel();
            return;
        }

        currentCaller = currentDay.callers[currentCallerIndex];

        if (currentCaller == null)
        {
            ClearCallWindow();
            return;
        }

        if (callerNameText != null)
            callerNameText.text = "Current Caller: " + currentCaller.caller_name;

        if (callerDialogueText != null)
            callerDialogueText.text = currentCaller.caller_dialogue;

        if (submitReportButton != null)
            submitReportButton.interactable = true;

        if (callerNameText_zoomed != null)
            callerNameText_zoomed.text = callerNameText.text;

        if (callerDialogueText_zoomed != null)
            callerDialogueText_zoomed.text = callerDialogueText.text;

    }

    private void ClearCallWindow()
    {
        if (callerNameText != null)
            callerNameText.text = "";

        if (callerDialogueText != null)
            callerDialogueText.text = "";

        if (submitReportButton != null)
            submitReportButton.interactable = true;
    }

    public void OpenSubmitPanel()
    {
        if (CurrentDayNumber == 3)
            return;

        if (currentCaller == null || submitPanel == null || archiveDropdown == null)
            return;

        PopulateDropdown();
        submitPanel.SetActive(true);
    }

    public void OpenZoomed()
    {
        if (zoomPanel != null)
            zoomPanel.SetActive(true);
    }

    public void CloseSubmitPanel()
    {
        if (submitPanel != null)
            submitPanel.SetActive(false);
    }

    private void PopulateDropdown()
    {
        if (archiveDropdown == null) return;

        archiveDropdown.ClearOptions();
        dropdownArchiveReferences.Clear();

        List<string> optionNames = new List<string>();

        if (allArchives == null) return;

        for (int i = 0; i < allArchives.Length; i++)
        {
            Archive_Scriptable archive = allArchives[i];
            if (archive == null) continue;

            if (!archive.IsUnlocked(CurrentDayNumber)) continue;

            dropdownArchiveReferences.Add(archive);
            optionNames.Add(archive.archive_name);
        }

        archiveDropdown.AddOptions(optionNames);

        if (archiveDropdown.options.Count > 0)
            archiveDropdown.value = 0;

        archiveDropdown.RefreshShownValue();
    }

    public void SubmitSelectedAnswer()
    {
        if (currentCaller == null) return;
        if (archiveDropdown == null) return;
        if (dropdownArchiveReferences.Count == 0) return;

        int selectedIndex = archiveDropdown.value;

        if (selectedIndex < 0 || selectedIndex >= dropdownArchiveReferences.Count)
            return;

        Archive_Scriptable selectedArchive = dropdownArchiveReferences[selectedIndex];
        bool isCorrect = selectedArchive == currentCaller.correct_answer;

        if (isCorrect)
        {
            dayCorrectAnswers++;
            totalCorrectAnswers++;
        }
        else
        {
            dayWrongAnswers++;
            totalWrongAnswers++;
        }

        CloseSubmitPanel();
        OpenReportSuccessPanel();
    }

    private void CloseReportPanel()
    {
        if (call_panel != null)
            call_panel.SetActive(false);
    }

    private void OpenReportSuccessPanel()
    {
        if (reportSuccessPanel != null)
            reportSuccessPanel.SetActive(true);
    }

    public void CloseReportSuccessPanelAndContinue()
    {
        if (reportSuccessPanel != null)
            reportSuccessPanel.SetActive(false);

        NextCaller();
    }

    public void NextCaller()
    {
        currentCallerIndex++;
        ShowCurrentCaller();
    }

    private void OpenShiftEndedPanel()
    {
        int salary = GetDaySalary();
        didReachGoalThisShift = salary >= salaryGoal;

        UpdateShiftEndedUI();

        if (didReachGoalThisShift && !daySalaryGranted)
        {
            GrantDaySalary(salary);
            daySalaryGranted = true;
        }

        if (shiftEndedPanel != null)
            shiftEndedPanel.SetActive(true);
    }

    private void UpdateShiftEndedUI()
    {
        int totalAnsweredThisDay = dayCorrectAnswers + dayWrongAnswers;

        float accuracy = 0f;
        if (totalAnsweredThisDay > 0)
        {
            accuracy = (dayCorrectAnswers / (float)totalAnsweredThisDay) * 100f;
        }

        int roundedAccuracy = Mathf.RoundToInt(accuracy);
        int salary = GetDaySalary();

        if (shiftEndTitleText != null)
            shiftEndTitleText.text = didReachGoalThisShift ? goalReachText : goalFailText;

        if (accuracyText != null)
            accuracyText.text = "Accuracy: " + roundedAccuracy + "%";

        if (salaryText != null)
            salaryText.text = "Salary: $" + salary;

        if (goalText != null)
            goalText.text = "Goal: $" + salaryGoal;

    }

    private int GetDaySalary()
    {
        return dayCorrectAnswers * 100;
    }

    private void GrantDaySalary(int moneyGained)
    {
        if (Profile_Manager.Instance == null) return;
        if (Profile_Manager.Instance.currentProfile == null) return;

        Profile_Manager.Instance.AddMoney(moneyGained);
    }

    public void Next()
    {
        if (shiftEndedPanel != null)
            shiftEndedPanel.SetActive(false);

        if (didReachGoalThisShift)
        {
            ProceedToNextDay();
        }
        else
        {
            QuitGame();
        }
    }

    private void ProceedToNextDay()
    {
        int nextDayIndex = currentDayIndex + 1;

        if (days == null || nextDayIndex >= days.Length)
        {
            QuitGame();
            return;
        }

        LoadDay(nextDayIndex);
        CloseReportPanel();
    }

    public void QuitGame()
    {
        Debug.Log("Game is closing...");

        Application.Quit();

    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

    public bool IsArchiveUnlocked(Archive_Scriptable archiveData)
    {
        if (archiveData == null) return false;
        return archiveData.IsUnlocked(CurrentDayNumber);
    }
}