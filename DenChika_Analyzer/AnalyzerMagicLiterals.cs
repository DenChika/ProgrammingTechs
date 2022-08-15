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
    public class AnalyzerLiterals : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MagicLiteralsReplacer";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(ResourcesMagicLiterals.AnalyzerTitle), ResourcesMagicLiterals.ResourceManager, typeof(ResourcesMagicLiterals));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(ResourcesMagicLiterals.AnalyzerMessageFormat), ResourcesMagicLiterals.ResourceManager, typeof(ResourcesMagicLiterals));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(ResourcesMagicLiterals.AnalyzerDescription), ResourcesMagicLiterals.ResourceManager, typeof(ResourcesMagicLiterals));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public class Lol
        {
            public const int MagicNumber1 = 10;
            public const string MagicNumber2 = "Fredi";
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(c =>
            {
                var literalExpression = c.Node;
                
                if (literalExpression.Ancestors().OfType<MethodDeclarationSyntax>().ToList().Count() != 0)
                {
                    var message = ((LiteralExpressionSyntax)literalExpression).ChildTokens().First().Value.ToString();
                    var diag = Diagnostic.Create(Rule, literalExpression.GetLocation(), message);
                    c.ReportDiagnostic(diag); 
                }
                                   
            }
            , SyntaxKind.NumericLiteralExpression, SyntaxKind.StringLiteralExpression, SyntaxKind.CharacterLiteralExpression);
        }
    }
}