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

	protected void RespawnPlayer( Client cl )
	{
		cl.Pawn?.Delete();

		var pawn = new PlayerCharacter( cl );
		cl.Pawn = pawn;
		pawn.Respawn();
	}

	public override void ClientJoined( Client cl )
	{
		Log.Info( $"{cl.Name} has joined the world" );

		RespawnPlayer( cl );

		BaseGamemode.Current?.OnClientJoined( cl );
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

		Current.RespawnPlayer( cl );
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
