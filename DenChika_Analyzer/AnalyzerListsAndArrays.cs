using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using DenChika_Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DenChika_Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AnalyzerListsAndArrays : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CollectionReplacer";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(ResourcesListsAndArrays.AnalyzerTitle), ResourcesListsAndArrays.ResourceManager, typeof(ResourcesListsAndArrays));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(ResourcesListsAndArrays.AnalyzerMessageFormat), ResourcesListsAndArrays.ResourceManager, typeof(ResourcesListsAndArrays));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(ResourcesListsAndArrays.AnalyzerDescription), ResourcesListsAndArrays.ResourceManager, typeof(ResourcesListsAndArrays));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(c =>
            {
                var node = c.Node;

                bool methodFlag = false;

                if (node.Ancestors().OfType<MethodDeclarationSyntax>().ToList().Count != 0)
                {
                    var method = node.Ancestors().OfType<MethodDeclarationSyntax>().First();
                    if (method.Modifiers.First().IsKind(SyntaxKind.PublicKeyword))
                        methodFlag = true;
                }

                if (((node.Ancestors().OfType<MethodDeclarationSyntax>().ToList().Count != 0 && methodFlag) ||
                (node.Ancestors().OfType<ParameterSyntax>().ToList().Count != 0 && methodFlag) ||
                node.Ancestors().OfType<PropertyDeclarationSyntax>().ToList().Count != 0) &&
                node.Ancestors().OfType<BlockSyntax>().ToList().Count == 0)
                {
                    if (c.Node.IsKind(SyntaxKind.GenericName))
                    {
                        var newNode = (GenericNameSyntax)node;
                        var genToken = newNode.Identifier;

                        if (genToken.Text.Equals("List"))
                        {
                            var diag = Diagnostic.Create(Rule, genToken.GetLocation(), genToken.Text);
                            c.ReportDiagnostic(diag);
                        }
                    }
                    else
                    {
                        var newNode = (ArrayTypeSyntax)node;
                        var message = string.Empty;

                        if (newNode.ChildNodes().OfType<GenericNameSyntax>().ToList().Count != 0)
                            message = newNode.ChildNodes().OfType<GenericNameSyntax>().First().Identifier.Text;
                        else
                            message = newNode.ElementType.ToString();

                        var diag = Diagnostic.Create(Rule, newNode.GetLocation(), message + "[]");
                        c.ReportDiagnostic(diag);
                    }
                }
            }
            , SyntaxKind.GenericName, SyntaxKind.ArrayType);
        }
    }
}