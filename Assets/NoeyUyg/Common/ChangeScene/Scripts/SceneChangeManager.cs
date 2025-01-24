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
        //GameScene �񵿱������ �θ���
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNum);

        //AsyncOperation �Լ� �� allowSceneActivation�� ��� �غ� ��� Ȱ��ȭ ��� ���� true = ���, false = �����
        //false�� progress�� 0.9f���� ����
        operation.allowSceneActivation = true;

        //AsyncOperation �Լ� �� isDone�� �ش� ������ �غ�Ǿ������� ���θ� bool������ ��ȯ
        float time = 0f;
        while (!operation.isDone)
        {
            yield return null;
            time += Time.deltaTime;

            //AsyncOperation �Լ� �� progress�� �۾��� ���� ������ 0 ~ 1 ������ ��ȯ
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
                    //�� ��ȯ�� �̷�������� �ϰ� ��ȯ���� �ڷ�ƾ �����Ŵ
                    operation.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}

