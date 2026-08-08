using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOrder : MonoBehaviour
{
    public static GetOrder Instance {get; private set;}

    private List<Order> OrdersThisRound = new();
    private int totalOrdersThisRound = 0;
    private int ordersCompletedThisRound = 0;

    [Header("Balancing")]
    [SerializeField] private float orderIntervals = 15f;
    [SerializeField] private int MinOrders = 10;
    [SerializeField] private int MaxOrders = 15;

    [Header("Orders")]
    [SerializeField] List<ShellObject> shells;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Instance found", Instance);
            Destroy(gameObject);
            return;
        } else
        {
            Instance = this;
        }
    }
    void Start()
    {
        StartNewRound();
    }
    public void StartNewRound()
    {
        // Reset
        OrdersThisRound = CreateNewOrders();
        totalOrdersThisRound = OrdersThisRound.Count;
        ordersCompletedThisRound = 0;
        DayTimeUI.Instance.StartNewDay();

        StartCoroutine(RoundRoutine());
    }
    public void FinishedOrder()
    {
        ordersCompletedThisRound += 1;

        if (ordersCompletedThisRound == totalOrdersThisRound)
        {
            // Prompt UI to give selectable buffs
            Debug.Log("Completed all orders this round");

            BuffsUI.Instance.LayoutBuffs();
            
        }
    }

    // HELPER FUNCTIONS
    IEnumerator RoundRoutine()
    {

        foreach (var order in OrdersThisRound)
        {
            DeliverOrder.Instance.AddOrder(order);
            Debug.Log($"adding new order of {order}");
            OrderUI.Instance.AddOrder(order);
            yield return new WaitForSeconds(orderIntervals);
        }
        yield return null;
    }
    private List<Order> CreateNewOrders()
    {
        List<Order> newOrders = new();
        int size = (int)Random.Range(MinOrders, MaxOrders + 1);

        for (int i = 0; i < size; i++)
            newOrders.Add(CreateRandomOrder());
        
        return newOrders;
    }
    private Order CreateRandomOrder()
    {
        Order newOrder = new();

        int idx = (int)Random.Range(0, shells.Count);
        newOrder.shell = ScriptableObject.Instantiate(shells[idx]);

        int colorIdx = (int)Random.Range(1, 7);
        newOrder.color = (Order.ShellColors)colorIdx;

        return newOrder;
    }



}
