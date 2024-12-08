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
		triggerTaskCompletionSource?.SetResult(trigger.gameObject);
		Destroy(gameObject, 1);
	}

	public Task<GameObject> WaitForTrigger()
	{
		triggerTaskCompletionSource = new TaskCompletionSource<GameObject>();
		return triggerTaskCompletionSource.Task;
	}
}
