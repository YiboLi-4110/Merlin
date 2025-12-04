using System;
using System.Collections.Generic;

[Serializable]
public struct SingleScene
{
    public int seed;
    public float moisture;
    public float temperature;

    public bool safe;

}


[Serializable]
public class Save
{
    public Dictionary<Tuple<int,int>, SingleScene>  GlobalMapInfoTree = new Dictionary<Tuple<int,int>, SingleScene>();
    
    public Tuple<int, int> LastSafeScene;

    public Tuple<int,int,int,int> ElementNum;
}