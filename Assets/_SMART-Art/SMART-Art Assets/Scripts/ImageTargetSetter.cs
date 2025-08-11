using System.IO;
using UnityEngine;
using Vuforia;

public class CreateFromDatabase : MonoBehaviour
{
    string dataSetPath = "Vuforia/ENGL480-Demo-Device.xml";
    string targetName = "portrait-of-dracula";

    // Start is called before the first frame update
    void Start()
    {
        VuforiaApplication.Instance.OnVuforiaInitialized += OnVuforiaInitialized;
    }

    void OnVuforiaInitialized(VuforiaInitError error)
    {
        if (error == VuforiaInitError.NONE)
            OnVuforiaStarted();
    }

    // Load and activate a data set at the given path.
    void OnVuforiaStarted()
    {
        // Create an Image Target from the database.
        var mImageTarget = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(
            dataSetPath,
            targetName);
        mImageTarget.OnTargetStatusChanged += OnTargetStatusChanged;
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.SetParent(mImageTarget.transform, false);
    }

    void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        Debug.Log($"target status: {status.Status}");
    }
}