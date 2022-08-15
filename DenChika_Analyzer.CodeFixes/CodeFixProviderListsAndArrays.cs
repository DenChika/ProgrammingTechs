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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CodeFixProviderListsAndArrays)), Shared]
    public class CodeFixProviderListsAndArrays : CodeFixProvider
    {
        private const string title = "Replace List/Array on IReadOnlyCollection";
            
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(AnalyzerListsAndArrays.DiagnosticId); }
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
            if (root.FindNode(diagnosticSpan).IsKind(SyntaxKind.GenericName))
            {
                var declarationGeneric = (GenericNameSyntax) root.FindNode(diagnosticSpan);

                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: title,
                        createChangedDocument: c => ChangeGeneric(context.Document, declarationGeneric, c),
                        equivalenceKey: title),
                    diagnostic);
            }
            else if (root.FindNode(diagnosticSpan).IsKind(SyntaxKind.ArrayType))
            {
                var declarationArray = (ArrayTypeSyntax)root.FindNode(diagnosticSpan);

                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: title,
                        createChangedDocument: c => ChangeArray(context.Document, declarationArray, c),
                        equivalenceKey: title),
                    diagnostic);
            } 
        }

        private async Task<Document> ChangeGeneric(
            Document document, 
            GenericNameSyntax genericName, 
            CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken);
            var newGeneric = genericName.WithIdentifier(SyntaxFactory.Identifier("IReadOnlyCollection"));
            editor.ReplaceNode(genericName, newGeneric);
            return editor.GetChangedDocument();
        }

        private async Task<Document> ChangeArray(
            Document document,
            ArrayTypeSyntax arrayType,
            CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken);
            var newType = (TypeSyntax) arrayType.ChildNodes().First();
            var newName = SyntaxFactory.GenericName(
                SyntaxFactory.Identifier("IReadOnlyCollection"))
                .WithTypeArgumentList(
                    SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(newType)));
            editor.ReplaceNode(arrayType, newName);
            return editor.GetChangedDocument();
        }
    }
}
