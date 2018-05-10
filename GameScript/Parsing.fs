namespace GameScript

module Parsing =
    open FParsec
    
    type Parser<'a> = Parser<'a, unit>

    let comment = pstring "//" >>. skipRestOfLine true
    let ws = many(comment <|> spaces1) >>% ()
    let ident:Parser<_> = identifier (IdentifierOptions()) .>> ws
    let keyword x = pstring x .>> notFollowedBy ident .>> ws
    let symbol x = pstring x .>> ws

    (**********************
     * Expression Parsing *
     **********************)
    
    type BinaryOperator =
    | Add
    | Subtract
    | Multiply
    | Divide
    | LessThan
    | LessOrEqual
    | Equal
    | GreaterOrEqual
    | Greater
    | Assign
    | AddAndAssign
    | SubtractAndAssign
    | MultiplyAndAssign
    | DivideAndAssign
    | MemberAccess

    type Expression =
    | Sequence of Expression list
    | BinaryOperation of BinaryOperator * Expression * Expression
    | IntLiteral of int
    | SingleLiteral of single
    | Variable of string
    | If of cond: Expression * ifTrue: Expression * ifFalse: Expression option
    
    let expression, expressionRef = createParserForwardedToRef()
    let subExpression = expression |> between (symbol "(") (symbol ")")
    let opp = OperatorPrecedenceParser<Expression, unit, unit>()

    let int' = pint32 .>> notFollowedBy ident .>> ws |>> IntLiteral
    let single' = pfloat .>> notFollowedBy ident .>> ws |>> single |>> SingleLiteral
    let variable = ident |>> Variable

    let sequence =
        sepEndBy expression (symbol ";")
        |> between (symbol "{") (symbol "}")
        |>> Sequence

    opp.TermParser <- choice [int'; single'; variable; subExpression; sequence]

    let addInfixL op str prec =
        opp.AddOperator(
            InfixOperator(
                str, ws, prec, Associativity.Left,
                fun l r -> BinaryOperation(op, l, r)))

    addInfixL Add "+" 10
    addInfixL Subtract "-" 10
    addInfixL Multiply "*" 20
    addInfixL Divide "/" 20
  
    addInfixL Assign ":=" 1
    addInfixL AddAndAssign "+=" 1

    addInfixL MemberAccess "." 50

    expressionRef := opp.ExpressionParser .>> ws

    (****************
     * Type parsing *
     ****************)

    type Type =
    | TypeName of string
    | Array of Type

    let type', typeRef = createParserForwardedToRef()
    let arrayType = (symbol "#") >>. type' |>> Array
    let typeName = ident |>> TypeName
    typeRef := choice
        [
            typeName
            arrayType
        ]

    (******************
     * System parsing *
     ******************)

    type System = 
        {
        Name: string
        Components: (string option * string) list
        Event: string * Type
        Action: Expression
        Condition: Expression option
        }

    

    let variableDeclaration = ident .>>. (symbol "::" >>. type')
    let componentName = (opt (ident .>> followedBy (symbol ".")) .>>. ident)
    let system = 
        pipe5
            (keyword "system" >>. ident)
            (keyword "on" >>. variableDeclaration)
            (opt (keyword "select" >>. sepBy componentName (symbol ",")))
            (opt (keyword "where" >>. expression))
            (keyword "do" >>. expression)
        <| fun name event components cond expr ->
        {
            System.Name = name
            Event = event
            Components = components |> Option.fold (fun _ x -> x) []
            Condition = cond
            Action = expr
        }

    (******************
     * Struct parsing *
     ******************)

    type Struct =
        {
        Name: string
        Base: string option
        Fields: (string * Type) list
        }

    let struct' = 
        pipe3
            (keyword "struct" >>. ident)
            (opt (symbol "::" >>. ident))
            (between (symbol "{") (symbol "}") (many variableDeclaration))
        <| fun name base' fields -> { Struct.Name = name; Base = base'; Fields = fields }

    type ModulePart =
    | Struct of Struct
    | System of System

    let module' = ws >>. many (choice [system |>> System; struct' |>> Struct] .>> ws) .>> eof
    let parseModule name stream encoding =
        runParserOnStream module' name stream encoding