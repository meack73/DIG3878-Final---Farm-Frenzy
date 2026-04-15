using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

//player name input - appears above character in game
[RequireComponent(typeof(InputField))]

public class PlayerNameInputField : MonoBehaviour
{

    //store playerpref key to avoid typos
    const string playerNamePrefKey = "PlayerName";
    void Start()
    {

        string defaultName = "Player" + Random.Range(1000, 9999);
        InputField inputField = GetComponent<InputField>();

        if (PlayerPrefs.HasKey(playerNamePrefKey))
        {
            defaultName = PlayerPrefs.GetString(playerNamePrefKey);
            Debug.Log("Player Name originally: " + defaultName);
        }

        inputField.text = defaultName;

        PhotonNetwork.NickName = defaultName;

    }

    //sets name of player and saves if for future use
    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(playerNamePrefKey, value);
        PlayerPrefs.Save();
    }

}