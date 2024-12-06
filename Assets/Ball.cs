using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Ball : MonoBehaviour
{
	private TaskCompletionSource<GameObject> triggerTaskCompletionSource;
	public SpriteRenderer sprite;
	private void OnTriggerEnter2D(Collider2D trigger)
	{
		print("triggered");
		triggerTaskCompletionSource?.SetResult(trigger.gameObject);
	}

	public Task<GameObject> WaitForTrigger()
	{
		triggerTaskCompletionSource = new TaskCompletionSource<GameObject>();
		return triggerTaskCompletionSource.Task;
	}
}
