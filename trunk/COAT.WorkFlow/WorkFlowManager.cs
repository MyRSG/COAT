using COAT.WorkFlow.Base;

namespace COAT.WorkFlow
{
    public class WorkFlowManager
    {
        private static IWorkFlowFactory _wfFactory;
        private static IDataFactory _dFactory;

        public static void InjectWorkFlowFactory(IWorkFlowFactory factory)
        {
            _wfFactory = factory;
        }

        public static void InjectDataFacotry(IDataFactory factory)
        {
            _dFactory = factory;
        }

        public static IWorkFlowFactory WorkFlowFactory
        {
            get { return _wfFactory; }
        }

        public static IDataFactory DataFactory
        {
            get { return _dFactory; }
        }
    }
}
