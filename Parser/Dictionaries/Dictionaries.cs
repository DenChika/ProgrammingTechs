using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Parser.Dictionaries;

public class Dictionaries
{
    public static Dictionary<string, SyntaxTokenList> Mods { get; } = new()
    {
        {"public", SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))},
        {"private", SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword))},
        {"protected", SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword))},
        {"private static readonly", SyntaxFactory.TokenList(
            SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
            SyntaxFactory.Token(SyntaxKind.StaticKeyword),
            SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword))},
        {"public static async", SyntaxFactory.TokenList(
            SyntaxFactory.Token(SyntaxKind.PublicKeyword),
            SyntaxFactory.Token(SyntaxKind.StaticKeyword),
            SyntaxFactory.Token(SyntaxKind.AsyncKeyword))}
    };
    
    public static Dictionary<string, TypeSyntax> Types { get; } = new()
    {
        {"int", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword))},
        {"Integer", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword))},
        {"string", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword))},
        {"String", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword))},
        {"char", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.CharKeyword))},
        {"Character", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.CharKeyword))},
        {"float", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.FloatKeyword))},
        {"Float", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.FloatKeyword))},
        {"boolean", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword))},
        {"Boolean", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword))},
        {"double", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword))},
        {"Double", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword))}
    };
}