using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartonBox : MonoBehaviour, IClickable
{
    Rigidbody _rigidBody;
    public AudioSource slidingSound;
    Vector3 _startPosition;
    Vector3 direction;
    Vector3 _currentPosition;
    
    //[SerializeField] float _launchForce = 100;

    //Vector3 z_positive = new Vector3 (0,0,1);
    //Vector3 z_negative = new Vector3 (0,0,-1);

    bool isGrounded;
    bool isNotBlock = true;
    public bool isOnWhiteBox;

    private Vector3 mOffset;

    private float mZCoord;

    void Awake() 
    {
        _rigidBody = GetComponent<Rigidbody>();
        slidingSound = GetComponent<AudioSource>();
        
    }
    void Start()
    {
        _startPosition = transform.position;
    }

    



    void OnMouseDown()

    {

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos

        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

    }



    private Vector3 GetMouseAsWorldPoint()

    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }


    IEnumerator  OnMouseUp()
    {
        direction = (GetMouseAsWorldPoint() + mOffset - transform.position).normalized;
        
        direction = GetXorZDirection(direction);
        
        isNotBlock = true;
        slidingSound.Play();
        while (isNotBlock && isOnWhiteBox == false) 
        {
            if (Physics.Raycast(_currentPosition, direction, 2f))
            {
                Debug.DrawRay(_currentPosition, direction, Color.black, 100);
                Debug.Log("Box hit obstacle");
                isNotBlock = false;
                break;
            }
            else
            {
                isNotBlock = true;
            }
            
            SlideBall(direction);
            yield return new WaitForSeconds(0.08f);

            //isGrounded = IsOnGround();
            
            RaycastHit hit;
            Ray downRay = new Ray(_currentPosition, -Vector3.up);

            if (Physics.Raycast(downRay, out hit))
            {
        
                while (hit.distance > 2f)
                {
                    //Debug.Log(hit.distance);
                    GoDown();
                    yield return new WaitForSeconds(0.08f);
                    hit.distance -= 2;
                }
                
            }
     
        }
        
        //Debug.Log(direction);
        
    }

    void SlideBall(Vector3 direction)
    {
        
        transform.position = transform.position + direction * 2;
        
    }

    void GoDown()
    {
        //Vector3 y_direction = new Vector3 (0f, -1f, 0f);
        transform.position = transform.position - Vector3.up * 2;
        
    }

   

    Vector3 GetXorZDirection(Vector3 direction)
    {
        direction = new Vector3 (Mathf.Round(direction.x), 0, Mathf.Round(direction.z));

        // The ball cannot move diagonally
        if (direction.x != 0f && direction.z != 0f)
        {
            direction = Vector3.zero;
        }
        return direction;
    }

    void OnTriggerStay(Collider col)
    {
        
    }
 
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "WhiteBox")
        {
            
            isOnWhiteBox = false;
        }
        
    }

    void OnTriggerEnter(Collider col)
    {
        
        if (col.tag == "WhiteBox")
        {
            Debug.Log("Box Collide with whitebox");
            isOnWhiteBox = true;
        }
        if (col.tag == "RightBox")
        {
            Debug.Log("Ball collide with right box");
            direction = Vector3.right;
        }
        if (col.tag == "LeftBox")
        {
            Debug.Log("Ball collide with left box");
            direction = -Vector3.right;
        }
        if (col.tag == "BackwardBox")
        {
            Debug.Log("Ball collide with Backward box");
            direction = -Vector3.forward;
        }
    }

    void OnCollisionExit(Collision col)
    {
        
    }

    
    void Update()
    {
        if (transform.position.y < 0 || transform.position.y > 10) 
        {
            Destroy(gameObject);
        }
       
        _currentPosition = transform.position;

        RaycastHit hit;
        Ray downRay = new Ray(_currentPosition, -Vector3.up);

        if (Physics.Raycast(downRay, out hit))
        {
    
            while (hit.distance > 2f)
            {
                //Debug.Log(hit.distance);
                GoDown();
                //yield return new WaitForSeconds(0.08f);
                hit.distance -= 2;
            }
            
        }
        
    }
    public void OnMouseEnterHover()
    {
        HoverOn();
        return;
    }


    public void OnMouseExistHover()
    {
        HoverOff();
        return;
    }

    private void HoverOn()
    {
        GetComponentInChildren<Outline>().OutlineWidth = 4;
    }

    private void HoverOff()
    {
        GetComponentInChildren<Outline>().OutlineWidth = 0;
    }
}
