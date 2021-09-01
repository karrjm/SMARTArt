using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class Audio : MonoBehaviour
    {
        [SerializeField] private string url = "";
        private AudioClip audioClip;
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(GetAudioClip());
        }

        private IEnumerator GetAudioClip()
        {
            using (var webRequest = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    var clip = DownloadHandlerAudioClip.GetContent(webRequest);
                    audioSource.clip = clip;
                }
            }
        }

        public void PauseAudio()
        {
            audioSource.Pause();
            print("pause button");
        }

        public void PlayAudio()
        {
            audioSource.Play();
            print("play button");
        }

        public void StopAudio()
        {
            audioSource.Stop();
            print("stop button");
        }
    }
}