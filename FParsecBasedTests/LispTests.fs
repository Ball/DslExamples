// Learn more about F# at http://fsharp.net



module Module1
open FParsec.Primitives
open FParsec.CharParsers
open FParsec.OperatorPrecedenceParser
open FsUnit
open NUnit.Framework

let succeed_with result =
    match result with
    |Success(act, _, _) -> act
    | _ -> failwith "no parsed"
let fail_with result =
    match result with
    |Failure(msg,_,_) -> msg
    | _ -> ""

type LispVal = Atom of string
               | List of LispVal list
               | DottedList of LispVal list * LispVal
               | Number of int
               | String of string
               | Bool of bool

let symbol = anyOf "!#$%&|*+-/:<=>?@^_~"
let program = (spaces >>.symbol)
let asString (x:char list) :string = x |> List.map (fun y -> y.ToString()) |> String.concat ""
let asNumber x = Number(System.Int32.Parse(asString x))
let asList x = List(x)

let parseString = parse{ do! skipChar '"'
                         let! x = many (noneOf "\"")
                         do! skipChar '"'
                         return String(asString x)}
let parseAtom = parse{ let! first = letter <|> symbol
                       let! rest = many (letter <|> symbol <|> digit)
                       let atom = first :: rest |> asString
                       return match atom with
                              | "#t" -> Bool(true)
                              | "#f" -> Bool(false)
                              | _ -> Atom(atom) }
let parseNumber = many1 digit |>> asNumber
let parseExpr = parseNumber <|> parseString <|> parseAtom
let parseList = sepBy parseExpr spaces1 |>> asList
let endBy p sep = many (p .>> sep)
let parseDottedList = parse{ let! head = endBy parseExpr spaces
                             let! tail = pchar '.' >>. spaces >>. parseExpr
                             return DottedList(head, tail) }
let openList x = match x with
                 | List(n) -> n
                 | _ -> [x]
[<TestFixture>]
type ``Basic Tests``()=
    [<Test>]member test.
     ``When I parse a symbol``()=
        run program "%" |> succeed_with |> should equal '%'
    [<Test>]member test.
     ``When I add in some whitespace``()=
      run program "  %" |> succeed_with |> should equal '%'
    [<Test>]member test.
     ``When I parse a string``()=
      run parseString "\"hello\"" |> succeed_with |> should equal (String("hello"))
    [<Test>]member test.
     ``When I parse a true``()=
      run parseAtom "#t" |> succeed_with |> should equal (Bool(true))
    [<Test>]member test.
     ``When I get any old atom``()=
      run parseAtom "!notMe!" |> succeed_with |> should equal (Atom("!notMe!"))
    [<Test>]member test.
     ``When I parse a false``()=
      run parseAtom "#f" |> succeed_with |> should equal (Bool(false))
    [<Test>]member test.
     ``When I parse a number``()=
      run parseNumber "1234" |> succeed_with |> should equal (Number(1234))
    [<Test>]member test.
     ``When I parse an expression list``()=
      run parseList "#t #f" |> succeed_with |> should equal (List([Bool(true); Bool(false)]))
    [<Test>]member test.
     ``When I parse an expression of one``()=
      run parseList "1" |> succeed_with |> should equal (List([Number(1)]))
    [<Test>]member test.
     ``When I seek understanding of sepBy``()=
      run (sepBy digit spaces1) "1" |> succeed_with |> should equal ['1']
//    [<Test>]member test.
//     ``When I parse a dotted expression list``()=
//      run parseDottedList "#t #f . 12" |> succeed_with |> should equal (DottedList([Bool(true);Bool(false)],List([Number(12)])))