using System.Collections;
using UnityEngine;
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
    [SerializeField] private UIDocument uiDocument;
    private Image timeImage;
    private Label dayLabel;
    private Label timeLabel;

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

    IEnumerator Start()
    {
        FindReferences();

        while (true)
        {
            UpdateTime();
            yield return new WaitForSeconds(timeTickSpeed);
        }
    }

    public void StartNewDay()
    {
        hour = startHour;
        minute = 0;
        day += 1;

        timeLabel.text = $"{hour}:{(minute < 10 ? $"0{minute}" : minute)}";
        dayLabel.text = $"D\nA\nY\n{day}";
        timeImage.sprite = MorningSprite;
    }

    private void UpdateTime()
    {
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
