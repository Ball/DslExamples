module ProjectParserTests

open FParseced
open FParsec.Primitives
open FParsec.CharParsers
open NUnit.Framework
open FsUnit

let tryParser p i =
    match run p i with
    | Success(r,_,_) -> r
    | Failure(s,_,_) -> raise (System.InvalidOperationException s)

[<TestFixture>]
type ``Note Parser``()=
    [<Test>]member test.
        ``Should build from random text``()=
            tryParser parseNote "this is a string" |> should equal (NoteText("this is a string"))
[<TestFixture>]
type ``Task Parser``()=
    [<Test>]member test.
        ``Should find a leading dash``()=
            tryParser parseTask "- buy milk" |> should equal (TaskText("buy milk"))
[<TestFixture>]
type ``Project Parser``()=
    [<Test>]member test.
        ``Should find the trailing colon``()=
            tryParser parseProject "The Store:" |> should equal (ProjectName("The Store"))
    [<Test>]member test.
        ``Should find the trailing colon and a newline``()=
            tryParser parseProject "The Store:\n" |> should equal (ProjectName("The Store"))
[<TestFixture>]
type ``Line Parser``()=
    [<Test>]member test.
        ``Should find a project``()=
            tryParser parseLine "The Store:\n" |> should equal (ProjectName("The Store"))    
    [<Test>]member test.
        ``Should find a task``()=
            tryParser parseLine "- buy milk" |> should equal (TaskText("buy milk"))
    [<Test>]member test.
        ``should find a note``()=
            tryParser parseLine "some note" |> should equal (NoteText("some note"))
[<TestFixture>]
type ``Project file  Parser``()=
    [<Test>]member test.
        ``should find a project``()=
            tryParser parseFile "The Store:" |> should equal [ProjectName("The Store")]
    [<Test>]member test.
        ``should find a project and task``()=
            tryParser parseFile "The Store:\n- buy milk" |> should equal [ProjectName("The Store"); TaskText("buy milk")]
    [<Test>]member test.
        ``should find a few projects``()=
            tryParser parseFile "The Store:\n- buy milk\nDSL Talk:\n- Do Things"
            |> should equal [ProjectName("The Store"); TaskText("buy milk"); ProjectName("DSL Talk"); TaskText("Do Things")]
    [<Test>]member test.
        ``should find a note on the project``()=
            tryParser parseFile "The Store:\na note of some import"
            |> should equal [ProjectName("The Store"); NoteText("a note of some import")]
    [<Test>]member test.
        ``should ignore excess white space``()=
            tryParser parseFile "The Store:\n \n- buy milk"
            |> should equal [ProjectName("The Store"); TaskText("buy milk")]
    [<Test>]member test.
        ``should parse a note``()=
            tryParser parseNote "Some notes\n"
            |> should equal (NoteText("Some notes"))
    [<Test>]member test.
        ``should parse the stackoverflow thing``()=
            tryParser parseFile "Project 1:\nSome note\nProject 2:\n- One Task"
            |> should equal [ProjectName("Project 1"); NoteText("Some note"); ProjectName("Project 2"); TaskText("One Task")]
    [<Test>]member test.
        ``should properly parse the demo text``()=
            tryParser parseFile "Project:
Some notes
Write a DSL:"
            |> should equal [ProjectName("Project");NoteText("Some notes");
                             ProjectName("Write a DSL")]

open TaskPaperDomain.Domain
[<TestFixture>]
type ``The interpreter``()=
    [<Test>]member test.
        ``Should build a project``()=
            match interpret "The Store:" with
            | p :: tail -> p.Name |> should equal "The Store"
            | _ -> failwith "project not found"
    [<Test>]member test.
        ``should build a project with a note``()=
            match interpret "The Store:\na note" with
            | p :: tail -> p.Notes |> should equal "a note"
                           p.Name |> should equal "The Store"
                           tail |> should equal []
            |_ -> failwith "project not found"