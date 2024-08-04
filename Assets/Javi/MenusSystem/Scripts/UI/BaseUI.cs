using UnityEngine;

public class BaseUI : MonoBehaviour {
    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
