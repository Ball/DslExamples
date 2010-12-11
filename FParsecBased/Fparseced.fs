// Learn more about F# at http://fsharp.net

module FParseced
open FParsec.Primitives
open FParsec.CharParsers
open TaskPaperDomain.Domain

type ProjectAst = ProjectName of string
                    | TaskText of string
                    | NoteText of string

let notNewLine = noneOf "\n\r\c"
let allTillEOF = manyChars notNewLine
let parseProject = manyCharsTill (noneOf ":\n") (pchar ':') |>> ProjectName
let parseTask = skipString "- " >>. allTillEOF |>> TaskText
let parseNote = allTillEOF |>> NoteText

let parseLine = parseTask <|> attempt parseProject <|> parseNote
let parseFile = sepBy parseLine (many1 (whitespace <|> newline))

let interpret (str:string) =
    match run parseFile str with
    | Success(x, _, _) ->
        let rec project lines prjs =
            match lines with
            | ProjectName(prj) :: tail ->
                let (pObject, rest) = buildProject (new Project(prj)) tail
                project rest (pObject::prjs)
            | [] -> prjs
        and buildProject project stream =
            match stream with
            | NoteText(nt) ::tail -> project.AddNote(nt)
                                     buildProject project tail
            | TaskText(tsk)::tail ->
                let (task, rest) = buildTask (new TaskItem(tsk)) tail
                project.AddTask(task)
                buildProject project rest
            | _ -> project, stream
        and buildTask task stream =
            match stream with
            | NoteText(nt) ::tail -> task.AddNote(nt)
                                     buildTask task tail
            | _ -> task, stream
        project x [] |> List.rev
    | Failure(_,_,_) -> []    