namespace Spire.Gamemodes.Duel;

public enum DuelGameState
{
	WaitingForPlayers,
	RoundCountdown,
	RoundActive,
	RoundWinnerDecided,
	GameWinnerDecided
}
public static class DuelGameStateExtensions
{
	public static string GetName( this DuelGameState state )
	{
		return state switch
		{
			DuelGameState.WaitingForPlayers => "Waiting For Players",
			DuelGameState.RoundCountdown => "Round Countdown",
			DuelGameState.RoundActive => "Round Active",
			DuelGameState.RoundWinnerDecided => "Round Winner Decided",
			DuelGameState.GameWinnerDecided => "Game Winner Decided",
			_ => "N/A"
		};
	}
}
