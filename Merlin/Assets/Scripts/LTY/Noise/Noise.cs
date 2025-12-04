using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise 
{
    //octaves:度
    //persistance:持久性，控制振幅
    //lacunarity: 间隙度，控制频率

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight,float scale, int octaves, float persistance, float lacunarity, int seed, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        //防止除以0，除以负数
        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        /*
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //使用unity的柏林函数
                float amplitude = 1;    //振幅
                float frequency = 1;    //频率
                float noiseHeight = 0;  //高度，即最终该点的颜色值，将每一度的振幅相加来获得
                //分octaves次级
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = x / scale * frequency;
                    float sampleY = y / scale * frequency;  //用频率影响采样点间隔

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                    noiseHeight += perlinValue * amplitude; //振幅影响

                    amplitude *= persistance;	//更换新的频率和振幅
                    frequency *= lacunarity;
                }
                
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }
		*/

		//把噪声的范围从[minNoiseHeight,maxNoiseHeight]归一化到[0,1]
        /*for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }*/

        		//引入两个新变量 随机种子seed ，还有自定义偏移offset
		System.Random prng = new System.Random(seed);
		
		//即上文提到的RandomOffset，我们需要对每一个度都随机一个新的采样点
		Vector2[] octaveOffsets = new Vector2[octaves];	
		
		for (int i = 0; i < octaves; i++)  
		{
			float offsetX = prng.Next(-100000, 100000) + offset.x;	//RandomOffset += offset
			float offsetY = prng.Next(-100000, 100000) + offset.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}
		
		for (int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++)
				{
					float sampleX = x / scale * frequency + octaveOffsets[i].x;
					float sampleY = y / scale * frequency + octaveOffsets[i].y;
					
					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                    noiseHeight += perlinValue * amplitude; //振幅影响

                    amplitude *= persistance;	//更换新的频率和振幅
                    frequency *= lacunarity;	
				}

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;

			}
		}

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}

