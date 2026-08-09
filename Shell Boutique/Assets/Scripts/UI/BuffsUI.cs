using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Buff
{
    public enum buffType {None, FurnaceSpeed, PlayerSpeed, AdditionalFurnace}
    public buffType type;
    public int percentage;

    // UI
    public string title;
    public string description;
    public string effect;
}
public class BuffsUI : MonoBehaviour
{
    public static BuffsUI Instance {get; private set;}
    [SerializeField] private List<Buff> buffs;
    [SerializeField] private Buff emptyBuff;
    Buff buffSelected = null;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private AudioClip selectSFX;
    [SerializeField] private AudioClip doneSFX;
    
    void Start()
    {
        uiDocument.enabled = false;
        
    } 
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

    public void LayoutBuffs()
    {
        // stop player movement
        PlayerMovement.Instance.enabled = false;

        uiDocument.enabled = true;
        var panel = uiDocument.rootVisualElement.Q<VisualElement>("Panel");
        var bContainer = panel.Q<VisualElement>("Buffs");
        var doneButton = panel.Q<Button>("Done");

        // clear all buffs
        foreach (BuffComponent curr in bContainer.Query<BuffComponent>().ToList())
            bContainer.Remove(curr);

        // display all possible buffs
        foreach (var buff in buffs) 
            AddBuff(bContainer, buff);
        
        if (bContainer.Query<BuffComponent>().ToList().Count == 0)
            AddBuff(bContainer, emptyBuff);

        // add done button logic
        doneButton.clicked += () =>
        {
            if (buffSelected == null)
                return;

            AudioSource.PlayClipAtPoint(doneSFX, transform.position, 2f);  
            ApplyBuff();
            uiDocument.enabled = false;  
            StartCoroutine(DayTimeUI.Instance.PrepareNewDay());
        };

    }
    public void SelectBuff(Buff buff)
    {
        buffSelected = buff;
    }

    public void ApplyBuff()
    {
        // do stuff
        if (buffSelected.type == Buff.buffType.FurnaceSpeed)
            IncreaseFurnaceSpeed(buffSelected.percentage);
        else if (buffSelected.type == Buff.buffType.PlayerSpeed)
            IncreasePlayerSpeed(buffSelected.percentage);
        else if (buffSelected.type == Buff.buffType.AdditionalFurnace)
            AddAdditionalFurnace();

        buffSelected = null;

        uiDocument.enabled = false;
    }

    // Helper functions
    private void AddBuff(VisualElement container, Buff buff)
    {
        // verify if buff can be added
        if (buff.type == Buff.buffType.FurnaceSpeed && !StatManager.Instance.CanFurnaceSpeedIncrease())
            return;
        else if (buff.type == Buff.buffType.PlayerSpeed && !StatManager.Instance.CanPlayerSpeedIncrease())
            return;
        else if (buff.type == Buff.buffType.AdditionalFurnace && !StatManager.Instance.CanAddFurnace())
            return;
    
        var parent = new BuffComponent();
        var buffContainer = parent.Q<VisualElement>("Buff");

        // Update text
        buffContainer.Q<Label>("Title").text = buff.title;
        buffContainer.Q<Label>("Description").text = buff.description;
        buffContainer.Q<Label>("Effect").text = buff.effect;

        // Register clickable callback (i forgot to make the element a button oops)
        parent.style.height = new Length(100, LengthUnit.Percent);
        parent.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log("clicked");

            foreach (var child in container.Children())
            {
                var buff = child.Q<VisualElement>("Buff");
                buff.RemoveFromClassList("Selected");
            }
            buffContainer.AddToClassList("Selected");

            AudioSource.PlayClipAtPoint(selectSFX, transform.position, 2f);  

            SelectBuff(buff);
        });

        container.Add(parent);
    }
    private void IncreaseFurnaceSpeed(float percentage)
    {
        StatManager.Instance.IncreaseFurnaceSpeed(percentage);
    }

    private void IncreasePlayerSpeed(float percentage)
    {
        StatManager.Instance.IncreasePlayerSpeed(percentage);
    }

    private void AddAdditionalFurnace()
    {
        StatManager.Instance.AddFurnace();
    }
}
