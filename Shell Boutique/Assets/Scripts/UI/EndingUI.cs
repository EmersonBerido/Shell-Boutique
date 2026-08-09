using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndingUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private float letterDelay = 0.2f;

    void Start()
    {
        int daysLasted = PlayerPrefs.GetInt("day", -1);

        var panel = uiDocument.rootVisualElement.Q<VisualElement>("Panel");
        var text = panel.Q<Label>("Text");
        var btn = panel.Q<Button>("Exit");

        btn.clicked += () => SceneManager.LoadScene(0);
        StartCoroutine(TypeWriter(daysLasted, text, btn));
    }

    IEnumerator TypeWriter(int days, Label text, Button btn)
    {
        string finishedText = $"Ferrah's Spe-SHELL (get it shell haha im so funny) lasted for {days} days before it closed...";
        text.text = "";

        for (int i = 0; i <finishedText.Length; i++)
        {
            text.text += finishedText[i];
            yield return new WaitForSeconds(letterDelay);
        }

        btn.RemoveFromClassList("Disabled");
    }
}
