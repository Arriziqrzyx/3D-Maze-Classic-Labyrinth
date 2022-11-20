using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] GameObject arrow;
    [SerializeField] LayerMask ballLayer;
    [SerializeField] TMP_Text ShootCountText;
    [SerializeField] Image aim;
    [SerializeField] LineRenderer line;
    [SerializeField] LayerMask RayLayer;
    [SerializeField] Camera cam;
    [SerializeField] FollowBall cameraPivot;
    [SerializeField] Vector2 camSensitivity;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float shootForce;
    Vector3 lastMousePosition;
    float ballDistance;
    bool isShooting;
    Vector3 forceDir;
    float forceFactor;

    Renderer[] arrowRends;
    Color[] arrowOriginalColors;

    int shootCount = 0;

    public int ShootCount { get => shootCount; }

    private void Start()
    {
        ballDistance = Vector3.Distance(cam.transform.position, ball.Position) + 1;
        arrowRends = arrow.GetComponentsInChildren<Renderer>();
        arrowOriginalColors = new Color[arrowRends.Length];
        for (int i = 0; i < arrowRends.Length; i++)
        {
            arrowOriginalColors[i] = arrowRends[i].material.color;
        }
        arrow.SetActive(false);
        ShootCountText.text = "Tembakan Ke : " + shootCount;

        arrow.SetActive(false);
    }

    void Update()
    {
        if (ball.IsMoving || ball.IsTeleporting)
            return;

        if (this.transform.position != ball.Position)
        {
            this.transform.position = ball.Position;
            aim.gameObject.SetActive(true);
            var rect = aim.GetComponent<RectTransform>();
            rect.anchoredPosition = cam.WorldToScreenPoint(ball.Position);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, ballDistance, ballLayer))
            {
                isShooting = true;
                arrow.SetActive(true);
                line.enabled = true;
            }
        }

        // Shooting Mode
        if (Input.GetMouseButton(0) && isShooting == true)
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit Hit;

            if (Physics.Raycast(ray, out Hit, ballDistance * 2, RayLayer))
            {
                Debug.DrawLine(ball.Position, Hit.point);
                Debug.Log(Hit.point);

                var forceVector = ball.Position - Hit.point;
                forceVector = new Vector3(forceVector.x, 0, forceVector.z);
                forceDir = forceVector.normalized;
                var forceMagnitude = forceVector.magnitude;
                Debug.Log(forceMagnitude);
                forceMagnitude = Mathf.Clamp(forceMagnitude, 0, 5);
                forceFactor = forceMagnitude / 5;
            }

            // Arrow
            this.transform.LookAt(this.transform.position + forceDir);
            arrow.transform.localScale = new Vector3(1 + 0.5f * forceFactor, 1 + 0.5f * forceFactor, 1 + 2 * forceFactor);

            for (int i = 0; i < arrowRends.Length; i++)
            {
                arrowRends[i].material.color = Color.Lerp(arrowOriginalColors[i], Color.red, forceFactor);
            }

            // Aim
            var rect = aim.GetComponent<RectTransform>();
            rect.anchoredPosition = Input.mousePosition;

            //line
            var ballScreenPos = cam.WorldToScreenPoint(ball.Position);
            line.SetPositions(new Vector3[]{
                ballScreenPos,
                Input.mousePosition
            });
        }

        //camera mode
        if (Input.GetMouseButton(0) && isShooting == false)
        {
            var Current = cam.ScreenToViewportPoint(Input.mousePosition);
            var last = cam.ScreenToViewportPoint(lastMousePosition);
            var delta = Current - last;

            //horizontal
            cameraPivot.transform.RotateAround(ball.Position, Vector3.up, delta.x * camSensitivity.x);
            //vertical
            cameraPivot.transform.RotateAround(ball.Position, cam.transform.right, -delta.y * camSensitivity.y);

            var angle = Vector3.SignedAngle(Vector3.up, cam.transform.up, cam.transform.right);

            // kalau melewati batas putar balik
            if (angle < 3)
                cameraPivot.transform.RotateAround(ball.Position, cam.transform.right, 3 - angle);

            else if (angle > 65)
                cameraPivot.transform.RotateAround(ball.Position, cam.transform.right, 65 - angle);
        }

        if (Input.GetMouseButtonUp(0) && isShooting)
        {
            ball.AddForce(forceDir * shootForce * forceFactor);
            shootCount += 1;
            ShootCountText.text = "Tembakan Ke : " + shootCount;
            forceFactor = 0;
            forceDir = Vector3.zero;
            isShooting = false;
            arrow.SetActive(false);
            aim.gameObject.SetActive(false);
            line.enabled = false;
            audioSource.PlayOneShot(audioSource.clip);
        }

        lastMousePosition = Input.mousePosition;
    }
}
