using UnityEngine;
using Vuforia;

namespace TallahasseePrototype.Scripts
{
    public class CloudRecoEventHandler : MonoBehaviour
    {
        private CloudRecoBehaviour _cloudRecoBehaviour;
        private bool _isScanning;
        private string _targetMetadata = "";

        [SerializeField] private ImageTargetBehaviour imageTargetTemplate;

        // register cloud reco callbacks
        private void Awake()
        {
            _cloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
            _cloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
            _cloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
            _cloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
            _cloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
            _cloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
        }

        private void OnDestroy()
        {
            _cloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
            _cloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
            _cloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
            _cloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
            _cloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
        }

        private void OnGUI()
        {
            // Display current 'scanning' status
            GUI.Box(new Rect(100, 100, 400, 100), _isScanning ? "Scanning" : "Not scanning");
            // Display metadata of latest detected cloud-target
            GUI.Box(new Rect(100, 200, 400, 100), "Metadata: " + _targetMetadata);
            // If not scanning, show button
            // so that user can restart cloud scanning
            if (_isScanning) return;
            if (GUI.Button(new Rect(100, 300, 400, 100), "Restart Scanning"))
                // Restart TargetFinder
                _cloudRecoBehaviour.CloudRecoEnabled = true;
        }

        private void OnInitialized(TargetFinder targetFinder)
        {
            Debug.Log("Initialized");
        }

        private void OnInitError(TargetFinder.InitState initError)
        {
            Debug.Log("Cloud Reco init error" + initError);
        }

        private void OnUpdateError(TargetFinder.UpdateState updateError)
        {
            Debug.Log("Cloud Reco update error " + updateError);
        }

        private void OnStateChanged(bool scanning)
        {
            _isScanning = scanning;
            if (!scanning) return;
            // clear all known trackables
            var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.GetTargetFinder<ImageTargetFinder>().ClearTrackables(false);
        }

        private void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
        {
            var cloudRecoSearchResult =
                (TargetFinder.CloudRecoSearchResult) targetSearchResult;
            // do something with the target metadata
            _targetMetadata = cloudRecoSearchResult.MetaData;
            // stop the target finder (i.e. stop scanning the cloud)
            _cloudRecoBehaviour.CloudRecoEnabled = false;
            
            // Build augmentation based on target 
            if (!imageTargetTemplate) return;
            // enable the new result with the same ImageTargetBehaviour: 
            var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>(); 
            tracker.GetTargetFinder<ImageTargetFinder>().EnableTracking(targetSearchResult, imageTargetTemplate.gameObject);
        }
    }
}