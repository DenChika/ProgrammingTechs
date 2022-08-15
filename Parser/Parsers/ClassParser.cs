using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Parser.Parsers;

public class ClassParser
{
    public static ClassDeclarationSyntax ClassDeclaration(string line)
    {
        var obj = line.Split(' ')[0];
        if (obj.Equals("class"))
        {
            var name = line.Split(' ')[1];
            if (name.Contains("Controller"))
                name = name.Replace("Controller", "") + "Client";
            return DeclareWithoutModif(name);
        }
        else
        {
            var name = line.Split(' ')[2];
            if (name.Contains("Controller"))
                name = name.Replace("Controller", "") + "Client";
            var modifier = line.Split(' ')[0];
            return DeclareWithModif(modifier, name);
        }
    }

    private static ClassDeclarationSyntax DeclareWithModif(string modifier, string name)
    {
        if (!Dictionaries.Dictionaries.Mods.ContainsKey(modifier))
            throw new Exception("This modifier doesn't exist in dictionary");
        return SyntaxFactory.ClassDeclaration(name)
            .WithModifiers(Dictionaries.Dictionaries.Mods[modifier]);
        
    }

    private static ClassDeclarationSyntax DeclareWithoutModif(string name)
    {
        return SyntaxFactory.ClassDeclaration(name);
    }
}