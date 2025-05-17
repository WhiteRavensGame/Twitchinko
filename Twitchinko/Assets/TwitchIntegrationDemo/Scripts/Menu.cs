using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace TwitchIntegration.Demo
{
    public class Menu : MonoBehaviour
    {
        public static Menu Instance;

        [SerializeField] private TMP_InputField _usernameField, _channelNameField;
        [SerializeField] private TextMeshProUGUI _statusText;
        [SerializeField] private Canvas _mainUI;
        [SerializeField] private Button _startGameButton;

        [SerializeField] 
        private string _authSuccess = "Successfully authenticated Twitch account!";
        [SerializeField, Multiline] 
        private string _authRequired = "By pressing the button below, you will be redirected to a page " +
                                       "where you must authorize your Twitch account with Sub-Optimal. " +
                                       "This is required to enable Twitch chat interactions with the game.";
        float PingCounter = 0;

        private void Awake()
        {
            if(Instance == null) { Instance = this; }
            else { Destroy(this.gameObject); }
        }

        private void Update()
        {
            PingCounter += Time.deltaTime;
            if (PingCounter > 60)
            {
                Console.WriteLine("Flush Streamwriter");
                //Call TwitchCommandManager to flush stream writer and refresh.
                
            }
        }

        public void OnAuthenticateButtonClicked()
        {
            //if(TwitchManager.IsAuthenticated)
            //{
            //    _statusText.text = _authSuccess;
            //    Invoke("DeactivateAuthWindow", 1.0f);
            //}
            
            TwitchManager.Authenticate(_usernameField.text, _channelNameField.text, isAuthenticated =>
            {
                if (isAuthenticated)
                {
                    _statusText.text = _authSuccess;
                    Invoke("DeactivateAuthWindow", 1.0f);
                }
                else
                {
                    _statusText.text = _authRequired;
                }

                
            });
        }

        private void DeactivateAuthWindow()
        {
            _startGameButton.gameObject.SetActive(true);

            if (_mainUI == null) return;

            //Deactivate this window.
            //_mainUI.gameObject.SetActive(false);
            
        }

        public void LoadPlayScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}
