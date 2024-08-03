using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUI : BaseUI {


    [SerializeField] private Button returnButton;


    private void Awake() {
        returnButton.onClick.AddListener(() => {
            Hide();
        });

        Hide();
    }
}
