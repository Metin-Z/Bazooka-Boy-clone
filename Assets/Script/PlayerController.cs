using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private Camera mainCamera;
    Animator _animator;
    GameObject RPG;
    public GameObject Granade;
    GameObject bombPos;

    Vector3 mousePos;
    public static PlayerController instance;
    public GameObject Aim;

    public int bombCount = -1;

    public virtual void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        //else
        //{
        //    Destroy(gameObject);
        //}
        mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
        GameManager.instance.player = gameObject;
    }

    void Update()
    {
        if (RPG == null)
        {
            RPG = GameObject.FindGameObjectWithTag("RPG");
            bombPos = RPG.transform.GetChild(0).gameObject;
            bombCount = -1;
            CanvasManager canvas = CanvasManager.instance;
            canvas.bombGroup.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            canvas.bombGroup.transform.GetChild(1).GetComponent<Image>().color = Color.white;
            canvas.bombGroup.transform.GetChild(2).GetComponent<Image>().color = Color.white;
        }

        RocketControl();
    }

    public Transform lookAtTarget;
    public float mouseSensivity;
    Vector3 firstMousePos;
    Vector3 lastMousePos;
    public float maxclampValue;
    public float minClampValue;
    public void RocketControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstMousePos = Input.mousePosition;
            Aim.SetActive(true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Aim.SetActive(false);
        }

        if (Input.GetMouseButton(0))
        {
            lastMousePos = Input.mousePosition;
            Vector3 deltaMouse = lastMousePos - firstMousePos;
            Vector3 movementVector = deltaMouse * mouseSensivity;
            float clampY = Mathf.Clamp(lookAtTarget.position.y + movementVector.y, minClampValue, maxclampValue);
            lookAtTarget.transform.position = new Vector3(lookAtTarget.position.x, clampY, transform.position.z);
        }
        if (bombCount < 2)
        {
            if (Input.GetMouseButtonUp(0))
            {
                GameObject BOMB = Instantiate(Granade, bombPos.transform.position, Quaternion.identity);
                BOMB.transform.parent = transform.root;
                BOMB.GetComponent<Rigidbody>().AddForce(RPG.transform.forward * 850);
                Bullet();
            }
        }
    }
    bool dead;
    public void Dead(float power, Vector3 explosionPos, float radius, float upForce)
    {
        if (dead)
            return;

        _animator.enabled = false;

        foreach (Rigidbody item in GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = false;
            item.velocity = Vector3.zero;
            item.angularVelocity = Vector3.zero;
        }

        foreach (Rigidbody item in GetComponentsInChildren<Rigidbody>())
            item.AddExplosionForce(power, explosionPos, radius, upForce);
        dead = true;
        GameManager.instance.player = null;
        GameManager.instance.LevelCheck();
        Destroy(gameObject, 2.5f);
    }
    public void Bullet()
    {
        bombCount++;
        CanvasManager.instance.bombGroup.transform.GetChild(bombCount).GetComponent<Image>().color = Color.black;
    }

    public float headWeight;
    public float bodyWeight;
    private void OnAnimatorIK()
    {
        if (_animator)
        {
            _animator.SetLookAtWeight(headWeight, bodyWeight);
            _animator.SetLookAtPosition(lookAtTarget.position);
        }
    }
}
