using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Login : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputPlayerName;
    public TMP_Text textStats;
    public Button playButton;
    public TMP_Text textPlayButton;
    public static string playerName;

    public readonly string gameVersion = "0.1.0";

    // Start is called before the first frame update
    void Start()
    {
       
        playButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        string name = inputPlayerName.text;

        if(name != "Player1" || name != "") {
            playButton.interactable = true;
        } else {
            playButton.interactable = false;
        }
    }

    public void onLoginButtonClick()
    {
        Connect();
        playButton.interactable = false;
    }

    void setPhoton()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = inputPlayerName.text;
        textStats.text = "- ONLINE:Connecting to Master Server... -";
        playButton.interactable = false;

    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        textStats.text = "- ONLINE:Connected to Master Server -";
        textPlayButton.text = "CONNECT";
        playButton.interactable = true;
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        playButton.interactable = false;
        textStats.text = "- OFFLINE:Connection Disabled {cause.ToString()}/Try reconnecting... -";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        playButton.interactable = false;

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LoadLevel("Main");

        } else if(PhotonNetwork.IsConnected) {
            setPhoton();
            textStats.text = "- ONLINE:Connecting to Random room -";
            
            PhotonNetwork.JoinRandomRoom();
        } else {

            textStats.text = "- OFFLINE:Connection Disabled/Try reconnecting... -";
            setPhoton();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        textStats.text = "- ONLINE:Can't find room/Creating new room -";
        PhotonNetwork.CreateRoom(null, new RoomOptions { });
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        textStats.text = "- ONLINE:Connected with room -";
        textPlayButton.text = "PLAY";
        PhotonNetwork.LoadLevel("Main");
    }

}
