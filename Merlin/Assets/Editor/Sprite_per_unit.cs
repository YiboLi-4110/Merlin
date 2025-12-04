using UnityEngine;
using UnityEditor;

public class Sprite_per_unit : Editor
{
    [MenuItem("Menu_1/Bunch_size", false, 100)]
    public static void Menu_1()
    {
        if (Selection.objects.Length > 0)
        {
            foreach (Texture texture in Selection.objects)
            {
                string selectionPath = AssetDatabase.GetAssetPath(texture);
                TextureImporter textureIm = AssetImporter.GetAtPath(selectionPath) as TextureImporter;
                textureIm.textureType = TextureImporterType.Sprite;
                textureIm.spriteImportMode = SpriteImportMode.Single;
                textureIm.spritePixelsPerUnit = 32f;
                textureIm.filterMode = FilterMode.Point;

                var setting = textureIm.GetDefaultPlatformTextureSettings();
                setting.format = TextureImporterFormat.RGBA32;
                setting.compressionQuality = 0;
                textureIm.SetPlatformTextureSettings(setting);
                /*textureIm.textureType = TextureImporterType.Sprite;
                textureIm.spriteImportMode = SpriteImportMode.Single;
                textureIm.isReadable = true;
                var setting = textureIm.GetDefaultPlatformTextureSettings();
                setting.format = TextureImporterFormat.RGBA32;
                textureIm.SetPlatformTextureSettings(setting);*/

                AssetDatabase.ImportAsset(selectionPath);
            }
        }
    }
}
