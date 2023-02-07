using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayingCardView : MonoBehaviour
{
    [SerializeField] private TMP_Text defenderType;
    [SerializeField] private TMP_Text defenderStrength;
    [SerializeField] private Button useButton;

    private void OnEnable()
    {
        useButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        useButton.onClick.RemoveListener(OnButtonClick);
    }

    public void Render(VisitorAI defender)
    {
        defenderType.text = defender.DefenderType.ToString();
        defenderStrength.text = defender.Strenght.ToString();
    }

    private void OnButtonClick()
    {
        Debug.Log("Use button click");
    }
}
