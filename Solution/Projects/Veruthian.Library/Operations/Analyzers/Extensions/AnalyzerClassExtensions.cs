namespace Veruthian.Library.Operations.Analyzers.Extensions
{
    public static class AnalyzerClassExtensions
    {
        public static ClassifiedOperation<TState> Literal<TState>(this IOperationGenerator<TState> builder, IOperation<TState> operation)
            => builder.Classify(operation, AnalyzerClass.Literal);

        public static bool IsRule<TState>(this ClassifiedOperation<TState> operation)
            => operation.Is(AnalyzerClass.Rule.Name);

        public static bool IsToken<TState>(this ClassifiedOperation<TState> operation)
            => operation.Is(AnalyzerClass.Token.Name);
            
        public static bool IsLiteral<TState>(this ClassifiedOperation<TState> operation)
            => operation.Is(AnalyzerClass.Literal.Name);
    }
}