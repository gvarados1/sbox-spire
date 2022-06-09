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

	// Team One
	[Net]
	public IList<Client> TeamOne { get; set; }
	[Net]
	public float TeamOneScore { get; set; }
	public float TeamOneAliveCount => TeamOne.Select( x => (x.Pawn is BaseCharacter character) && character.IsValid() && character.LifeState == LifeState.Alive ).Count();

	// Team Two
	[Net]
	public IList<Client> TeamTwo { get; set; }
	[Net]
	public float TeamTwoScore { get; set; }
	public float TeamTwoAliveCount => TeamTwo.Select( x => (x.Pawn is BaseCharacter character) && character.IsValid() && character.LifeState == LifeState.Alive ).Count();

	[Net, Predicted]
	public TimeUntil TimeUntilRoundStart { get; set; }
	[Net, Predicted]
	public TimeUntil TimeUntilRoundEnd { get; set; }

	[Net, Predicted]
	public TimeUntil TimeUntilRoundRestart { get; set; }

	protected void AddToSuitableTeam( Client cl )
	{
		bool teamTwo = TeamOne.Count < TeamTwo.Count;

		if ( teamTwo )
			TeamTwo.Add( cl );
		else
			TeamOne.Add( cl );

		if ( PlayerCount >= MinPlayers && CurrentState == DuelGameState.WaitingForPlayers )
		{
			TryStartCountdown();
		}
	}

	public override void OnClientJoined( Client cl )
	{
		base.OnClientJoined( cl );

		AddToSuitableTeam( cl );
	}

	protected void ResetGameMode()
	{
		ChatPanel.Announce( $"Waiting for players since there are not enough people on.", ChatCategory.System );

		TeamOneScore = 0;
		TeamTwoScore = 0;
		CurrentState = DuelGameState.WaitingForPlayers;
	}

	protected void TryStartCountdown()
	{
		ChatPanel.Announce( $"The round will start in {RoundStartCountdownTime} seconds.", ChatCategory.System );

		CurrentState = DuelGameState.RoundCountdown;
		TimeUntilRoundStart = RoundStartCountdownTime;
	}

	protected void BeginRound()
	{
		ChatPanel.Announce( $"The fight begins.", ChatCategory.System );

		CurrentState = DuelGameState.RoundActive;
		TimeUntilRoundEnd = RoundLength;
	}

	protected void DecideRoundWinner()
	{
		var teamOneCount = TeamOneAliveCount;
		var teamTwoCount = TeamTwoAliveCount;

		if ( teamOneCount > teamTwoCount )
		{
			ChatPanel.Announce( $"Team One wins the round!", ChatCategory.System );
		}
		else if ( teamTwoCount > teamOneCount )
		{
			ChatPanel.Announce( $"Team Two wins the round!", ChatCategory.System );
		}
		else
		{
			ChatPanel.Announce( $"Stalemate!", ChatCategory.System );
		}

		CurrentState = DuelGameState.RoundWinnerDecided;
		TimeUntilRoundRestart = RoundStartCountdownTime;
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( CurrentState == DuelGameState.RoundCountdown )
		{

			if ( TimeUntilRoundStart <= 0 )
			{
				BeginRound();
			}
		}
		//
		else if ( CurrentState == DuelGameState.RoundActive )
		{
			if ( TimeUntilRoundEnd <= 0 )
			{
				DecideRoundWinner();
			}
		}
		//
		else if ( CurrentState == DuelGameState.RoundWinnerDecided )
		{
			if ( TimeUntilRoundRestart <= 0 )
			{
				if ( PlayerCount >= MinPlayers )
				{
					TryStartCountdown();
				}
				else
				{
					ResetGameMode();
				}
			}
		}
	}

	public override void OnCharacterKilled( BaseCharacter character, DamageInfo damageInfo )
	{
		base.OnCharacterKilled( character, damageInfo );

		if ( CurrentState == DuelGameState.RoundActive )
		{
			bool anyIsZero = TeamOneAliveCount == 0 || TeamTwoAliveCount == 0;
			if ( !anyIsZero )
				return;

			DecideRoundWinner();
		}
	}
}
