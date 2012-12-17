using COAT.Data.Import;
using COAT.Schedule;
using COAT.Util.IDS;

namespace COAT.Models
{
    public class COATImportTask : IScheduleTask
    {
        public const string ImportTaskName = "ImportTask";

        public COATImportTask()
        {
            ImportTask = new ImportTask();
            Name = ImportTaskName;
        }

        public COATImportTask(ImportTask task)
        {
            ImportTask = task;
            Name = ImportTaskName;
        }

        public ImportTask ImportTask { get; private set; }

        public string FilePath
        {
            get { return ImportTask.FilePath; }
            set { ImportTask.FilePath = value; }
        }

        #region IScheduleTask Members

        public int RunTimes { get; set; }

        public void Run(object obj)
        {
            if (string.IsNullOrWhiteSpace(Type))
                return;

            switch (Type)
            {
                case TaskType.ImportORPDeal:
                    new DealImportHelper(FilePath).ImportRawData();
                    break;
                case TaskType.ImportExcutiveDeal:
                    new ExcutivedDealImportHelper(FilePath).ImportRawData();
                    break;
                case TaskType.ImportNameAccountList:
                    new NameAccountImporttHelper(FilePath).ImportRawData();
                    break;
                case TaskType.ImportChannelAssigner:
                    GetImportHelper(SystemRoleIds.ChannelAssigner, BusinessRoleIds.ChannelManager, FilePath).
                        ImportRawData();
                    break;
                case TaskType.ImportChannelApprover:
                    GetImportHelper(SystemRoleIds.ChannelApprover, BusinessRoleIds.ChannelManager, FilePath).
                        ImportRawData();
                    break;
                case TaskType.ImportChannelDirector:
                    GetImportHelper(SystemRoleIds.ChannelDirector, BusinessRoleIds.ChannelDirector, FilePath).
                        ImportRawData();
                    break;
                case TaskType.ImportSalesAssigner:
                    GetImportHelper(SystemRoleIds.SalesAssigner, BusinessRoleIds.InsideSales, FilePath).ImportRawData();
                    break;
                case TaskType.ImportSalesApprover:
                    GetImportHelper(SystemRoleIds.SalesApprover, BusinessRoleIds.InsideSales, FilePath).ImportRawData();
                    break;
                case TaskType.ImportSalesDirector:
                    GetImportHelper(SystemRoleIds.SalesDirector, BusinessRoleIds.SalesDirector, FilePath).ImportRawData();
                    break;
                case TaskType.ImportVisitor:
                    GetImportHelper(SystemRoleIds.Visitor, BusinessRoleIds.Unkonwn, FilePath).ImportRawData();
                    break;
                case TaskType.ImportNameAccountSales:
                    GetImportHelper(SystemRoleIds.Forbidden, BusinessRoleIds.NameAccountSales, FilePath).ImportRawData();
                    break;
                case TaskType.ImportVolumeSales:
                    GetImportHelper(SystemRoleIds.Forbidden, BusinessRoleIds.VolumeSales, FilePath).ImportRawData();
                    break;
            }
        }

        public void Success()
        {
            ImportTask.IsComplete = true;
        }

        public void Error(string msg)
        {
            if (RunTimes > 3)
                ImportTask.IsComplete = true;

            ErrorMessage = msg;
        }


        public int Id
        {
            get { return ImportTask.Id; }
            set { ImportTask.Id = value; }
        }

        public string Name { get; set; }


        public string Type
        {
            get { return ImportTask.Type; }
            set { ImportTask.Type = value; }
        }

        public string ErrorMessage
        {
            get { return ImportTask.ErrorMessage; }
            set { ImportTask.ErrorMessage = value; }
        }

        #endregion

        private UserImportHelper GetImportHelper(int sysRoleId, int busRoleId, string path)
        {
            return new UserImportHelper(sysRoleId, busRoleId, null, path);
        }

        #region Nested type: TaskType

        public struct TaskType
        {
            public const string ImportORPDeal = "ImpotORPDeal";
            public const string ImportExcutiveDeal = "ImportExcutiveDeal";
            public const string ImportNameAccountList = "ImportNameAccountList";
            public const string ImportChannelAssigner = "ImportChannelAssigner";
            public const string ImportChannelApprover = "ImportChannelApprover";
            public const string ImportChannelDirector = "ImportChannelDirector";
            public const string ImportSalesAssigner = "ImportSalesAssigner";
            public const string ImportSalesApprover = "ImportSalesApprover";
            public const string ImportSalesDirector = "ImportSalesDirector";
            public const string ImportVisitor = "ImportVisitor";
            public const string ImportNameAccountSales = "ImportNameAccountSales";
            public const string ImportVolumeSales = "ImportVolumeSales";
        }

        #endregion
    }
}