namespace COAT.ViewModel
{
    public class Counter
    {
        private int _count;
        public int Time { get; set; }

        public int Count
        {
            get { return _count++/Time; }
        }
    }
}