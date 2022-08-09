using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LauncherMy : Photon.Pun.Demo.PunBasics.Launcher
{
	[Tooltip("The Ui Panel to let the user enter name, connect and play")]
	[SerializeField]
	private GameObject controlPanel;

	[Tooltip("The Ui Text to inform the user about the connection progress")]
	[SerializeField]
	private Text feedbackText;

	[Tooltip("The maximum number of players per room")]
	[SerializeField]
	private byte maxPlayersPerRoom = 4;

	[Tooltip("The UI Loader Anime")]
	[SerializeField]
	private LoaderAnime loaderAnime;

	[SerializeField]
    private PhysicsScene _nameScene;

	void LogFeedback(string message)
	{
		// we do not assume there is a feedbackText defined.
		if (feedbackText == null)
		{
			return;
		}

		// add new messages as a new line and at the bottom of the log.
		feedbackText.text += System.Environment.NewLine + message;
	}

	public override void OnJoinedRoom()
	{
		LogFeedback("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
		Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

		// #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
		if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
		{
			Debug.Log("We load the 'Room for 1' ");

			// #Critical
			// Load the Room Level. _nameScene
			PhotonNetwork.LoadLevel("PunBasics-Room  my");

		}
	}
}
