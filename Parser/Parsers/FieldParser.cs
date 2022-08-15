using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Parser.Parsers;

public class FieldParser
{
    public static PropertyDeclarationSyntax GetField(string line)
    {
        var name = line.Split(' ')[^1].Replace(";", "");
        var type = line.Split(' ')[^2];
        var modifier = line.Replace(name, "").Replace(type, "").Replace(";", "").Trim();
        if (!Dictionaries.Dictionaries.Mods.ContainsKey(modifier)) 
            throw new Exception("This modifier doesn't exist in dictionary");
        return SyntaxFactory.PropertyDeclaration(
                TypeParser.ParseComplexMembers(type), 
                SyntaxFactory.Identifier(name))
            .WithModifiers(Dictionaries.Dictionaries.Mods[modifier])
            .WithAccessorList(
                SyntaxFactory.AccessorList(
                    SyntaxFactory.List<AccessorDeclarationSyntax>(
                        new AccessorDeclarationSyntax[]{
                            SyntaxFactory.AccessorDeclaration(
                                    SyntaxKind.GetAccessorDeclaration)
                                .WithSemicolonToken(
                                    SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                            SyntaxFactory.AccessorDeclaration(
                                    SyntaxKind.SetAccessorDeclaration)
                                .WithSemicolonToken(
                                    SyntaxFactory.Token(SyntaxKind.SemicolonToken))})));
    }

    public static FieldDeclarationSyntax HttpClientField(string type, string name, string mod)
    {
        return SyntaxFactory.FieldDeclaration(
                SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName(type))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(name))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                                SyntaxFactory.IdentifierName(type))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList()))))))
            .WithModifiers(Dictionaries.Dictionaries.Mods[mod]);
    }
}