using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Compilers.Lexers
{
    public abstract class SpeculativeLexer<TToken, TType, TReader> : Lexer<TToken, TType, TReader>
        where TToken : IToken<TType>
        where TReader : ISpeculativeReader<char>
    {
        protected List<TextLocation> markedLocations;

        protected virtual void OnRetreated(int fromPosition, int markCount)
        {
            if (markCount > markedLocations.Count)
                throw new ArgumentOutOfRangeException("markCount");

            if (IsCapturing)
            {
                int length = fromPosition - Reader.Position;

                RemoveFromBufferEnd(length);
            }

            int markIndex = markedLocations.Count - markCount;
            
            this.Location = markedLocations[markIndex];

            markedLocations.RemoveRange(markIndex, markCount);
        }

        protected void Mark()
        {
            if (markedLocations == null)
                markedLocations = new List<TextLocation>();
            markedLocations.Add(this.Location);
            Reader.Mark();
        }

        protected void Commit()
        {
            Reader.Commit();
        }


        protected int Retreat()
        {
            int fromPosition = Reader.Retreat();

            OnRetreated(fromPosition, 1);

            return fromPosition;
        }

        protected int Retreat(int marks)
        {
            int fromPosition = Reader.Retreat(marks);

            OnRetreated(fromPosition, marks);

            return fromPosition;
        }

        protected int RetreatAll()
        {
            int markCount = Reader.MarkCount;

            int fromPosition = Reader.RetreatAll();

            OnRetreated(fromPosition, markCount);

            return fromPosition;
        }
    }
}