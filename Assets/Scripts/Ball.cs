using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, IClickable
{
    public AudioSource winningSound;
    public AudioSource ballRolling;
    public GameObject IngameCanvas;
    public GameObject WinCanvas;
    Rigidbody _rigidBody;
    
    Vector3 _startPosition;
    Vector3 direction;
    Vector3 _currentPosition;
    
    
    bool isOnGround;
    bool isNotBlocked = true;
    public bool isOnWhiteBox;

    private Vector3 mOffset;

    private float mZCoord;

    void Awake() 
    {
        _rigidBody = GetComponent<Rigidbody>();
        winningSound = GetComponent<AudioSource>();
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
        
        isNotBlocked = true;
        
        while (isNotBlocked && isOnWhiteBox == false) 
        {
            if (Physics.Raycast(_currentPosition, direction, 2f))
            {
                Debug.Log("Ball hit obstacle");
                isNotBlocked = false;
                break;
            }
            else
            {
                isNotBlocked = true;
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
                    transform.Rotate(direction*30f, Space.World);
                    yield return new WaitForSeconds(0.08f);
                    hit.distance -= 2;
                }
                
            }
        }
        
        Debug.Log(direction);
        
    }

    void SlideBall(Vector3 direction)
    {
        //movable = true;
        transform.position = transform.position + direction * 2;
        transform.Rotate(direction*30f, Space.World);
        ballRolling.Play();
    }

    void GoDown()
    {
        //Vector3 y_direction = new Vector3 (0f, -1f, 0f);
        transform.position = transform.position - Vector3.up * 2;
        transform.Rotate(-Vector3.up*45f, Space.World);
    }

    bool IsOnGround()
    {
        RaycastHit hit;
        Ray downRay = new Ray(_currentPosition, -Vector3.up);

        if (Physics.Raycast(downRay, out hit))
        {
    
            if (hit.distance > 2f)
            {
               return false;
            }
        }
        return true;
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
        // if (col.tag == "FinishBox")
        // {
        //     //GetComponent<AudioSource>().Play();
        //     gameObject.SetActive(false);
        // }
        if (col.tag == "WhiteBox")
        {
            Debug.Log("Ball Collide with whitebox");
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

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "FinishBox")
        {
            //Debug.Log(col.gameObject.tag);
            //_rigidBody.velocity = Vector3.zero;
            //_rigidBody.angularVelocity = Vector3.zero; 
            _rigidBody.isKinematic = true;

            Vector3 collideDir = (col.GetContact(0).point - transform.position).normalized;
            //Debug.Log(collideDir);
            if (direction == collideDir)
            {
                Debug.Log("hit");
                GameStateManager.Instance.winRound = true;
                winningSound.Play();
                transform.position = transform.position + direction * 2;
                IngameCanvas.SetActive(false);
                WinCanvas.SetActive(true);
                
                //gameObject.SetActive(false);
            }
            else
            {
                _rigidBody.isKinematic = false;
            }
        }
        
    }

    void OnCollisionExit(Collision col)
    {
        
    }

    
    void Update()
    {
        if (transform.position.y < 0) 
        {
            transform.position = new Vector3 (transform.position.x, 0, transform.position.z);
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
