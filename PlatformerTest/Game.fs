module MBPlat.Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

open MBPlat.Actors
open MBPlat.Physics
open MBPlat.Input

type PlatformerGame () as x =
    inherit Game()
 
    do x.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(x)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
 
    let CreateActor' = CreateActor x.Content
    let mutable WorldObjects = lazy ([("player.png", Player(Nothing), Vector2(10.f, 28.f), Vector2(32.f, 32.f), false);
                                      ("obstacle.png", Obstacle, Vector2(10.f, 60.f), Vector2(32.f, 32.f), true);
                                      ("obstacle.png", Obstacle, Vector2(42.f, 60.f), Vector2(32.f, 32.f), true);
                                      ("obstacle.png", Obstacle, Vector2(74.f, 60.f), Vector2(32.f, 32.f), true);
                                      ("obstacle.png", Obstacle, Vector2(156.f, 60.f), Vector2(32.f, 32.f), true);] 
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
        WorldObjects.Force() 
            |> ignore
        ()
 
    override x.Update (gameTime) =
        let AddGravity' = AddGravity gameTime
        let current = WorldObjects.Value
        let HandleInput' = HandleInput (Keyboard.GetState())
        
        WorldObjects <- lazy (current 
                              |> List.map HandleInput'
                              |> List.map AddGravity'
                              |> List.map AddFriction
                              |> HandleCollisions
                              |> List.map ResolveVelocities)

        WorldObjects.Force() 
            |> ignore

        ()
 
    override x.Draw (gameTime) =
        x.GraphicsDevice.Clear Color.CornflowerBlue
        
        let DrawActor' = DrawActor spriteBatch
        
        spriteBatch.Begin()
        
        WorldObjects.Value
            |> List.iter DrawActor'
        
        spriteBatch.End()

        ()
