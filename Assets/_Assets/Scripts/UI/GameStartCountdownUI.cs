using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;


public class GameStartCountdownUI : MonoBehaviour
{
   private const string NUMBER_POPUP = "NumberPopup";
   [SerializeField] private TextMeshProUGUI countDownText;

   private Animator animator;
   private int previousCountdownNumber;
   

   private void Awake()
   {
      animator = GetComponent<Animator>();
   }

   private void Start()
   {
      KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
      Hide();
   }

   private void Update()
   {
      int countDownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());
      countDownText.text =countDownNumber.ToString();
      if (previousCountdownNumber != countDownNumber)
      {
         previousCountdownNumber = countDownNumber;
         animator.SetTrigger(NUMBER_POPUP);
         SoundManager.Instance.PlayCountdownSound();
      }
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
