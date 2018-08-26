using System.Collections.Generic;
using System.Diagnostics;

namespace Dahl.Data.Common
{
    public class StopwatchCollection
    {
        private static readonly List<Stopwatch> _swList = new List<Stopwatch>();

        public void Init( int numStopwatches )
        {
            for ( int i = 0; i < numStopwatches; i++ )
            {
                var sw = new Stopwatch();
                sw.Reset();
                _swList.Add( sw );
            }
        }

        public Stopwatch Get( int i )
        {
            return _swList[i];
        }
    }
}
