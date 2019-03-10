using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{  

    private Rigidbody rigidbody;
    private Vector3 startPos;
    private Vector3 endPos;

    private float startTime;
    private float endTime;

    [SerializeField]
    private float minimumDist = 0.01f;

    [SerializeField]
    private float forwardConstant = 0;

    [SerializeField]
    private float velocityConstant = 3;

    [SerializeField]
    private float maxDistanceFromCamera = 25;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.isKinematic = true;
        transform.parent = Camera.main.transform;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth/2.0f, 0f, Camera.main.nearClipPlane+ 0.01f));
        transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown(){
        startPos = transform.position;
        startTime = Time.time;
    }

    void OnMouseDrag(){
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 0.01f));
    }

    void OnMouseUp(){
        endPos = transform.position;
        endTime = Time.time;
        Vector3 dist = (endPos-startPos) / (endTime - startTime);
        if (dist.magnitude >= minimumDist){
            Vector3 look = Camera.main.transform.forward.normalized;
            Vector3 newVec = look + Vector3.Normalize(dist);
            Vector3 throwDir = newVec * dist.magnitude* velocityConstant;
            transform.parent.DetachChildren();
            rigidbody.isKinematic = false;
            rigidbody.velocity = throwDir;
            Instantiate(gameObject);
        }
    }
}
