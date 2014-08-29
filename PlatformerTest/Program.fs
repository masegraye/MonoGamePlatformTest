// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open MBPlat.Game

[<EntryPoint>]
let main argv = 
    use g = new PlatformerGame();
    g.Run()
    0 // return an integer exit code
    
 