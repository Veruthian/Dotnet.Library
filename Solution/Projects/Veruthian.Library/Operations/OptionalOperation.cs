using System;

namespace Veruthian.Library.Operations
{
    public class OptionalOperation<TState> : NestedOperation<TState>
    {
        public OptionalOperation(IOperation<TState> operation) : base(operation) { }

        public override string Description => "Optional";
        
        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null)
        {
            Operation.Perform(state, tracer);

            return true;
        }
    }
}