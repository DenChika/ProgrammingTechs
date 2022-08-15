using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Parser.Parsers.MethodParsers;

public class ParametersParser
{
    public static ParameterListSyntax GetParameters(string parameters)
    {
        var paramsArray = parameters.Split(',');
        ParameterListSyntax list = SyntaxFactory.ParameterList(
            SyntaxFactory.SeparatedList<ParameterSyntax>(
                new SyntaxNodeOrToken[] { }));
        foreach (var str in paramsArray)
        {
            if (str == string.Empty)
                continue;
            var paramName = str.Trim().Split(' ')[^1];
            var paramType = str.Trim().Split(' ')[^2];
            var paramAttribute = str.Trim().Split(' ')[0].Replace("@", "");
            list = list.AddParameters(
                SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier(paramName))
                    .WithType(TypeParser.ParseComplexMembers(paramType)));
        }

        return list;
    }
}