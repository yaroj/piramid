using UnityEngine;
using Zenject;

public class MyInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		Container.Bind<PiramidManager>().FromComponentInHierarchy().AsSingle();
		Container.Bind<PaymentManager>().FromComponentInHierarchy().AsSingle();
	}
}
