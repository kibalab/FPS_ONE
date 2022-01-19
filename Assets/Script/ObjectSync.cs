using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Threading;
using System;

public class ObjectSync : MonoBehaviour {
    public GameObject player;
	public GameObject player2;
	public float waitingTime;
    public bool isMine = false;
	float timer;
	// Use this for initialization
	void Start () {
		timer = 0.0f;

    }

    // Update is called once per frame
    void Update () {
        string data = this.GetComponent<ServerClient>().message;
        try
        {
            if (data.Split('&')[1] == player.name)
            {
                isMine = true;
            }
            else
            {
                isMine = false;
            }
        }
        catch { }
        
            timer += Time.deltaTime;

		if(timer > waitingTime)
		{
			//Action
			playerPositionSend();
            playerPositionUpdate();
            timer = 0;
		}


	}

    public void playerPositionUpdate()
    {
        string data = this.GetComponent<ServerClient>().message;
        string pID = this.GetComponent<ServerClient>().pID;

        if (!isMine && data.Contains("&"))
        {
            if (data.Split('&')[0] == player.name)
            {
                //Debug.Log(data + " " + data.Contains(pID).ToString());
                //player2.transform.position = Vector3.MoveTowards(transform.position, StringToVector3(data), MoveSpeed);
                //player2.transform.rotation = Quaternion.RotateTowards(transform.rotation, StringToQuaternion(data), TrunSpeed);
                player2.transform.position = StringToVector3(data);
                player2.transform.rotation = StringToQuaternion(data);
            }
        }
    }

    void playerPositionSend(){
		Vector3 pPos = player.transform.position;
        Quaternion pRot = player.transform.rotation;

        string strpRot = String.Format("({0},{1},{2},{3})", pRot.x, pRot.y, pRot.z, pRot.w);
        this.GetComponent<ServerClient>().SendMessage(player.name + '&' + pPos.ToString() + '&' + strpRot);
    }

    public static Vector3 StringToVector3(string sVector)
	{
        sVector = sVector.Split('&')[2].ToString();
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
    public static Quaternion StringToQuaternion(string sQuaternion)
    {
        sQuaternion = sQuaternion.Split('&')[3].ToString();
        if (sQuaternion.StartsWith("(") && sQuaternion.EndsWith(")"))
        {
            sQuaternion = sQuaternion.Substring(1, sQuaternion.Length - 2);
        }

        // split the items
        string[] sArray = sQuaternion.Split(',');

        // store as a Vector3
        Quaternion result = new Quaternion(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]),
            float.Parse(sArray[3]));

        return result;
    }
}