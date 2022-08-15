using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Parser.Parsers;
using Parser.Parsers.MethodParsers;

namespace Parser.MainParser;

public class MainParser
{
    private Dictionary<string, ClassDeclarationSyntax> _trees;

    public MainParser()
    {
        _trees = new Dictionary<string, ClassDeclarationSyntax>();
    }

    public void Distribute(string dirToPath, string path)
    {
        foreach (var file in Directory.GetFiles(path))
        {
            var fileName = file.Substring(file.LastIndexOf('\\') + 1,
                file.LastIndexOf('.') - file.LastIndexOf('\\') - 1);

            if (fileName.Contains("Controller"))
                fileName = fileName.Replace("Controller", "") + "Client";

            using (var sr = new StreamReader(file, System.Text.Encoding.Default))
            {
                string line;
                var classFlag = false;
                var fieldFlag = true;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == string.Empty || line.Equals("}"))
                        continue;
                    
                    if (line.Contains("class"))
                    {
                        var classDeclaration = ClassParser.ClassDeclaration(line);
                        _trees.Add(fileName, classDeclaration);
                        classFlag = true;
                        if (fileName.Contains("Client"))
                        {
                            var type = "HttpClient";
                            var name = "_client";
                            var mod = "private static readonly";
                            _trees[fileName] = _trees[fileName]
                                .AddMembers(
                                    FieldParser.HttpClientField(type, name, mod));
                        }
                        
                        continue;
                    }

                    if (line.Contains('('))
                        fieldFlag = false;
                    
                    if (fieldFlag && !fileName.Contains("Client") && classFlag && line.Contains('=') | !line.Contains(')'))
                    {
                        var fieldDeclaration = FieldParser.GetField(line);
                        _trees[fileName] = _trees[fileName]
                            .AddMembers(fieldDeclaration);
                        continue;
                    }

                    if (classFlag && line.Contains('@') && line.Contains('"'))
                    {
                        var methodLine = sr.ReadLine();
                        var methodDeclaration = MethodDistributer.Distribute(methodLine, line);
                        _trees[fileName] = _trees[fileName]
                            .AddMembers(methodDeclaration);
                    }
                }
            }
            
            if (fileName.Contains("Client"))
                ParseControllers(dirToPath, fileName);
            else
            {
                ParseDtos(dirToPath, fileName);
            }
        }
    }

    private void toFile(CompilationUnitSyntax comp, string path)
    {
        if (File.Exists(path))
            File.Delete(path);
        FileStream file = File.Create(path);
        file.Close();
        File.WriteAllText(path, comp.NormalizeWhitespace().ToString());
    }

    private void ParseControllers(string dirToPath, string fileName)
    {
        toFile(SyntaxFactory.CompilationUnit()
            .AddUsings(
                SyntaxFactory.UsingDirective(
                    SyntaxFactory.QualifiedName(
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.QualifiedName(
                                SyntaxFactory.IdentifierName("System"),
                                SyntaxFactory.IdentifierName("Net")),
                            SyntaxFactory.IdentifierName("Http")),
                        SyntaxFactory.IdentifierName("Json"))))
            .AddUsings(
                SyntaxFactory.UsingDirective(
                    SyntaxFactory.QualifiedName(
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.IdentifierName("System"),
                            SyntaxFactory.IdentifierName("Text")),
                        SyntaxFactory.IdentifierName("Json"))))
            .AddUsings(
                SyntaxFactory.UsingDirective(
                    SyntaxFactory.QualifiedName(
                        SyntaxFactory.QualifiedName(
                        SyntaxFactory.IdentifierName("Parser"),
                            SyntaxFactory.IdentifierName("ParserResults")),
                        SyntaxFactory.IdentifierName("Entities"))))
            .AddMembers(
                SyntaxFactory.NamespaceDeclaration(
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.QualifiedName(
                            SyntaxFactory.IdentifierName("Parser"),
                            SyntaxFactory.IdentifierName("ParserResults")),
                            SyntaxFactory.IdentifierName("ClientGens")))
                    .AddMembers(_trees[fileName])), Path.Combine(dirToPath, fileName + ".cs"));
    }

    private void ParseDtos(string dirToPath, string fileName)
    {
        toFile(SyntaxFactory.CompilationUnit()
            .AddMembers(
                SyntaxFactory.NamespaceDeclaration(
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.QualifiedName(
                            SyntaxFactory.IdentifierName("Parser"),
                            SyntaxFactory.IdentifierName("ParserResults")),
                            SyntaxFactory.IdentifierName("Entities")))
                    .AddMembers(_trees[fileName])), Path.Combine(dirToPath, fileName + ".cs"));
    }
}