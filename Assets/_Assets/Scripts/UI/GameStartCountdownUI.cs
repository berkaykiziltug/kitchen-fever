using System;
using TMPro;
using UnityEngine;


public class GameStartCountdownUI : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI countDownText;

   private void Start()
   {
      KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
      Hide();
   }

   private void Update()
   {
      countDownText.text = MathF.Ceiling(KitchenGameManager.Instance.GetCountdownToStartTimer()).ToString();
   }

   private void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
   {
      if (KitchenGameManager.Instance.isCountDownToStartActive())
      {
         Show();
      }
      else
      {
         Hide();
      }
   }

   private void Hide()
   {
      gameObject.SetActive(false);
   }

   private void Show()
   {
      gameObject.SetActive(true);
   }

   
}
