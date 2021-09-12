using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectCtrl : MonoBehaviour
{
    [SerializeField] private int StageCount;
    private RectTransform stageUi;
    private const float stageUiLength = 800;
    private int StageIndex = 0;

    private void Awake()
    {
        stageUi = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            UiMove(true);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            UiMove(false);
        }
    }

    private void UiMove(bool where)
    {
        StopAllCoroutines();
        StageIndex += where ? (StageIndex + 1 > StageCount - 1? 0 : 1) : (StageIndex - 1 >= 0 ? -1 : 0);

        
        StartCoroutine(MoveStageUi(where));
    }
    private IEnumerator MoveStageUi(bool where) // true : 오른쪽으로 이동   false : 왼쪽으로 이동
    {
        while (true)
        {
            stageUi.localPosition = new Vector3(Mathf.Lerp(stageUi.localPosition.x, -StageIndex * stageUiLength, 0.075f), stageUi.localPosition.y, stageUi.localPosition.z);
            
            if ((where && Mathf.Abs((-StageIndex * stageUiLength)) - Mathf.Abs(stageUi.localPosition.x) < 0.1f) || (!where && Mathf.Abs((-StageIndex * stageUiLength)) - Mathf.Abs(stageUi.localPosition.x) > 0.1f))
            {
                stageUi.localPosition = new Vector3(-StageIndex * stageUiLength, stageUi.localPosition.y, stageUi.localPosition.z);
                break;
            }
            yield return null;
        }
    }
}
