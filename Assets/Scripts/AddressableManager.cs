using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

[Serializable]
public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
{
    public AssetReferenceAudioClip(string guid) : base(guid) { }
}
public class AddressableManager : MonoBehaviour
{
    [SerializeField]
    private AssetReference playerArmatureAssetReference;

    [SerializeField]
    private AssetReferenceAudioClip musicAssetReference;

    [SerializeField]
    private AssetReferenceTexture2D unityLogoAssetReference;

    [SerializeField]
    private RawImage rawImageUnityLogo;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        // start the loader
        Addressables.InitializeAsync().Completed += AddressablesManager_Completed;

    }

    private void AddressablesManager_Completed(AsyncOperationHandle<IResourceLocator> obj)
    {
        // Commenting this for loading UI indicator
        //playerArmatureAssetReference.InstantiateAsync().Completed += (go) =>
        //{
        //    var playerController = go.Result;
        //    cinemachineVirtualCamera.Follow = playerController.transform.Find("PlayerCameraRoot");
        //};

        playerArmatureAssetReference.LoadAssetAsync<GameObject>().Completed += (playerArmatureAsset) =>
        {
            playerArmatureAssetReference.InstantiateAsync().Completed += (playerArmatureGameObject) =>
            {
                var playerController = playerArmatureGameObject.Result;
                cinemachineVirtualCamera.Follow = playerController.transform.Find("PlayerCameraRoot");
            };            
        };

        musicAssetReference.LoadAssetAsync<AudioClip>().Completed += (clip) =>
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip.Result;
            audioSource.playOnAwake = false;
            audioSource.loop = true;
            audioSource.Play();
        };

        unityLogoAssetReference.LoadAssetAsync<Texture2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (unityLogoAssetReference.Asset != null && rawImageUnityLogo.texture == null)
        {
            rawImageUnityLogo.texture = unityLogoAssetReference.Asset as Texture2D;
            Color currentColor = rawImageUnityLogo.color;
            currentColor.a = 1.0f;
            rawImageUnityLogo.color = currentColor;
        }

        //Checking for loading completion
        {
            if (playerArmatureAssetReference != null && musicAssetReference != null && unityLogoAssetReference != null)
            {

            }

        }
    }

    //private void OnDestroy()
    //{
    //    playerArmatureAssetReference.ReleaseInstance()
    //}
}
