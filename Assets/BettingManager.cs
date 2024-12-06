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
		if (_currentBet <= 1)
		{
			return;
		}
		_currentBet--;
		currentBetText.text = _currentBet.ToString();
	}

	public async void LaunchGreenBall()
	{
		LaunchBallAsync(Color.green, _greenCoefficients);

	}
	public async void LaunchRedBall()
	{
		LaunchBallAsync(Color.red, _redCoefficients);
	}
	public async void LaunchYellowBall()
	{
		LaunchBallAsync(Color.yellow, _yellowCoefficients);
	}


	private async void LaunchBallAsync(Color color, double[] coefs)
	{
		double currentBet = _currentBet;
		if (_paymentManager.TakeFromBalance(currentBet))
		{
			var ball = Instantiate(ballPrefab);
			ball.sprite.material.color = color;

			_piramidManager.SetPositionANdParentForBall(ball);
			var t = await ball.WaitForTrigger();
			double coefficient = coefs[int.Parse(t.name)];
			_paymentManager.ClaimReward(coefficient * currentBet);
		}

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
