using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Parser.Parsers.MethodParsers.BodyParser;

namespace Parser.Parsers.MethodParsers;

public class MethodDistributer
{
    public static MethodDeclarationSyntax Distribute(string line, string attributeLine)
    {
        var lineWithoutParams = line.Substring(0, line.IndexOf('('));
        var parameters = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1);
        var methodName = (lineWithoutParams.Substring(0, 1).ToUpper() + 
                         lineWithoutParams.Substring(1, lineWithoutParams.Length - 1).Split(' ')[^1]).Trim();
        var methodType = lineWithoutParams.Split(' ')[^2];
        var asyncMethodType = "Task<" + methodType + ">";
        var modifier = lineWithoutParams.Replace(methodName, "").Replace(methodType, "").Trim();
        var newModifier = modifier + " static " + "async";
        var typeOfMapping = attributeLine.Substring(attributeLine.IndexOf('@') + 1, 
            attributeLine.IndexOf('(') - attributeLine.IndexOf('@') - 1);
        var route = attributeLine.Substring(attributeLine.IndexOf('"') + 1, 
            attributeLine.LastIndexOf('"') - attributeLine.IndexOf('"') - 1);
        var globalRoute = $"http://localhost:8080" + route;
        if (!Dictionaries.Dictionaries.Mods.ContainsKey(modifier)) 
            throw new Exception("This modifier doesn't exist in dictionary");
        return SyntaxFactory.MethodDeclaration(
                TypeParser.ParseComplexMembers(asyncMethodType),
                SyntaxFactory.Identifier(methodName))
            .WithModifiers(
                Dictionaries.Dictionaries.Mods[newModifier])
            .WithParameterList(ParametersParser.GetParameters(parameters))
            .WithBody(
                ClientBody.DistributeVariable(methodType, parameters, globalRoute, typeOfMapping));
    }
}