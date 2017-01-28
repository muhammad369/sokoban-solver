using System;
using System.Collections.Generic;


namespace inferenceEngine.svmEngine
{
    public interface IState
    {
        IState clone();

        List<IState> Next();

        bool isTargetState();

        /// <summary>
        /// for optimization, to check if a state is known that it won't produce any other states
        /// </summary>
        /// <returns></returns>
        bool isBlockedState();
    }
}
