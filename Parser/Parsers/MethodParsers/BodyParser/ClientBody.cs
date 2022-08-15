using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Parser.Parsers.MethodParsers.BodyParser;

public class ClientBody
{
    public static BlockSyntax DistributeVariable(string type, string parametersLine, string path, string attribute)
    {
        var paramsArray = parametersLine.Split(',');
        var queryName = string.Empty;
        var flag = false;
        var queries = new List<string> {};
        var queryNames = new List<string>() {};
        var createContent = string.Empty;
        ArgumentListSyntax stringOrIdArgument =
            ArgumentsParser.StringArgument("");
        foreach (var str in paramsArray)
        {
            if (str == string.Empty)
                continue;
            var paramName = str.Trim().Split(' ')[^1];
            var paramAttribute = str.Trim().Split(' ')[0].Replace("@", "");
            if (paramAttribute.Equals("RequestParam"))
            {
                if (!flag)
                    queryName += "?";
                else
                {
                    queryName += "&";
                }
                flag = true;
                queryName += $"{paramName}=";
                queries.Add(queryName);
                queryNames.Add(paramName);
                queryName = string.Empty;
            }

            if (paramAttribute.Equals("RequestBody"))
            {
                createContent = paramName;
                stringOrIdArgument =
                    ArgumentsParser.IdentifierNameArgument(createContent);
            }
        }

        var argumentName = string.Empty;
        var nameAsync = "GetAsync";
        ArgumentListSyntax argumentList =
            ArgumentsParser.GetArguments(path, queries, queryNames);
        if (attribute.Equals("PostMapping"))
        {
            nameAsync = "PostAsync";
            argumentName = "jsonContent";
            argumentList =
                ArgumentsParser.PostArguments(path, queries, queryNames, argumentName);
        }
        
        if (attribute.Equals("DeleteMapping"))
        {
            nameAsync = "DeleteAsync";
            argumentList =
                ArgumentsParser.GetArguments(path, queries, queryNames);
        }

        LocalDeclarationStatementSyntax statementFirst =
            SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName(
                            SyntaxFactory.Identifier(
                                SyntaxFactory.TriviaList(),
                                SyntaxKind.VarKeyword,
                                "var",
                                "var",
                                SyntaxFactory.TriviaList())))
                    .AddVariables(
                        SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier("jsonContent"))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("JsonContent"),
                                                SyntaxFactory.IdentifierName("Create")))
                                        .WithArgumentList(
                                            stringOrIdArgument)))));
        
        
        LocalDeclarationStatementSyntax statementSecond =
            SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(
                    SyntaxFactory.IdentifierName(
                        SyntaxFactory.Identifier(
                            SyntaxFactory.TriviaList(),
                            SyntaxKind.VarKeyword,
                            "var",
                            "var",
                            SyntaxFactory.TriviaList())))
                    .AddVariables(
                        SyntaxFactory.VariableDeclarator(
                            SyntaxFactory.Identifier("response"))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.AwaitExpression(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("_client"),
                                                SyntaxFactory.IdentifierName(nameAsync)))
                                            .WithArgumentList(
                                                argumentList))))));


        LocalDeclarationStatementSyntax statementThird =
            SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName(
                            SyntaxFactory.Identifier(
                                SyntaxFactory.TriviaList(),
                                SyntaxKind.VarKeyword,
                                "var",
                                "var",
                                SyntaxFactory.TriviaList())))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("responseContent"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.AwaitExpression(
                                            SyntaxFactory.InvocationExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("response"),
                                                        SyntaxFactory.IdentifierName("Content")),
                                                    SyntaxFactory.IdentifierName("ReadAsStringAsync")))))))));

        ReturnStatementSyntax statementFourth =
            SyntaxFactory.ReturnStatement(
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("JsonSerializer"),
                        SyntaxFactory.GenericName(
                                SyntaxFactory.Identifier("Deserialize"))
                            .WithTypeArgumentList(
                                SyntaxFactory.TypeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                        TypeParser.ParseComplexMembers(type))))))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName("responseContent"))))));
        
        return SyntaxFactory.Block(
            statementFirst,
            statementSecond,
            statementThird,
            statementFourth);
    }
}