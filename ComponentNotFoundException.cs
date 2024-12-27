namespace SunflowerECS
{
    public sealed class ComponentNotFoundException<T> : Exception where T : class, IComponent
    {
        public override string Message => $"Couldn't find component of type \"{nameof(T)}\"";
    }

    public sealed class ComponentNotFoundException<T, U> : Exception where T : class, IComponent
    {
        public override string Message => $"Couldn't find components of type \"{nameof(T)}\" or \"{nameof(U)}\"";
    }

    public sealed class ComponentNotFoundException<T, U, V> : Exception where T : class, IComponent
    {
        public override string Message => $"Couldn't find components of type \"{nameof(T)},\" \"{nameof(U)},\" or \"{nameof(V)}\"";
    }

    public sealed class ComponentNotFoundException<T, U, V, W> : Exception where T : class, IComponent
    {
        public override string Message => $"Couldn't find components of type \"{nameof(T)},\" \"{nameof(U)},\" \"{nameof(V)},\" or \"{nameof(W)}\"";
    }

    public sealed class ComponentNotFoundException<T, U, V, W, X> : Exception where T : class, IComponent
    {
        public override string Message => $"Couldn't find components of type \"{nameof(T)},\" \"{nameof(U)},\" \"{nameof(V)},\" \"{nameof(W)},\" or \"{nameof(X)}\"";
    }

    public sealed class ComponentNotFoundException<T, U, V, W, X, Y> : Exception where T : class, IComponent
    {
        public override string Message => $"Couldn't find components of type \"{nameof(T)},\" \"{nameof(U)},\" \"{nameof(V)},\" \"{nameof(W)},\" \"{nameof(X)},\" or \"{nameof(Y)}\"";
    }
}