using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Parser.Parsers.MethodParsers.BodyParser;

public class ArgumentsParser
{
    private static InterpolatedStringContentSyntax InterpolatedString(string query)
    {
        return SyntaxFactory.InterpolatedStringText()
            .WithTextToken(
                SyntaxFactory.Token(
                    SyntaxFactory.TriviaList(),
                    SyntaxKind.InterpolatedStringTextToken,
                    query,
                    query,
                    SyntaxFactory.TriviaList()));
    }

    private static InterpolationSyntax Interpolation(string queryName)
    {
        return SyntaxFactory.Interpolation(
            SyntaxFactory.IdentifierName(queryName));
    }

    public static List<InterpolatedStringContentSyntax> InterpolatedStrings(List<string> queries, List<string> names)
    {
        var strings = new List<InterpolatedStringContentSyntax>();
        for (var i = 0; i < queries.Count; i++)
        {
            strings.Add(InterpolatedString(queries[i]));
            strings.Add(Interpolation(names[i]));
        }

        return strings;
    }
    private static ArgumentSyntax GetFirstArgument(string path, List<string> queries, List<string> names)
    {
        return SyntaxFactory.Argument(
            SyntaxFactory.BinaryExpression(
                SyntaxKind.AddExpression,
                SyntaxFactory.InterpolatedStringExpression(
                        SyntaxFactory.Token(
                            SyntaxKind.InterpolatedStringStartToken))
                    .AddContents(
                        SyntaxFactory.InterpolatedStringText()
                            .WithTextToken(
                                SyntaxFactory.Token(
                                    SyntaxFactory.TriviaList(),
                                    SyntaxKind.InterpolatedStringTextToken,
                                    path,
                                    path,
                                    SyntaxFactory.TriviaList()))),
                SyntaxFactory.InterpolatedStringExpression(
                        SyntaxFactory.Token(
                            SyntaxKind.InterpolatedStringStartToken))
                    .WithContents(
                        new SyntaxList<InterpolatedStringContentSyntax>(InterpolatedStrings(queries, names)))));
    }

    private static ArgumentSyntax GetSecondArgument(string name)
    {
        return SyntaxFactory.Argument(
            SyntaxFactory.IdentifierName(name));
    }

    public static ArgumentListSyntax PostArguments(string path, List<string> queries, List<string> queryNames, string argumentName)
    {
        return SyntaxFactory.ArgumentList()
            .AddArguments(GetFirstArgument(path, queries, queryNames))
            .AddArguments(GetSecondArgument(argumentName));
    }

    public static ArgumentListSyntax GetArguments(string path, List<string> queries, List<string> queryNames)
    {
        return SyntaxFactory.ArgumentList()
            .AddArguments(GetFirstArgument(path, queries, queryNames));
    }

    public static ArgumentListSyntax StringArgument(string argument)
    {
        return SyntaxFactory.ArgumentList()
            .AddArguments(
                SyntaxFactory.Argument(
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(argument))));
    }

    public static ArgumentListSyntax IdentifierNameArgument(string name)
    {
        return SyntaxFactory.ArgumentList()
            .AddArguments(
                SyntaxFactory.Argument(
                    SyntaxFactory.IdentifierName(name)));
    }
}