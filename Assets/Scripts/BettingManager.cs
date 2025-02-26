using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class BettingManager : MonoBehaviour
{
	public double[] _redCoefficients;
	public double[] _yellowCoefficients;
	public double[] _greenCoefficients;

	public TextMeshProUGUI currentBetText;
	private double _currentBet;

	public Button _launchGreenBallButton;
	public Button _launchRedBallButton;
	public Button _launchYellowBallButton;

	public Button _increaseBetButton;
	public Button _decreaseBetButton;

	public Ball ballPrefab;

	private PiramidManager _piramidManager;
	private PaymentManager _paymentManager;

	public Queue<Ball> ballsPool = new();

	[Inject]
	private void Init(PiramidManager piramidManager, PaymentManager paymentManager)
	{
		_paymentManager = paymentManager;
		_piramidManager = piramidManager;
	}

	private void Start()
	{
		_currentBet = double.Parse(currentBetText.text, CultureInfo.InvariantCulture);
		_piramidManager.CreatePiramid();
		_piramidManager.SetCoefficients(_greenCoefficients, _yellowCoefficients, _redCoefficients);
		_launchGreenBallButton.onClick.RemoveAllListeners();
		_launchGreenBallButton.onClick.AddListener(LaunchGreenBall);
		_launchRedBallButton.onClick.RemoveAllListeners();
		_launchRedBallButton.onClick.AddListener(LaunchRedBall);
		_launchYellowBallButton.onClick.RemoveAllListeners();
		_launchYellowBallButton.onClick.AddListener(LaunchYellowBall);
		_increaseBetButton.onClick.RemoveAllListeners();
		_increaseBetButton.onClick.AddListener(IncreaseBet);
		_decreaseBetButton.onClick.RemoveAllListeners();
		_decreaseBetButton.onClick.AddListener(DecreaseBet);
	}

	private void IncreaseBet()
	{
		_currentBet++;
		currentBetText.text = _currentBet.ToString();
	}

	private void DecreaseBet()
	{
		if (_currentBet <= 1)
		{
			return;
		}
		_currentBet--;
		currentBetText.text = _currentBet.ToString();
	}

	public void LaunchGreenBall()
	{
		LaunchBallAsync(Color.green, _greenCoefficients);

	}
	public void LaunchRedBall()
	{
		LaunchBallAsync(Color.red, _redCoefficients);
	}
	public void LaunchYellowBall()
	{
		LaunchBallAsync(Color.yellow, _yellowCoefficients);
	}


	private async void LaunchBallAsync(Color color, double[] coefs)
	{
		double currentBet = _currentBet;
		if (_paymentManager.TakeFromBalance(currentBet))
		{

			//var ball = Instantiate(ballPrefab);
			var ball = GetBall();
			ball._sprite.material.color = color;

			_piramidManager.SetPositionAndParentForBall(ball);
			var t = await ball.WaitForTrigger();
			StartCoroutine( ReleaseBall(ball));
			double coefficient = coefs[int.Parse(t.name)];
			_paymentManager.ClaimReward(coefficient * currentBet);
		}

	}

	private Ball GetBall()
	{	if(ballsPool.Count == 0)
		{
			return Instantiate(ballPrefab);
		}

		var ball = ballsPool.Dequeue();
		ball.gameObject.SetActive(true);
		ball.transform.localPosition = Vector3.zero;

		return ball;
	}

	private IEnumerator ReleaseBall(Ball ball)
	{
		yield return new WaitForSeconds(1f);
		ball.gameObject.SetActive(false);
		ball._body.velocity = Vector3.zero;
		ballsPool.Enqueue(ball);
	}

}
