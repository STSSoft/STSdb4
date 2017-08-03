using System;

namespace STSdb4.General
{
    public static class Environment
    {
        public static readonly bool RunningOnMono = Type.GetType("Mono.Runtime") != null;
    }
}
