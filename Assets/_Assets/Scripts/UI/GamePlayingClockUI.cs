using UnityEngine;
using UnityEngine.UI;
public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timerImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
