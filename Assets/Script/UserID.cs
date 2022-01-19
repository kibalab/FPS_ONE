using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

public class UserID : MonoBehaviour {
	public string LocalUserID;
    public string GetRandomID(int _totLen)
    {
        Random rand = new Random();
        string input = "abcdefghijklmnopqrstuvwxyz0123456789";
        var chars = Enumerable.Range(0, _totLen).Select(x => input[rand.Next(0, input.Length)]);
        LocalUserID = new string(chars.ToArray());
        return new string(chars.ToArray());
    }
}
