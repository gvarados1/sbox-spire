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
	public IList<int> TeamScores { get; set; }

	public int GetTeamScore( DuelTeam team )
	{
		return TeamScores[(int)team];
	}

	public void SetupScores()
	{
		TeamScores = null;

		foreach ( var team in Enum.GetValues( typeof( DuelTeam ) ) )
			TeamScores.Add( 0 );
	}

	public void IncrementScore( DuelTeam team )
	{
		TeamScores[(int)team]++;
	}

	public override void Spawn()
	{
		base.Spawn();

		SetupScores();
	}

	public float TeamOneAliveCount => DuelTeam.Blue.GetMembers().Select( x => (x.Pawn is BaseCharacter character) && character.IsValid() && character.LifeState == LifeState.Alive ).Count();
	public float TeamTwoAliveCount => DuelTeam.Red.GetMembers().Select( x => (x.Pawn is BaseCharacter character) && character.IsValid() && character.LifeState == LifeState.Alive ).Count();

	[Net, Predicted]
	public TimeUntil TimeUntilRoundStart { get; set; }
	[Net, Predicted]
	public TimeUntil TimeUntilRoundEnd { get; set; }

	[Net, Predicted]
	public TimeUntil TimeUntilRoundRestart { get; set; }

	protected void AddToSuitableTeam( Client cl )
	{
		var lowest = DuelTeamHelpers.GetLowestTeam();
		cl.SetTeam( lowest );

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

		SetupScores();
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
			IncrementScore( DuelTeam.Blue );
			ChatPanel.Announce( $"{DuelTeam.Blue.GetName()} wins the round!", ChatCategory.System );
		}
		else if ( teamTwoCount > teamOneCount )
		{
			IncrementScore( DuelTeam.Red );
			ChatPanel.Announce( $"{DuelTeam.Red.GetName()} wins the round!", ChatCategory.System );
		}
		else
		{
			ChatPanel.Announce( $"Stalemate!", ChatCategory.System );
		}

		CurrentState = DuelGameState.RoundWinnerDecided;
		TimeUntilRoundRestart = RoundStartCountdownTime;
	}

	public string GetGameStateText()
	{
		return CurrentState switch
		{
			DuelGameState.WaitingForPlayers => "Waiting",
			DuelGameState.RoundCountdown => TimeSpan.FromSeconds( TimeUntilRoundStart ).ToString( @"mm\:ss" ),
			DuelGameState.RoundActive => TimeSpan.FromSeconds( TimeUntilRoundEnd ).ToString( @"mm\:ss" ),
			DuelGameState.RoundWinnerDecided => "Round Over",
			DuelGameState.GameWinnerDecided => "Game Over",
			_ => ""
		};
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

	public override Transform? GetSpawn( BaseCharacter character )
	{
		var teamName = character.Client.GetTeam().ToString().ToLower();

		return All.OfType<SpawnPoint>()
			.Where( x => x.Tags.Has( teamName ) )
			.OrderBy( x => Guid.NewGuid() )
			.FirstOrDefault()?.Transform ?? null;
	}

	public override bool AllowMovement()
	{
		return CurrentState != DuelGameState.RoundCountdown;
	}
}
