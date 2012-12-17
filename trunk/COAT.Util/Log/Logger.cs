namespace COAT.Util.Log
{
    public class Logger
    {

        public void Debug(string msg)
        {
            Log(LogLevel.Debug, msg);
        }

        public void Info(string msg)
        {
            Log(LogLevel.Info, msg);
        }

        public void Warn(string msg)
        {
            Log(LogLevel.Warn, msg);
        }

        public void Error(string msg)
        {
            Log(LogLevel.Error, msg);
        }

        public void Assert(string msg)
        {
            Log(LogLevel.Assert,msg);
        }

        public void Log(LogLevel level, string msg)
        {

        }

    }
}
