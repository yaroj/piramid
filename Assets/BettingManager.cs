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

	[Inject]
	private void Init(PiramidManager piramidManager, PaymentManager paymentManager)
	{
		print(piramidManager);
		_paymentManager = paymentManager;
		_piramidManager = piramidManager;
	}

	private void Start()
	{
		print(currentBetText.text);
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
		if (_currentBet <= 1) {
			return;
		}
		_currentBet--;
		currentBetText.text = _currentBet.ToString();
	}

	public async void LaunchGreenBall()
	{
		var coefficientSource = _greenCoefficients;
		double currentBet = _currentBet;
		if (_paymentManager.TakeFromBalance(currentBet))
		{
			print("asdfg");
			print(_piramidManager);
			var ball = Instantiate(ballPrefab);
			var t = await _piramidManager.LaunchBallAsync(ball);
			double coefficient = coefficientSource[int.Parse(t.name)];
			_paymentManager.ClaimReward(coefficient * currentBet);
			print(coefficient);
			print(currentBet);
		}

	}


	private async Task<GameObject> WaitForBallTrigger(Transform ball)
	{
		// Create a simple trigger detector
		// Wait for the ball to hit the trigger and return the GameObject
		return await ball.GetComponent<Ball>().WaitForTrigger();
	}
	public async void LaunchRedBall()
	{
		var ball = Instantiate(ballPrefab);
		var t = await _piramidManager.LaunchBallAsync(ball);

	}
	public async void LaunchYellowBall()
	{
		var ball = Instantiate(ballPrefab);
		var t = await _piramidManager.LaunchBallAsync(ball);

	}

	public void DisableUI()
	{
		_launchGreenBallButton.enabled = false;
		_launchRedBallButton.enabled = false;
		_launchYellowBallButton.enabled = false;

		_increaseBetButton.enabled = false;
		_decreaseBetButton.enabled = false;
	}


	public void EnableUI()
	{
		_launchGreenBallButton.enabled = true;
		_launchRedBallButton.enabled = true;
		_launchYellowBallButton.enabled = true;

		_increaseBetButton.enabled = true;
		_decreaseBetButton.enabled = true;

	}





}
