using UnityEngine;

//Purpose: A system created to track puck placement on the plunk - o game and award funds to the player.
//Created: 4/15/2021 by Benjamin Wagley

public class puck_prize_controller : MonoBehaviour
{

	public GameObject moneyManager;
	public float prizeAmount;
	public Interactor interactor;
	public Plinko plinkoScript;
	private Player player;

	private void Start()
	{
		player = GameObject.FindObjectOfType<Player>();
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "puck")
		{
			winPrize(prizeAmount);
		}
	}

	public void winPrize(float winAmount)
	{
		//send the winamount over to the money manager and tell the player the amount they won
		moneyManager.GetComponent<money_tracking_system>().addFunds(winAmount);
		plinkoScript.prize = prizeAmount;
		plinkoScript.puckReached = true;
	}
}
