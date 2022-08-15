using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Parser.Parsers;

public class TypeParser
{
    public static TypeSyntax ParseComplexMembers(string type)
    {
        if (Dictionaries.Dictionaries.Types.ContainsKey(type))
            return Dictionaries.Dictionaries.Types[type];

        if (!type.Contains('<'))
            return SyntaxFactory.IdentifierName(type);
        
        var collectionName = type.Substring(0, type.IndexOf('<'));
        var newType = type.Substring(type.IndexOf('<') + 1, 
            type.LastIndexOf('>') - type.IndexOf('<') - 1);
        if (collectionName.Equals("LinkedList") || collectionName.Equals("Task"))
        {
            return SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier(collectionName))
                .WithTypeArgumentList(
                    SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>
                            (ParseComplexMembers(newType))));
        }

        if (collectionName.Equals("Dictionary"))
        {
            var firstType = newType.Split(',')[0];
            var secondType = newType.Replace(firstType, "").Trim();
            return SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier(collectionName))
                .WithTypeArgumentList(
                    SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SeparatedList<TypeSyntax>
                        (new SyntaxNodeOrToken[]
                        {
                            ParseComplexMembers(firstType),
                            SyntaxFactory.Token(
                                SyntaxKind.CommaToken),
                            ParseComplexMembers(secondType)
                        })));
        }

        throw new Exception("Collection is unparsed");
    }
}