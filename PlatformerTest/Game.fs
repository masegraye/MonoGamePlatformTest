module MBPlat.Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open MBPlat.Actors

type PlatformerGame () as x =
    inherit Game()
 
    do x.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(x)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
 
    let CreateActor' = CreateActor x.Content
    let WorldObjects = lazy ([("player.png", Player(Nothing), Vector2(10.f, 28.f), Vector2(32.f, 32.f), false);
                              ("obstacle.png", Obstacle, Vector2(10.f, 60.f), Vector2(32.f, 32.f), true);
                              ("", Obstacle, Vector2(42.f, 60.f), Vector2(32.f, 32.f), true);] 
                             |> List.map CreateActor')

    let DrawActor (sb: SpriteBatch) (actor: WorldActor) =
        if actor.Texture.IsSome then
           sb.Draw(actor.Texture.Value, actor.Position, Color.White)
        ()

    override x.Initialize() =
        spriteBatch <- new SpriteBatch(x.GraphicsDevice)    
        base.Initialize()
        ()
 
    override x.LoadContent() =
        do WorldObjects.Force() |> ignore
        ()
 
    override x.Update (gameTime) =
        WorldObjects.Force() |> ignore

        ()
 
    override x.Draw (gameTime) =
        x.GraphicsDevice.Clear Color.CornflowerBlue
        let DrawActor' = DrawActor spriteBatch
        spriteBatch.Begin()
        WorldObjects.Value
        |> List.iter DrawActor'
        spriteBatch.End()
        ()
