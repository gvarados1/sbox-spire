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

	StandardPostProcess _PostProcess;

	public Game()
	{
		if ( Host.IsClient )
		{
			Hud = new();

			_PostProcess = new StandardPostProcess();
			PostProcess.Add( _PostProcess );
		}
		else
		{
			DayNightSystem = new();
		}

		Global.TickRate = 20;
	}

	public void RespawnPlayer( Client cl )
	{
		var gamemode = BaseGamemode.Current;
		if ( gamemode.IsValid() )
		{
			gamemode.CreatePawn( cl );
		}
		else
		{
			cl.Pawn?.Delete();

			var pawn = new PlayerCharacter( cl );
			cl.Pawn = pawn;
			pawn.Respawn();
		}
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

	public void RespawnEveryone()
	{
		Client.All.ToList().ForEach( x => Game.Current.RespawnPlayer( x ) );
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

		PostProcessTick();
	}


	protected void PostProcessTick()
	{
		_PostProcess.Vignette.Enabled = true;
		_PostProcess.Vignette.Intensity = 1.0f;
		_PostProcess.Vignette.Roundness = 1.5f;
		_PostProcess.Vignette.Smoothness = 0.5f;
		_PostProcess.Vignette.Color = Color.Black;

		_PostProcess.FilmGrain.Enabled = true;
		_PostProcess.FilmGrain.Intensity = 0f;
		_PostProcess.FilmGrain.Response = 1;

		BaseGamemode.Current?.PostProcessTick( _PostProcess );
	}
}
