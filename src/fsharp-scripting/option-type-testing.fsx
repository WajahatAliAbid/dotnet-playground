let getData value = 
    if value <> "test"
    then
        Some($"Data: '{value}'")
    else
        None

let printValue (value: string) = 
    let data = getData value
    if data.IsSome then
        printfn $"Value found: {data.Value}"
    else
        printfn "Value not found"

printValue "test"
printValue "This is value"
