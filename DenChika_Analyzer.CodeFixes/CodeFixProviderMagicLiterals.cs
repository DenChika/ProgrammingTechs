using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace DenChika_Analyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CodeFixProviderMagicLiterals)), Shared]
    public class CodeFixProviderMagicLiterals : CodeFixProvider
    {
        private const string title = "Replace declaration on const field";

        private Dictionary<PredefinedTypeSyntax, SyntaxKind> LiteralTypeToToken = new Dictionary<PredefinedTypeSyntax, SyntaxKind>()
        {
            { SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword)), SyntaxKind.NumericLiteralExpression },
            { SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.CharKeyword)), SyntaxKind.CharacterLiteralExpression },
            { SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)), SyntaxKind.StringLiteralExpression }
        };
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(AnalyzerLiterals.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var node = root.FindNode(diagnosticSpan);
            if (root.FindNode(diagnosticSpan).IsKind(SyntaxKind.Argument))
            {
                var literalNode = node.ChildNodes().OfType<LiteralExpressionSyntax>().First();
                context.RegisterCodeFix(
                CodeAction.Create(
                    title: title,
                    createChangedDocument: c => ReplaceLiterals(context.Document, literalNode, c),
                    equivalenceKey: title),
                diagnostic);
            }
            else
            {
                var literalNode = (LiteralExpressionSyntax) node;
                context.RegisterCodeFix(
                CodeAction.Create(
                    title: title,
                    createChangedDocument: c => ReplaceLiterals(context.Document, literalNode, c),
                    equivalenceKey: title),
                diagnostic);
            }
        }

        private async Task<Document> ReplaceLiterals(
            Document document,
            LiteralExpressionSyntax literalExpression,
            CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken);

            var classDeclaration = literalExpression.Ancestors().OfType<ClassDeclarationSyntax>().First();

            var keyword = SyntaxFactory.PredefinedType(
                SyntaxFactory.Token(SyntaxKind.IntKeyword));

            foreach (var type in LiteralTypeToToken.Keys)
            {
                if (literalExpression.Kind().Equals(LiteralTypeToToken[type]))
                    keyword = type;

            }

            var identifierName = Identifier(classDeclaration);

            var field = SyntaxFactory.FieldDeclaration(
                SyntaxFactory.VariableDeclaration(
                    keyword)
                .WithVariables(
                    SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                        SyntaxFactory.VariableDeclarator(
                            SyntaxFactory.Identifier(identifierName))
                        .WithInitializer(
                            SyntaxFactory.EqualsValueClause(literalExpression)))))
                .WithModifiers(
                SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
                        SyntaxFactory.Token(SyntaxKind.ConstKeyword)
                    }));

            editor.AddMember(classDeclaration, field);

            foreach (var node in classDeclaration.Members)
            {
                if (node.IsKind(SyntaxKind.MethodDeclaration) &&
                    node.ChildNodes().OfType<BlockSyntax>().ToList().Count() != 0)
                    ReplaceInBlock(editor, literalExpression, node, identifierName);

                if (node.IsKind(SyntaxKind.MethodDeclaration) &&
                    node.ChildNodes().OfType<ParameterListSyntax>().ToList().Count() != 0)
                    ReplaceInParameters(editor, literalExpression, node, identifierName); 
            }

            return editor.GetChangedDocument();
        }

        private void ReplaceInBlock(
            DocumentEditor editor,
            LiteralExpressionSyntax literalExpression, 
            MemberDeclarationSyntax member,
            string identifier)
        {
            var block = member.ChildNodes().OfType<BlockSyntax>().First();

            if (block.DescendantNodesAndSelf().OfType<LiteralExpressionSyntax>().ToList().Count() != 0)
            {
                foreach (var literal in block.DescendantNodesAndSelf().OfType<LiteralExpressionSyntax>())
                {

                    if (!(literal.AncestorsAndSelf().OfType<IfStatementSyntax>().ToList().Count() != 0 ||
                        literal.AncestorsAndSelf().OfType<ForStatementSyntax>().ToList().Count() != 0 ||
                        literal.AncestorsAndSelf().OfType<LocalDeclarationStatementSyntax>().ToList().Count() != 0 ||
                        literal.AncestorsAndSelf().OfType<ExpressionStatementSyntax>().ToList().Count() != 0 ||
                        literal.AncestorsAndSelf().OfType<ArgumentSyntax>().ToList().Count() != 0)) continue;

                        if (!literal.ChildTokens().First().Text
                        .Equals(literalExpression.ChildTokens().First().Text)) continue;

                    editor.ReplaceNode(literal, SyntaxFactory.IdentifierName(identifier));
                }

            }
        }

        private void ReplaceInParameters(
            DocumentEditor editor,
            LiteralExpressionSyntax literalExpression,
            MemberDeclarationSyntax member,
            string identifier)
        {
            var parameters = member.ChildNodes().OfType<ParameterListSyntax>().First();

            foreach (var child in parameters.ChildNodes())
            {
                if (child.IsKind(SyntaxKind.Parameter) &&
                    child.DescendantNodesAndSelf().OfType<EqualsValueClauseSyntax>().ToList().Count() != 0 &&
                    child.DescendantNodesAndSelf().OfType<LiteralExpressionSyntax>().ToList().Count() != 0
                    )
                {
                    var literal = child.DescendantNodesAndSelf().OfType<LiteralExpressionSyntax>().First();

                    if (!literal.ChildTokens().First().Text
                        .Equals(literalExpression.ChildTokens().First().Text)) continue;

                    editor.ReplaceNode(literal, SyntaxFactory.IdentifierName(identifier));
                }
            }
        }

        private string Identifier(ClassDeclarationSyntax classDeclaration)
        {
            var number = 0;
            foreach (var field in classDeclaration.ChildNodes().OfType<FieldDeclarationSyntax>().ToList())
            {
                var identifier = field.DescendantNodes().OfType<VariableDeclaratorSyntax>().FirstOrDefault()
                    .ChildTokens().FirstOrDefault().Value.ToString();
                number = int.Parse(identifier.Replace("MagicNumber", ""));
            }

            return $"MagicNumber{number + 1}";
        }
    }
}
