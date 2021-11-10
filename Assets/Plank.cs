using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class Plank : MonoBehaviour
{
    public static Plank instance;
    public GameObject plank;
    public GameObject curr_plank;
    public GameObject pre_plank;
    public GameObject ExPlank;
    private int blockcount; // blockcountt
    private int movingSomething;
    private float distancez;
    private float distancex;
    private float exDistanceZ;
    private float exDistanceX;
    public bool end;
    private Vector3 NewMoveDir;
    private int score;
    private float exScalez;
    private float exScalex;
    private float exLocationz;
    private float exLocationx;
    public GameObject mainCam;
    public Vector3 target;
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;
    public int moveDirect;

    public float someVar;


    void Start()
    {
        blockcount = 2;
        movingSomething = 1;
        score = 0;
    }

    public void Awake()
    {      
        end = true;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MoveCamera()
    {
        mainCam.transform.position = Vector3.SmoothDamp(mainCam.transform.position, target, ref velocity, smoothTime);
    }
    void Update()
    {
        if (end == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                target.y++;               
                if (Mathf.Abs(curr_plank.transform.position.z - pre_plank.transform.position.z) > pre_plank.transform.localScale.z || Mathf.Abs(curr_plank.transform.position.x - pre_plank.transform.position.x) > pre_plank.transform.localScale.x)
                {
                    end = true;
                    UIManager.instance.EndGame();
                    SaveRecord();
                    Destroy();
                }
                else
                {
                    IncreaseScore();
                }
                PlankColor();
                NewPlankx();
                ChopPlank();

                SpawnPlank();               

            }
            MoveBlock();
            MoveCamera();

        }
        
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MoveBlock()
    {
        if (curr_plank.transform.position.z >= 6 && curr_plank.transform.position.x == pre_plank.transform.position.x)
        {
            movingSomething = -1;
            NewMoveDir = new Vector3(0, 0, 0);
            moveDirect = 1;
        }
        if (curr_plank.transform.position.z <= -6 && curr_plank.transform.position.x == pre_plank.transform.position.x)
        {
            movingSomething = 1;
            NewMoveDir = new Vector3(0, 0, 0);
            moveDirect = 1;
        }
        if (curr_plank.transform.position.x <= -6 && curr_plank.transform.position.z == pre_plank.transform.position.z)
        {
            movingSomething = 1;
            NewMoveDir = new Vector3(6, 0, -6);
            moveDirect = 0;
        }
        if (curr_plank.transform.position.x >= 6 && curr_plank.transform.position.z == pre_plank.transform.position.z)
        {
            movingSomething = -1;
            NewMoveDir = new Vector3(6, 0, -6);
            moveDirect = 0;
        }
        curr_plank.transform.Translate((new Vector3(0, 0, 6) + NewMoveDir) * movingSomething * Time.deltaTime);
        //ExPlank.transform.Translate(new Vector3(0, -10, 0) * Time.deltaTime);
    }
    public void Destroy()
    {
        Destroy(ExPlank,5);
    }
    public void SaveRecord()
    {
        UIManager.instance.HighestScore(score);
    }
    public void IncreaseScore()
    {
        score++;
        UIManager.instance.ChangeScore(score);
    }
    public void SpawnPlank()
    {
        pre_plank = curr_plank;
        Vector3 vector1 = new Vector3(6, blockcount, pre_plank.transform.localPosition.z);
        Vector3 vector2 = new Vector3(pre_plank.transform.localPosition.x, blockcount, 6);
        Vector3 vector3 = new Vector3(pre_plank.transform.localPosition.x, blockcount, -6);
        Vector3 vector4 = new Vector3(-6, blockcount, pre_plank.transform.localPosition.z);
        Vector3[] vector = new Vector3[] { vector1,vector2,vector3 ,vector4 };
        int randomValvector = Random.Range(0, vector.Length);
        Vector3 vectors = vector[randomValvector];
        curr_plank.GetComponent<BoxCollider>().isTrigger = false;
        curr_plank = Instantiate(plank, new Vector3(vectors.x, blockcount++, vectors.z), Quaternion.identity);
        curr_plank.transform.localScale = pre_plank.transform.localScale;
    }
        
    public void PlankColor()
    {
        curr_plank.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    public void ChopPlank()
    {
        if (Mathf.Abs(curr_plank.transform.position.z - pre_plank.transform.position.z) == 0 && Mathf.Abs(curr_plank.transform.position.x - pre_plank.transform.position.x) == 0 )
        {
            return;
        }
        if (moveDirect == 1)
        {
            if (curr_plank.transform.position.z < pre_plank.transform.position.z)
            {
                exScalez = Mathf.Abs(pre_plank.transform.localScale.z - distancez);
                exLocationz = (pre_plank.transform.position.z + exDistanceZ) - distancez / 2 - distancez / 2;
            }
            if (curr_plank.transform.position.z > pre_plank.transform.position.z)
            {
                exScalez = Mathf.Abs(pre_plank.transform.localScale.z - distancez);
                exLocationz = (pre_plank.transform.position.z + exDistanceZ) + distancez / 2 + distancez / 2;
            }
            exLocationx = pre_plank.transform.position.x;
            exScalex = pre_plank.transform.localScale.x;
        }
        if (moveDirect == 0)
        {
            if (curr_plank.transform.position.x > pre_plank.transform.position.x)
            {
                exLocationx = (curr_plank.transform.position.x + exDistanceX) + distancex / 2 + distancex / 2; ;
                exScalex = Mathf.Abs(pre_plank.transform.localScale.x - distancex);
            }
            if (curr_plank.transform.position.x < pre_plank.transform.position.x)
            {
                exLocationx = (curr_plank.transform.position.x + exDistanceX) - distancex / 2 - distancex / 2; ;
                exScalex = Mathf.Abs(pre_plank.transform.localScale.x - distancex);
            }
            exLocationz = pre_plank.transform.position.z;
            exScalez = pre_plank.transform.localScale.z;
        }
        ExPlank = Instantiate(plank, new Vector3(exLocationx, blockcount - 1, exLocationz), Quaternion.identity);
        ExPlank.transform.localScale = new Vector3(exScalex, ExPlank.transform.localScale.y, exScalez);
        ExPlank.GetComponent<Rigidbody>().useGravity = true;
        ExPlank.GetComponent<Rigidbody>().isKinematic = false;
        ExPlank.GetComponent<BoxCollider>().isTrigger = false;
        GameObject tempPlank = ExPlank;
        StartCoroutine(DelayDestroyPlank(tempPlank));   
    }
    public void NewPlankx()
    {
        distancez = Mathf.Abs(pre_plank.transform.localScale.z - (Mathf.Abs(curr_plank.transform.position.z - pre_plank.transform.position.z) < someVar ? 0 : Mathf.Abs(curr_plank.transform.position.z - pre_plank.transform.position.z)));
        distancex = Mathf.Abs(pre_plank.transform.localScale.x - (Mathf.Abs(curr_plank.transform.position.x - pre_plank.transform.position.x) < someVar ? 0 : Mathf.Abs(curr_plank.transform.position.x - pre_plank.transform.position.x)));
        if (curr_plank.transform.position.z <= pre_plank.transform.position.z)
        {
            exDistanceZ = distancez / 2 - pre_plank.transform.localScale.z / 2;
        }
        if (curr_plank.transform.position.z > pre_plank.transform.position.z)
        {
            exDistanceZ = pre_plank.transform.localScale.z / 2 - distancez / 2;        
        }
        if (curr_plank.transform.position.x <= pre_plank.transform.position.x)
        {
            exDistanceX = distancex / 2 - pre_plank.transform.localScale.x / 2;
        }
        if (curr_plank.transform.position.x > pre_plank.transform.position.x)
        {
            exDistanceX = pre_plank.transform.localScale.x / 2 - distancex / 2;
        }

        curr_plank.transform.position = new Vector3(pre_plank.transform.position.x + exDistanceX, curr_plank.transform.position.y, pre_plank.transform.position.z + exDistanceZ);
        curr_plank.transform.localScale = new Vector3(distancex, curr_plank.transform.localScale.y, distancez);
    }
    IEnumerator DelayDestroyPlank(GameObject plank)
    {
        yield return new WaitForSeconds(3);
        Destroy(plank);
    }

}
