using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Operations
{
    public class SequentialOperation<TState> : BaseListOperation<TState>
    {
        public SequentialOperation()
            : base(DataIndex<IOperation<TState>>.New(0)) { }

        public SequentialOperation(IOperation<TState> operation)
            : base(DataIndex<IOperation<TState>>.Of(operation)) { }

        public SequentialOperation(DataIndex<IOperation<TState>> operations)
            : base(operations) { }

        public SequentialOperation(params IOperation<TState>[] operations)
            : base(operations.ToDataIndex()) { }


        public override string Description => this.ToListString("(", ")", " ");

        protected override bool DoAction(TState state, ITracer<TState> tracer = null)
        {
            foreach (var operation in operations)
            {
                if (!operation.Perform(state, tracer))
                    return false;
            }

            return true;
        }
    }
}