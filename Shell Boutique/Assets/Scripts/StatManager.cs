using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour

{
  public static StatManager Instance {get; private set;}

  [SerializeField] private float basePlayerSpeed;
  [SerializeField] private float maxPlayerSpeed;
  private int furnaceCount = 1;
  [SerializeField] private List<GameObject> furnaces;
  [SerializeField] private float baseFurnaceSpeed;
  [SerializeField] private float minFurnaceSpeed;

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

  void Start()
  {
    basePlayerSpeed = PlayerMovement.Instance.GetInitialSpeed();
    
  }

  // Verification Methods
  public bool CanPlayerSpeedIncrease() => basePlayerSpeed <= maxPlayerSpeed;
  public bool CanFurnaceSpeedIncrease() => baseFurnaceSpeed >= minFurnaceSpeed;
  public bool CanAddFurnace() => furnaceCount < furnaces.Count;

  // Stat Modifier Methods
  public void IncreasePlayerSpeed(float percentage)
  {
    basePlayerSpeed += basePlayerSpeed * (percentage / 100);
    PlayerMovement.Instance.UpdateSpeed(basePlayerSpeed);
  }

  public void IncreaseFurnaceSpeed(float percentage)
  {
    baseFurnaceSpeed -= baseFurnaceSpeed * (percentage / 100);

    foreach (var obj in furnaces)
      if (obj.TryGetComponent<Smelt>(out Smelt s))
        s.UpdateSmeltTime(baseFurnaceSpeed);
    
  }

  public void AddFurnace()
  {
    furnaces[furnaceCount].SetActive(true);
    furnaceCount += 1;
  }
  
}