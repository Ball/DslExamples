namespace ParserCombinatorBased
open FParseced
open System.Collections.Generic
open TaskPaperDomain
open TaskPaperDomain.Domain

type FParsecParser()= class
    member x.Name = "FParsec Based"
    member x.Interpret(str) = interpret str |> Seq.ofList
    interface IInterpret with
        member this.Name = this.Name
        member this.Interpret(str):IEnumerable<Project> = this.Interpret(str)
end

