using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerate : MonoBehaviour
{
    public CinemachineVirtualCamera CVC = null;
	public int mapWidth;
	public int mapHeight;
	public float noiseScale = 10f;
    //public int m_seed;
    private float speed = 20.0f;
    private Tile[] dirt_tile_set = new Tile[28];
    private Tile[] grass_tile_set = new Tile[17];
    private Tile[] water_tile_set = new Tile[36];
    private Tile[] feature_tile_set = new Tile[25];

    private Tile[] tree_like_set = new Tile[5];

    private int[,,] map_info = null;

    private int[,,] map_info2 = null;


    public Vector2 offset = new Vector2(0,0);

    public Tilemap main_tilemap = null;
    public Tilemap tilemap2 = null;

	[Range(1, 30)]
	public int octaves;			//度，需要大于1
	[Range(0, 1)]
	public float persistance;	//持久度，需要大于0，小于1
	[Range(1, 20)]
	public float lacunarity;    //间隙度，控制频率，需要大于1

	//是否开启在编辑器中自动更新
	public bool autoUpdate = true;

    //目标缩放值以及缩放时间、缩放速度
    private float targetZoom;
    private float zoomSmoothTime = 0.2f;
    private float zoomVelocity = 0f;
    

    void Start()
    {
        Init_tileset();
        map_info = new int[mapWidth*2, mapHeight*2, 2]; // 1->dirt, 2->water, 3->grass, 4->feature
        map_info2 = new int[mapWidth*2, mapHeight*2, 2];
        int climate_num = TheGlobalManager.TGM.Climate();
        if(climate_num == 1)
        {
            GenerateMap_3(TheGlobalManager.TGM.getseed());
            Debug.Log("New snow landdddddddddddd");
        }
        else if(climate_num == 2)
        {
            GenerateMap_2(TheGlobalManager.TGM.getseed());
            Debug.Log("New rare grass landdddddddddddd");
        }
        else if(climate_num == 3)
        {
            GenerateMap_4(TheGlobalManager.TGM.getseed());
            Debug.Log("New desert landdddddddddddd");
        }
        else
        {
            GenerateMap_1(TheGlobalManager.TGM.getseed());
            Debug.Log("New grass landdddddddddddd");
        }
        targetZoom = CVC.m_Lens.OrthographicSize;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            // 计算新的目标缩放值
            targetZoom -= scroll * speed;
            // 限制目标缩放范围
            targetZoom = Mathf.Clamp(targetZoom, 3f, 10f);
        }
        // 平滑过渡到目标缩放值
        CVC.m_Lens.OrthographicSize = Mathf.SmoothDamp(
            CVC.m_Lens.OrthographicSize, targetZoom, ref zoomVelocity, zoomSmoothTime);
    }

    private void Init_tileset()
    {
        
        dirt_tile_set[0] = Resources.Load<Tile>("Tiles/tile_000");
        dirt_tile_set[1] = Resources.Load<Tile>("Tiles/tile_001");
        dirt_tile_set[2] = Resources.Load<Tile>("Tiles/tile_002");
        dirt_tile_set[3] = Resources.Load<Tile>("Tiles/tile_003");
        dirt_tile_set[4] = Resources.Load<Tile>("Tiles/tile_004");
        dirt_tile_set[5] = Resources.Load<Tile>("Tiles/tile_005");
        dirt_tile_set[6] = Resources.Load<Tile>("Tiles/tile_006");
        dirt_tile_set[7] = Resources.Load<Tile>("Tiles/tile_007");
        dirt_tile_set[8] = Resources.Load<Tile>("Tiles/tile_008");
        dirt_tile_set[9] = Resources.Load<Tile>("Tiles/tile_009");
        dirt_tile_set[10] = Resources.Load<Tile>("Tiles/tile_010");
        dirt_tile_set[11] = Resources.Load<Tile>("Tiles/tile_011");
        dirt_tile_set[12] = Resources.Load<Tile>("Tiles/tile_012");
        dirt_tile_set[13] = Resources.Load<Tile>("Tiles/tile_013");
        dirt_tile_set[14] = Resources.Load<Tile>("Tiles/tile_014");
        dirt_tile_set[15] = Resources.Load<Tile>("Tiles/tile_015");
        dirt_tile_set[16] = Resources.Load<Tile>("Tiles/tile_016");
        dirt_tile_set[17] = Resources.Load<Tile>("Tiles/tile_017");
        dirt_tile_set[18] = Resources.Load<Tile>("Tiles/tile_018");
        dirt_tile_set[19] = Resources.Load<Tile>("Tiles/tile_019");
        dirt_tile_set[20] = Resources.Load<Tile>("Tiles/tile_020");
        dirt_tile_set[21] = Resources.Load<Tile>("Tiles/tile_021");
        dirt_tile_set[22] = Resources.Load<Tile>("Tiles/tile_025");
        dirt_tile_set[23] = Resources.Load<Tile>("Tiles/tile_026");
        dirt_tile_set[24] = Resources.Load<Tile>("Tiles/tile_121");
        dirt_tile_set[25] = Resources.Load<Tile>("Tiles/tile_130");
        dirt_tile_set[26] = Resources.Load<Tile>("Tiles/tile_131");
        dirt_tile_set[27] = Resources.Load<Tile>("Tiles/tile_120");

        grass_tile_set[0] = Resources.Load<Tile>("Tiles/tile_022");
        grass_tile_set[1] = Resources.Load<Tile>("Tiles/tile_023");
        grass_tile_set[2] = Resources.Load<Tile>("Tiles/tile_024");
        grass_tile_set[3] = Resources.Load<Tile>("Tiles/tile_037");
        grass_tile_set[4] = Resources.Load<Tile>("Tiles/tile_038");
        grass_tile_set[5] = Resources.Load<Tile>("Tiles/tile_039");

        grass_tile_set[6] = Resources.Load<Tile>("Tiles/tile_030");
        grass_tile_set[7] = Resources.Load<Tile>("Tiles/tile_031");
        grass_tile_set[8] = Resources.Load<Tile>("Tiles/tile_032");
        grass_tile_set[9] = Resources.Load<Tile>("Tiles/tile_027");
        grass_tile_set[10] = Resources.Load<Tile>("Tiles/tile_028");
        grass_tile_set[11] = Resources.Load<Tile>("Tiles/tile_029");

        grass_tile_set[12] = Resources.Load<Tile>("Tiles/tile_040");  
        grass_tile_set[13] = Resources.Load<Tile>("Tiles/tile_033");
        grass_tile_set[14] = Resources.Load<Tile>("Tiles/tile_034");
        grass_tile_set[15] = Resources.Load<Tile>("Tiles/tile_035");
        grass_tile_set[16] = Resources.Load<Tile>("Tiles/tile_036");


        //water_tile_set[15] = Resources.Load<Tile>("Tiles/tile_035");
        water_tile_set[0] = Resources.Load<Tile>("Tiles/tile_104");
        water_tile_set[1] = Resources.Load<Tile>("Tiles/tile_105");
        water_tile_set[2] = Resources.Load<Tile>("Tiles/tile_106");
        water_tile_set[3] = Resources.Load<Tile>("Tiles/tile_107");
        water_tile_set[4] = Resources.Load<Tile>("Tiles/tile_108");
        water_tile_set[5] = Resources.Load<Tile>("Tiles/tile_109");
        water_tile_set[6] = Resources.Load<Tile>("Tiles/tile_110");
        water_tile_set[7] = Resources.Load<Tile>("Tiles/tile_111");
        water_tile_set[8] = Resources.Load<Tile>("Tiles/tile_112");
        water_tile_set[9] = Resources.Load<Tile>("Tiles/tile_113");
        water_tile_set[10] = Resources.Load<Tile>("Tiles/tile_114");

        water_tile_set[11] = Resources.Load<Tile>("Tiles/tile_086");    //Deep water
        water_tile_set[12] = Resources.Load<Tile>("Tiles/tile_087");
        water_tile_set[14] = Resources.Load<Tile>("Tiles/tile_088");
        water_tile_set[15] = Resources.Load<Tile>("Tiles/tile_089");
        water_tile_set[16] = Resources.Load<Tile>("Tiles/tile_090");
        water_tile_set[17] = Resources.Load<Tile>("Tiles/tile_091");    
        water_tile_set[18] = Resources.Load<Tile>("Tiles/tile_092");
        water_tile_set[19] = Resources.Load<Tile>("Tiles/tile_093");
        water_tile_set[20] = Resources.Load<Tile>("Tiles/tile_094");

        water_tile_set[21] = Resources.Load<Tile>("Tiles/tile_070");    //Deep water
        water_tile_set[22] = Resources.Load<Tile>("Tiles/tile_071");
        water_tile_set[23] = Resources.Load<Tile>("Tiles/tile_072");
        water_tile_set[24] = Resources.Load<Tile>("Tiles/tile_073");
        water_tile_set[25] = Resources.Load<Tile>("Tiles/tile_074");
        water_tile_set[26] = Resources.Load<Tile>("Tiles/tile_075");    
        water_tile_set[27] = Resources.Load<Tile>("Tiles/tile_076");
        water_tile_set[28] = Resources.Load<Tile>("Tiles/tile_077");
        water_tile_set[29] = Resources.Load<Tile>("Tiles/tile_078");
        water_tile_set[30] = Resources.Load<Tile>("Tiles/tile_115");
        water_tile_set[31] = Resources.Load<Tile>("Tiles/tile_123");
        water_tile_set[32] = Resources.Load<Tile>("Tiles/tile_082");
        water_tile_set[33] = Resources.Load<Tile>("Tiles/tile_083");
        water_tile_set[34] = Resources.Load<Tile>("Tiles/tile_084");
        water_tile_set[35] = Resources.Load<Tile>("Tiles/tile_085");


        feature_tile_set[0] = Resources.Load<Tile>("Tiles/tile_041");
        feature_tile_set[1] = Resources.Load<Tile>("Tiles/tile_042");
        feature_tile_set[2] = Resources.Load<Tile>("Tiles/tile_043");
        feature_tile_set[3] = Resources.Load<Tile>("Tiles/tile_044");
        feature_tile_set[4] = Resources.Load<Tile>("Tiles/tile_045");
        feature_tile_set[5] = Resources.Load<Tile>("Tiles/tile_046");
        feature_tile_set[6] = Resources.Load<Tile>("Tiles/tile_047");
        feature_tile_set[7] = Resources.Load<Tile>("Tiles/tile_048");
        feature_tile_set[8] = Resources.Load<Tile>("Tiles/tile_049");
        feature_tile_set[9] = Resources.Load<Tile>("Tiles/tile_050");
        feature_tile_set[10] = Resources.Load<Tile>("Tiles/tile_051");
        feature_tile_set[11] = Resources.Load<Tile>("Tiles/tile_052");
        feature_tile_set[12] = Resources.Load<Tile>("Tiles/tile_053");
        feature_tile_set[13] = Resources.Load<Tile>("Tiles/tile_054");
        feature_tile_set[14] = Resources.Load<Tile>("Tiles/tile_055");
        feature_tile_set[15] = Resources.Load<Tile>("Tiles/tile_056");
        feature_tile_set[16] = Resources.Load<Tile>("Tiles/tile_057");
        feature_tile_set[17] = Resources.Load<Tile>("Tiles/tile_058");
        feature_tile_set[18] = Resources.Load<Tile>("Tiles/tile_059");
        feature_tile_set[19] = Resources.Load<Tile>("Tiles/tile_060");

        feature_tile_set[20] = Resources.Load<Tile>("Tiles/water_plant0");
        feature_tile_set[21] = Resources.Load<Tile>("Tiles/water_plant1");
        feature_tile_set[22] = Resources.Load<Tile>("Tiles/tile_122");
        feature_tile_set[23] = Resources.Load<Tile>("Tiles/feature_1");
        feature_tile_set[24] = Resources.Load<Tile>("Tiles/feature_2");

        tree_like_set[0] = Resources.Load<Tile>("Tiles/tree_0");
        tree_like_set[1] = Resources.Load<Tile>("Tiles/tree_1");
        tree_like_set[2] = Resources.Load<Tile>("Tiles/tree_2");
        tree_like_set[3] = Resources.Load<Tile>("Tiles/tree_3");

    }


    public void GenerateMap_1(int m_seed)
	{
        //Init_tileset();

        UnityEngine.Random.InitState(m_seed);
		//生成噪声
		float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight,noiseScale,octaves,persistance,lacunarity,m_seed,offset);

        float nosie_value;
        Vector3Int p = new Vector3Int(0,0,1);
        Vector3Int p_offset1 = new Vector3Int(1,0,0);
        Vector3Int p_offset2 = new Vector3Int(0,1,0);
        Vector3Int p_offset3 = new Vector3Int(1,1,0);
        Vector3Int p_offset0 = new Vector3Int(mapHeight, mapWidth,0);

        Tile a_tile;
        //int sprite_id;
        for(int i = 0; i < mapHeight; i++)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                //p.Set(2*i, 2*j, 1);
                nosie_value = noiseMap[i, j];
                int kk = UnityEngine.Random.Range(14, 17);

                if(nosie_value <= 0.1f)
                {
                    
                    map_info[2*i, 2*j, 0] = 3;
                    map_info[2*i, 2*j, 1] = 16;

                    map_info[2*i+1, 2*j, 0] = 3;
                    map_info[2*i+1, 2*j, 1] = 16;

                    map_info[2*i, 2*j+1, 0] = 3;
                    map_info[2*i, 2*j+1, 1] = 16;

                    map_info[2*i+1, 2*j+1, 0] = 3;
                    map_info[2*i+1, 2*j+1, 1] = 16;
                    
                }
                else if(nosie_value > 0.8f)
                {
                    //map_info[i, j] = Tuple.Create<int, int>(2, 18);
                    map_info[2*i, 2*j, 0] = 2;
                    map_info[2*i, 2*j, 1] = 18;

                    map_info[2*i+1, 2*j, 0] = 2;
                    map_info[2*i+1, 2*j, 1] = 18;

                    map_info[2*i, 2*j+1, 0] = 2;
                    map_info[2*i, 2*j+1, 1] = 18;

                    map_info[2*i+1, 2*j+1, 0] = 2;
                    map_info[2*i+1, 2*j+1, 1] = 18;
                    
                }
                else if(nosie_value > 0.15 && nosie_value <= 0.8)
                {
                    //map_info[i, j] = Tuple.Create<int, int>(3, 2);
                    map_info[2*i, 2*j, 0] = 3;
                    map_info[2*i, 2*j, 1] = 12;

                    map_info[2*i+1, 2*j, 0] = 3;
                    map_info[2*i+1, 2*j, 1] = 12;

                    map_info[2*i, 2*j+1, 0] = 3;
                    map_info[2*i, 2*j+1, 1] = 12;

                    map_info[2*i+1, 2*j+1, 0] = 3;
                    map_info[2*i+1, 2*j+1, 1] = 12;
                    
                }
                else
                {
                    //map_info[i, j] = Tuple.Create<int, int>(3, UnityEngine.Random.Range(0, 2));
                    kk = UnityEngine.Random.Range(15, 17);
                    map_info[2*i, 2*j, 0] = 3;
                    map_info[2*i, 2*j, 1] = kk;

                    map_info[2*i+1, 2*j, 0] = 3;
                    map_info[2*i+1, 2*j, 1] = kk;

                    map_info[2*i, 2*j+1, 0] = 3;
                    map_info[2*i, 2*j+1, 1] = kk;

                    map_info[2*i+1, 2*j+1, 0] = 3;
                    map_info[2*i+1, 2*j+1, 1] = kk;

                }
                
                
                
                //main_tilemap.SetTile((p - p_offset0)/2, a_tile);
            }
        }


        for(int i2 = 0; i2 < 2*mapHeight; i2++)
        {
            for(int j2 = 0; j2 < 2*mapWidth; j2++)
            {
                if(map_info[i2, j2, 0] == 2)
                {
                    if((i2 > 0 && map_info[i2-1, j2, 0] != 2) 
                        || (j2 > 0 && map_info[i2, j2-1, 0] != 2) 
                        || (i2 < 2*mapHeight-1 && map_info[i2+1, j2, 0] != 2)
                        || (j2 < 2*mapWidth-1 && map_info[i2, j2+1, 0] != 2)
                        )
                    {
                        int kk = UnityEngine.Random.Range(21, 50);
                        if(kk < 30)
                            map_info[i2, j2, 1] = kk;
                            
                        else if(kk >= 30 && kk < 33)
                        {
                            map_info2[i2, j2, 0] = 3;
                            map_info2[i2, j2, 1] = 20;
                            //Debug.Log("Here!");
                        }
                        else if(kk >= 35 && kk < 40)
                        {
                            map_info2[i2, j2, 0] = 3;
                            map_info2[i2, j2, 1] = 21;
                            //Debug.Log("HereB!");
                        }
                        //map_info[i,j] = new Tuple<int, int>(map_info[i,j].Item1, UnityEngine.Random.Range(21, 30));
                    }
                }
            }

        }

        int map_block_type;
        int map_block_id_in_mapinfo;


        for(int i2 = 0; i2 < 2*mapHeight; i2++)
        {
            for(int j2 = 0; j2 < 2*mapWidth; j2++)
            {
                p.Set(i2, j2, 0);
                map_block_type = map_info[i2, j2, 0];
                map_block_id_in_mapinfo = map_info[i2, j2, 1];

                if(map_block_type == 1)
                {
                    a_tile = dirt_tile_set[map_block_id_in_mapinfo];
                }
                else if(map_block_type == 2)
                {
                    a_tile = water_tile_set[map_block_id_in_mapinfo];
                    if(a_tile == null || a_tile.sprite == null)
                    {
                        Debug.Log("NOOO" + map_block_id_in_mapinfo);
                    }
                }
                else if(map_block_type == 3)
                {
                    a_tile = grass_tile_set[map_block_id_in_mapinfo];
                }
                else
                {
                    a_tile = water_tile_set[map_block_id_in_mapinfo];
                }

                main_tilemap.SetTile(p-p_offset0, a_tile);
                
            }
        }


        //features-----------------------------------------------

        float[,] noiseMap2 = Noise.GenerateNoiseMap(mapWidth*2, mapHeight*2, 11, octaves, persistance, lacunarity-1, m_seed+2, offset);

        for(int i = 0; i < mapHeight*2; i++)
        {
            for(int j = 0; j < mapWidth*2; j++)
            {
                if(map_info[i, j, 0] != 2)
                {
                    if(noiseMap2[i,j] < 0.18f)
                    {
                        map_info2[i, j, 0] = 1; //feature
                        map_info2[i, j, 1] = 0; //flower
                    }
                    else if(noiseMap2[i,j] > 0.83f)
                    {
                        map_info2[i, j, 0] = 1; //feature
                        map_info2[i, j, 1] = 3; //flower
                    }
                }
                /*else
                {
                    map_info2[i, j, 0] = 0; //null
                }*/
            }
        }

        int cnt1 = 0;
        int try_x;
        int try_y;
        int k_num = UnityEngine.Random.Range(10, 30);
        for(int k = 0; k <= k_num; k++)
        {
            cnt1 = 0;
            while(cnt1 < 50)
            {
                try_x = UnityEngine.Random.Range(0, mapHeight * 2);
                try_y = UnityEngine.Random.Range(0, mapWidth * 2);
                if(map_info2[try_x, try_y, 0] == 0 && map_info[try_x, try_y, 0] != 2)
                {
                    map_info2[try_x, try_y, 0] = 1;
                    map_info2[try_x, try_y, 1] = 10;
                    break;
                }
                cnt1++;
            }
        }


        //tree ---------------
        k_num = UnityEngine.Random.Range(40, 50);

        for(int k = 0; k <= k_num; k++)
        {
            cnt1 = 0;
            while(cnt1 < 50)
            {
                try_x = UnityEngine.Random.Range(0, mapHeight * 2);
                try_y = UnityEngine.Random.Range(0, mapWidth * 2);
                if(map_info[try_x, try_y, 0] != 2) //map_info2[try_x, try_y, 0] == 0 && 
                {
                    map_info2[try_x, try_y, 0] = 2;
                    if(UnityEngine.Random.Range(0, 11) < 4)
                    {
                        map_info2[try_x, try_y, 1] = 0;
                    }
                    else
                    {
                        map_info2[try_x, try_y, 1] = 1;
                    }
                    break;
                }
                cnt1++;
            }
        }

        
         for(int i2 = 0; i2 < 2*mapHeight; i2++)
        {
            for(int j2 = 0; j2 < 2*mapWidth; j2++)
            {
                p.Set(i2, j2, 0);
                map_block_type = map_info2[i2, j2, 0];
                map_block_id_in_mapinfo = map_info2[i2, j2, 1];

                if(map_block_type == 1)
                {
                    a_tile = feature_tile_set[map_block_id_in_mapinfo];
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                else if(map_block_type== 2)
                {
                    a_tile = tree_like_set[map_block_id_in_mapinfo];
                    //a_tile.sprite.
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                else if(map_block_type == 3)
                {
                    if(feature_tile_set[map_block_id_in_mapinfo] == null)
                    {
                        //Debug.Log("Cant find this");
                    }
                    else
                    {
                        //Debug.Log("Find this");
                    }
                    a_tile = feature_tile_set[map_block_id_in_mapinfo];
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                
            }
        }

	}

    public void GenerateMap_2(int m_seed) //草原
	{
        //Init_tileset();

        UnityEngine.Random.InitState(m_seed);
		//生成噪声
		float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight,noiseScale,octaves,persistance,lacunarity,m_seed,offset);

        float nosie_value;
        Vector3Int p = new Vector3Int(0,0,1);
        Vector3Int p_offset1 = new Vector3Int(1,0,0);
        Vector3Int p_offset2 = new Vector3Int(0,1,0);
        Vector3Int p_offset3 = new Vector3Int(1,1,0);
        Vector3Int p_offset0 = new Vector3Int(mapHeight, mapWidth,0);

        Tile a_tile;
        //int sprite_id;
        for(int i = 0; i < mapHeight; i++)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                //p.Set(2*i, 2*j, 1);
                nosie_value = noiseMap[i, j];
                int kk = UnityEngine.Random.Range(14, 17);

                if(nosie_value <= 0.1f)
                {
                    
                    map_info[2*i, 2*j, 0] = 3;
                    map_info[2*i, 2*j, 1] = 4;

                    map_info[2*i+1, 2*j, 0] = 3;
                    map_info[2*i+1, 2*j, 1] = 4;

                    map_info[2*i, 2*j+1, 0] = 3;
                    map_info[2*i, 2*j+1, 1] = 4;

                    map_info[2*i+1, 2*j+1, 0] = 3;
                    map_info[2*i+1, 2*j+1, 1] = 4;
                    
                }
                else if(nosie_value > 0.85f)
                {
                    //map_info[i, j] = Tuple.Create<int, int>(2, 18);
                    map_info[2*i, 2*j, 0] = 2;
                    map_info[2*i, 2*j, 1] = 30;

                    map_info[2*i+1, 2*j, 0] = 2;
                    map_info[2*i+1, 2*j, 1] = 30;

                    map_info[2*i, 2*j+1, 0] = 2;
                    map_info[2*i, 2*j+1, 1] = 30;

                    map_info[2*i+1, 2*j+1, 0] = 2;
                    map_info[2*i+1, 2*j+1, 1] = 30;
                    
                }
                else if(nosie_value > 0.15 && nosie_value <= 0.85)
                {
                    //map_info[i, j] = Tuple.Create<int, int>(3, 2);
                    map_info[2*i, 2*j, 0] = 3;
                    map_info[2*i, 2*j, 1] = 2;

                    map_info[2*i+1, 2*j, 0] = 3;
                    map_info[2*i+1, 2*j, 1] = 2;

                    map_info[2*i, 2*j+1, 0] = 3;
                    map_info[2*i, 2*j+1, 1] = 2;

                    map_info[2*i+1, 2*j+1, 0] = 3;
                    map_info[2*i+1, 2*j+1, 1] = 2;
                    
                }
                else
                {
                    //map_info[i, j] = Tuple.Create<int, int>(3, UnityEngine.Random.Range(0, 2));
                    kk = UnityEngine.Random.Range(3, 6);
                    map_info[2*i, 2*j, 0] = 3;
                    map_info[2*i, 2*j, 1] = kk;

                    map_info[2*i+1, 2*j, 0] = 3;
                    map_info[2*i+1, 2*j, 1] = kk;

                    map_info[2*i, 2*j+1, 0] = 3;
                    map_info[2*i, 2*j+1, 1] = kk;

                    map_info[2*i+1, 2*j+1, 0] = 3;
                    map_info[2*i+1, 2*j+1, 1] = kk;

                }
                
                
                
                //main_tilemap.SetTile((p - p_offset0)/2, a_tile);
            }
        }


        /*
        for(int i2 = 0; i2 < 2*mapHeight; i2++)
        {
            for(int j2 = 0; j2 < 2*mapWidth; j2++)
            {
                if(map_info[i2, j2, 0] == 2)
                {
                    if((i2 > 0 && map_info[i2-1, j2, 0] != 2) 
                        || (j2 > 0 && map_info[i2, j2-1, 0] != 2) 
                        || (i2 < 2*mapHeight-1 && map_info[i2+1, j2, 0] != 2)
                        || (j2 < 2*mapWidth-1 && map_info[i2, j2+1, 0] != 2)
                        )
                    {
                        int kk = UnityEngine.Random.Range(21, 50);
                        if(kk < 30)
                            map_info[i2, j2, 1] = kk;
                            
                        else if(kk >= 30 && kk < 33)
                        {
                            map_info2[i2, j2, 0] = 3;
                            map_info2[i2, j2, 1] = 20;
                            Debug.Log("Here!");
                        }
                        else if(kk >= 35 && kk < 40)
                        {
                            map_info2[i2, j2, 0] = 3;
                            map_info2[i2, j2, 1] = 21;
                            Debug.Log("HereB!");
                        }
                        //map_info[i,j] = new Tuple<int, int>(map_info[i,j].Item1, UnityEngine.Random.Range(21, 30));
                    }
                }
            }

        }*/

        int map_block_type;
        int map_block_id_in_mapinfo;


        for(int i2 = 0; i2 < 2*mapHeight; i2++)
        {
            for(int j2 = 0; j2 < 2*mapWidth; j2++)
            {
                p.Set(i2, j2, 0);
                map_block_type = map_info[i2, j2, 0];
                map_block_id_in_mapinfo = map_info[i2, j2, 1];

                if(map_block_type == 1)
                {
                    a_tile = dirt_tile_set[map_block_id_in_mapinfo];
                }
                else if(map_block_type == 2)
                {
                    a_tile = water_tile_set[map_block_id_in_mapinfo];
                    if(a_tile == null || a_tile.sprite == null)
                    {
                        Debug.Log("NOOO" + map_block_id_in_mapinfo);
                    }
                }
                else if(map_block_type == 3)
                {
                    a_tile = grass_tile_set[map_block_id_in_mapinfo];
                }
                else
                {
                    a_tile = water_tile_set[map_block_id_in_mapinfo];
                }

                main_tilemap.SetTile(p-p_offset0, a_tile);
                
            }
        }


        //features-----------------------------------------------

        float[,] noiseMap2 = Noise.GenerateNoiseMap(mapWidth*2, mapHeight*2, 11, octaves, persistance, lacunarity-1, m_seed+2, offset);

        for(int i = 0; i < mapHeight*2; i++)
        {
            for(int j = 0; j < mapWidth*2; j++)
            {
                if(map_info[i, j, 0] != 2)
                {
                    if(noiseMap2[i,j] < 0.18f)
                    {
                        map_info2[i, j, 0] = 1; //feature
                        map_info2[i, j, 1] = 24; //flower
                    }
                    else if(noiseMap2[i,j] > 0.95f)
                    {
                        map_info2[i, j, 0] = 1; //feature
                        map_info2[i, j, 1] = UnityEngine.Random.Range(12, 20); //flower
                    }
                }
                /*else
                {
                    map_info2[i, j, 0] = 0; //null
                }*/
            }
        }

        int cnt1 = 0;
        int try_x;
        int try_y;
        int k_num = UnityEngine.Random.Range(8, 20);
        for(int k = 0; k <= k_num; k++)
        {
            cnt1 = 0;
            while(cnt1 < 50)
            {
                try_x = UnityEngine.Random.Range(0, mapHeight * 2);
                try_y = UnityEngine.Random.Range(0, mapWidth * 2);
                if(map_info2[try_x, try_y, 0] == 0 && map_info[try_x, try_y, 0] != 2)
                {
                    map_info2[try_x, try_y, 0] = 1;
                    map_info2[try_x, try_y, 1] = 8;
                    break;
                }
                cnt1++;
            }
        }


        //tree ---------------

        
        k_num = UnityEngine.Random.Range(15, 30);

        for(int k = 0; k <= k_num; k++)
        {
            cnt1 = 0;
            while(cnt1 < 50)
            {
                try_x = UnityEngine.Random.Range(0, mapHeight * 2);
                try_y = UnityEngine.Random.Range(0, mapWidth * 2);
                if(map_info[try_x, try_y, 0] != 2) //map_info2[try_x, try_y, 0] == 0 && 
                {
                    map_info2[try_x, try_y, 0] = 2;
                
                    map_info2[try_x, try_y, 1] = 2;
                    
                    break;
                }
                cnt1++;
            }
        }

        
         for(int i2 = 0; i2 < 2*mapHeight; i2++)
        {
            for(int j2 = 0; j2 < 2*mapWidth; j2++)
            {
                p.Set(i2, j2, 0);
                map_block_type = map_info2[i2, j2, 0];
                map_block_id_in_mapinfo = map_info2[i2, j2, 1];

                if(map_block_type == 1)
                {
                    a_tile = feature_tile_set[map_block_id_in_mapinfo];
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                else if(map_block_type== 2)
                {
                    a_tile = tree_like_set[map_block_id_in_mapinfo];
                    //a_tile.sprite.
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                else if(map_block_type == 3)
                {
                    a_tile = feature_tile_set[map_block_id_in_mapinfo];
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                else if(map_block_type == 4)
                {
                    a_tile = water_tile_set[map_block_id_in_mapinfo];
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                
            }
        }

	}






    public void GenerateMap_3(int m_seed) //雪地
	{
        //Init_tileset();

        UnityEngine.Random.InitState(m_seed);
		//生成噪声
		float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight,noiseScale,octaves,persistance,lacunarity,m_seed,offset);

        float nosie_value;
        Vector3Int p = new Vector3Int(0,0,1);
        Vector3Int p_offset1 = new Vector3Int(1,0,0);
        Vector3Int p_offset2 = new Vector3Int(0,1,0);
        Vector3Int p_offset3 = new Vector3Int(1,1,0);
        Vector3Int p_offset0 = new Vector3Int(mapHeight, mapWidth,0);

        Tile a_tile;
        //int sprite_id;
        for(int i = 0; i < mapHeight; i++)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                //p.Set(2*i, 2*j, 1);
                nosie_value = noiseMap[i, j];
                int kk = UnityEngine.Random.Range(14, 17);

                if(nosie_value <= 0.04f)
                {
                    
                    map_info[2*i, 2*j, 0] = 1;
                    map_info[2*i, 2*j, 1] = 21;

                    map_info[2*i+1, 2*j, 0] = 1;
                    map_info[2*i+1, 2*j, 1] = 21;

                    map_info[2*i, 2*j+1, 0] = 1;
                    map_info[2*i, 2*j+1, 1] = 21;

                    map_info[2*i+1, 2*j+1, 0] = 1;
                    map_info[2*i+1, 2*j+1, 1] = 21;
                    
                }
                else if(nosie_value > 0.8f)
                {
                    //map_info[i, j] = Tuple.Create<int, int>(2, 18);
                    map_info[2*i, 2*j, 0] = 2;
                    map_info[2*i, 2*j, 1] = 10;

                    map_info[2*i+1, 2*j, 0] = 2;
                    map_info[2*i+1, 2*j, 1] = 10;

                    map_info[2*i, 2*j+1, 0] = 2;
                    map_info[2*i, 2*j+1, 1] = 10;

                    map_info[2*i+1, 2*j+1, 0] = 2;
                    map_info[2*i+1, 2*j+1, 1] = 10;
                    
                }
                else if(nosie_value > 0.07f && nosie_value <= 0.8f)
                {
                    //map_info[i, j] = Tuple.Create<int, int>(3, 2);
                    map_info[2*i, 2*j, 0] = 1;
                    map_info[2*i, 2*j, 1] = 26;

                    map_info[2*i+1, 2*j, 0] = 1;
                    map_info[2*i+1, 2*j, 1] = 26;

                    map_info[2*i, 2*j+1, 0] = 1;
                    map_info[2*i, 2*j+1, 1] = 26;

                    map_info[2*i+1, 2*j+1, 0] = 1;
                    map_info[2*i+1, 2*j+1, 1] = 26;
                    
                }
                else
                {
                    //map_info[i, j] = Tuple.Create<int, int>(3, UnityEngine.Random.Range(0, 2));
                    kk = UnityEngine.Random.Range(17, 19);
                    map_info[2*i, 2*j, 0] = 1;
                    map_info[2*i, 2*j, 1] = kk;

                    map_info[2*i+1, 2*j, 0] = 1;
                    map_info[2*i+1, 2*j, 1] = kk;

                    map_info[2*i, 2*j+1, 0] = 1;
                    map_info[2*i, 2*j+1, 1] = kk;

                    map_info[2*i+1, 2*j+1, 0] = 1;
                    map_info[2*i+1, 2*j+1, 1] = kk;

                }
                
                
                
                //main_tilemap.SetTile((p - p_offset0)/2, a_tile);
            }
        }


        int map_block_type;
        int map_block_id_in_mapinfo;


        for(int i2 = 0; i2 < 2*mapHeight; i2++)
        {
            for(int j2 = 0; j2 < 2*mapWidth; j2++)
            {
                p.Set(i2, j2, 0);
                map_block_type = map_info[i2, j2, 0];
                map_block_id_in_mapinfo = map_info[i2, j2, 1];

                if(map_block_type == 1)
                {
                    a_tile = dirt_tile_set[map_block_id_in_mapinfo];
                }
                else if(map_block_type == 2)
                {
                    a_tile = water_tile_set[map_block_id_in_mapinfo];
                    if(a_tile == null || a_tile.sprite == null)
                    {
                        Debug.Log("NOOO" + map_block_id_in_mapinfo);
                    }
                }
                else if(map_block_type == 3)
                {
                    a_tile = grass_tile_set[map_block_id_in_mapinfo];
                }
                else
                {
                    a_tile = water_tile_set[map_block_id_in_mapinfo];
                }

                main_tilemap.SetTile(p-p_offset0, a_tile);
                
            }
        }



        //features-----------------------------------------------

        float[,] noiseMap2 = Noise.GenerateNoiseMap(mapWidth*2, mapHeight*2, 11, octaves, persistance, lacunarity-1, m_seed+2, offset);

        for(int i = 0; i < mapHeight*2; i++)
        {
            for(int j = 0; j < mapWidth*2; j++)
            {
                if(map_info[i, j, 0] != 2)
                {
                    if(noiseMap2[i,j] > 0.9f)
                    {
                        map_info2[i, j, 0] = 4; //feature
                        map_info2[i, j, 1] = UnityEngine.Random.Range(32, 36); //flower
                    }
                }
                /*else
                {
                    map_info2[i, j, 0] = 0; //null
                }*/
            }
        }

        int cnt1 = 0;
        int try_x;
        int try_y;
        int k_num = UnityEngine.Random.Range(8, 20);
        for(int k = 0; k <= k_num; k++)
        {
            cnt1 = 0;
            while(cnt1 < 50)
            {
                try_x = UnityEngine.Random.Range(0, mapHeight * 2);
                try_y = UnityEngine.Random.Range(0, mapWidth * 2);
                if(map_info2[try_x, try_y, 0] == 0 && map_info[try_x, try_y, 0] != 2)
                {
                    map_info2[try_x, try_y, 0] = 1;
                    map_info2[try_x, try_y, 1] = 23;
                    break;
                }
                cnt1++;
            }
        }


        //tree ---------------

        
        k_num = UnityEngine.Random.Range(15, 20);

        for(int k = 0; k <= k_num; k++)
        {
            cnt1 = 0;
            while(cnt1 < 50)
            {
                try_x = UnityEngine.Random.Range(0, mapHeight * 2);
                try_y = UnityEngine.Random.Range(0, mapWidth * 2);
                if(map_info[try_x, try_y, 0] != 2) //map_info2[try_x, try_y, 0] == 0 && 
                {
                    map_info2[try_x, try_y, 0] = 2;
                
                    map_info2[try_x, try_y, 1] = 3;
                    
                    break;
                }
                cnt1++;
            }
        }

        
         for(int i2 = 0; i2 < 2*mapHeight; i2++)
        {
            for(int j2 = 0; j2 < 2*mapWidth; j2++)
            {
                p.Set(i2, j2, 0);
                map_block_type = map_info2[i2, j2, 0];
                map_block_id_in_mapinfo = map_info2[i2, j2, 1];

                if(map_block_type == 1)
                {
                    a_tile = feature_tile_set[map_block_id_in_mapinfo];
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                else if(map_block_type== 2)
                {
                    a_tile = tree_like_set[map_block_id_in_mapinfo];
                    //a_tile.sprite.
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                else if(map_block_type == 3)
                {
                    a_tile = feature_tile_set[map_block_id_in_mapinfo];
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                else if(map_block_type == 4)
                {
                    a_tile = water_tile_set[map_block_id_in_mapinfo];
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                
            }
        }

	}







    public void GenerateMap_4(int m_seed) //雪地
	{
        //Init_tileset();

        UnityEngine.Random.InitState(m_seed);
		//生成噪声
		float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight,noiseScale,octaves,persistance,lacunarity,m_seed,offset);

        float nosie_value;
        Vector3Int p = new Vector3Int(0,0,1);
        Vector3Int p_offset1 = new Vector3Int(1,0,0);
        Vector3Int p_offset2 = new Vector3Int(0,1,0);
        Vector3Int p_offset3 = new Vector3Int(1,1,0);
        Vector3Int p_offset0 = new Vector3Int(mapHeight, mapWidth,0);

        Tile a_tile;
        //int sprite_id;
        for(int i = 0; i < mapHeight; i++)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                //p.Set(2*i, 2*j, 1);
                nosie_value = noiseMap[i, j];
                int kk = UnityEngine.Random.Range(14, 17);

                if(nosie_value > 0.1f && nosie_value <= 0.9f)
                {
                    //map_info[i, j] = Tuple.Create<int, int>(3, 2);
                    map_info[2*i, 2*j, 0] = 1;
                    map_info[2*i, 2*j, 1] = 24;

                    map_info[2*i+1, 2*j, 0] = 1;
                    map_info[2*i+1, 2*j, 1] = 24;

                    map_info[2*i, 2*j+1, 0] = 1;
                    map_info[2*i, 2*j+1, 1] = 24;

                    map_info[2*i+1, 2*j+1, 0] = 1;
                    map_info[2*i+1, 2*j+1, 1] = 24;
                    
                }
                else
                {
                    //map_info[i, j] = Tuple.Create<int, int>(2, 18);
                    map_info[2*i, 2*j, 0] = 2;
                    map_info[2*i, 2*j, 1] = 31;

                    map_info[2*i+1, 2*j, 0] = 2;
                    map_info[2*i+1, 2*j, 1] = 31;

                    map_info[2*i, 2*j+1, 0] = 2;
                    map_info[2*i, 2*j+1, 1] = 31;

                    map_info[2*i+1, 2*j+1, 0] = 2;
                    map_info[2*i+1, 2*j+1, 1] = 31;
                    
                }
                
                
                
                
                //main_tilemap.SetTile((p - p_offset0)/2, a_tile);
            }
        }


        int map_block_type;
        int map_block_id_in_mapinfo;


        for(int i2 = 0; i2 < 2*mapHeight; i2++)
        {
            for(int j2 = 0; j2 < 2*mapWidth; j2++)
            {
                p.Set(i2, j2, 0);
                map_block_type = map_info[i2, j2, 0];
                map_block_id_in_mapinfo = map_info[i2, j2, 1];

                if(map_block_type == 1)
                {
                    a_tile = dirt_tile_set[map_block_id_in_mapinfo];
                }
                else if(map_block_type == 2)
                {
                    a_tile = water_tile_set[map_block_id_in_mapinfo];
                    if(a_tile == null || a_tile.sprite == null)
                    {
                        Debug.Log("NOOO" + map_block_id_in_mapinfo);
                    }
                }
                else if(map_block_type == 3)
                {
                    a_tile = grass_tile_set[map_block_id_in_mapinfo];
                }
                else
                {
                    a_tile = water_tile_set[map_block_id_in_mapinfo];
                }

                main_tilemap.SetTile(p-p_offset0, a_tile);
                
            }
        }



        //features-----------------------------------------------

        float[,] noiseMap2 = Noise.GenerateNoiseMap(mapWidth*2, mapHeight*2, 11, octaves, persistance, lacunarity-1, m_seed+2, offset);

        
        for(int i = 0; i < mapHeight*2; i++)
        {
            for(int j = 0; j < mapWidth*2; j++)
            {
                if(map_info[i, j, 0] != 2)
                {
                    if(noiseMap2[i,j] > 0.8f)
                    {
                        map_info2[i, j, 0] = 5; //feature
                        map_info2[i, j, 1] = 27; //flower
                    }
                }
                
            }
        }

        
        int cnt1 = 0;
        int try_x;
        int try_y;
        int k_num = UnityEngine.Random.Range(8, 20);
        /*
        for(int k = 0; k <= k_num; k++)
        {
            cnt1 = 0;
            while(cnt1 < 50)
            {
                try_x = UnityEngine.Random.Range(0, mapHeight * 2);
                try_y = UnityEngine.Random.Range(0, mapWidth * 2);
                if(map_info2[try_x, try_y, 0] == 0 && map_info[try_x, try_y, 0] != 2)
                {
                    map_info2[try_x, try_y, 0] = 1;
                    map_info2[try_x, try_y, 1] = 23;
                    break;
                }
                cnt1++;
            }
        }
        */


        //tree ---------------

        
        k_num = UnityEngine.Random.Range(15, 20);

        for(int k = 0; k <= k_num; k++)
        {
            cnt1 = 0;
            while(cnt1 < 50)
            {
                try_x = UnityEngine.Random.Range(0, mapHeight * 2);
                try_y = UnityEngine.Random.Range(0, mapWidth * 2);
                if(map_info[try_x, try_y, 0] != 2) //map_info2[try_x, try_y, 0] == 0 && 
                {
                    map_info2[try_x, try_y, 0] = 1;
                
                    map_info2[try_x, try_y, 1] = 22;
                    
                    break;
                }
                cnt1++;
            }
        }

        
         for(int i2 = 0; i2 < 2*mapHeight; i2++)
        {
            for(int j2 = 0; j2 < 2*mapWidth; j2++)
            {
                p.Set(i2, j2, 0);
                map_block_type = map_info2[i2, j2, 0];
                map_block_id_in_mapinfo = map_info2[i2, j2, 1];

                if(map_block_type == 1)
                {
                    a_tile = feature_tile_set[map_block_id_in_mapinfo];
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                else if(map_block_type== 2)
                {
                    a_tile = tree_like_set[map_block_id_in_mapinfo];
                    //a_tile.sprite.
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                else if(map_block_type == 3)
                {
                    a_tile = feature_tile_set[map_block_id_in_mapinfo];
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                else if(map_block_type == 5)
                {
                    a_tile = dirt_tile_set[map_block_id_in_mapinfo];
                    tilemap2.SetTile(p - p_offset0, a_tile);
                }
                
                
            }
        }

	}




}

