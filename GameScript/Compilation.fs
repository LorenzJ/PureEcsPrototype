namespace GameScript

module Compilation =
    open Parsing
    open Mono.Cecil
    open System.Numerics

    type Context =
        {
        Assembly: AssemblyDefinition
        Module: ModuleDefinition
        TypeReferences: Map<string, TypeReference>
        }

    
    let getDefaultTypeReferences (module':ModuleDefinition) = 
        Map 
            [
                "i32", module'.ImportReference(typeof<int>)
                "f32", module'.ImportReference(typeof<single>)
                "vec2", module'.ImportReference(typeof<Vector2>)
                "vec3", module'.ImportReference(typeof<Vector3>)
                "vec4", module'.ImportReference(typeof<Vector4>)
            ]

    let getTypeReference context type' =
        let rec getType = function
            | Array t -> getType t
            | TypeName name -> name
        let innerType = getType type'
        context.TypeReferences.[innerType]
        
    let createType context (struct':Struct) =
        let typeDef = new TypeDefinition("Scripted", struct'.Name, TypeAttributes.Public ||| TypeAttributes.AnsiClass);
        typeDef.IsValueType <- true
        for (name, type') in struct'.Fields do
            let fieldDef = new FieldDefinition(name, FieldAttributes.Public, getTypeReference context type')
            typeDef.Fields.Add(fieldDef)
        typeDef

    let createTypeReference struct' =
        new TypeDefinition("Scripted", struct'.Name, TypeAttributes.Public) :> TypeReference
        
    let compileModule context moduleParts =
        let structs = 
            moduleParts
            |> List.choose(fun item ->
                match item with
                | Struct s -> Some s
                | _ -> None)
        
        let newTypeReferences = 
            structs 
            |> Seq.map (fun s -> s.Name, createTypeReference s)
            |> Seq.append (Map.toList context.TypeReferences)
            |> Map.ofSeq

        let context' = { context with TypeReferences = newTypeReferences }


        for part in moduleParts do
            match part with
            | Struct s -> context.Module.Types.Add(createType context s)
            | System s -> ()
        ()


