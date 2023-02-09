using UnityEngine;

public class GrabObjects : MonoBehaviour
{
    [SerializeField] private string[] objectTags;
    [SerializeField] private float maxGrabDistance;
    [SerializeField] private LayerMask acceptLayers = 0;
    [SerializeField] private float forceGrab = 5;
    [SerializeField] private float forceThrow = 100f;
    
    private GameObject _grabObject;
    private Vector2 rigSaveGrabed;
    private Vector3 _rayEndPoint;

    public static bool grabObject;
    private void Update()
    {
        Transform cam = Camera.main.transform;
        _rayEndPoint = transform.position + transform.forward;
        
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(cam.position, cam.forward,out hit,maxGrabDistance,acceptLayers,QueryTriggerInteraction.Ignore))
        {
            foreach (var tag in objectTags)
            {
                if (hit.transform.CompareTag(tag))
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        _grabObject = hit.transform.gameObject;
                    }
                }
            }
            Debug.DrawLine(cam.position, hit.point);
        }
        else
        {
            Debug.DrawLine(cam.position,cam.position + cam.forward * maxGrabDistance);
        }

        if (_grabObject != null)
        {
            if (!_grabObject.GetComponent<Rigidbody>())
            {
                Debug.LogError("Object to grab needs RigidBory");
                return;
            }

            Rigidbody objRig = _grabObject.GetComponent<Rigidbody>();
            Vector3 posGrab = cam.position + cam.forward * maxGrabDistance;
            float dist = Vector3.Distance(_grabObject.transform.position, posGrab);
            float calc = forceGrab * dist * 6 * Time.deltaTime;

            if (rigSaveGrabed == Vector2.zero)
            {
                rigSaveGrabed = new Vector2(objRig.drag, objRig.angularDrag);
            }
            
            objRig.drag = 2.5f;
            objRig.angularDrag = 2.5f;

            objRig.AddForce(-(_grabObject.transform.position - posGrab) * calc, ForceMode.Impulse);

            if (Input.GetMouseButtonDown(0))
            {
                objRig.AddForce((_rayEndPoint - transform.position).normalized * (forceThrow * 10));
                UngrabObject();
            }
            if (Input.GetKeyUp(KeyCode.F) || objRig.velocity.magnitude >= 25 || dist >= 8)
            {
                UngrabObject();
            }
        }
    }

    //method user to ungrab objects
    private void UngrabObject()
    {
        Rigidbody objRig = _grabObject.GetComponent<Rigidbody>();
        objRig.drag = rigSaveGrabed.x;
        objRig.angularDrag = rigSaveGrabed.y;
        rigSaveGrabed = Vector2.zero;

        _grabObject = null;
    }
    
    
    //method used to show distance to grab objects
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Transform cam = Camera.main.transform;
        if(!Physics.Raycast(cam.position, cam.forward, maxGrabDistance))
        {
            Gizmos.DrawLine(cam.position, cam.position + cam.forward * maxGrabDistance);   
        }
    }
}
