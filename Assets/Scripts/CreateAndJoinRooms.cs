using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createField;
    public InputField joinField;
    public InputField playerNameField;

    private void Start()
    {
        playerNameField.text = PlayerPrefs.GetString("PlayerName","");
    }

    public void CreateRoom()
    {
        PhotonNetwork.NetworkingClient.NickName = playerNameField.text;
        PlayerPrefs.SetString("PlayerName", playerNameField.text);
        PhotonNetwork.CreateRoom(createField.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.NetworkingClient.NickName = playerNameField.text;
        PlayerPrefs.SetString("PlayerName", playerNameField.text);
        PhotonNetwork.JoinRoom(joinField.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Main");
    }
}