using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.Linq;

public class DatasetLoader : MonoBehaviour
{
    private List<TrackerPolling> gos = new List<TrackerPolling>();
    public List<TrackerPolling> TrackerTargets
    {
        get
        {
            return gos;
        }
    }
    // Model is the GameObject to be augmented
    public GameObject Model;

    // Use this for initialization
    void Start()
    {
        // Registering call back to know when Vuforia is ready
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // This function is called when vuforia gives the started callback
    void OnVuforiaStarted()
    {

        // The 'path' string determines the location of xml file
        // For convinence the RealTime.xml is placed in the StreamingAssets folder
        // This file can be downloaded and the relative path will be used accordingly

        string path = "";
#if UNITY_IPHONE
		path = Application.dataPath + "/Raw/Vuforia/WifiVisualizer.xml";
#elif UNITY_ANDROID
		path = "jar:file://" + Application.dataPath + "!/assets/Vuforia/WifiVisualizer.xml";
#else
        path = Application.dataPath + "/StreamingAssets/Vuforia/WifiVisualizer.xml";
#endif

        bool status = LoadDataSet(path, VuforiaUnity.StorageType.STORAGE_ABSOLUTE);

        if (status)
        {
            Debug.Log("Dataset Loaded");
        }
        else
        {
            Debug.Log("Dataset Load Failed");
        }
    }

    // Load and activate a data set at the given path.
    private bool LoadDataSet(string dataSetPath, VuforiaUnity.StorageType storageType)
    {
        // Request an ImageTracker instance from the TrackerManager.
        ObjectTracker objectTracker = Vuforia.TrackerManager.Instance.GetTracker<ObjectTracker>();

        objectTracker.Stop();
        IEnumerable<DataSet> dataSetList = objectTracker.GetActiveDataSets();
        foreach (DataSet set in dataSetList.ToList())
        {
            objectTracker.DeactivateDataSet(set);
        }

        // Check if the data set exists at the given path.
        if (!DataSet.Exists(dataSetPath, storageType))
        {
            Debug.LogError("Data set " + dataSetPath + " does not exist.");
            return false;
        }

        // Create a new empty data set.
        DataSet dataSet = objectTracker.CreateDataSet();

        // Load the data set from the given path.
        if (!dataSet.Load(dataSetPath, storageType))
        {
            Debug.LogError("Failed to load data set " + dataSetPath + ".");
            return false;
        }

        // (Optional) Activate the data set.
        objectTracker.ActivateDataSet(dataSet);
        objectTracker.Start();

        AttachContentToTrackables(dataSet);

        return true;
    }

    // Add Trackable event handler and content (cubes) to the Targets.
    private void AttachContentToTrackables(DataSet dataSet)
    {
        // get all current TrackableBehaviours
        IEnumerable<TrackableBehaviour> trackableBehaviours =
        Vuforia.TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();

        // Loop over all TrackableBehaviours.
        foreach (TrackableBehaviour trackableBehaviour in trackableBehaviours)
        {
            // check if the Trackable of the current Behaviour is part of this dataset
            if (dataSet.Contains(trackableBehaviour.Trackable))
            {
                GameObject go = trackableBehaviour.gameObject;

                // Add a Trackable event handler to the Trackable.
                // This Behaviour handles Trackable lost/found callbacks.
                //go.AddComponent<TrackableBehaviour>();
                TrackerPolling tp = go.AddComponent<TrackerPolling>();

                // Instantiate the model.
                // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                GameObject cube = Instantiate(Model) as GameObject;

                // Attach the cube to the Trackable and make sure it has a proper size.
                cube.transform.parent = trackableBehaviour.transform;
                cube.transform.localScale = new Vector3(1f, 0.1f, 1f);
                cube.transform.localPosition = new Vector3(0.0f, 0.05f, 0.0f);
                cube.transform.localRotation = Quaternion.identity;
                cube.gameObject.SetActive(true);
                trackableBehaviour.gameObject.SetActive(false);
                gos.Add(tp);
            }
        }
    }

    public void ResizeMarkers(float width)
    {
        foreach(TrackerPolling trackableBehaviour in TrackerTargets) {
            ImageTargetBehaviour imageTarget = trackableBehaviour.transform.GetComponent<ImageTargetBehaviour>();
            //No point trying to rescale if it's already scaled !
            if (imageTarget.transform.localScale.x == width && imageTarget.transform.localScale.z == width)
            {
                return;
            }
            //Retreiving the track
            ObjectTracker tracker = Vuforia.TrackerManager.Instance.GetTracker<ObjectTracker>();
            //foolproofing !
            if (tracker == null)
            {
                Debug.LogWarning("You might be using a virtual camera (Vuforia Play). The scaling of the trackable " + imageTarget.name + " will only be virtual wich doesn't unsure your function will work in actual runtime");
            }
            else
            {
                //Retreiving all datasets
                IEnumerable<DataSet> sets = tracker.GetActiveDataSets();
                bool foundAndReplaced = false;
                //browsing through the datasets
                using (var setsEnum = sets.GetEnumerator())
                {
                    while (!foundAndReplaced && setsEnum.MoveNext())
                    {
                        DataSet ds = setsEnum.Current;
                        //deactivating every datasets, one at a time
                        tracker.DeactivateDataSet(ds);
                        //retreiving all trackables
                        IEnumerable<Trackable> tracks = ds.GetTrackables();
                        //browsing through trackables
                        using (var tracksEnum = tracks.GetEnumerator())
                        {
                            while (!foundAndReplaced && tracksEnum.MoveNext())
                            {
                                Trackable trackable = tracksEnum.Current;
                                //Looking for Image Targets
                                if (trackable is ImageTarget)
                                {
                                    ImageTarget it = trackable as ImageTarget;
                                    //Looking for MY image target that I need to update
                                    if (it.ID == imageTarget.ImageTarget.ID)
                                    {
                                        //Now, Vuforia knows the real size of my target
                                        it.SetSize(new Vector2(width, width));
                                        foundAndReplaced = true;
                                    }
                                }
                            }
                        }
                        //Once I'm done with this dataset, I can reactivate it
                        tracker.ActivateDataSet(ds);
                    }
                }
            }
            //Updates the virtual size of my imagetarget along with his child GameObjects. (Also does the scaling on Vuforia previews !)
            imageTarget.transform.localScale = new Vector3(width, imageTarget.transform.localScale.y, width);
        }
    }
}
