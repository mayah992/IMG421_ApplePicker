using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public int score = 0;
    private TextMeshProUGUI uiText;

    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        uiText.text = score.ToString("#,0");
    }
}
