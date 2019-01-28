using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLogic : MonoBehaviour
{
    private Material mat;
    public Material GameOverMat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        mat.color = Color.clear;
    }

    public void ShowGoal()
    {
        StartCoroutine(AppearAndDisappear());
    }

    private IEnumerator AppearAndDisappear()
    {
        yield return new WaitForSeconds(2.0f);

        float T = 0.0f;
        while (T < 2.0f)
        {
            mat.color = new Color(1.0f, 1.0f, 1.0f, T / 2.0f);
            yield return null;
            T += Time.deltaTime;
        }

        mat.color = Color.white;
        yield return new WaitForSeconds(3.0f);

        T = 2.0f;
        while (T > 0.0f)
        {
            mat.color = new Color(1.0f, 1.0f, 1.0f, T / 2.0f);
            yield return null;
            T -= Time.deltaTime;
        }

        mat.color = Color.clear;
    }

    public void GameOver()
    {
        StartCoroutine(game_over());
    }


    private IEnumerator game_over()
    {
        gameObject.GetComponent<MeshRenderer>().material = GameOverMat;

        mat = gameObject.GetComponent<MeshRenderer>().material;
        mat.color = Color.clear;

        float T = 0.0f;
        while (T < 2.0f)
        {
            mat.color = new Color(1.0f, 1.0f, 1.0f, T / 2.0f);
            yield return null;
            T += Time.deltaTime;
        }

        mat.color = Color.white;
    }

}
