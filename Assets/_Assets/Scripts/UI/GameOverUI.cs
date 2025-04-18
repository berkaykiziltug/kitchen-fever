using System;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI recipesDeliveredText;
   
   
   private void Start()
   {
      KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
      Hide();
   }

   private void Update()
   {
     
   }

   private void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
   {
      if (KitchenGameManager.Instance.IsGameOver())
      {
         Show();
         recipesDeliveredText.text = DeliveryManager.Instance.GetSuccesfulRecipesAmount().ToString();
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
