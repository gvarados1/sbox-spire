global using Sandbox;
global using Sandbox.UI;
global using System;
global using System.Linq;
global using System.Threading.Tasks;
global using System.Collections.Generic;

// Spire
global using Spire.ExtensionMethods;
global using Spire.UI;

using Spire.Gamemodes;

namespace Spire;

public partial class Game : Sandbox.Game
{
	public static new Game Current => Sandbox.Game.Current as Game;

	public DayNight.DayNightSystem DayNightSystem { get; protected set; }

	public SpireHud Hud { get; set; }

	public Game()
	{
		if ( Host.IsClient )
			Hud = new();
		else
		{
			DayNightSystem = new();
		}

		Global.TickRate = 20;
	}

	protected void SetupDefaultPawn( Client cl )
	{
		cl.Pawn?.Delete();

		var pawn = new PlayerCharacter( cl );
		cl.Pawn = pawn;

		pawn.Respawn();
		pawn.Transform = FindSpawnPoint( pawn ).WithScale( 1f );
	}

	protected Transform FindSpawnPoint( BaseCharacter character )
	{
		return BaseGamemode.Current?.GetSpawn( character ) ?? All.OfType<SpawnPoint>().FirstOrDefault()?.Transform ?? Transform.Zero;
	}

	public override void ClientJoined( Client cl )
	{
		Log.Info( $"{cl.Name} has joined the world" );

		BaseGamemode.Current?.OnClientJoined( cl );

		SetupDefaultPawn( cl );
	}

	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		base.ClientDisconnect( cl, reason );

		BaseGamemode.Current?.OnClientLeft( cl, reason );
	}

	[ConCmd.Server( "spire_respawn" )]
	public static void ForceRespawn()
	{
		var cl = ConsoleSystem.Caller;

		Current.SetupDefaultPawn( cl );
	}

	private async void RespawnPlayerDelayed( Client cl, float seconds = 3f )
	{
		await GameTask.DelaySeconds( seconds );

		SetupDefaultPawn( cl );
	}

	public void RespawnPlayer( Client cl )
	{
		if ( !cl.IsValid() )
			return;

		RespawnPlayerDelayed( cl );
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		// Simulate active gamemode
		BaseGamemode.Current?.Simulate( cl );
	}

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );

		// Simulate active gamemode
		BaseGamemode.Current?.FrameSimulate( cl );
	}
}
