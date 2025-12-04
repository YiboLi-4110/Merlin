using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Mathematics;

public class TheGlobalManager : MonoBehaviour
{
    private static TheGlobalManager TGM_ = null;

    int current_x;
    int current_y;

    int current_seed;
    float current_temperature;

    float current_moisture;

    bool current_safe;

    bool in_warscene = false;

    private Save thissave;


    private void SaveByBin()
    {
        //Save save = GetSaveInfo();//先将需要存档的游戏信息读取过来并保存起来
        BinaryFormatter bf = new BinaryFormatter();//创建一个二进制格式化程序
        FileStream fileStream = File.Create(Application.dataPath + "/Saves" + "/save0.txt");//创建一个文件流
        bf.Serialize(fileStream, thissave);//利用二进制格式化程序的序列化方法来序列化save对象，参数：创建的文件流和需要序列化的对象
        fileStream.Close();  //关闭流
        if (!File.Exists(Application.dataPath + "/Saves" + "/save0.txt"))
        {
            Debug.Log("Cant save the game");
        }
    }


    private Save LoadByBin()
    {
        if (File.Exists(Application.dataPath + "/Saves" + "/save0.txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();//创建一个二进制格式化程序
            FileStream fileStream = File.Open(Application.dataPath + "/Saves" + "/save0.txt", FileMode.Open);//打开数据流
            Save save = (Save)bf.Deserialize(fileStream);//调用二进制格式化程序中的反序列化方法，将数据流反序列化为save对象并进行保存
            fileStream.Close();//关闭文件流
            return save;
        }
        else
        {
            
            Debug.Log("加载失败！！");
            return null;
        }
    }

    public bool TrytoLoad()
    {
        Save loadsave = LoadByBin();
        if(loadsave != null)
        {
            thissave = loadsave;
            Tuple<int,int> LastSafeinSave = thissave.LastSafeScene;
            current_x = LastSafeinSave.Item1;
            current_y = LastSafeinSave.Item2;
            Debug.Log("load success----x: "+ current_x + " ,y: "+ current_y );
            SingleScene ss = thissave.GlobalMapInfoTree[LastSafeinSave];
            current_moisture = ss.moisture;
            current_temperature = ss.temperature;
            current_safe = true;
            current_seed = ss.seed;
            if(thissave.ElementNum != null)
            {
                Tuple<int,int,int,int> oldata = thissave.ElementNum;
                RandomDataBehavior.savedData[0] = oldata.Item1;
                RandomDataBehavior.savedData[1] = oldata.Item2;
                RandomDataBehavior.savedData[2] = oldata.Item3;
                RandomDataBehavior.savedData[3] = oldata.Item4;
            }
            return true;
        }
        return false;
    } 


    void Awake()
    {
        TGM_ = this;
        DontDestroyOnLoad(TGM_.gameObject);
    }

    void Start()
    {
        Debug.Log("GlobalStart");
        thissave = new Save();
        //SetNewWorld(114514);
    }

    // Start is called before the first frame update

    public static TheGlobalManager TGM
    {
        get
        {
            return TGM_;
        }
    }

    public void SetNewWorld(int m_seed)
    {
        thissave.GlobalMapInfoTree.Clear();
        thissave.LastSafeScene = new Tuple<int,int>(0,0);
        SingleScene bornscene = new SingleScene();

        current_x = 0;
        current_y = 0;
        current_moisture = 10.0f;
        current_temperature = 10.0f;
        current_safe = false;
        current_seed = m_seed;

        bornscene.seed = m_seed;
        bornscene.moisture = 10.0f;
        bornscene.safe = true;
        bornscene.temperature = 10.0f;  
        
        thissave.GlobalMapInfoTree.Add(new Tuple<int, int>(current_x, current_y), bornscene);
        thissave.ElementNum = new Tuple<int, int, int, int>(2,2,2,2);

        SaveByBin();
    }

    public bool EnterScene(int direction, int m_seed)
    {
        in_warscene = false;
        bool is_new = false;
        Tuple<int,int> CurrentPos = new Tuple<int, int>(current_x, current_y);
        if( !thissave.GlobalMapInfoTree.ContainsKey(CurrentPos))
        {
            Debug.Log("saved a new map");
            SingleScene lastscene = new SingleScene();
            lastscene.seed = current_seed;
            lastscene.temperature = current_temperature;
            lastscene.moisture = current_moisture;
            lastscene.safe = true;

            thissave.GlobalMapInfoTree.TryAdd(CurrentPos, lastscene);
            thissave.LastSafeScene = CurrentPos;
            thissave.ElementNum = new Tuple<int, int, int, int>(RandomDataBehavior.savedData[0],
                                                                RandomDataBehavior.savedData[1],
                                                                RandomDataBehavior.savedData[2],
                                                                RandomDataBehavior.savedData[3]);
            SaveByBin();
        }
        Debug.Log("saved a already haved map");



        if(direction == 1)
        {
            current_y += 1;
            current_moisture += 0.5f;
            current_temperature -= 0.5f;
        }
        else if(direction == 2)
        {
            current_x -= 1;
            current_moisture -= 0.5f;
            current_temperature -= 0.5f;
        }
        else if(direction == 3)
        {
            current_y -= 1;
            current_moisture -= 0.5f;
            current_temperature += 0.5f;
        }
        else
        {
            current_x += 1;
            current_moisture += 0.5f;
            current_temperature += 0.5f;
        }



        CurrentPos = new Tuple<int, int>(current_x, current_y);
        if(thissave.GlobalMapInfoTree.ContainsKey(CurrentPos))
        {
            Debug.Log("Already have");
            current_safe = true;
            current_seed = thissave.GlobalMapInfoTree[CurrentPos].seed;
        }
        else
        {
            Debug.Log("New map");
            current_safe = false;
            is_new = true;
            current_seed = m_seed;
        }

        return is_new;
    }


    public int Climate()
    {
        if(current_temperature <= 9.0f)
        {
            if(current_moisture >= 11.0f)
            {
                return 1;   //雪地
            }
            else
                return 2;   //草原
        }
        else 
        {
            if(current_moisture <= 9.0f)
                return 3;   //沙漠
            else    
                return 4;  //森林
        }
    }

    public int getseed()
    {
        return current_seed;
    }

    public bool IsSafe()
    {
        return current_safe;
    }

    public void SetSafe()
    {
        current_safe = true;
    }

    public void InWar()
    {
        in_warscene = true;
    }

    public bool IsInWar()
    {
        return in_warscene;
    }

    public Tuple<int,int> GetCurrentPosition()
    {
        return new Tuple<int, int>(current_x, current_y);
    }

    public int GetdDifficult()
    {
        int cnum = Math.Abs(current_x) + Math.Abs(current_y);
        if(cnum <= 1)
        {
            return 4;
        }
        else if(cnum <= 3)
        {
            return 6;
        }
        else if(cnum <= 5)
        {
            return 8;
        }
        else
        {
            return 10;
        }
        
    }

}
