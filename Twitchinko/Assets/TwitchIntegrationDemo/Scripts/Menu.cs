using TMPro;
using UnityEngine;

namespace TwitchIntegration.Demo
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _usernameField, _channelNameField;
        [SerializeField] private TextMeshProUGUI _statusText;
        [SerializeField] private Canvas _mainUI;

        [SerializeField] 
        private string _authSuccess = "Successfully authenticated Twitch account!";
        [SerializeField, Multiline] 
        private string _authRequired = "By pressing the button below, you will be redirected to a page " +
                                       "where you must authorize your Twitch account with Sub-Optimal. " +
                                       "This is required to enable Twitch chat interactions with the game.";

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
            if (_mainUI == null) return;

            //Deactivate this window.
            _mainUI.gameObject.SetActive(false);
        }
    }
}
