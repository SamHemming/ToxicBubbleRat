using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

	static public void ChangeScene(int num)
	{
		SceneManager.LoadSceneAsync(num);
	}

	static public void Quit()
	{
		Application.Quit();
	}
}
