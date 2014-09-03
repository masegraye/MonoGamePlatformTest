module MBPlat.Input

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input
open MBPlat.Actors

let HandleInput (kbState : KeyboardState) (actor : WorldActor) =
    let rec HandleKeys (keys : Keys list) ((currentVelocity : Vector2), (state : PlayerState)) = 
        match keys with 
        | [] -> (currentVelocity , state)
        | x :: xs -> match x with
                     | Keys.Left -> let newSpeed = if (currentVelocity.X - 0.1f) < -1.f then
                                                       -1.f
                                                   else
                                                       currentVelocity.X - 0.1f
                                    let newV = Vector2(newSpeed, currentVelocity.Y)
                                    HandleKeys xs (newV, state)
                     
                     | Keys.Right -> let newSpeed = if (currentVelocity.X + 0.1f) > 1.f then
                                                        1.f
                                                    else
                                                        currentVelocity.X + 0.1f
                                     let newV = Vector2(newSpeed, currentVelocity.Y)
                                     HandleKeys xs (newV, state)
                                     
                     | Keys.Space -> match state with
                                     | Nothing -> let newV = Vector2(currentVelocity.X, currentVelocity.Y - 3.f)
                                                  HandleKeys xs (newV, Jumping)

                                     | Jumping -> HandleKeys xs (currentVelocity, state)    
                                     
                                      
                     | _ -> HandleKeys xs (currentVelocity,state)

    match actor.ActorType with
    | Player(s) -> let initialVelocity = match actor.BodyType with
                                         | Dynamic(v) -> v
                                         | _ -> Vector2()
                   let (velocity, newState) = HandleKeys (kbState.GetPressedKeys() |> Array.toList) (initialVelocity,s)
                   { actor with BodyType = Dynamic(velocity); ActorType = Player(newState) }
    | _ -> actor