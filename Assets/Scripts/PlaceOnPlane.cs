using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
/// 
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARSessionOrigin))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }
    
    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    private enum Mode {PlaceCan, ThrowTrash}
    private Mode currMode;

    public GameObject banana;
    public GameObject button;// { get; private set; }

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        currMode = Mode.PlaceCan;
        button.SetActive(false);
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (currMode == Mode.PlaceCan){
            if (m_SessionOrigin.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                Pose hitPose = s_Hits[0].pose;

                if (spawnedObject == null)

                {
                    button.SetActive(true);
                    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                    spawnedObject.transform.position += new Vector3(0, spawnedObject.GetComponent<MeshCollider>().bounds.extents.y,0);
                }
                else
                {
                    spawnedObject.transform.position = hitPose.position;
                    spawnedObject.transform.position += new Vector3(0, spawnedObject.GetComponent<MeshCollider>().bounds.extents.y,0);
                }
            }
        }
    }

    public void SwitchMode(){
        currMode = Mode.ThrowTrash;
        button.SetActive(false);
        Instantiate(banana);
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARSessionOrigin m_SessionOrigin;
}
