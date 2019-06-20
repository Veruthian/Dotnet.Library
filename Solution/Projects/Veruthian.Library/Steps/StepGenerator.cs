using System.Collections.Generic;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Steps
{
    public class StepGenerator
    {
        private Number linkNumber = Number.Zero;


        // Generate General
        public bool TypeAllConstructs { get; set; }


        private LinkStep NewStep(IStep shunt = null, IStep down = null, IStep next = null)
            => new LinkStep { Shunt = shunt, Down = down, Next = next };

        public IStep GenerateConstruct(IStep step, string type = null, bool? setType = null)
        {
            bool wrap = setType == null ? TypeAllConstructs : setType.Value;

            return (wrap && type != null) ? new NestedStep(type, step) : step;
        }


        // Typed
        public IStep Typed(string type)
            => new GeneralStep(type);

        public IStep Typed(string type, IStep step)
            => new GeneralStep(type) { Down = step };


        // Sequence
        public const string SequenceType = "Sequence";

        public IStep Sequence(params IStep[] steps)
            => Sequence(steps as IEnumerable<IStep>);

        public IStep Sequence(IStep[] steps, bool? setType)
            => Sequence(steps as IEnumerable<IStep>);

        public IStep Sequence(IEnumerable<IStep> steps, bool? setType = null)
            => GenerateConstruct(RawSequence(steps), SequenceType);

        private IStep RawSequence(IEnumerable<IStep> steps)
        {
            var first = NewStep();

            var current = null as LinkStep;

            foreach (var step in steps)
            {
                if (current == null)
                {
                    current = first;
                }
                else
                {
                    var next = NewStep();

                    current.Next = next;

                    current = next;
                }

                current.Down = step;
            }
            return first;
        }


        // Condition
        private IStep Condition(IStep condition, IStep onTrue, IStep onFalse, string type = null)
        {
            var result = NewStep(shunt: condition, down: onTrue, next: onFalse);

            return GenerateConstruct(result, type);
        }

        private IStep ShuntTrue => new TerminalStep();

        private IStep ShuntFalse => null;


        // Boolean
        public const string TrueType = "True";

        public IStep True(bool? setType = null) => GenerateConstruct(RawTrue, TrueType);

        private IStep RawTrue => NewStep(shunt: ShuntTrue, down: ShuntTrue);


        public const string FalseType = "False";

        public IStep False(bool? setType = null) => GenerateConstruct(RawFalse, FalseType);

        private IStep RawFalse => NewStep(shunt: ShuntTrue, down: ShuntFalse);



        // If
        public const string IfType = "If";

        public const string IfThenType = "IfThen";

        public const string IfElseType = "IfElse";

        public const string IfThenElseType = "IfThenElse";


        public IStep If(IStep condition, bool? setType = null)
            => Condition(condition, onTrue: ShuntTrue, onFalse: ShuntFalse, type: IfType);

        public IStep IfThen(IStep condition, IStep thenStep, bool? setType = null)
            => Condition(condition, onTrue: thenStep, onFalse: ShuntFalse, type: IfThenType);

        public IStep IfElse(IStep condition, IStep elseStep, bool? setType = null)
            => Condition(condition, onTrue: ShuntTrue, onFalse: elseStep, type: IfElseType);

        public IStep IfThenElse(IStep condition, IStep thenStep, IStep elseStep, bool? setType = null)
            => Condition(condition, onTrue: thenStep, onFalse: elseStep, type: IfThenElseType);


        // Unless
        public const string UnlessType = "Unless";

        public const string UnlessThenType = "UnlessThen";

        public const string UnlessElseType = "UnlessElse";

        public const string UnlessThenElseType = "UnlessThenElse";


        public IStep Unless(IStep condition, bool? setType = null)
            => Condition(condition, onTrue: ShuntFalse, onFalse: ShuntTrue, type: UnlessType);

        public IStep UnlessThen(IStep condition, IStep thenStep, bool? setType = null)
            => Condition(condition, onTrue: ShuntFalse, onFalse: thenStep, type: UnlessThenType);

        public IStep UnlessElse(IStep condition, IStep elseStep, bool? setType = null)
            => Condition(condition, onTrue: elseStep, onFalse: ShuntTrue, type: UnlessElseType);

        public IStep UnlessThenElse(IStep condition, IStep thenStep, IStep elseStep, bool? setType = null)
            => Condition(condition, onTrue: elseStep, onFalse: thenStep, type: UnlessThenElseType);


        // Repeat
        public const string WhileType = "While";

        public const string UntilType = "Until";

        public const string ExactlyType = "Exactly";

        public const string AtMostType = "AtMost";

        public const string AtLeastType = "AtLeast";

        public const string BetweenType = "Between";


        public IStep While(IStep condition, IStep step, bool? setType = null)
            => GenerateConstruct(RawWhile(condition, step), WhileType);


        public IStep Until(IStep condition, IStep step, bool? setType = null)
            => GenerateConstruct(RawUntil(condition, step), UntilType);

        public IStep Exactly(int times, IStep step, bool? setType = null)
            => GenerateConstruct(RawExactly(times, step), ExactlyType);

        public IStep AtMost(int times, IStep condition, IStep step, bool? setType = null)
            => GenerateConstruct(RawAtMost(times, condition, step), AtMostType);

        public IStep AtLeast(int times, IStep condition, IStep step, bool? setType = null)
            => GenerateConstruct(RawAtLeast(times, condition, step), AtLeastType);

        public IStep Between(int min, int max, IStep condition, IStep step, bool? setType = null)
            => GenerateConstruct(RawBetween(min, max, condition, step), BetweenType);


        private LinkStep RawWhile(IStep condition, IStep step)
        {
            var repeater = NewStep();

            repeater.Shunt = condition;

            repeater.Down = NewStep
            (
                down: step,

                next: repeater
            );

            repeater.Next = ShuntTrue;

            return repeater;
        }

        private LinkStep RawUntil(IStep condition, IStep step)
        {
            var repeater = NewStep();

            repeater.Shunt = condition;

            repeater.Down = ShuntTrue;

            repeater.Next = NewStep
            (
                down: step,

                next: repeater
            );

            return repeater;
        }

        private LinkStep RawExactly(int times, IStep step)
        {
            var first = NewStep();

            var current = first;

            for (int i = 0; i < times; i++)
            {
                if (i > 0)
                {
                    var next = NewStep();

                    current.Next = next;

                    current = next;
                }

                current.Down = step;
            }

            return first;
        }

        private LinkStep RawAtMost(int times, IStep condition, IStep step)
        {
            var first = NewStep();

            var current = first;

            for (int i = 0; i < times; i++)
            {
                if (i > 0)
                {
                    var next = NewStep();

                    current.Next = next;

                    current = next;
                }

                current.Shunt = condition;

                var down = NewStep();

                current.Down = down;

                down.Down = step;

                current = down;
            }

            return first;
        }

        private LinkStep RawAtLeast(int times, IStep condition, IStep step)
        {
            var result = RawExactly(times, step);

            result.Next = RawWhile(condition, step);

            return result;
        }

        private LinkStep RawBetween(int min, int max, IStep condition, IStep step)
        {
            var result = RawExactly(min, step);

            result.Next = RawAtMost(max - min, condition, step);

            return result;
        }
    }
}