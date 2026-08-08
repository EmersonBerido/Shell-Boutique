using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DayTimeUI : MonoBehaviour
{
    public static DayTimeUI Instance {get; private set;}

    [Header("Time of Day Images")]
    [SerializeField] private Sprite MorningSprite;
    [SerializeField] private Sprite NoonSprite;
    [SerializeField] private Sprite AfternoonSprite;
    [SerializeField] private Sprite EveningSprite;

    // Day
    private int day = 0;

    // Time
    [SerializeField] private int startHour = 10;
    private int hour = 10;
    private int minute = 0;
    [SerializeField] private int hourCutOff;
    [SerializeField] private float timeTickSpeed = 1f;


    [Header("UI")]
    [SerializeField] List<string> countdownText = new();
    [SerializeField] private UIDocument uiDocument;
    private Image timeImage;
    private Label dayLabel;
    private Label timeLabel;

    private IEnumerator countdown;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        } else
        {
            Instance = this;
        }
    }

    public IEnumerator CountDownTime()
    {
        while (true)
        {
            UpdateTime();
            yield return new WaitForSeconds(timeTickSpeed);
        }
    }

    public void StartNewDay()
    {
        if (timeImage == null)
            FindReferences();

        hour = startHour;
        minute = 0;
        day += 1;

        timeLabel.text = $"{hour}:{(minute < 10 ? $"0{minute}" : minute)}";
        dayLabel.text = $"D\nA\nY\n{day}";
        timeImage.sprite = MorningSprite;
    }

    public IEnumerator PrepareNewDay()
    {
        // disable player movement
        PlayerMovement.Instance.enabled = false;

        // count down
        var container = uiDocument.rootVisualElement
            .Q<VisualElement>("DayStart");
        container.RemoveFromClassList("Disabled");

        var label = container.Q<Label>("Countdown");
        label.text = "";
        foreach (var text in countdownText)
        {
            yield return new WaitForSeconds(1f);
            label.text = text;
        }
        yield return new WaitForSeconds(1f);
        container.AddToClassList("Disabled");

        GetOrder.Instance.StartNewRound();
    }

    private void UpdateTime()
    {
        if (timeImage == null)
            FindReferences();

        minute += 1;
        if (minute >= 60)
            RegisterNewHour();

        timeLabel.text = $"{hour}:{(minute < 10 ? $"0{minute}" : minute)}";
    }
    private void RegisterNewHour()
    {
        hour += 1;
        minute = 0;

        // check if gameover
        if (hour >= hourCutOff && DeliverOrder.Instance.OrdersRemain())
        {
            Debug.LogWarning("Game end!");
            PlayerPrefs.SetInt("day", day);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Ending");
            return;
        }

        // check if change to new time
        if (hour >= 17)
            timeImage.sprite = EveningSprite;
        else if (hour >= 12)
            timeImage.sprite = NoonSprite;
    }

  private void FindReferences()
    {
        var root = uiDocument.rootVisualElement;
        var container = root.Q<VisualElement>("Panel")
            .Q<VisualElement>("TimeContainer");

        dayLabel = container.Q<Label>("Day");

        var timeContainer = container.Q<VisualElement>("TimeContainer");

        timeImage = timeContainer.Q<Image>("Image");
        timeLabel = timeContainer.Q<Label>("Time");
    }

    
}
