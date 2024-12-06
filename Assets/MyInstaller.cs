using UnityEngine;
using Zenject;

public class MyInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		var piramidManagerInScene = FindObjectOfType<PiramidManager>();
		if (piramidManagerInScene == null)
		{
			Debug.LogError("PiramidManager not found in the scene!");
		}
		else
		{
			Debug.Log("PiramidManager found in the scene.");
		}
		Container.Bind<PiramidManager>().FromComponentInHierarchy().AsSingle();
		Container.Bind<PaymentManager>().FromComponentInHierarchy().AsSingle();
		Container.Bind<BettingManager>().FromComponentInHierarchy().AsSingle();

	}
}
