using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PiramidManager : MonoBehaviour
{
	// Start is called before the first frame update
	public float heightBetweeenRows;
	public float widthBetweeencircles;
	public int rowCount;
	public Transform pegPrefab;
	public Transform ballPrefab;
	public Image coefficientPrefab;
	public TextMeshProUGUI coefficientTextPrefab;
	public TextMeshPro coefficientTextPref;
	public Text _textPref;
	public Canvas _canvas;
	private Vector3 horizontalOffset;

	const int FINISHTRIGGERLAYER = 8;


	public async Task<GameObject> LaunchBallAsync(Ball ball)
	{
		var ballT = ball.transform;
		ballT.parent = transform;
		var xOfBall = Random.Range(-0.5f * widthBetweeencircles, 0.5f * widthBetweeencircles);
		ballT.localPosition = new Vector3(xOfBall, 2 * heightBetweeenRows, 0);
		// Wait for the ball to enter a trigger and return the triggered GameObject
		return await WaitForBallTrigger(ballT);
	}

	private async Task<GameObject> WaitForBallTrigger(Transform ball)
	{
		// Create a simple trigger detector
		// Wait for the ball to hit the trigger and return the GameObject
		return await ball.GetComponent<Ball>().WaitForTrigger();
	}

	//public void LaunchBall(Color color)
	//{
	//	var ball = Instantiate(ballPrefab, transform);
	//	var xOfBall = Random.Range(-0.5f * widthBetweeencircles, 0.5f * widthBetweeencircles);
	//	ball.localPosition = new Vector3(xOfBall, 2 * heightBetweeenRows, 0);

	//}



	public void CreatePiramid()
	{
		horizontalOffset = new Vector3(widthBetweeencircles, 0, 0);
		for (int i = 3; i <= rowCount; i++)
		{
			CreateRow(i * heightBetweeenRows, i);
		}
		CreateTriggers(rowCount - 1, transform.position.y + (rowCount) * heightBetweeenRows);


	}

	public void SetCoefficients(double[] greenCoefficients, double[] yellowCoefficients, double[] redCoefficients)
	{
		PlaceTextBetweenPegs(transform.position.y + (rowCount + 1) * heightBetweeenRows, rowCount - 1, Color.green, greenCoefficients);
		PlaceTextBetweenPegs(transform.position.y + (rowCount + 2) * heightBetweeenRows, rowCount - 1, Color.yellow, yellowCoefficients);
		PlaceTextBetweenPegs(transform.position.y + (rowCount + 3) * heightBetweeenRows, rowCount - 1, Color.red, redCoefficients);
	}

	private void CreateTriggers(int count, float height)
	{
		for (int i = 0; i < count; i++)
		{
			var g = new GameObject();

			g.transform.position = getPostitonForPeg(i, count, height);
			g.name = i.ToString();
			var coll = g.AddComponent<BoxCollider2D>();
			coll.isTrigger = true;
			coll.size = new Vector2(0.01f, 0.01f);
		}
	}

	private Vector3 getPostitonForPeg(float pegNumber, float pegsInRow, float height)
	{
		return new Vector3((pegNumber - (pegsInRow - 1) / 2) * widthBetweeencircles, height, 0);
	}

	private void CreateRow(float height, int countOfCirclesInRow)
	{
		for (int i = 0; i < countOfCirclesInRow; i++)
		{
			var t = Instantiate(pegPrefab, transform);
			t.localPosition = getPostitonForPeg(i, countOfCirclesInRow, height);
		}
	}


	private void PlaceTextBetweenPegs(float height, int countOfCirclesInRow, Color color, double[] coefficients)
	{
		for (int i = 0; i < countOfCirclesInRow; i++)
		{
			var im = Instantiate(coefficientPrefab, _canvas.transform);
			im.color = color;
			var x = im.transform.GetComponentInChildren<TextMeshProUGUI>();
			x.text = coefficients[i].ToString();
			im.transform.localPosition = getPostitonForPeg(i, countOfCirclesInRow, height);
		}


	}
	private void SetCoeficients()
	{

	}


	// Update is called once per frame
	void Update()
	{
		//if (Input.GetKeyDown(KeyCode.G))
		//{
		//	LaunchBall(Color.green);
		//}
		//if (Input.GetKeyDown(KeyCode.R)) { LaunchBall(Color.red); }
		//if (Input.GetKeyDown(KeyCode.Y))
		//{
		//	LaunchBall(Color.yellow);
		//}
	}
}
public class BallDropper : MonoBehaviour
{
	public GameObject ballPrefab;
	public Transform dropPoint;
	public float dropForce = 10f;
	public float cooldownTime = 2f;
	private float nextDropTime = 0f;

	void Update()
	{
		if (Time.time > nextDropTime && Input.GetKeyDown(KeyCode.Space))
		{
			DropBall();
			nextDropTime = Time.time + cooldownTime;
		}
	}

	void DropBall()
	{
		GameObject ball = Instantiate(ballPrefab, dropPoint.position, Quaternion.identity);
		Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(Random.Range(-2f, 2f), -dropForce);
	}
}