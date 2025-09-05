using UnityEngine;
using UnityEngine.EventSystems;
public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static TouchField Instance;
   // public Vector2 TouchDist;
    //public Vector2 PointerOld;
    //public int PointerId;
    public bool Pressed;
    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    /*void Update()
    {
        if (Pressed)
        {
            if (PointerId >= 0 && PointerId < Input.touches.Length)
            {
                TouchDist = Input.touches[PointerId].position - PointerOld;
                PointerOld = Input.touches[PointerId].position;
            }
            else
            {
                TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                PointerOld = Input.mousePosition;
            }
        }
        else
        {
            TouchDist = new Vector2();
        }
    }
    */
    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        //PointerId = eventData.pointerId;
        //PointerOld = eventData.position;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
    public void MoveNextTutorialStage()
    {
        // TutorialManager.instance.OnCloseTouchFieldTutorial();
    }
    private void OnDisable()
    {
        Pressed = false;
        //TouchDist = Vector2.zero;
    }
}






