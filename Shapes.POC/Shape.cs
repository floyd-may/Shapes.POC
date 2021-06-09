using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Shapes.POC
{
    public sealed class Shape
    {
        public string Name { get; }
        public Type ImplementationType { get; }
        public ImmutableArray<ImmutableArray<Feature>> ValidFeatureCombinations { get; }
        public ImmutableSortedSet<string> SupportedFeatureNames =>
            ValidFeatureCombinations
                .SelectMany(x => x)
                .Select(x => x.Name)
                .ToImmutableSortedSet();
        public ImmutableArray<Feature> Features =>
            ValidFeatureCombinations
                .SelectMany(x => x)
                .GroupBy(x => x.Name)
                .Select(x => x.First())
                .ToImmutableArray();

        public Shape(string name, Type implementationType, ImmutableArray<ImmutableArray<Feature>> validFeatureCombinations)
        {
            Name = name;
            ImplementationType = implementationType;
            ValidFeatureCombinations = validFeatureCombinations;
        }

        public IEnumerable<ImmutableArray<Feature>> GetValidPermutations()
        {
            return PermuteFeatures(SupportedFeatureNames.ToImmutableArray())
                .Where(CanLeadToValid)
                .Select(x => x.Select(GetFeatureByName).ToImmutableArray());
        }

        public Feature GetFeatureByName(string name)
        {
            return Features.Single(x => x.Name == name);
        }

        public bool CanDraw(ImmutableArray<Feature> enabledFeatures)
        {
            var featureNames = enabledFeatures.Select(x => x.Name).ToArray();
            return ValidFeatureCombinations
                .Select(x => x.Select(f => f.Name).ToImmutableHashSet())
                .Any(set => set.SetEquals(featureNames));
        }

        private bool CanLeadToValid(ImmutableSortedSet<string> candidate)
        {
            return ValidFeatureCombinations.Any(x => candidate.IsSubsetOf(x.Select(f => f.Name)));
        }

        private IEnumerable<ImmutableSortedSet<string>> PermuteFeatures(ImmutableArray<string> features)
        {
            if(features.IsEmpty)
                return new[] { ImmutableSortedSet<string>.Empty };

            var first = features[0];

            var rest = features.Skip(1).ToImmutableArray();

            return PermuteFeatures(rest)
                .SelectMany(x => new[] { x, x.Add(first) })
                .ToArray();
        }
    }

    public sealed class Feature
    {
        public string Name { get; }
        public IDictionary<string, Type> Parameters { get; }

        public Feature(string name, IDictionary<string, Type> parameters)
        {
            Name = name;
            Parameters = parameters;
        }
    }

}
