using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SequentialImageDisplay : MonoBehaviour
{
    // 引用四个要显示的 Image
    public Image firstImage;
    public Image secondImage;
    public Image thirdImage;
    public Image fourthImage;

    // 用于存储每个 Image 是否已隐藏的键名
    private const string firstImageHiddenKey = "FirstImageHidden";
    private const string secondImageHiddenKey = "SecondImageHidden";
    private const string thirdImageHiddenKey = "ThirdImageHidden";
    private const string fourthImageHiddenKey = "FourthImageHidden";

    // 渐变持续时间
    public float fadeDuration = 1f;
    // 图片显示时间
    public float displayDuration = 1.5f;

    // 要切换到的新场景名称
    private string newSceneName = "NewPlayerScene";

    void Start()
    {
        // 清除 PlayerPrefs 数据（仅用于测试）
        PlayerPrefs.DeleteAll();

        // 初始时隐藏所有 Image 并将透明度设为 0
        SetImageAlpha(firstImage, 0f);
        firstImage.gameObject.SetActive(false);
        SetImageAlpha(secondImage, 0f);
        secondImage.gameObject.SetActive(false);
        SetImageAlpha(thirdImage, 0f);
        thirdImage.gameObject.SetActive(false);
        SetImageAlpha(fourthImage, 0f);
        fourthImage.gameObject.SetActive(false);

        // 检查第一个 Image 是否已经被永久隐藏
        if (!PlayerPrefs.HasKey(firstImageHiddenKey))
        {
            // 启动协程，在 0.5 秒后显示第一个 Image 并渐变
            StartCoroutine(ShowFirstImageAfterDelay(0.5f));
        }
    }

    // 设置 Image 的透明度
    void SetImageAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    IEnumerator ShowFirstImageAfterDelay(float delay)
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(delay);
        // 显示第一个 Image
        firstImage.gameObject.SetActive(true);
        // 启动渐变协程
        yield return StartCoroutine(FadeInImage(firstImage));
        // 等待显示时间
        yield return new WaitForSeconds(displayDuration);
        // 隐藏第一个 Image
        firstImage.gameObject.SetActive(false);
        // 记录第一个 Image 已被永久隐藏
        PlayerPrefs.SetInt(firstImageHiddenKey, 1);
        PlayerPrefs.Save();
        // 显示第二个 Image
        StartCoroutine(ShowSecondImageAfterDelay(0.5f));
    }

    IEnumerator ShowSecondImageAfterDelay(float delay)
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(delay);
        // 显示第二个 Image
        secondImage.gameObject.SetActive(true);
        // 启动渐变协程
        yield return StartCoroutine(FadeInImage(secondImage));
        // 等待显示时间
        yield return new WaitForSeconds(displayDuration);
        // 隐藏第二个 Image
        secondImage.gameObject.SetActive(false);
        // 记录第二个 Image 已被永久隐藏
        PlayerPrefs.SetInt(secondImageHiddenKey, 1);
        PlayerPrefs.Save();
        // 显示第三个 Image
        StartCoroutine(ShowThirdImageAfterDelay(0.5f));
    }

    IEnumerator ShowThirdImageAfterDelay(float delay)
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(delay);
        // 显示第三个 Image
        thirdImage.gameObject.SetActive(true);
        // 启动渐变协程
        yield return StartCoroutine(FadeInImage(thirdImage));
        // 等待显示时间
        yield return new WaitForSeconds(displayDuration);
        // 隐藏第三个 Image
        thirdImage.gameObject.SetActive(false);
        // 记录第三个 Image 已被永久隐藏
        PlayerPrefs.SetInt(thirdImageHiddenKey, 1);
        PlayerPrefs.Save();
        // 显示第四个 Image
        StartCoroutine(ShowFourthImageAfterDelay(0.5f));
    }

    IEnumerator ShowFourthImageAfterDelay(float delay)
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(delay);
        // 显示第四个 Image
        fourthImage.gameObject.SetActive(true);
        // 启动渐变协程
        yield return StartCoroutine(FadeInImage(fourthImage));
    }

    // 图片渐变协程
    IEnumerator FadeInImage(Image image)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            SetImageAlpha(image, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 确保最终透明度为 1
        SetImageAlpha(image, 1f);
    }

    void Update()
    {
        // 检查第四个 Image 是否显示且玩家是否按下了 S 键
        if (fourthImage.gameObject.activeSelf && Input.GetKeyDown(KeyCode.S))
        {
            // 隐藏第四个 Image
            fourthImage.gameObject.SetActive(false);
            // 记录第四个 Image 已被永久隐藏
            PlayerPrefs.SetInt(fourthImageHiddenKey, 1);
            PlayerPrefs.Save();

            // 切换到新场景
            SceneManager.LoadScene(newSceneName);
        }
    }
}