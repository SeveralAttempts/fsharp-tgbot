open System

let GetSum(x: uint) : string = 
    (x + uint32(42)).ToString()

[<EntryPoint>]
let main argv =
    if argv.Length <= 0 then
        exit -1
    let sum = GetSum(Convert.ToUInt32(argv.[0]))
    Console.WriteLine(sum)
    0
