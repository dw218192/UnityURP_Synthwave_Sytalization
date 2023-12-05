using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
    public static DemoManager Instance { get; private set; }
    [SerializeField] float speed = 1.0f;
    public event System.Action<float> OnSpeedChanged;
    public float Speed
    {
        get => speed;
        set
        {
            speed = value;
            OnSpeedChanged?.Invoke(speed);
        }
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
