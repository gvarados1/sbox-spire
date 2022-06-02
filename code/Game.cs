global using Sandbox;
global using Sandbox.UI;
global using System;
global using System.Linq;
global using System.Threading.Tasks;
global using System.Collections.Generic;

// Spire
global using Spire.ExtensionMethods;
global using Spire.UI;

namespace Spire;

public partial class Game : Sandbox.Game
{
	public static new Game Current => Sandbox.Game.Current as Game;

	public SpireHud Hud { get; set; }

	public Game()
	{
		if ( Host.IsClient )
			Hud = new();
	}

	protected void SetupDefaultPawn( Client cl )
	{
		cl.Pawn?.Delete();

		var pawn = new PlayerCharacter( cl );
		cl.Pawn = pawn;

		pawn.Respawn();
		pawn.Position = FindSpawnPoint( pawn );
	}

	protected Vector3 FindSpawnPoint( PlayerCharacter pawn )
	{
		return All.OfType<SpawnPoint>().FirstOrDefault()?.Position ?? Vector3.Zero;
	}

	public override void ClientJoined( Client cl )
	{
		Log.Info( $"{cl.Name} has joined the world" );

		SetupDefaultPawn( cl );
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
}
