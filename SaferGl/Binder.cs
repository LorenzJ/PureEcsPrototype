namespace SaferGl
{
    public class Binder
    {
        public static Binder Instance { get; }

        static Binder()
        {
            Instance = new Binder();
        }

        private Binder() { }
    }

}
