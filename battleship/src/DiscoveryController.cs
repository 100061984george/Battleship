
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data;
using System.Diagnostics;
using SwinGameSDK;

/// <summary>
/// The battle phase is handled by the DiscoveryController.
/// </summary>
static class DiscoveryController
{
    private static bool flag_mute = true;
	/// <summary>
	/// Handles input during the discovery phase of the game.
	/// </summary>
	/// <remarks>
	/// Escape opens the game menu. Clicking the mouse will
	/// attack a location.
	/// </remarks>
	public static void HandleDiscoveryInput()
	{
		if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE)) {
			GameController.AddNewState(GameState.ViewingGameMenu);
		}

        if (SwinGame.KeyTyped(KeyCode.vk_m))
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            if (flag_mute == true)
            {
                Audio.StopMusic();
                player.Stop();
                flag_mute = false;
            }
        }
        if (SwinGame.KeyTyped(KeyCode.vk_a))
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            Audio.StopMusic();
            player.Stop();
            SwinGame.PlayMusic(GameResources.GameMusic("Background"));
            flag_mute = true;
        }

        if (SwinGame.KeyTyped(KeyCode.vk_b))
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            Audio.StopMusic();
            player.Stop();
            player.SoundLocation = "musicB.wav";
            player.Play();
            flag_mute = true;
        }

        if (SwinGame.KeyTyped(KeyCode.vk_c))
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            Audio.StopMusic();
            player.Stop();
            player.SoundLocation = "musicC.wav";
            player.Play();
            flag_mute = true;
        }

		if (SwinGame.MouseClicked(MouseButton.LeftButton)) {
			DoAttack();
		}

		if (SwinGame.MouseClicked(MouseButton.LeftButton) & UtilityFunctions.IsMouseInRectangle(200, 330, 51, 46)) {
			GameController.StartGame();
		}
	}

	/// <summary>
	/// Attack the location that the mouse if over.
	/// </summary>
	private static void DoAttack()
	{
		Point2D mouse = default(Point2D);

		mouse = SwinGame.MousePosition();

		//Calculate the row/col clicked
		int row = 0;
		int col = 0;
		row = Convert.ToInt32(Math.Floor((mouse.Y - UtilityFunctions.FIELD_TOP) / (UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP)));
		col = Convert.ToInt32(Math.Floor((mouse.X - UtilityFunctions.FIELD_LEFT) / (UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP)));

		if (row >= 0 & row < GameController.HumanPlayer.EnemyGrid.Height) {
			if (col >= 0 & col < GameController.HumanPlayer.EnemyGrid.Width) {
				GameController.Attack(row, col);
			}
		}
	}

	/// <summary>
	/// Draws the game during the attack phase.
	/// </summary>s
	public static void DrawDiscovery()
	{
		const int SCORES_LEFT = 172;
		const int SHOTS_TOP = 157;
		const int HITS_TOP = 206;
		const int SPLASH_TOP = 256;

		if ((SwinGame.KeyDown(KeyCode.vk_LSHIFT) | SwinGame.KeyDown(KeyCode.vk_RSHIFT)) & SwinGame.KeyDown(KeyCode.vk_c)) {
			UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, true);
		} else {
			UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, false);
		}

		UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
		UtilityFunctions.DrawMessage();

		SwinGame.DrawText(GameController.HumanPlayer.Shots.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SHOTS_TOP);
		SwinGame.DrawText(GameController.HumanPlayer.Hits.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, HITS_TOP);
		SwinGame.DrawText(GameController.HumanPlayer.Missed.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SPLASH_TOP);
		SwinGame.DrawText("RESET", Color.Snow, GameResources.GameFont("Menu"), 200, 330);
		SwinGame.DrawText ("Highest Score: " + HighScoreController.Highscore , Color.White, GameResources.GameFont ("Menu"), 630, 100);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
