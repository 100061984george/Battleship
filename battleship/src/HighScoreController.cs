
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data;
using System.Diagnostics;
using System.IO;
using SwinGameSDK;

/// <summary>
/// Controls displaying and collecting high score data.
/// </summary>
/// <remarks>
/// Data is saved to a file.
/// </remarks>
static class HighScoreController
{
	private const int NAME_WIDTH = 3;

	private const int SCORES_LEFT = 490;

	private static int HighScore;
	/// <summary>
	/// The score structure is used to keep the name and
	/// score of the top players together.
	/// </summary>
	private struct Score : IComparable
	{
		public string Name;

		public int Value;
		/// <summary>
		/// Allows scores to be compared to facilitate sorting
		/// </summary>
		/// <param name="obj">the object to compare to</param>
		/// <returns>a value that indicates the sort order</returns>
		public int CompareTo(object obj)
		{
			if (obj is Score) {
				Score other = (Score)obj;

				return other.Value - this.Value;
			} else {
				return 0;
			}
		}
	}


	private static List<Score> _Scores = new List<Score>();
	/// <summary>
	/// Loads the scores from the highscores text file.
	/// </summary>
	/// <remarks>
	/// The format is
	/// # of scores
	/// NNNSSS
	/// 
	/// Where NNN is the name and SSS is the score
	/// </remarks>
	public static void LoadScores()
	{
		string filename = null;
		filename = SwinGame.PathToResource("highscores.txt");

		StreamReader input = default(StreamReader);
		input = new StreamReader(filename);

		//Read in the # of scores
		int numScores = 0;
		numScores = Convert.ToInt32(input.ReadLine());

		_Scores.Clear();

		int i = 0;

		for (i = 1; i <= numScores; i++) {
			Score s = default(Score);
			string line = null;

			line = input.ReadLine();

			s.Name = line.Substring(0, NAME_WIDTH);
			s.Value = Convert.ToInt32(line.Substring(NAME_WIDTH));
			_Scores.Add(s);
		}
		input.Close();
	}

	/// <summary>
	/// Saves the scores back to the highscores text file.
	/// </summary>
	/// <remarks>
	/// The format is
	/// # of scores
	/// NNNSSS
	/// 
	/// Where NNN is the name and SSS is the score
	/// </remarks>
	private static void SaveScores()
	{
		string filename = null;
		filename = SwinGame.PathToResource("highscores.txt");

		StreamWriter output = default(StreamWriter);
		output = new StreamWriter(filename);

		output.WriteLine(_Scores.Count);

		foreach (Score s in _Scores) {
			output.WriteLine(s.Name + s.Value);
		}

		output.Close();
	}

	/// <summary>
	/// Draws the high scores to the screen.
	/// </summary>
	public static void DrawHighScores()
	{
		const int SCORES_HEADING = 40;
		const int SCORES_TOP = 80;
		const int SCORE_GAP = 30;

		if (_Scores.Count == 0)
			LoadScores();

		SwinGame.DrawText("   High Scores   ", Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_HEADING);

		//For all of the scores
		int i = 0;
		for (i = 0; i <= _Scores.Count - 1; i++) {
			Score s = default(Score);

			s = _Scores[i];

			//for scores 1 - 9 use 01 - 09
			//store the highest score into a variable
			if (i < 9) {
				SwinGame.DrawText(" " + (i + 1) + ":   " + s.Name + "   " + s.Value, Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
				if (i == 0)
				{
					HighScore = s.Value;
				}
			} else {
				SwinGame.DrawText(i + 1 + ":   " + s.Name + "   " + s.Value, Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
			}
		}
	}
	/// <summary>
	/// Gives users some instruction about the gameplay
	/// </summary>
	public static void DrawInstruction(){
		SwinGame.DrawText ("Instruction",Color.AliceBlue,GameResources.GameFont("Courier"),2,30);
		SwinGame.DrawText ("***********************************",Color.AliceBlue,GameResources.GameFont("Courier"),2,40);
		SwinGame.DrawText ("1. Choose the option of the game",Color.AliceBlue,GameResources.GameFont("Courier"),2,100);
		SwinGame.DrawText ("2. Click the play button to start the game",Color.AliceBlue,GameResources.GameFont("Courier"),2,130);
		SwinGame.DrawText ("3. Arrange your ship before start the game",Color.AliceBlue,GameResources.GameFont("Courier"),2,160);
		SwinGame.DrawText ("4. Press the play button at the top of the right to start the game",Color.AliceBlue,GameResources.GameFont("Courier"),2,190);
		SwinGame.DrawText ("5. Press the column until you able defeat the enemy's ships",Color.AliceBlue,GameResources.GameFont("Courier"),2,220);
        SwinGame.DrawText ("6. Press m to mute", Color.AliceBlue, GameResources.GameFont("Courier"), 2, 250);
        SwinGame.DrawText ("7. Press a to play music A", Color.AliceBlue, GameResources.GameFont("Courier"), 2, 280);
        SwinGame.DrawText ("8. Press b to play music B", Color.AliceBlue, GameResources.GameFont("Courier"), 2, 310);
        SwinGame.DrawText ("9. Press c to play music C", Color.AliceBlue, GameResources.GameFont("Courier"), 2, 340);
	}
	/// <summary>
	/// Handles the user input during the top score screen.
	/// </summary>
	/// <remarks></remarks>
	public static void HandleHighScoreInput()
	{
		if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.vk_ESCAPE) || SwinGame.KeyTyped(KeyCode.vk_RETURN)) {
			GameController.EndCurrentState();
		}
	}

	/// <summary>
	/// Read the user's name for their highsSwinGame.
	/// </summary>
	/// <param name="value">the player's sSwinGame.</param>
	/// <remarks>
	/// This verifies if the score is a highsSwinGame.
	/// </remarks>
	public static void ReadHighScore(int value)
	{
		const int ENTRY_TOP = 500;

		if (_Scores.Count == 0)
			LoadScores();

		//is it a high score
		if (value > _Scores[_Scores.Count - 1].Value) {
			Score s = new Score();
			s.Value = value;

			GameController.AddNewState(GameState.ViewingHighScores);

			int x = 0;
			x = SCORES_LEFT + SwinGame.TextWidth(GameResources.GameFont("Courier"), "Name: ");

			SwinGame.StartReadingText(Color.White, NAME_WIDTH+1, GameResources.GameFont("Courier"), x, ENTRY_TOP);

			//Read the text from the user
			while (SwinGame.ReadingText()) {
				SwinGame.ProcessEvents();

				UtilityFunctions.DrawBackground();
				DrawHighScores();
				SwinGame.DrawText("Name: ", Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, ENTRY_TOP);
				SwinGame.RefreshScreen();
			}

			s.Name = SwinGame.TextReadAsASCII();

			if (s.Name.Length < 3) {
				s.Name = s.Name + new string(Convert.ToChar(" "), 3 - s.Name.Length);
			}

			_Scores.RemoveAt(_Scores.Count - 1);
			_Scores.Add(s);
			_Scores.Sort();

			GameController.EndCurrentState();
		}
	}

	public static int Highscore
	{
		get {return HighScore;}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
