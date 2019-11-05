using BezierSolution;
using UnityEngine;

public class SplineInCode : MonoBehaviour {

    BezierSpline spline;           

    /// <summary>
    /// Колесо, из которого исходит хвост. Используй polySurface140 
    /// </summary>
    public GameObject cubForSledovaniyaPoints;

    /// <summary>
    /// Вектор направления движения
    /// </summary>
    private Vector3 moveVector;

    /// <summary>
    /// Время жизни блока хвоста
    /// </summary>
    private float liveTime = 1f;

    /// <summary>
    /// Скорость байка
    /// </summary>
    private float speedMove = 2f;    

    // Для паузы между установкой точек
    float nextSetTime;
    float setDelay = 0.25f;

    void Update()
    {
        if (Time.time < nextSetTime) return;

        nextSetTime = Time.time + setDelay;
        AddPointForSpline();                   
    }

    
    

    void FixedUpdate()
    {
        
    }

    void Start()
    {
        spline = new GameObject().AddComponent<BezierSpline>();
        spline.Initialize(2);       
        spline[0].localPosition = gameObject.transform.position;
        spline[1].localPosition = gameObject.transform.position;
        //spline[numberOfPointInSpline].handleMode = BezierPoint.HandleMode.Free;  

    }

    /// <summary>
    /// Порядковый номер точки
    /// </summary>
    private int numberOfPointInSpline = 2;

    /// <summary>
    /// Вектор головы змеи
    /// </summary>
    private Vector3 headPos;

    private void AddPointForSpline()
    {
        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * speedMove * Time.deltaTime;
        moveVector.z = Input.GetAxis("Vertical") * speedMove * Time.deltaTime;

        // Хвост не генерится во время стоянки байка

        // Координаты байка        
        Vector3 bikeCirclePos = cubForSledovaniyaPoints.transform.position;

        // Координаты головы хвоста
        

        headPos.x = bikeCirclePos.x - moveVector.x * 2f; // чем меньше - тем ближе друг к другу появляются
        headPos.y = bikeCirclePos.y;
        headPos.z = bikeCirclePos.z - moveVector.z * 2f;

        spline.InsertNewPointAt(numberOfPointInSpline);
        //Задали ей положение
        spline[numberOfPointInSpline].localPosition = headPos;
        spline.AutoConstructSpline();

        // С этим сплайн чего-то ждёт
        //StartCoroutine(waitingForAddPint(numberOfPointInSpline));

        numberOfPointInSpline++;

       
        

        
    }

    /*IEnumerator waitingForAddPint(int numberOfPointInSpline)
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Добавили точку");
        spline.InsertNewPointAt(numberOfPointInSpline);
        //Задали ей положение
        spline[numberOfPointInSpline].localPosition = headPos;

        spline.AutoConstructSpline();

    }

    IEnumerator waitingForAddPintForMethod()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Добавили точку");
        AddPointForSpline();
    }*/


}
