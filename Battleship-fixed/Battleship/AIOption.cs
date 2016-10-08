using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
// using System.Data;
using System.Diagnostics;

public enum AIOption
{
	/// <summary>
	/// Easy, total random shooting
	/// </summary>
	Easy,

	/// <summary>
	/// Medium, marks squares around hits
	/// </summary>
	Medium,

	/// <summary>
	/// As medium, but removes shots once it misses
	/// </summary>
	Hard
}
