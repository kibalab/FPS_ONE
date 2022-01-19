using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitManager : MonoBehaviourPun {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.GetComponent<ServerClient>().Close();
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("menu");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
