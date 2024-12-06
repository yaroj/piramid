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
	public Canvas _canvas;

	//public async Task<GameObject> LaunchBallAsync(Ball ball)
	//{
	//	var ballT = ball.transform;
	//	ballT.parent = transform;
	//	var xOfBall = Random.Range(-0.5f * widthBetweeencircles, 0.5f * widthBetweeencircles);
	//	ballT.localPosition = new Vector3(xOfBall, 2 * heightBetweeenRows, 0);
	//	return await WaitForBallTrigger(ballT);
	//}

	//private async Task<GameObject> WaitForBallTrigger(Transform ball)
	//{
	//	return await ball.GetComponent<Ball>().WaitForTrigger();
	//}


	public void SetPositionANdParentForBall(Ball ball)
	{
		var ballT = ball.transform;
		ballT.parent = transform;
		var xOfBall = Random.Range(-0.5f * widthBetweeencircles, 0.5f * widthBetweeencircles);
		ballT.localPosition = new Vector3(xOfBall, 2 * heightBetweeenRows, 0);
	}

	public void CreatePiramid()
	{
		for (int i = 3; i <= rowCount; i++)
		{
			CreateRow(i * heightBetweeenRows, i);
		}
		CreateTriggers(rowCount - 1, transform.position.y + (rowCount) * heightBetweeenRows);


	}

	public void SetCoefficients(double[] greenCoefficients, double[] yellowCoefficients, double[] redCoefficients)
	{
		PlaceCoefficientsBelowPegs(transform.position.y + (rowCount + 1) * heightBetweeenRows, rowCount - 1, Color.green, greenCoefficients);
		PlaceCoefficientsBelowPegs(transform.position.y + (rowCount + 2) * heightBetweeenRows, rowCount - 1, Color.yellow, yellowCoefficients);
		PlaceCoefficientsBelowPegs(transform.position.y + (rowCount + 3) * heightBetweeenRows, rowCount - 1, Color.red, redCoefficients);
	}

	private void CreateTriggers(int count, float height)
	{
		for (int i = 0; i < count; i++)
		{
			var g = new GameObject();

			g.transform.position = GetPostitonForPeg(i, count, height);
			g.name = i.ToString();
			var coll = g.AddComponent<BoxCollider2D>();
			coll.isTrigger = true;
			coll.size = new Vector2(0.01f, 0.01f);
		}
	}

	private Vector3 GetPostitonForPeg(float pegNumber, float pegsInRow, float height)
	{
		return new Vector3((pegNumber - (pegsInRow - 1) / 2) * widthBetweeencircles, height, 0);
	}

	private void CreateRow(float height, int countOfCirclesInRow)
	{
		for (int i = 0; i < countOfCirclesInRow; i++)
		{
			var t = Instantiate(pegPrefab, transform);
			t.localPosition = GetPostitonForPeg(i, countOfCirclesInRow, height);
		}
	}


	private void PlaceCoefficientsBelowPegs(float height, int countOfCirclesInRow, Color color, double[] coefficients)
	{
		for (int i = 0; i < countOfCirclesInRow; i++)
		{
			var im = Instantiate(coefficientPrefab, _canvas.transform);
			im.color = color;
			var x = im.transform.GetComponentInChildren<TextMeshProUGUI>();
			x.text = coefficients[i].ToString();
			im.transform.localPosition = GetPostitonForPeg(i, countOfCirclesInRow, height);
		}


	}

}