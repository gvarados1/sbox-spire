global using Sandbox;
global using Sandbox.UI;

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
	}

	public override void ClientJoined( Client cl )
	{
		Log.Info( $"{cl.Name} has joined the world" );

		SetupDefaultPawn( cl );
	}
}
