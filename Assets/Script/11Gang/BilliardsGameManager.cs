using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BilliardsGameManager : MonoBehaviour
{
    public PlayerBall p1;
    public PlayerBall p2;
    public LayerMask ground;
    public Rigidbody[] allBallsRb;
    public BilliardBall[] allBalls;

    [Header("TEXT")]
    public TMP_Animator red;
    public TMP_Animator blue;
    public TMP_Animator info;

    [Header("Pos")]
    public Vector3[] WatchPos;

    bool isP1Turn;
    bool canControl = false;
    int redScore = 3;
    int blueScore = 3;

    LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.enabled = false;
    }

    void Start()
    {
        red.SetText(string.Empty);
        blue.SetText(string.Empty);
        info.SetText(string.Empty);
        StartCoroutine(MainGame());
        CameraMovement.Instance.AddManager(this);
    }

    IEnumerator MainGame()
    {
        float textSize = info.GetSize();
        info.SetColor(Color.gold);
        info.SetText("잠시 후 게임 시작");
        yield return new WaitForSeconds(1.5f);
        isP1Turn = Random.value > 0.5f;
        red.SetText("RED : " + redScore, true);
        blue.SetText("BLUE : " + blueScore, true);
        string startText = "첫 번째 턴";
        info.SetText(startText, true, 0.01f);
        yield return new WaitForSeconds(0.01f * startText.Length + 0.5f);

        info.SetSize(textSize * 1.2f);
        info.SetColor(Color.black);

        if (isP1Turn)
        {
            info.SetColor(Color.red, 0.3f);
            info.SetText("RED");
        }
        else
        {
            info.SetColor(Color.blue, 0.3f);
            info.SetText("BLUE");
        }
        info.SetSize(textSize, 0.3f);

        yield return new WaitForSeconds(0.75f);

        info.SetColor(Color.clear, 0.15f);

        while (redScore != 5 && blueScore != 5)
        {
            CameraMoveToBallTop(0.3f);
            yield return new WaitForSeconds(0.3f);

            yield return WaitForClick(isP1Turn ? p1.transform : p2.transform);
            CameraMovement.Instance.RotateBy(Vector3.zero);

            Vector3 dir = GetMouseRaycastPosition(Vector3.zero) - (isP1Turn ? p1.transform.position : p2.transform.position);
            float power = Mathf.Clamp(dir.magnitude * 3f, 3f, 25.0f);
            dir = dir.normalized * power;

            bool scoreChanged = false;
            bool hasIncreased = true;

            p1.SaveCurrentPos();
            p2.SaveCurrentPos();

            foreach (BilliardBall ball in allBalls)
            {
                ball.SaveCurrentPos();
            }

            CameraMovement.Instance.GoTo(GetFarthestPositionFrom(CameraMovement.Instance.transform.position), 1f);
            Coroutine tranckCamera = StartCoroutine(CameraTrackBall(isP1Turn ? p1.transform : p2.transform));
            yield return new WaitForSeconds(1.25f);
            lr.enabled = false;

            if (isP1Turn)
            {
                p1.Launch(dir);
                yield return WaitAndStopAllBalls();
                StopCoroutine(tranckCamera);

                if (p2.WasMove())
                {
                    if (redScore != 0)
                    {
                        redScore--;
                        scoreChanged = true;
                        hasIncreased = false;
                    }
                }
                else if (AllBallsMove())
                {
                    redScore++;
                    scoreChanged = true;
                }
            }
            else
            {
                p2.Launch(dir);
                yield return WaitAndStopAllBalls();
                StopCoroutine(tranckCamera);

                if (p1.WasMove())
                {
                    if (blueScore != 0)
                    {
                        blueScore--;
                        scoreChanged = true;
                        hasIncreased = false;
                    }
                }
                else if (AllBallsMove())
                {
                    blueScore++;
                    scoreChanged = true;
                }
            }

            if (scoreChanged)
            {
                if (isP1Turn)
                {
                    info.SetColor(Color.red);
                    info.SetText("RED 점수 :", true);
                    yield return new WaitForSeconds(0.75f);
                    info.AddText(" " + redScore);
                    if (hasIncreased) info.SetSize(textSize * 1.2f);
                    else info.SetSize(textSize * 0.5f);
                    info.SetColor(Color.black);
                    info.SetSize(textSize, 0.4f);
                    info.SetColor(Color.red, 0.4f);
                    red.SetText("RED : " + redScore);
                }
                else
                {
                    info.SetColor(Color.blue);
                    info.SetText("BLUE 점수 :", true);
                    yield return new WaitForSeconds(0.75f);
                    info.AddText(" " + blueScore);
                    if (hasIncreased) info.SetSize(textSize * 1.2f);
                    else info.SetSize(textSize * 0.5f);
                    info.SetColor(Color.black);
                    info.SetSize(textSize, 0.4f);
                    info.SetColor(Color.blue, 0.4f);
                    blue.SetText("BLUE : " + blueScore);
                }

                yield return new WaitForSeconds(1f);
                info.SetColor(Color.clear, 0.25f);
                yield return new WaitForSeconds(0.1f);
            }

            isP1Turn = !isP1Turn;
        }

        info.SetColor(Color.white);
        info.SetText("승리 : ", true);

        yield return new WaitForSeconds(1f);

        info.SetSize(textSize * 1.2f);

        if (blueScore == 5)
        {
            info.AddText("BLUE");
            info.SetColor(Color.black);
            info.SetColor(Color.blue, 0.4f);
        }
        else
        {
            info.AddText("RED");
            info.SetColor(Color.black);
            info.SetColor(Color.red, 0.4f);
        }

        info.SetSize(textSize, 0.4f);
    }


    void CameraMoveToBallTop(float time)
    {
        Vector3 ballPos = isP1Turn ? p1.transform.position : p2.transform.position;
        Vector3 ballUp = ballPos + Vector3.up * 23;

        Vector3 lookDirection = Vector3.down;
        Vector3 targetUpDirection = Vector3.zero - new Vector3(ballPos.x, 0f, ballPos.z);

        if (targetUpDirection == Vector3.zero) targetUpDirection = Vector3.forward;
        else targetUpDirection.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection, targetUpDirection);

        CameraMovement.Instance.GoTo(ballUp, time);
        CameraMovement.Instance.RotateToPos(targetRotation, time);
    }

    public void OnMove(InputValue value)
    {
        if (!canControl) return;

        Vector3 move = value.Get<Vector2>();
        move.y = move.x;
        move.x = move.z = 0;
        CameraMovement.Instance.RotateBy(move);
    }

    public void OnClick(InputValue value)
    {
        if (canControl && value.isPressed) canControl = false;
    }

    IEnumerator WaitForClick(Transform target)
    {
        canControl = lr.enabled = true;

        while (canControl)
        {
            lr.SetPosition(0, target.position);
            lr.SetPosition(1, GetMouseRaycastPosition(target.position));
            SetLRColor(target.position);

            yield return null;
        }
    }

    void SetLRColor(Vector3 target)
    {
        Vector3 dir = GetMouseRaycastPosition(Vector3.zero) - target;
        float powerLevel = Mathf.Clamp(dir.magnitude * 3f, 3f, 25.0f) - 3f;

        Color col = Color.Lerp(Color.black, Color.red, powerLevel / 22.0f);
        lr.startColor = col;
        lr.endColor = col;
    }

    public Vector3 GetMouseRaycastPosition(Vector3 defaultPos)
    {
        if (Camera.main == null) return Vector3.zero;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
        {
            return hit.point;
        }

        return defaultPos;
    }

    IEnumerator WaitAndStopAllBalls()
    {
        yield return new WaitForSeconds(0.3f);
        bool isMoving = true;

        while (isMoving)
        {
            isMoving = false;

            foreach (Rigidbody ball in allBallsRb)
            {
                if (ball == null) continue;

                if (ball.linearVelocity.magnitude >= 0.1f)
                {
                    isMoving = true;
                    break;
                }
            }

            if (isMoving)
            {
                yield return null;
            }
        }

        foreach (Rigidbody ball in allBallsRb)
        {
            if (ball == null) continue;

            ball.linearVelocity = Vector3.zero;
            ball.angularVelocity = Vector3.zero;
        }
    }

    IEnumerator CameraTrackBall(Transform target)
    {
        while (true)
        {
            CameraMovement.Instance.Look(target.position);
            yield return null;
        }
    }

    bool AllBallsMove()
    {
        foreach (BilliardBall ball in allBalls)
        {
            if (!ball.WasMove())
            {
                return false;
            }
        }

        return true;
    }


    public Vector3 GetFarthestPositionFrom(Vector3 referencePosition)
    {
        if (WatchPos == null || WatchPos.Length == 0)
        {
            return Vector3.zero;
        }

        Vector3 farthestPos = WatchPos[0];
        float maxSqrDistance = (WatchPos[0] - referencePosition).sqrMagnitude;

        for (int i = 1; i < WatchPos.Length; i++)
        {
            float sqrDistance = (WatchPos[i] - referencePosition).sqrMagnitude;
            if (sqrDistance > maxSqrDistance)
            {
                maxSqrDistance = sqrDistance;
                farthestPos = WatchPos[i];
            }
        }

        return farthestPos;
    }
}
