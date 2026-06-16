using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public bool isOpen = false;        
    public float openAngle = 90f;      
    public float openSpeed = 3f;       

    private Quaternion defaultRotation;
    private Quaternion openRotation;

    void Start()
    {
        defaultRotation = transform.rotation;
        openRotation = Quaternion.Euler(defaultRotation.eulerAngles + Vector3.up * openAngle);
    }

    void Update()
    {
        if (isOpen)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * openSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, defaultRotation, Time.deltaTime * openSpeed);
        }
    }

    // Fungsi baru: Akan terpanggil otomatis jika pintu diklik kiri pakai mouse
    void OnMouseDown()
    {
        ToggleDoor();
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen; 
    }
}