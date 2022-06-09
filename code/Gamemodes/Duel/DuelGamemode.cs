using SandboxEditor;

namespace Spire.Gamemodes.Duel;

[HammerEntity]
[Title( "Duel" ), Category( "Spire - Game Modes" ), Icon( "sports_kabaddi" )]
public partial class DuelGamemode : BaseGamemode
{
	public override Panel CreateHud() => new DuelHudPanel();

	[ConVar.Server( "spire_duel_minplayers" )]
	public static int MinPlayers { get; set; } = 2;

	[ConVar.Server( "spire_duel_round_start_countdown" )]
	public static int RoundStartCountdownTime = 5;

	[ConVar.Server( "spire_duel_round_length" )]
	public static int RoundLength = 120;

	[Net]
	public DuelGameState CurrentState { get; set; } = DuelGameState.WaitingForPlayers;

	[Net]
	public IList<Client> TeamOne { get; set; }
	[Net]
	public float TeamOneScore { get; set; }

	[Net]
	public IList<Client> TeamTwo { get; set; }
	[Net]
	public float TeamTwoScore { get; set; }

	[Net, Predicted]
	public TimeUntil TimeUntilRoundStart { get; set; }
	[Net, Predicted]
	public TimeUntil TimeUntilRoundEnd { get; set; }

	protected void AddToSuitableTeam( Client cl )
	{
		bool teamTwo = TeamOne.Count < TeamTwo.Count;

		if ( teamTwo )
			TeamTwo.Add( cl );
		else
			TeamOne.Add( cl );

		if ( PlayerCount > MinPlayers && CurrentState == DuelGameState.WaitingForPlayers )
		{
			TryStartCountdown();
		}
	}

	public override void OnClientJoined( Client cl )
	{
		base.OnClientJoined( cl );

		AddToSuitableTeam( cl );
	}

	protected void TryStartCountdown()
	{
		if ( CurrentState == DuelGameState.WaitingForPlayers )
		{
			CurrentState = DuelGameState.RoundCountdown;
			TimeUntilRoundStart = RoundStartCountdownTime;
		}
	}

	protected void BeginRound()
	{
		TimeUntilRoundEnd = RoundLength;
	}
}
