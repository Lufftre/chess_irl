using UnityEngine;
using Vuforia;

public class TrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PRIVATE_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;

    #endregion // PRIVATE_MEMBER_VARIABLES

	public GameObject OnLost;
	public GameObject OnFound;

    #region UNTIY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS

    protected virtual void OnTrackingFound()
    {
        // var rendererComponents = GetComponentsInChildren<Renderer>(true);
        // var colliderComponents = GetComponentsInChildren<Collider>(true);
        // var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // // Enable rendering:
        // foreach (var component in rendererComponents)
        //     component.enabled = true;

        // // Enable colliders:
        // foreach (var component in colliderComponents)
        //     component.enabled = true;

        // // // Enable canvas':
        // // foreach (var component in canvasComponents)
        // //     component.enabled = true;
		// MarkerCanvas.enabled = true;
		// LostCanvas.enabled = false;
		OnFound.SetActive(true);
		OnLost.SetActive(false);

    }


    protected virtual void OnTrackingLost()
    {
        // var rendererComponents = GetComponentsInChildren<Renderer>(true);
        // var colliderComponents = GetComponentsInChildren<Collider>(true);
        // var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // // Disable rendering:
        // foreach (var component in rendererComponents)
        //     component.enabled = false;

        // // Disable colliders:
        // foreach (var component in colliderComponents)
        //     component.enabled = false;

        // // Disable canvas':
        // // foreach (var component in canvasComponents)
        // //     component.enabled = false;
		// MarkerCanvas.enabled = false;
		// LostCanvas.enabled = true;

		OnFound.SetActive(false);
		OnLost.SetActive(true);

		Camera.main.transform.position = new Vector3(4f, 15f, -5f);
		Camera.main.transform.rotation = Quaternion.Euler(60f, 0, 0);
    }

    #endregion // PRIVATE_METHODS
}
