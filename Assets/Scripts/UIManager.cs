using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Quantum;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;


    public Transform TextHolder;
    public GameObject TextPrefab;


    private List<TextMeshProUGUI> namesList = new List<TextMeshProUGUI>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayerUi(string playerName)
    {

        var textObject = Instantiate(TextPrefab, TextHolder);

        var nameText = textObject.GetComponent<TextMeshProUGUI>();

        nameText.text = playerName;
    }


    public void ClearName()
    {
        if (namesList.Count > 0)
        {
            foreach (var name in namesList)
            {
                //namesList.Remove(name);

                Destroy(name.gameObject);
            }
        }

        namesList.Clear();
    }

    public float GetHealthOfPlayer(EntityRef player)
    {
        var frame = GameManager.Instance.Game.Frames.Predicted;

        frame.TryGet<Health>(player, out var health);

        return (float) health.CurrentHealth;

    }
}
