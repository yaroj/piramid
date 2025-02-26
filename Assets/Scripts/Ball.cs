using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Ball : MonoBehaviour
{
	private TaskCompletionSource<GameObject> _triggerTaskCompletionSource;
	public SpriteRenderer _sprite;
	public Rigidbody2D _body;
	private void OnTriggerEnter2D(Collider2D trigger)
	{
		_triggerTaskCompletionSource?.SetResult(trigger.gameObject);
	}

	public Task<GameObject> WaitForTrigger()
	{
		_triggerTaskCompletionSource = new TaskCompletionSource<GameObject>();
		return _triggerTaskCompletionSource.Task;
	}
}
