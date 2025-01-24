using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Transform _buttonsParent;

    private void Start()
    {
        for (int i = 0; i < (int)SceneName.LastInstance; i++)
        {
            SceneChangeButton button = SceneChangeButtonPool.Instance.GetSceneChangeButton();

            button.SetType((SceneName)i);
            button.transform.SetParent(_buttonsParent);
            button.transform.localScale = Vector3.one;
            button.transform.localPosition = Vector3.zero;
            button.Init();
            button.AddOnClickListener(ChangeScene);
        }
    }

    private void Update()
    {
        OnTabKey();
    }

    private void OnTabKey()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            canvas.gameObject.SetActive(!canvas.gameObject.activeInHierarchy);
        }
    }

    private void ChangeScene(SceneName scene)
    {
        canvas.gameObject.SetActive(false);
        StartCoroutine(LoadScene((int)scene));
    }

    IEnumerator LoadScene(int sceneNum)
    {
        yield return null;
        //GameScene 비동기식으로 부르기
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNum);

        //AsyncOperation 함수 중 allowSceneActivation은 장면 준비 즉시 활성화 허용 여부 true = 허용, false = 비허용
        //false시 progress가 0.9f에서 멈춤
        operation.allowSceneActivation = true;

        //AsyncOperation 함수 중 isDone은 해당 동작이 준비되었는지의 여부를 bool형으로 반환
        float time = 0f;
        while (!operation.isDone)
        {
            yield return null;
            time += Time.deltaTime;

            //AsyncOperation 함수 중 progress는 작업의 진행 정도를 0 ~ 1 값으로 반환
            if (operation.progress < 0.9f)
            {
                if (time >= operation.progress)
                {
                    time = 0f;
                }
            }
            else
            {
                time = 1f;
                if (time == 1f)
                {
                    //씬 전환이 이루어지도록 하고 반환없이 코루틴 종료시킴
                    operation.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}

