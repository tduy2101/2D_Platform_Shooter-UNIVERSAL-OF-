using System;
using System.Collections.Generic;

[Serializable]
public class UserData
{
    public string username;
    public string password;
    public int highestLevel;
}

[Serializable]
public class GameData
{
    public List<UserData> allUsers = new List<UserData>();
}