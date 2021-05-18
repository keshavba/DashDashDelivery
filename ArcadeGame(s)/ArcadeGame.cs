/* Author: Jonah Bui
 * Contributor(s): 
 * Created: 02-05-2021
 * Last Modified: 04-11-2021
 */
using System.Collections;
using UnityEngine;
public class ArcadeGame : Interaction
{
    public string gameName = "Arcade Game";
    public float cost = 10f;                // How much it costs to play
    public float prizeAmount = 100f;        // How much they get if they win

    [Header("Point-based System")]
    public float prizePerPoint = 1f;        // How much money they get per points;
    protected int points = 0;

    //-------------------------------------------------------------------------------------------//
    // Interaction Functions
    //-------------------------------------------------------------------------------------------//
    public override void OnInteractionStart(Interactor interactor)
    {
        base.OnInteractionStart(interactor);
        money_tracking_system  mts = FindObjectOfType<money_tracking_system>();
        if (mts != null && mts.currentAmount >= cost)
        {
            mts.ArcadeSubtractCost(cost);
            StartCoroutine(StartArcadeGame());
        }
        else
        { 
            Notification.Instance.PushNotification($"Not enough money to player {gameName}");
            OnInteractionEnd();
        }
    }

    protected override void OnInteractionEnd()
    {
        base.OnInteractionEnd();
    }

    //-------------------------------------------------------------------------------------------//
    // Arcade Game Functions, Subroutines
    //-------------------------------------------------------------------------------------------//
    protected virtual IEnumerator StartArcadeGame()
    {		
		yield return null;
    }
    public virtual string Info()
    {
        return $"Play {this.gameName}?\nCosts {this.cost.ToPrice()}";
    }
}
