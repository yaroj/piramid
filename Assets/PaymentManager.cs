using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaymentManager : MonoBehaviour
{
	[SerializeField] Button _fillBalanceButton;
	[SerializeField] TextMeshProUGUI balanceText;
	private double _money = 3000;

	private void Start()
	{
		_fillBalanceButton.onClick.RemoveAllListeners();
		_fillBalanceButton.onClick.AddListener(FillBalance);
	}

	public void ClaimReward(double reward)
	{
		_money += reward;
		UpdateBalanceUI();
	}

	public bool TakeFromBalance(double amount)
	{
		if (_money < amount)
		{
			return false;
		}
		_money -= amount;
		UpdateBalanceUI();
		return true;
	}

	void FillBalance()
	{
		_money++;
		UpdateBalanceUI();
	}

	void UpdateBalanceUI()
	{
		balanceText.text = _money.ToString() + " USD";
	}

}
