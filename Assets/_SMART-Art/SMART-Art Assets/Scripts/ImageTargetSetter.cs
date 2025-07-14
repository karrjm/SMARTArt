using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Vuforia;

public class ImageTargetSetter : MonoBehaviour
{
    private string imageLink = "https://www.furlonggallery.net/files/square_thumbnails/21d80099f0176bd2fa914a08d5dc69d8.jpg";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadImage(imageLink));
    }

    IEnumerator LoadImage(string link)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(link);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D myTex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            //imageTargetBehaviour.SetRuntimeTargetTexture(myTex);
        }
    }

}
