using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shapes.POC
{
    public sealed class Program
    {
        public static void Main()
        {
            var root = Path.Combine(Directory.GetCurrentDirectory(), "Generated");

            var ns = NamespaceDeclaration(ParseName("Shapes.POC.Generated"));

            using var writer = new StreamWriter(Path.Combine(root, "Shapes.Generated.cs"), false);

            var triangleClasses = Shapes.Triangle.GetValidPermutations()
                .Select(x => CreateClass(Shapes.Triangle, x));

            ns.AddMembers(triangleClasses.ToArray())
                .NormalizeWhitespace()
                .WriteTo(writer);
        }

        private static ClassDeclarationSyntax CreateClass(Shape shape, ImmutableArray<Feature> enabledFeatures)
        {
            var featureSuffix = string.Join("_", enabledFeatures.Select(x => x.Name).OrderBy(x => x));

            var className = $"{shape.Name}_{featureSuffix}".TrimEnd('_');

            var fields = enabledFeatures.SelectMany(x => x.Parameters)
                .Select(x =>
                    VariableDeclaration(IdentifierName(x.Value.FullName))
                        .WithVariables(SingletonSeparatedList(VariableDeclarator(Identifier(x.Key)))))
                .Select(x => FieldDeclaration(x).AddModifiers(Token(SyntaxKind.PrivateKeyword)));

            var klass = ClassDeclaration(Identifier(className))
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddMembers(fields.ToArray());

            if (shape.CanDraw(enabledFeatures))
            {
                klass = klass.AddMembers(
                    MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)), Identifier("Draw"))
                        .AddModifiers(Token(SyntaxKind.PublicKeyword))
                        .WithBody(Block()));
            }

            foreach (var otherFeature in shape.Features.Where(f => !enabledFeatures.Any(ef => ef.Name == f.Name)))
            {
                var featureSet = enabledFeatures.Select(x => x.Name).ToImmutableArray().Add(otherFeature.Name).ToImmutableSortedSet();
                if (shape.ValidFeatureCombinations.Any(comb => featureSet.IsSubsetOf(comb.Select(x => x.Name))))
                {
                    var otherClassName = featureSet.ToFluentClassName(shape);
                    klass = klass.AddMembers(
                        MethodDeclaration(IdentifierName(otherClassName), Identifier(otherFeature.Name))
                            .AddModifiers(Token(SyntaxKind.PublicKeyword))
                            .AddParameterListParameters(
                                otherFeature.Parameters.Select(kvp => Parameter(Identifier(kvp.Key)).WithType(IdentifierName(kvp.Value.FullName))).ToArray()
                                )
                            .WithBody(Block(SingletonList<StatementSyntax>(
                                ReturnStatement(
                                    ObjectCreationExpression(IdentifierName(otherClassName))
                                        .WithArgumentList(ArgumentList()))))));
                }
            }
            return klass;
        }
    }

    public sealed class Features
    {
        public static readonly Feature StrokeColor = new Feature("StrokeColor", new Dictionary<string, Type> {
                ["strokeColor"] = typeof(uint),
            });
        public static readonly Feature FillColor = new Feature("FillColor", new Dictionary<string, Type> {
                ["fillColor"] = typeof(uint),
            });
        public static readonly Feature CornerRadius = new Feature("CornerRadius", new Dictionary<string, Type> {
                ["cornerRadius"] = typeof(float),
            });
    }

    public sealed class Shapes
    {
        public static readonly Shape Triangle = new Shape("Triangle", typeof(Triangle), ImmutableArray.Create(new[]
        {
            ImmutableArray.Create(Features.StrokeColor),
            ImmutableArray.Create(Features.StrokeColor, Features.FillColor),
            ImmutableArray.Create(Features.StrokeColor, Features.FillColor, Features.CornerRadius),
        }));
    }

    public static class Triangle
    {
        public static void Draw(uint strokeColor) { }
        public static void Draw(uint strokeColor, uint fillColor) { }
        public static void Draw(uint strokeColor, uint fillColor, float cornerRadius) { }
    }

    public static class EnumerableExtensions
    {
        public static string ToFluentClassName(this IEnumerable<string> featureNames, Shape shape)
        {
            var featureSuffix = string.Join("_", featureNames.OrderBy(x => x));
            var className = $"{shape.Name}_{featureSuffix}".TrimEnd('_');

            return className;
        }
    }
}
