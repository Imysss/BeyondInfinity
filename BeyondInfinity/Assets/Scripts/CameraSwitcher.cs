using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    //Camera Pivots and Transform
    [SerializeField] private Transform firstPersonPivot;
    [SerializeField] private Transform thirdPersonPivot;
    [SerializeField] private Transform cameraTransform;

    //Camera State
    private bool isFirstPersonPerspective;

    #region Unity Methods
    private void Start()
    {
        firstPersonPivot = transform.Find("FirstPersonPivot");
        thirdPersonPivot = transform.Find("ThirdPersonPivot");
        cameraTransform = GetComponentInChildren<Camera>().transform;
        isFirstPersonPerspective = true;
        ChangeView();
    }
    #endregion

    #region View Switching Logic
    public void SwitchView()
    {
        isFirstPersonPerspective = !isFirstPersonPerspective;
        ChangeView();
    }

    private void ChangeView()
    {
        Transform targetPivot = isFirstPersonPerspective ? firstPersonPivot : thirdPersonPivot;
        cameraTransform.position = targetPivot.position;
    }
    #endregion
}
